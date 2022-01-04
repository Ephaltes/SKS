using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.Extensions.Logging;

using NetTopologySuite.Geometries;

using NLSL.SKS.Package.BusinessLogic.CustomExceptions;
using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Entities.Enums;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.DataAccess.Entities;
using NLSL.SKS.Package.DataAccess.Interfaces;
using NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos;
using NLSL.SKS.Package.ServiceAgents.Entities;
using NLSL.SKS.Package.ServiceAgents.Exceptions;
using NLSL.SKS.Package.ServiceAgents.Interface;
using NLSL.SKS.Package.WebhookManager.Interfaces;

using Hop = NLSL.SKS.Package.DataAccess.Entities.Hop;
using HopArrival = NLSL.SKS.Package.BusinessLogic.Entities.HopArrival;
using Parcel = NLSL.SKS.Package.BusinessLogic.Entities.Parcel;
using Truck = NLSL.SKS.Package.DataAccess.Entities.Truck;
using Warehouse = NLSL.SKS.Package.DataAccess.Entities.Warehouse;
//using NLSL.SKS.Package.Services.DTOs;

namespace NLSL.SKS.Package.BusinessLogic
{
    public class ParcelLogic : IParcelLogic
    {
        private readonly List<Hop> _endTruckToWarehouse = new List<Hop>();
        private readonly IGeoCodingAgent _geoCodingAgent;
        private readonly IHttpAgent _httpAgent;
        private readonly ILogger<ParcelLogic> _logger;
        private readonly IMapper _mapper;

        private readonly IParcelRepository _parcelRepository;
        private readonly IValidator<Parcel> _parcelValidator;
        private readonly IValidator<ReportHop> _reportHopValidator;

        private readonly List<Hop> _startTruckToWarehouse = new List<Hop>();
        private readonly IValidator<TrackingId> _trackingIdValidator;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IWebHookManager _webHookManager;

        public ParcelLogic(IValidator<Parcel> parcelValidator,
            IValidator<TrackingId> trackingIdValidator,
            IValidator<ReportHop> reportHopValidator,
            IParcelRepository parcelRepository,
            IMapper mapper,
            ILogger<ParcelLogic> logger,
            IGeoCodingAgent geoCodingAgent,
            IWarehouseRepository warehouseRepository, IHttpAgent httpAgent, IWebHookManager webHookManager)
        {
            _parcelValidator = parcelValidator;
            _trackingIdValidator = trackingIdValidator;
            _reportHopValidator = reportHopValidator;
            _parcelRepository = parcelRepository;
            _mapper = mapper;
            _logger = logger;
            _geoCodingAgent = geoCodingAgent;
            _warehouseRepository = warehouseRepository;
            _httpAgent = httpAgent;
            _webHookManager = webHookManager;
        }

        public Parcel? Track(TrackingId trackingId)
        {
            try
            {
                _logger.LogDebug("starting, track a parcel");

                _logger.LogDebug("validating trackingID");
                ValidationResult result = _trackingIdValidator.Validate(trackingId);
                if (!result.IsValid)
                {
                    _logger.LogWarning("validation error for parcel");

                    throw new BusinessLayerValidationException(result.Errors.First().ErrorMessage);
                }

                DataAccess.Entities.Parcel? parcelFromDb = _parcelRepository.GetParcelByTrackingId(trackingId.Id);
                if (parcelFromDb is null)
                {
                    _logger.LogWarning("no parcel for tracking ID");

                    throw new BusinessLayerDataNotFoundException("no parcel for tracking ID");
                }

                Parcel? newParcel = _mapper.Map<DataAccess.Entities.Parcel, Parcel>(parcelFromDb);

                _logger.LogDebug("track a parcel complete");

                return newParcel;
            }
            catch (BusinessLayerValidationException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in Validation", e);
            }
            catch (BusinessLayerDataNotFoundException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("No Data found", e);
            }
            catch (DataAccessExceptionBase e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in DataAccessLayer", e);
            }
            catch (ServiceAgentsExceptionBase e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in ServiceAgents", e);
            }
        }

