using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace NLSL.SKS.Package.Services.DTOs
{
    public class WebhookResponse
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>

        [DataMember(Name="id")]
        public long? Id { get; set; }

        /// <summary>
        /// Gets or Sets TrackingId
        /// </summary>
        [RegularExpression("/^[A-Z0-9]{9}$/")]
        [DataMember(Name="trackingId")]
        public string TrackingId { get; set; }

        /// <summary>
        /// Gets or Sets Url
        /// </summary>

        [DataMember(Name="url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or Sets CreatedAt
        /// </summary>

        [DataMember(Name="created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class WebhookResponse {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  TrackingId: ").Append(TrackingId).Append("\n");
            sb.Append("  Url: ").Append(Url).Append("\n");
            sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}