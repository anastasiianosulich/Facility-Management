using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace FacilityLeasing.Helpers;

public static class SerializationHelper
{
    public static string SerializeJson<T>(T data)
    {
        var camelCasePropertiesSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        return JsonConvert.SerializeObject(data, camelCasePropertiesSetting);
    }
}
