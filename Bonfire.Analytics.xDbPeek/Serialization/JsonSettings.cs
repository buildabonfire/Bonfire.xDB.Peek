using Bonfire.Analytics.XdbPeek.Extensions;
using Newtonsoft.Json;
using Sitecore.XConnect;

namespace Bonfire.Analytics.XdbPeek.Serialization
{
    public static class JsonSettings
    {
        public static JsonSerializerSettings NoXobjectSettings()
        {
            var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
            jsonResolver.IgnoreProperty(typeof(XdbExtensible), "XObject");

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = jsonResolver;
            return serializerSettings;
        }
    }
}