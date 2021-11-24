using System.Diagnostics.CodeAnalysis;

namespace NLSL.SKS.Package.ServiceAgents.Entities
{
    [ExcludeFromCodeCoverage]
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}