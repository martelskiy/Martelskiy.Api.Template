namespace Martelskiy.Api.Template.Features.Shared.Logging
{
    public class LoggingOptions
    {
        public string DirectoryPath { get; set; }
        public string ElasticSearchNodeUrls { get; set; }
        public bool? ShouldLogToElasticSearch { get; set; }
        public bool? ShouldLogToConsole { get; set; }
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }
        public string System { get; set; }
    }
}
