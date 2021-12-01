using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text;

using Newtonsoft.Json;

namespace NLSL.SKS.Package.WebhookManager.Entities
{
    [ExcludeFromCodeCoverage]
    public class WebhookMessage : TrackingInformation
    {
        /// <summary>
        /// Gets or Sets TrackingId
        /// </summary>
        [RegularExpression("/^[A-Z0-9]{9}$/")]
        [DataMember(Name="trackingId")]
        public string TrackingId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class WebhookMessage {\n");
            sb.Append("  TrackingId: ").Append(TrackingId).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public  new string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }
}