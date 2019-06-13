using Microsoft.Extensions.DependencyInjection;

namespace Martelskiy.Api.Template.Features.Shared.Serializers
{
    public static class SerializerConfigurator
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSingleton<NewtonsoftJsonSerializer>();
            services.AddSingleton<ISerializer>(x => x.GetRequiredService<NewtonsoftJsonSerializer>());
            services.AddSingleton<IJsonSerializer>(x => x.GetRequiredService<NewtonsoftJsonSerializer>());
        }
    }
}
