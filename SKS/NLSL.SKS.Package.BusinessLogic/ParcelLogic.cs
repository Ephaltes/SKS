using System;
using System.Linq;

using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.DataAccess.Interfaces;

using HopArrival = NLSL.SKS.Package.DataAccess.Entities.HopArrival;

namespace NLSL.SKS.Package.BusinessLogic
{
    public class ParcelLogic : IParcelLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _parcelRepository;
        private readonly IValidator<Parcel> _parcelValidator;
        private readonly IValidator<ReportHop> _reportHopValidator;
        private readonly IValidator<TrackingId> _trackingIdValidator;
        private readonly ILogger<ParcelLogic> _logger;
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
            _logger.LogDebug("starting, track a parcel");
            
            _logger.LogDebug("validating trackingID");
            ValidationResult result = _trackingIdValidator.Validate(trackingId);
            if (!result.IsValid)
            {
                _logger.LogWarning("validation error for parcel");
                throw new ArgumentException(result.Errors.First().ErrorMessage);
            }
            
            DataAccess.Entities.Parcel? parcelFromDb = _parcelRepository.GetParcelByTrackingId(trackingId.Id);
            
            Parcel? newParcel = _mapper.Map<DataAccess.Entities.Parcel, Parcel>(parcelFromDb);
            
            _logger.LogDebug("track a parcel complete");
            return newParcel;
        }

        public Parcel? Submit(Parcel parcel)
        {
            _logger.LogDebug("starting, submit a new parcel");
            
            _logger.LogDebug("validating parcel");
            ValidationResult result = _parcelValidator.Validate(parcel);

            if (!result.IsValid)
            {
                _logger.LogWarning("validation error for parcel");
                throw new ArgumentException(result.Errors.First().ErrorMessage);
            }

            DataAccess.Entities.Parcel dataAccessParcel = _mapper.Map<Parcel, DataAccess.Entities.Parcel>(parcel);

            int newID = _parcelRepository.Create(dataAccessParcel);
            DataAccess.Entities.Parcel? parcelFromDb = _parcelRepository.GetById(newID);
            _logger.LogDebug("parcel successfully created");
            
            
            
            Parcel newParcel = _mapper.Map<DataAccess.Entities.Parcel, Parcel>(parcelFromDb);
            _logger.LogDebug("submit a new parcel complete");
            return newParcel;
        }

        public bool? Delivered(TrackingId trackingId)
        {
            _logger.LogDebug("starting, parcel delivery status");
            
            _logger.LogDebug("validating trackingId");
            ValidationResult result = _trackingIdValidator.Validate(trackingId);
            if (!result.IsValid)
            {
                _logger.LogWarning("validation error for trackingId");
                throw new ArgumentException(result.Errors.First().ErrorMessage);
            }

            DataAccess.Entities.Parcel? parcelFromDb = _parcelRepository.GetParcelByTrackingId(trackingId.Id);
            if (parcelFromDb is null)
            {
                _logger.LogInformation("no parcel found in db");
                return null;
            }

            bool status = parcelFromDb?.FutureHops.Count == 0;
            _logger.LogDebug("parcel delivery status complete");
            return status;
        }

        public bool ReportHop(ReportHop reportHop)
        {
            _logger.LogDebug("started report hop");
            
            _logger.LogDebug("validating reportHop");
            ValidationResult result = _reportHopValidator.Validate(reportHop);
            if (!result.IsValid)
            {
                _logger.LogWarning("validation error for reportHop");
                throw new ArgumentException(result.Errors.First().ErrorMessage);
            }

            DataAccess.Entities.Parcel? parcel = _parcelRepository.GetParcelByTrackingId(reportHop.TrackingId.Id);

            HopArrival? matchedHop = parcel?.FutureHops.FirstOrDefault(x => x.Code == reportHop.HopCode);
            if (matchedHop == null)
            {
                _logger.LogInformation("hop does not match future hops");
                return false;
            }

            parcel.VisitedHops.Add(matchedHop);
            parcel.FutureHops.Remove(matchedHop);

            _parcelRepository.Update(parcel);
            _logger.LogDebug("report hop complete");
            return true;
        }
    }
}