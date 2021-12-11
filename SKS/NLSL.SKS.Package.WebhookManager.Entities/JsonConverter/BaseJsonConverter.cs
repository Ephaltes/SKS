using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NLSL.SKS.Package.WebhookManager.Entities.JsonConverter
{
    public abstract class BaseJsonConverter<T> : Newtonsoft.Json.JsonConverter
    {
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (reader.TokenType == JsonToken.Null)
                return null;

            JObject jObject = JObject.Load(reader);
            T target = Create(objectType, jObject);
            serializer.Populate(jObject.CreateReader(), target);
            return target;
        }
        
    }
}