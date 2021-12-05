using System;

using Newtonsoft.Json.Linq;

namespace NLSL.SKS.Package.Services.DTOs.JsonConverter
{
    public class HopJsonConverter : BaseJsonConverter<Hop>
    {
        public override Hop Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException(nameof(jObject));

            if (!jObject.ContainsKey("hopType"))
                throw new ArgumentNullException("hopType");
            
            string hopType = jObject["hopType"].ToString().Trim();
            
            if (string.Compare(hopType,"warehouse",StringComparison.InvariantCultureIgnoreCase) == 0)
                return new Warehouse();

            if (string.Compare(hopType,"truck",StringComparison.InvariantCultureIgnoreCase) == 0)
                return new Truck();

            if (string.Compare(hopType,"transferwarehouse",StringComparison.InvariantCultureIgnoreCase) == 0)
                return new Transferwarehouse();

            throw new NotImplementedException();
        }
    }
}