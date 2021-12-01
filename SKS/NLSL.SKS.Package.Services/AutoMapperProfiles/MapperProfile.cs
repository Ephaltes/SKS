using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AutoMapper;

using NetTopologySuite.Geometries;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.DataAccess.Entities;
using NLSL.SKS.Package.ServiceAgents.Entities;
using NLSL.SKS.Package.Services.Converter;
using NLSL.SKS.Package.WebhookManager.Entities;
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
            CreateMap<BusinessLogic.Entities.Parcel, DTOs.TrackingInformation>();//.ForMember(p => p.FutureHops, p => p.MapFrom<BusinessLogic.Entities.HopArrival, DTOs.HopArrival>(p => p.FutureHops));
            
            CreateMap<DTOs.NewParcelInfo, BusinessLogic.Entities.Parcel>();
            CreateMap<BusinessLogic.Entities.Parcel, DTOs.NewParcelInfo>();

            CreateMap<DTOs.Transferwarehouse, BusinessLogic.Entities.TransferWarehouse>()
                .ForMember(p => p.RegionGeometry,
                p => p.ConvertUsing<GeometryConverter, string>(p => p.RegionGeoJson));
            CreateMap<BusinessLogic.Entities.TransferWarehouse, DTOs.Transferwarehouse>()
                .ForMember(p => p.RegionGeoJson,
                p => p.ConvertUsing<GeometryConverter, Geometry>(p => p.RegionGeometry));
            
            CreateMap<DTOs.Truck, BusinessLogic.Entities.Truck>()
                .ForMember(p => p.RegionGeometry,
                p => p.ConvertUsing<GeometryConverter, string>(p => p.RegionGeoJson));
            CreateMap<BusinessLogic.Entities.Truck, DTOs.Truck>()
                .ForMember(p => p.RegionGeoJson,
                p => p.ConvertUsing<GeometryConverter, Geometry>(p => p.RegionGeometry));

            CreateMap<DTOs.Warehouse, BusinessLogic.Entities.Warehouse>();
            CreateMap<BusinessLogic.Entities.Warehouse, DTOs.Warehouse>();
            
            CreateMap<DTOs.GeoCoordinate, BusinessLogic.Entities.GeoCoordinate>()
                .ForMember(dest => dest.Location,
                    opt => 
                        opt.MapFrom(src => new Point(src.Lon.Value, src.Lat.Value){SRID = 4326}));
            
            CreateMap<BusinessLogic.Entities.GeoCoordinate, DTOs.GeoCoordinate>()
                .ForMember(dest => dest.Lat,
                    opt => 
                        opt.MapFrom(src => src.Location.Y))
                .ForMember(dest => dest.Lon,
                    opt => 
                        opt.MapFrom(src => src.Location.X));
            
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
            
            CreateMap<DTOs.HopArrival, BusinessLogic.Entities.HopArrival>();
            CreateMap<BusinessLogic.Entities.HopArrival,DTOs.HopArrival>();
            
            // BL to DAL

            CreateMap<BusinessLogic.Entities.GeoCoordinate, Package.DataAccess.Entities.GeoCoordinate>();
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
                        src => src.PostalCode));
            
            CreateMap<GeocodeResponse, GeoCoordinates>()
                .ForMember(dest => dest.Address,
                    opt => opt.MapFrom(
                        src => src.DisplayName));

            CreateMap<BusinessLogic.Entities.Recipient, Address>();
            CreateMap<Address, BusinessLogic.Entities.Recipient>();


            // Webhooks
            CreateMap<WebhookManager.Entities.WebHook, DataAccess.Entities.WebHook>();
            CreateMap<WebhookManager.Entities.WebHook, WebhookManager.Entities.WebhookResponse>();
            CreateMap<WebhookManager.Entities.Parcel,WebhookManager.Entities.WebhookMessage>();


            CreateMap<BusinessLogic.Entities.WebHook, WebhookManager.Entities.WebHook>();
            CreateMap<WebhookManager.Entities.WebhookResponse, BusinessLogic.Entities.WebhookResponse>();
            
            CreateMap<BusinessLogic.Entities.WebhookResponse, DTOs.WebhookResponse>();

            CreateMap<BusinessLogic.Entities.Parcel, WebhookManager.Entities.Parcel>();
            CreateMap<DataAccess.Entities.Parcel, WebhookManager.Entities.Parcel>(); 
            CreateMap<BusinessLogic.Entities.Recipient, WebhookManager.Entities.Recipient>(); 
            CreateMap<BusinessLogic.Entities.HopArrival, WebhookManager.Entities.HopArrival>();
            CreateMap<DataAccess.Entities.WebHook, WebhookManager.Entities.WebhookResponse>();

        }
    }
}