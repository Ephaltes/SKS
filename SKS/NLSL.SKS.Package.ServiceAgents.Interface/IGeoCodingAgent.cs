using System.Collections.Generic;

using NLSL.SKS.Package.ServiceAgents.Entities;

namespace NLSL.SKS.Package.ServiceAgents.Interface
{
    public interface IGeoCodingAgent
    {
        List<GeoCoordinates> GetGeoCoordinates(Address address);
    }
}