        public Parcel? Submit(Parcel parcel)
        {
            try
            {
                _logger.LogDebug("starting, submit a new parcel");

                _logger.LogDebug("validating parcel");
                ValidationResult result = _parcelValidator.Validate(parcel);

                if (!result.IsValid)
                {
                    _logger.LogWarning("validation error for parcel");

                    throw new BusinessLayerValidationException(result.Errors.First().ErrorMessage);
                }

                if (string.IsNullOrEmpty(parcel.TrackingId) ||
                    _parcelRepository.GetParcelByTrackingId(parcel.TrackingId) is not null)
                    parcel.TrackingId = _parcelRepository.GenerateTrackingId();

                parcel.FutureHops = GetFutureHopsForPackage(parcel);

                parcel.State = StateEnum.Pickup;

                _webHookManager.ParcelStateChanged(_mapper.Map<Parcel, WebhookManager.Entities.Parcel>(parcel));

                DataAccess.Entities.Parcel dataAccessParcel = _mapper.Map<Parcel, DataAccess.Entities.Parcel>(parcel);

                int newID = _parcelRepository.Create(dataAccessParcel);
                DataAccess.Entities.Parcel? parcelFromDb = _parcelRepository.GetById(newID);

                if (parcelFromDb is null)
                {
                    throw new BusinessLayerDataNotFoundException("created parcel not found");
                }

                _logger.LogDebug("parcel successfully created");


                Parcel newParcel = _mapper.Map<DataAccess.Entities.Parcel, Parcel>(parcelFromDb);
                _logger.LogDebug("submit a new parcel complete");

                return newParcel;
            }
            catch (BusinessLayerValidationException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in Validation", e);
            }
            catch (BusinessLayerDataNotFoundException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("No Data found", e);
            }
            catch (DataAccessExceptionBase e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in DataAccessLayer", e);
            }
            catch (ServiceAgentsExceptionBase e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in ServiceAgents", e);
            }
        }

        public bool? Delivered(TrackingId trackingId)
        {
            try
            {
                _logger.LogDebug("starting, parcel delivery status");

                _logger.LogDebug("validating trackingId");
                ValidationResult result = _trackingIdValidator.Validate(trackingId);
                if (!result.IsValid)
                {
                    _logger.LogWarning("validation error for trackingId");

                    throw new BusinessLayerValidationException(result.Errors.First().ErrorMessage);
                }

                DataAccess.Entities.Parcel? parcelFromDb = _parcelRepository.GetParcelByTrackingId(trackingId.Id);
                if (parcelFromDb is null)
                {
                    _logger.LogInformation("no parcel found in db");

                    throw new BusinessLayerDataNotFoundException("no parcel found in db with trackingID");
                }

                if (parcelFromDb?.FutureHops.Count > 0)
                    return false;

                parcelFromDb.State = DataAccess.Entities.Enums.StateEnum.Delivered;
                _webHookManager.ParcelStateChanged(_mapper.Map<DataAccess.Entities.Parcel, WebhookManager.Entities.Parcel>(parcelFromDb));

                _parcelRepository.Update(parcelFromDb);

                _logger.LogDebug("parcel delivery status complete");

                return true;
            }
            catch (BusinessLayerValidationException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in Validation", e);
            }
            catch (BusinessLayerDataNotFoundException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("No Data found", e);
            }
            catch (DataAccessExceptionBase e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in DataAccessLayer", e);
            }
            catch (ServiceAgentsExceptionBase e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in ServiceAgents", e);
            }
        }

