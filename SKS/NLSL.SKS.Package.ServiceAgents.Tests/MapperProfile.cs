using AutoMapper;

using NLSL.SKS.Package.ServiceAgents.Entities;

using Nominatim.API.Models;

namespace NLSL.SKS.Package.ServiceAgents.Tests
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Address, ForwardGeocodeRequest>()
                .ForMember(dest => dest.StreetAddress,
                    opt => opt.MapFrom(
                        src => src.Street))
                .ForMember(dest => dest.PostalCode,
                    opt => opt.MapFrom(
                        src => src.ZipCode));
            
            CreateMap<GeocodeResponse, GeoCoordinates>()
                .ForMember(dest => dest.Address,
                    opt => opt.MapFrom(
                        src => src.DisplayName));
        }

    }
}