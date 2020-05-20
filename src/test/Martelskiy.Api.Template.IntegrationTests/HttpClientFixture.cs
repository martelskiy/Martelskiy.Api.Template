using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Martelskiy.Api.Template.IntegrationTests
{
    public class HttpClientFixture : IClassFixture<WebApplicationFactory<Startup>>
    {
        internal readonly WebApplicationFactory<Startup> WebApplicationFactory;

        public HttpClientFixture(WebApplicationFactory<Startup> webApplicationFactory)
        {
            WebApplicationFactory = webApplicationFactory;
        }

        public HttpClient CreateDefaultHttpClient(Action<IServiceCollection> configureServices = null)
        {
            var httpClient = WebApplicationFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(collection =>
                {
                    configureServices?.Invoke(collection);
                });
            }).CreateClient();

            httpClient.Timeout = TimeSpan.FromSeconds(10);

            return httpClient;
        }
    }
}
