using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Martelskiy.Api.Template.Features.Shared.Serializers
{
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, JsonSerializerSettings);
        }
    }
}
