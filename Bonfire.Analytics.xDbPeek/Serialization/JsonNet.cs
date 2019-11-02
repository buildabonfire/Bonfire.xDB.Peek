using System;
using System.Text;
using System.Web.Mvc;
using Bonfire.Analytics.XdbPeek.Extensions;
using Newtonsoft.Json;
using Sitecore.XConnect;

namespace Bonfire.Analytics.XdbPeek.Serialization
{
    public class JsonNet : ActionResult
    {
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public JsonNet(object data, Formatting formatting)
            : this(data)
        {
            this.Formatting = formatting;
        }

        public JsonNet(object data) : this()
        {
            this.Data = data;
        }

        public JsonNet()
        {
            var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
            jsonResolver.IgnoreProperty(typeof(XdbExtensible), "XObject");

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = jsonResolver;

            this.Formatting = Formatting.None;
            this.SerializerSettings = serializerSettings;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(this.ContentType)
                ? this.ContentType
                : "application/json";
            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;

            if (this.Data == null) return;

            var writer = new JsonTextWriter(response.Output) { Formatting = this.Formatting };
            var serializer = JsonSerializer.Create(this.SerializerSettings);
            serializer.Serialize(writer, this.Data);
            writer.Flush();
        }
    }
}