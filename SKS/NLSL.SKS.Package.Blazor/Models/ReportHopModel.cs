using ExpressiveAnnotations.Attributes;

using System.ComponentModel.DataAnnotations;

namespace NLSL.SKS.Package.Blazor.Models
{
    public class ReportHopModel
    {
        [Required]
        [RegularExpression("^[A-Z0-9]{9}$")]
        public string TrackingId
        {
            get; set;
        }

        [RequiredIf("!Delivered")]
        [RegularExpression("^[A-Z]{4}\\d{1,4}$")]
        public string HopCode
        {
            get; set;
        }

        public bool Delivered
        {
            get; set;
        } = false;
    }
}
