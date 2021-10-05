using System.Collections.Generic;
using System.Linq;

using FakeItEasy;

using FizzWare.NBuilder;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using NLSL.SKS.Package.BusinessLogic.Entities;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class ParcelManagementBehaviour
    {
        private ParcelManagement _parcelManagement;
        private IValidator<Parcel> _parcelValidator;
        private IValidator<ReportHop> _reportHop;
        private IValidator<TrackingId> _trackingIdValidator;
        [SetUp]
        public void Setup()
        {
            _parcelValidator = A.Fake<IValidator<Parcel>>();
            _trackingIdValidator = A.Fake<IValidator<TrackingId>>();
            _reportHop = A.Fake<IValidator<ReportHop>>();

            _parcelManagement = new ParcelManagement(_parcelValidator, _trackingIdValidator, _reportHop);
        }

        [Test]
        public void Transition_ValidParcel_ReturnsParcel()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _parcelValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            Parcel? result = _parcelManagement.Transition(null);

            result.Should().NotBeNull();
        }

        [Test]
        public void Transition_InvalidParcel_ReturnsNull()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);

            A.CallTo(() => _parcelValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            Parcel? result = _parcelManagement.Transition(null);

            result.Should().BeNull();
        }

        [Test]
        public void Track_ValidTrackingId_ReturnsParcel()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            Parcel? result = _parcelManagement.Track(null);

            result.Should().NotBeNull();
        }

        [Test]
        public void Track_InvalidTrackingId_ReturnsNull()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);

            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            Parcel? result = _parcelManagement.Track(null);

            result.Should().BeNull();
        }

        [Test]
        public void Submit_ValidParcel_ReturnsParcel()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _parcelValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            Parcel? result = _parcelManagement.Submit(null);

            result.Should().NotBeNull();
        }

        [Test]
        public void Submit_InvalidParcel_ReturnsNull()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);

            A.CallTo(() => _parcelValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            Parcel? result = _parcelManagement.Submit(null);

            result.Should().BeNull();
        }

        [Test]
        public void Delivered_ValidTrackingId_ReturnsTrue()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            bool? result = _parcelManagement.Delivered(null);

            result.Should().BeTrue();
        }

        [Test]
        public void Delivered_InvalidTrackingId_ReturnsNull()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);

            A.CallTo(() => _trackingIdValidator.Validate(null)).WithAnyArguments().Returns(validationResult);

            bool? result = _parcelManagement.Delivered(null);

            result.Should().BeNull();
        }

        [Test]
        public void ReportHop_ValidHop_ReturnsTrue()
        {
            ValidationResult validationResult = new ValidationResult();

            A.CallTo(() => _reportHop.Validate(null)).WithAnyArguments().Returns(validationResult);

            bool result = _parcelManagement.ReportHop(null);

            result.Should().BeTrue();
        }

        [Test]
        public void ReportHop_InvalidHop_ReturnsFalse()
        {
            IReadOnlyCollection<ValidationFailure> validationFailures = Builder<ValidationFailure>.CreateListOfSize(2).Build().ToList();
            ValidationResult validationResult = new ValidationResult(validationFailures);

            A.CallTo(() => _reportHop.Validate(null)).WithAnyArguments().Returns(validationResult);

            bool result = _parcelManagement.ReportHop(null);

            result.Should().BeFalse();
        }
    }
}