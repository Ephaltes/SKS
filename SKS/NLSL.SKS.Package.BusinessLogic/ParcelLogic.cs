using System.Linq;

using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.BusinessLogic.CustomExceptions;
using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.DataAccess.Interfaces;
using NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos;
using NLSL.SKS.Package.ServiceAgents.Exceptions;

using HopArrival = NLSL.SKS.Package.DataAccess.Entities.HopArrival;

namespace NLSL.SKS.Package.BusinessLogic
{
    public class ParcelLogic : IParcelLogic
    {
        private readonly ILogger<ParcelLogic> _logger;
        private readonly IMapper _mapper;
        private readonly IParcelRepository _parcelRepository;
        private readonly IValidator<Parcel> _parcelValidator;
        private readonly IValidator<ReportHop> _reportHopValidator;
        private readonly IValidator<TrackingId> _trackingIdValidator;

        public ParcelLogic(IValidator<Parcel> parcelValidator, IValidator<TrackingId> trackingIdValidator, IValidator<ReportHop> reportHopValidator, IParcelRepository parcelRepository, IMapper mapper, ILogger<ParcelLogic> logger)
        {
            _parcelValidator = parcelValidator;
            _trackingIdValidator = trackingIdValidator;
            _reportHopValidator = reportHopValidator;
            _parcelRepository = parcelRepository;
            _mapper = mapper;
            _logger = logger;
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
                throw new BusinessLayerExceptionBase("Error in Validation", e);
            }
            catch (BusinessLayerDataNotFoundException e)
            {
                throw new BusinessLayerExceptionBase("No Data found", e);
            }
            catch (DataAccessExceptionbase e)
            {
                throw new BusinessLayerExceptionBase("Error in DataAccessLayer", e);
            }
            catch (ServiceAgentsExceptionBase e)
            {
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
                throw new BusinessLayerExceptionBase("Error in Validation", e);
            }
            catch (BusinessLayerDataNotFoundException e)
            {
                throw new BusinessLayerExceptionBase("No Data found", e);
            }
            catch (DataAccessExceptionbase e)
            {
                throw new BusinessLayerExceptionBase("Error in DataAccessLayer", e);
            }
            catch (ServiceAgentsExceptionBase e)
            {
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

                bool status = parcelFromDb?.FutureHops.Count == 0;
                _logger.LogDebug("parcel delivery status complete");
                return status;
            }
            catch (BusinessLayerValidationException e)
            {
                throw new BusinessLayerExceptionBase("Error in Validation", e);
            }
            catch (BusinessLayerDataNotFoundException e)
            {
                throw new BusinessLayerExceptionBase("No Data found", e);
            }
            catch (DataAccessExceptionbase e)
            {
                throw new BusinessLayerExceptionBase("Error in DataAccessLayer", e);
            }
            catch (ServiceAgentsExceptionBase e)
            {
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

                HopArrival? matchedHop = parcel?.FutureHops.FirstOrDefault(x => x.Code == reportHop.HopCode);
                if (matchedHop == null)
                {
                    _logger.LogInformation("hop does not match future hops");
                    throw new BusinessLayerDataNotFoundException(result.Errors.First().ErrorMessage);
                }

                parcel.VisitedHops.Add(matchedHop);
                parcel.FutureHops.Remove(matchedHop);

                _parcelRepository.Update(parcel);
                _logger.LogDebug("report hop complete");
                return true;
            }
            catch (BusinessLayerValidationException e)
            {
                throw new BusinessLayerExceptionBase("Error in Validation", e);
            }
            catch (BusinessLayerDataNotFoundException e)
            {
                throw new BusinessLayerExceptionBase("No Data found", e);
            }
            catch (DataAccessExceptionbase e)
            {
                throw new BusinessLayerExceptionBase("Error in DataAccessLayer", e);
            }
            catch (ServiceAgentsExceptionBase e)
            {
                throw new BusinessLayerExceptionBase("Error in ServiceAgents", e);
            }
        }
    }
}