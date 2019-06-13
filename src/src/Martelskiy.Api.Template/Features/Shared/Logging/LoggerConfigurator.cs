using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Martelskiy.Api.Template.Features.Shared.Environment;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.RollingFile;

namespace Martelskiy.Api.Template.Features.Shared.Logging
{
    public class LoggerConfigurator : IConfigurator
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<ApplicationOptions> _applicationOptions;
        private readonly IOptions<LoggingOptions> _loggingConfig;

        public LoggerConfigurator(
            IHostingEnvironment hostingEnvironment,
            IOptions<ApplicationOptions> applicationOptions,
            IOptions<LoggingOptions> loggingConfig)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _applicationOptions = applicationOptions;
            _loggingConfig = loggingConfig ?? throw new ArgumentNullException(nameof(loggingConfig));
        }

        public void Configure()
        {
            var applicationName = _applicationOptions.Value.ApplicationName;
            var outputTemplate = "[{Timestamp:yyyyMMddTHH:mm:sszzz}][{Level:u3}][{CorrelationId}] - {Message}{NewLine}{Exception}";
            var logFilePath = GetLogFileLocation();
            var fileName = $"{applicationName}.{_hostingEnvironment.ShortName()}..txt";

            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("application-name", applicationName)
                .WriteTo.Async(a => a.File(
                    path: Path.Combine(logFilePath, fileName),
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 200000000,
                    outputTemplate: outputTemplate))
                .Destructure.ToMaximumStringLength(100)
                .Destructure.ToMaximumCollectionCount(10);

            SetMinimumLogLevel(loggerConfiguration);

            if (ShouldLogToConsole())
            {
                loggerConfiguration.WriteTo.Async(a => a.Console(outputTemplate: outputTemplate));
            }

            if (ShouldLogToElasticSearch())
            {
                ConfigureElasticSearchSink(loggerConfiguration, outputTemplate, applicationName);
            }

            Log.Logger = loggerConfiguration.CreateLogger();
        }

        private void SetMinimumLogLevel(LoggerConfiguration loggerConfiguration)
        {
            var logLevel = _loggingConfig?.Value?.LogLevel;
            if (_hostingEnvironment.IsDevelopment())
            {
                loggerConfiguration.MinimumLevel.Is(GetLogEventLevelFromString(logLevel?.Default ?? "DEBUG"));
                loggerConfiguration.MinimumLevel.Override("Microsoft", GetLogEventLevelFromString(logLevel?.Microsoft ?? "INFORMATION"));
                loggerConfiguration.MinimumLevel.Override("System", GetLogEventLevelFromString(logLevel?.System ?? "INFORMATION"));
            }
            else
            {
                loggerConfiguration.MinimumLevel.Is(GetLogEventLevelFromString(logLevel?.Default ?? "INFORMATION"));
                loggerConfiguration.MinimumLevel.Override("Microsoft", GetLogEventLevelFromString(logLevel?.Microsoft ?? "WARNING"));
                loggerConfiguration.MinimumLevel.Override("System", GetLogEventLevelFromString(logLevel?.System ?? "WARNING"));
            }
        }

        private bool ShouldLogToConsole()
        {
            return _loggingConfig?.Value?.ShouldLogToConsole ?? _hostingEnvironment.IsDevelopment();
        }

        private bool ShouldLogToElasticSearch()
        {
            return _loggingConfig?.Value?.ShouldLogToElasticSearch ?? !_hostingEnvironment.IsDevelopment();
        }

        private void ConfigureElasticSearchSink(
            LoggerConfiguration loggerConfiguration,
            string outputTemplate,
            string applicationName)
        {
            var nodeUrls = GetElasticSearchNodeUrls();
            var indexName = $"{applicationName}.{_hostingEnvironment.ShortName()}.logs-".ToLower() + "{0:yyyy.MM}";
            var failureSinkLogPathLocation = GetLogFileLocation();
            var fileName = $"{applicationName}.{_hostingEnvironment.ShortName()}.FailureSink..txt";

            loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(nodeUrls)
            {
                QueueSizeLimit = 500000,
                AutoRegisterTemplate = true,
                IndexFormat = indexName,
                TemplateName = "application.template",
                OverwriteTemplate = true,
                CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                EmitEventFailure = EmitEventFailureHandling.WriteToFailureSink,
                FailureSink = new RollingFileSink(
                    Path.Combine(failureSinkLogPathLocation, fileName),
                    new MessageTemplateTextFormatter(outputTemplate, CultureInfo.InvariantCulture),
                    200000000,
                    null
                )
            });
        }

        private string GetLogFileLocation()
        {
            var defaultPath = string.Format("..{0}..{0}logs", Path.DirectorySeparatorChar);
            if (_hostingEnvironment.IsDevelopment())
            {
                return defaultPath;
            }

            var logDirectoryPath = _loggingConfig?.Value?.DirectoryPath ?? defaultPath;
            return Path.Combine(logDirectoryPath, $"{_applicationOptions.Value.ApplicationName}.{_hostingEnvironment.ShortName()}");
        }

        private LogEventLevel GetLogEventLevelFromString(string level)
        {
            switch (level.ToUpper())
            {
                case "VERBOSE":
                    return LogEventLevel.Verbose;
                case "DEBUG":
                    return LogEventLevel.Debug;
                case "INFORMATION":
                    return LogEventLevel.Information;
                case "WARNING":
                    return LogEventLevel.Warning;
                case "ERROR":
                    return LogEventLevel.Error;
                case "FATAL":
                    return LogEventLevel.Fatal;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level));
            }
        }

        private IEnumerable<Uri> GetElasticSearchNodeUrls()
        {
            var nodes = _loggingConfig.Value.ElasticSearchNodeUrls;
            if (string.IsNullOrWhiteSpace(nodes))
            {
                return new List<Uri> { new Uri("http://localhost:9200") };
            }

            return nodes.Split(',').Select(x => new Uri(x)).ToArray();
        }
    }
}
