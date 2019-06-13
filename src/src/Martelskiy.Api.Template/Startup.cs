using System;
using Martelskiy.Api.Template.Features.HealthCheck;
using Martelskiy.Api.Template.Features.Shared.CorrelationId;
using Martelskiy.Api.Template.Features.Shared.ErrorHandling;
using Martelskiy.Api.Template.Features.Shared.Serializers;
using Martelskiy.Api.Template.Features.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Martelskiy.Api.Template
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = NewtonsoftJsonSerializer.JsonSerializerSettings.ContractResolver);

            services.AddApiVersioning();

            services
                .AddHealthChecks()
                .AddCheck<DefaultHealthCheck>("default_health_check");

            services.AddSingleton<ErrorHandlingMiddleware>();
            services.AddHttpContextAccessor();
            services.AddSingleton<ICorrelationIdAccessor, CorrelationIdAccessor>();

            SwaggerConfigurator.Configure(services);

            SerializerConfigurator.Configure(services);

            var serviceProvider = services.BuildServiceProvider(true);

            return serviceProvider;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ErrorHandlingMiddleware>();
            }

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = new HealthCheckResponseWriter(app.ApplicationServices.GetRequiredService<IJsonSerializer>()).WriteResponse
            });

            SwaggerConfigurator.AddMiddleware(app);

            app.UseMvc();
        }
    }
}
