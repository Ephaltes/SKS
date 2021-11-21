using System.Diagnostics.CodeAnalysis;

using AutoMapper;

using NLSL.SKS.Package.ServiceAgents.Entities;

using Nominatim.API.Models;

namespace NLSL.SKS.Package.Services.AutoMapperProfiles
{
    [ExcludeFromCodeCoverage]
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<DTOs.Parcel, BusinessLogic.Entities.Parcel>();
            CreateMap<BusinessLogic.Entities.Parcel, DTOs.Parcel>();

            CreateMap<DTOs.TrackingInformation, BusinessLogic.Entities.Parcel>();
            CreateMap<BusinessLogic.Entities.Parcel, DTOs.TrackingInformation>();
            
            CreateMap<DTOs.NewParcelInfo, BusinessLogic.Entities.Parcel>();
            CreateMap<BusinessLogic.Entities.Parcel, DTOs.NewParcelInfo>();
            
            CreateMap<DTOs.Transferwarehouse, BusinessLogic.Entities.TransferWarehouse>();
            CreateMap<BusinessLogic.Entities.TransferWarehouse, DTOs.Transferwarehouse>();
            
            CreateMap<DTOs.Truck, BusinessLogic.Entities.Truck>();
            CreateMap<BusinessLogic.Entities.Truck, DTOs.Truck>();

            CreateMap<DTOs.Warehouse, BusinessLogic.Entities.Warehouse>();
            CreateMap<BusinessLogic.Entities.Warehouse, DTOs.Warehouse>();
            
            CreateMap<DTOs.GeoCoordinate, BusinessLogic.Entities.GeoCoordinate>();
            CreateMap<BusinessLogic.Entities.GeoCoordinate, DTOs.GeoCoordinate>();
            
            
            CreateMap<DTOs.Recipient, BusinessLogic.Entities.Recipient>();
            CreateMap<BusinessLogic.Entities.Recipient, DTOs.Recipient>();
            
            CreateMap<DTOs.WarehouseNextHops,BusinessLogic.Entities.WarehouseNextHops>();
            CreateMap<BusinessLogic.Entities.WarehouseNextHops, DTOs.WarehouseNextHops>();

            CreateMap<DTOs.Hop, BusinessLogic.Entities.Hop>()
                .Include<DTOs.Truck,BusinessLogic.Entities.Truck>()
                .Include<DTOs.Transferwarehouse, BusinessLogic.Entities.TransferWarehouse>()
                .Include<DTOs.Warehouse,BusinessLogic.Entities.Warehouse>();
            
            CreateMap<BusinessLogic.Entities.Hop, DTOs.Hop>()
                .Include<BusinessLogic.Entities.Truck,DTOs.Truck>()
                .Include<BusinessLogic.Entities.TransferWarehouse,DTOs.Transferwarehouse>()
                .Include<BusinessLogic.Entities.Warehouse,DTOs.Warehouse>();
            
            
            // BL to DAL
            
            CreateMap<BusinessLogic.Entities.GeoCoordinate,Package.DataAccess.Entities.GeoCoordinate>();
            CreateMap<Package.DataAccess.Entities.GeoCoordinate, BusinessLogic.Entities.GeoCoordinate>();
            
            
            CreateMap<BusinessLogic.Entities.HopArrival, Package.DataAccess.Entities.HopArrival>();
            CreateMap<Package.DataAccess.Entities.HopArrival,BusinessLogic.Entities.HopArrival>();
            
            CreateMap<BusinessLogic.Entities.Parcel, Package.DataAccess.Entities.Parcel>();
            CreateMap<Package.DataAccess.Entities.Parcel,BusinessLogic.Entities.Parcel>();
            
            CreateMap<BusinessLogic.Entities.Recipient, Package.DataAccess.Entities.Recipient>();
            CreateMap<Package.DataAccess.Entities.Recipient,BusinessLogic.Entities.Recipient>();
            
            CreateMap<BusinessLogic.Entities.Warehouse, Package.DataAccess.Entities.Warehouse>();
            CreateMap<Package.DataAccess.Entities.Warehouse, BusinessLogic.Entities.Warehouse>();
            
            CreateMap<BusinessLogic.Entities.WarehouseNextHops, Package.DataAccess.Entities.WarehouseNextHops>();
            CreateMap<Package.DataAccess.Entities.WarehouseNextHops, BusinessLogic.Entities.WarehouseNextHops>();

            CreateMap<BusinessLogic.Entities.Truck, DataAccess.Entities.Truck>();
            CreateMap<DataAccess.Entities.Truck, BusinessLogic.Entities.Truck>();
            
            CreateMap<BusinessLogic.Entities.TransferWarehouse, DataAccess.Entities.Transferwarehouse>();
            CreateMap<DataAccess.Entities.Transferwarehouse, BusinessLogic.Entities.TransferWarehouse>();
            
            CreateMap<BusinessLogic.Entities.Hop, Package.DataAccess.Entities.Hop>()
                .Include<BusinessLogic.Entities.Truck,DataAccess.Entities.Truck>()
                .Include<BusinessLogic.Entities.Warehouse,DataAccess.Entities.Warehouse>()
                .Include<BusinessLogic.Entities.TransferWarehouse,DataAccess.Entities.Transferwarehouse>();

            CreateMap<Package.DataAccess.Entities.Hop, BusinessLogic.Entities.Hop>()
                .Include<DataAccess.Entities.Truck, BusinessLogic.Entities.Truck>()
                .Include<DataAccess.Entities.Warehouse, BusinessLogic.Entities.Warehouse>()
                .Include<DataAccess.Entities.Transferwarehouse, BusinessLogic.Entities.TransferWarehouse>();
            
            
            //ServiceAgents

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