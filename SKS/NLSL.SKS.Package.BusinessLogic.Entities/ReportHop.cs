using System.Diagnostics.CodeAnalysis;

namespace NLSL.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class ReportHop
    {
        public TrackingId TrackingId
        {
            get;
            init;
        }
        
        public string HopCode
        {
            get;
            init;
        }
    }
}