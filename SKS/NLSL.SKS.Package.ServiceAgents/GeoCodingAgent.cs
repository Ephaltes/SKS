using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using NLSL.SKS.Package.ServiceAgents.Entities;
using NLSL.SKS.Package.ServiceAgents.Interface;

using Nominatim.API.Geocoders;
using Nominatim.API.Models;

namespace NLSL.SKS.Package.ServiceAgents
{
    public class GeoCodingAgent : IGeoCodingAgent
    {
        private readonly ForwardGeocoder _geocoder;
        private readonly IMapper _mapper;
        public GeoCodingAgent(IMapper mapper)
        {
            _mapper = mapper;
            _geocoder = new ForwardGeocoder();
        }
        public List<GeoCoordinates> GetGeoCoordinates(Address address)
        {
            List<GeoCoordinates> resultList = new List<GeoCoordinates>();

            ForwardGeocodeRequest request = _mapper.Map<Address, ForwardGeocodeRequest>(address);
            request.ShowGeoJSON = true;

            Task<GeocodeResponse[]>? geoCodeResponseTask = _geocoder.Geocode(request);
            geoCodeResponseTask.Wait();

            List<GeocodeResponse>? geoCodeResponseList = geoCodeResponseTask.Result.ToList();

            geoCodeResponseList.ForEach(response =>
                                        {
                                            resultList.Add(_mapper.Map<GeocodeResponse, GeoCoordinates>(response));
                                        });

            return resultList;
        }
    }
}