        public bool ReportHop(ReportHop reportHop)
        {
            try
            {
                _logger.LogDebug("started report hop");

                _logger.LogDebug("validating reportHop");
                ValidationResult result = _reportHopValidator.Validate(reportHop);
                if (!result.IsValid)
                {
                    _logger.LogWarning("validation error for reportHop");

                    throw new BusinessLayerValidationException(result.Errors.First().ErrorMessage);
                }

                DataAccess.Entities.Parcel? parcel = _parcelRepository.GetParcelByTrackingId(reportHop.TrackingId.Id);
                if (parcel is null)
                {
                    _logger.LogInformation("parcel not found");

                    throw new BusinessLayerDataNotFoundException(result.Errors.First().ErrorMessage);
                }

                DataAccess.Entities.HopArrival? matchedHop = parcel?.FutureHops.FirstOrDefault(x => x.Code == reportHop.HopCode);
                if (matchedHop == null)
                {
                    _logger.LogInformation("hop does not match future hops");

                    throw new BusinessLayerDataNotFoundException(result.Errors.First().ErrorMessage);
                }


                //throw new BusinessLayerExceptionBase(JsonConvert.SerializeObject(matchedHop));
                switch (matchedHop.Hop)
                {
                    case Truck:
                        parcel.State = DataAccess.Entities.Enums.StateEnum.InTruckDelivery;

                        break;
                    case Warehouse:
                        parcel.State = DataAccess.Entities.Enums.StateEnum.InTransport;

                        break;
                    case Transferwarehouse c:
                        if (parcel.FutureHops.Count != 0)
                        {
                            parcel.State = DataAccess.Entities.Enums.StateEnum.InTransport;

                            break;
                        }

                        _httpAgent.SendParcelToLogisticPartnerPost(c.LogisticsPartnerUrl, parcel);

                        parcel.State = DataAccess.Entities.Enums.StateEnum.Transferred;

                        break;
                    default:
                        _logger.LogDebug("Hoptype not recognised");

                        throw new BusinessLayerExceptionBase("Hoptype not recognised");
                }

                parcel.VisitedHops.Add(matchedHop);
                parcel.FutureHops.Remove(matchedHop);
                _parcelRepository.Update(parcel);
                WebhookManager.Entities.Parcel webHookParcel = _mapper.Map<DataAccess.Entities.Parcel, WebhookManager.Entities.Parcel>(parcel);
                _webHookManager.ParcelStateChanged(webHookParcel);


                _logger.LogDebug("report hop complete");

                return true;
            }
            catch (BusinessLayerValidationException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in Validation", e);
            }
            catch (BusinessLayerDataNotFoundException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("No Data found", e);
            }
            catch (DataAccessExceptionBase e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in DataAccessLayer", e);
            }
            catch (ServiceAgentsExceptionBase e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new BusinessLayerExceptionBase("Error in ServiceAgents", e);
            }
        }

        private List<HopArrival> GetFutureHopsForPackage(Parcel parcel)
        {
            Address senderAddress = _mapper.Map<Address>(parcel.Sender);
            Address receiverAddress = _mapper.Map<Address>(parcel.Recipient);

            GeoCoordinates senderGeoCoordinates = _geoCodingAgent.GetGeoCoordinates(senderAddress).First();
            GeoCoordinates receiverGeoCoordinates = _geoCodingAgent.GetGeoCoordinates(receiverAddress).First();

            Point senderPoint = new Point(senderGeoCoordinates.Longitude, senderGeoCoordinates.Latitude);
            Point receiverPoint = new Point(receiverGeoCoordinates.Longitude, receiverGeoCoordinates.Latitude);

            senderPoint.SRID = 4326;
            receiverPoint.SRID = 4326;

            Hop startHop = _warehouseRepository.GetHopForPoint(senderPoint);
            Hop endHop = _warehouseRepository.GetHopForPoint(receiverPoint);

            List<Hop> path = GetPathForTrucks(startHop, endHop);
            path.Insert(0, startHop);
            path.Add(endHop);

            return path.Select(hop => new HopArrival
                                      {
                                          Code = hop.Code,
                                          Description = hop.Description
                                      })
                .ToList();
        }
        private List<Hop> GetPathForTrucks(Hop startHop, Hop endHop)
        {
            while (true)
            {
                Hop startParentHop = _warehouseRepository.GetParentOfHopByCode(startHop.Code);
                Hop endParentHop = _warehouseRepository.GetParentOfHopByCode(endHop.Code);

                if (startParentHop.Code == endParentHop.Code)
                {
                    List<Hop> path = new List<Hop>();
                    path.AddRange(_endTruckToWarehouse);
                    _startTruckToWarehouse.Reverse();
                    path.Add(endParentHop);
                    path.AddRange(_startTruckToWarehouse);

                    return path;
                }

                startHop = startParentHop;

                endHop = endParentHop;
            }
        }
    }
}