using System.Diagnostics.CodeAnalysis;

using AutoMapper;

using NLSL.SKS.Package.BusinessLogic.Entities;

namespace NLSL.SKS.Package.BusinessLogic.AutoMapperProfiles
{
    [ExcludeFromCodeCoverage]
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<GeoCoordinate,Pacakge.DataAccess.Entities.GeoCoordinate>();
            CreateMap<Pacakge.DataAccess.Entities.GeoCoordinate, GeoCoordinate>();

            CreateMap<Hop, Pacakge.DataAccess.Entities.Hop>();
            CreateMap<Pacakge.DataAccess.Entities.Hop,Hop>();
            
            CreateMap<HopArrival, Pacakge.DataAccess.Entities.HopArrival>();
            CreateMap<Pacakge.DataAccess.Entities.HopArrival,HopArrival>();
            
            CreateMap<Parcel, Pacakge.DataAccess.Entities.Parcel>();
            CreateMap<Pacakge.DataAccess.Entities.Parcel,Parcel>();
            
            CreateMap<Recipient, Pacakge.DataAccess.Entities.Recipient>();
            CreateMap<Pacakge.DataAccess.Entities.Recipient,Recipient>();
            
            CreateMap<Warehouse, Pacakge.DataAccess.Entities.Warehouse>();
            CreateMap<Pacakge.DataAccess.Entities.Warehouse, Warehouse>();
            
            CreateMap<WarehouseNextHops, Pacakge.DataAccess.Entities.WarehouseNextHops>();
            CreateMap<Pacakge.DataAccess.Entities.WarehouseNextHops, WarehouseNextHops>();
        }
    }
}