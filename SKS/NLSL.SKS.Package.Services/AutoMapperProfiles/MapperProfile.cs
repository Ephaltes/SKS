using AutoMapper;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.Services.DTOs;

using Parcel = NLSL.SKS.Package.Services.DTOs.Parcel;
using Recipient = NLSL.SKS.Package.Services.DTOs.Recipient;
using Warehouse = NLSL.SKS.Package.Services.DTOs.Warehouse;

namespace NLSL.SKS.Package.Services.AutoMapperProfiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Parcel, BusinessLogic.Entities.Parcel>();
            CreateMap<BusinessLogic.Entities.Parcel, Parcel>();

            CreateMap<TrackingInformation, BusinessLogic.Entities.Parcel>();
            CreateMap<BusinessLogic.Entities.Parcel, TrackingInformation>();
            
            CreateMap<NewParcelInfo, BusinessLogic.Entities.Parcel>();
            CreateMap<BusinessLogic.Entities.Parcel, NewParcelInfo>();
            
            CreateMap<Warehouse, BusinessLogic.Entities.Warehouse>();
            CreateMap<BusinessLogic.Entities.Warehouse, Warehouse>();
            
            CreateMap<Transferwarehouse, BusinessLogic.Entities.Warehouse>();
            CreateMap<BusinessLogic.Entities.Warehouse, Transferwarehouse>();
            
            CreateMap<Truck, BusinessLogic.Entities.Warehouse>();
            CreateMap<BusinessLogic.Entities.Warehouse, Truck>();
            
            CreateMap<Recipient, BusinessLogic.Entities.Recipient>();
            CreateMap<BusinessLogic.Entities.Recipient, Recipient>();

            CreateMap<string, TrackingId>();
            CreateMap<string, WarehouseCode>();
        }
    }
}