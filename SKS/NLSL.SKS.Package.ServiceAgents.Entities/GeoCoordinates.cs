using System.Diagnostics.CodeAnalysis;

namespace NLSL.SKS.Package.ServiceAgents.Entities
{
    [ExcludeFromCodeCoverage]
    public class GeoCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
    }
}