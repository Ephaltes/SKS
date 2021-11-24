using System.Collections.Generic;

using NLSL.SKS.Package.ServiceAgents.Entities;

namespace NLSL.SKS.Package.ServiceAgents.Interface
{
    public interface IGeoCodingAgent
    {
        IReadOnlyCollection<GeoCoordinates> GetGeoCoordinates(Address address);
    }
}