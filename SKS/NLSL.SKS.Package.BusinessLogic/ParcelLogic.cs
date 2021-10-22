using System;

using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using NLSL.SKS.Pacakge.DataAccess.Interfaces;
using NLSL.SKS.Pacakge.DataAccess.Sql;
using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.BusinessLogic.Validators;

namespace NLSL.SKS.Package.BusinessLogic
{
    public class ParcelLogic : IParcelLogic
    {
        private IValidator<Parcel> _parcelValidator;
        private IValidator<TrackingId> _trackingIdValidator;
        private IValidator<ReportHop> _reportHopValidator;
        private IParcelRepository _parcelRepository;
        private readonly IMapper _mapper;
        public ParcelLogic(IValidator<Parcel> parcelValidator,IValidator<TrackingId> trackingIdValidator, IValidator<ReportHop> reportHopValidator, IParcelRepository parcelRepository, IMapper mapper)
        {
            _parcelValidator = parcelValidator;
            _trackingIdValidator = trackingIdValidator;
            _reportHopValidator = reportHopValidator;
            _parcelRepository = parcelRepository;
            _mapper = mapper;
        }
        public Parcel? Transition(Parcel parcel)
        {
            ValidationResult result = _parcelValidator.Validate(parcel);

            if (!result.IsValid)
            {
                return null;
            }

            return null;
        }
        public Parcel? Track(TrackingId trackingId)
        {
            ValidationResult result = _trackingIdValidator.Validate(trackingId);

            _parcelRepository.GetParcelByTrackingId("2");

            return result.IsValid ? new Parcel() : null;
        }
        public Parcel? Submit(Parcel parcel)
        {

            ValidationResult result = _parcelValidator.Validate(parcel);

            return result.IsValid ? new Parcel() : null;
        }
        public bool? Delivered(TrackingId trackingId)
        {
            ValidationResult result = _trackingIdValidator.Validate(trackingId);

            return result.IsValid ? true : null;
        }
        public bool ReportHop(ReportHop reportHop)
        {
            ValidationResult result = _reportHopValidator.Validate(reportHop);

            return result.IsValid;
        }
    }
}