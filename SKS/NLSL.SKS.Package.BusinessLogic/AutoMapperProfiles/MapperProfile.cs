using System.Collections.Generic;
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
            CreateMap<GeoCoordinate,Package.DataAccess.Entities.GeoCoordinate>();
            CreateMap<Package.DataAccess.Entities.GeoCoordinate, GeoCoordinate>();

            CreateMap<Hop, Package.DataAccess.Entities.Hop>();
            CreateMap<Package.DataAccess.Entities.Hop,Hop>();
            
            CreateMap<HopArrival, Package.DataAccess.Entities.HopArrival>();
            CreateMap<Package.DataAccess.Entities.HopArrival,HopArrival>();
            
            CreateMap<Parcel, Package.DataAccess.Entities.Parcel>();
            CreateMap<Package.DataAccess.Entities.Parcel,Parcel>();
            
            CreateMap<Recipient, Package.DataAccess.Entities.Recipient>();
            CreateMap<Package.DataAccess.Entities.Recipient,Recipient>();
            
            CreateMap<Warehouse, Package.DataAccess.Entities.Warehouse>();
            CreateMap<Package.DataAccess.Entities.Warehouse, Warehouse>();
            
            CreateMap<WarehouseNextHops, Package.DataAccess.Entities.WarehouseNextHops>();
            CreateMap<Package.DataAccess.Entities.WarehouseNextHops, WarehouseNextHops>();
            
            CreateMap<IReadOnlyCollection<Package.DataAccess.Entities.Warehouse>, IReadOnlyCollection<Warehouse>>();
        }
    }
}