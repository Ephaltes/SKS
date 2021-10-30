using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using FakeItEasy;

using FizzWare.NBuilder;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.DataAccess.Entities;
using NLSL.SKS.Package.DataAccess.Interfaces;

using NUnit.Framework;
using NUnit.Framework.Constraints;

using Parcel = NLSL.SKS.Package.BusinessLogic.Entities.Parcel;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class ParcelManagementBehaviour
    {
        private ParcelLogic _parcelLogic;
        private IValidator<Parcel> _parcelValidator;
        private IValidator<ReportHop> _reportHop;
        private IValidator<TrackingId> _trackingIdValidator;
        private IMapper _mapper;
        private IParcelRepository _parcelRepository;
        [SetUp]
        public void Setup()
        {
            _parcelValidator = A.Fake<IValidator<Parcel>>();
            _trackingIdValidator = A.Fake<IValidator<TrackingId>>();
            _reportHop = A.Fake<IValidator<ReportHop>>();

            _parcelRepository = A.Fake<IParcelRepository>();
            _mapper = A.Fake<IMapper>();
            
            _parcelLogic = new ParcelLogic(_parcelValidator, _trackingIdValidator, _reportHop,_parcelRepository,_mapper);
        }

        [Test]
        public void Transition_ValidParcel_ReturnsParcel()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _parcelValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            Parcel? result = _parcelLogic.Submit(null);

            result.Should().NotBeNull();
        }

        [Test]
        public void Transition_InvalidParcel_ArgumentException()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);
            Action act;
            
            A.CallTo(() => _parcelValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            act = ()=> _parcelLogic.Submit(null);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Track_ValidTrackingId_ReturnsParcel()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments().Returns(validationResult);
            A.CallTo(() => _parcelRepository.GetParcelByTrackingId(null)).WithAnyArguments().Returns(new DataAccess.Entities.Parcel());
            A.CallTo(_mapper).Where(call => call.Method.Name == "Map").WithNonVoidReturnType().Returns(new Parcel());


            Parcel? result = _parcelLogic.Track(new TrackingId("ABCABC123"));

            result.Should().NotBeNull();
        }

        [Test]
        public void Track_InvalidTrackingId_ArgumentException()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);
            Action act;
            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            act = () => _parcelLogic.Track(null);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Submit_ValidParcel_ReturnsParcel()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _parcelValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            Parcel? result = _parcelLogic.Submit(null);

            result.Should().NotBeNull();
        }

        [Test]
        public void Submit_InvalidParcel_Arguemntexception()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);
            Action act;
            A.CallTo(() => _parcelValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            act = () => _parcelLogic.Submit(null);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Delivered_ValidTrackingId_ReturnsTrue()
        {
            ValidationResult validationResult = new ValidationResult();
            
            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments().Returns(validationResult);
            A.CallTo(() => _parcelRepository.GetParcelByTrackingId(null)).WithAnyArguments().Returns(new Package.DataAccess.Entities.Parcel(){FutureHops = new()});
            
            
            bool? result = _parcelLogic.Delivered(new TrackingId("ABCABC123"));

            result.Should().BeTrue();
        }

        [Test]
        public void Delivered_InvalidTrackingId_ArgumentException()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);

            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments().Returns(validationResult);
            
            Action act = () => _parcelLogic.Delivered(null);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void ReportHop_ValidHop_ReturnsTrue()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _reportHop.Validate(null)).WithAnyArguments().Returns(validationResult);
            A.CallTo(() => _parcelRepository.GetParcelByTrackingId(null)).WithAnyArguments().Returns(new Package.DataAccess.Entities.Parcel()
                                                                                                     {
                                                                                                         FutureHops = new()
                                                                                                                      {
                                                                                                                          new Package.DataAccess.Entities.HopArrival()
                                                                                                                          {
                                                                                                                              Code="Warehouse123"
                                                                                                                          }
                                                                                                                      },
                                                                                                         VisitedHops = new(),
                                                                                                         TrackingId = "ABCABC123",
                                                                                                     });
            
            bool result = _parcelLogic.ReportHop(new ReportHop(){TrackingId = new TrackingId("ABCABC123"),HopCode = "Warehouse123"});

            result.Should().BeTrue();
        }

        [Test]
        public void ReportHop_InvalidHop_ArgumentException()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);
            Action act;
            A.CallTo(() => _reportHop.Validate(null)).WithAnyArguments().Returns(validationResult);

            act = () => _parcelLogic.ReportHop(null);

            act.Should().Throw<ArgumentException>();
        }
    }
}