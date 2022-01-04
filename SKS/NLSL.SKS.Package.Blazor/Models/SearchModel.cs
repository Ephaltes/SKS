using System.ComponentModel.DataAnnotations;

namespace NLSL.SKS.Package.Blazor.Models
{
    public class SearchModel
    {
        [Required]
        [RegularExpression("^[A-Z0-9]{9}$")]
        public string TrackingId
        {
            get;set;
        }
    }
}
