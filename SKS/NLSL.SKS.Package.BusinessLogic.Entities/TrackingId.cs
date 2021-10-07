using System.Diagnostics.CodeAnalysis;

namespace NLSL.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class TrackingId
    {
        public string Id
        {
            get;
            init;
        }

        public TrackingId(string id)
        {
            Id = id;
        }
    }
}