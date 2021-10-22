using System;
using System.Linq;

using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

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

        public ParcelLogic(IValidator<Parcel> parcelValidator, IValidator<TrackingId> trackingIdValidator, IValidator<ReportHop> reportHopValidator, IParcelRepository parcelRepository, IMapper mapper)
        {
            _parcelValidator = parcelValidator;
            _trackingIdValidator = trackingIdValidator;
            _reportHopValidator = reportHopValidator;
            _parcelRepository = parcelRepository;
            _mapper = mapper;
        }

        public Parcel? Track(TrackingId trackingId)
        {
            ValidationResult result = _trackingIdValidator.Validate(trackingId);
            if (!result.IsValid)
            {
                throw new ArgumentException(result.Errors.First().ErrorMessage);
            }

            DataAccess.Entities.Parcel? parcelFromDb = _parcelRepository.GetParcelByTrackingId(trackingId.Id);
            Parcel? newParcel = _mapper.Map<DataAccess.Entities.Parcel, Parcel>(parcelFromDb);
            return newParcel;
        }

        public Parcel? Submit(Parcel parcel)
        {
            ValidationResult result = _parcelValidator.Validate(parcel);

            if (!result.IsValid)
            {
                throw new ArgumentException(result.Errors.First().ErrorMessage);
            }

            DataAccess.Entities.Parcel dataAccessParcel = _mapper.Map<Parcel, DataAccess.Entities.Parcel>(parcel);

            int newID = _parcelRepository.Create(dataAccessParcel);
            DataAccess.Entities.Parcel? parcelFromDb = _parcelRepository.GetById(newID);

            Parcel newParcel = _mapper.Map<DataAccess.Entities.Parcel, Parcel>(parcelFromDb);

            return newParcel;
        }

        public bool? Delivered(TrackingId trackingId)
        {
            ValidationResult result = _trackingIdValidator.Validate(trackingId);
            if (!result.IsValid)
            {
                throw new ArgumentException(result.Errors.First().ErrorMessage);
            }

            DataAccess.Entities.Parcel? parcelFromDb = _parcelRepository.GetParcelByTrackingId(trackingId.Id);
            if (parcelFromDb is null)
            {
                return null;
            }

            bool status = parcelFromDb?.FutureHops.Count == 0;

            return status;
        }

        public bool ReportHop(ReportHop reportHop)
        {
            ValidationResult result = _reportHopValidator.Validate(reportHop);
            if (!result.IsValid)
            {
                throw new ArgumentException(result.Errors.First().ErrorMessage);
            }

            DataAccess.Entities.Parcel? parcel = _parcelRepository.GetParcelByTrackingId(reportHop.TrackingId.Id);

            HopArrival? matchedHop = parcel?.FutureHops.FirstOrDefault(x => x.Code == reportHop.HopCode);
            if (matchedHop == null)
            {
                return false;
            }
            parcel.VisitedHops.Add(matchedHop);
            parcel.FutureHops.Remove(matchedHop);
            
            _parcelRepository.Update(parcel);

            return true;
        }
    }
}