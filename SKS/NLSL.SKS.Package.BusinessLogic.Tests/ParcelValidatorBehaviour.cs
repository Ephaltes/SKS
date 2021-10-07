using FluentValidation;
using FluentValidation.TestHelper;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Validators;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class ParcelValidatorBehaviour
    {
        private ParcelValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new ParcelValidator();
            validator.CascadeMode = CascadeMode.Continue;
        }
        [Test]
        public void ParcelValidator_RecipientIsNull_ValidationError()
        {
            Parcel model = new Parcel { Recipient = null };
            TestValidationResult<Parcel> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Recipient);
        }
        [Test]
        public void ParcelValidator_SenderIsNull_ValidationError()
        {
            Parcel model = new Parcel { Sender = null };
            TestValidationResult<Parcel> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Sender);
        }
        [Test]
        public void ParcelValidator_FutureHopsIsNull_ValidationError()
        {
            Parcel model = new Parcel { FutureHops = null };
            TestValidationResult<Parcel> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.FutureHops);
        }
        [Test]
        public void ParcelValidator_StateIsNull_Success()
        {
            Parcel model = new Parcel { State = null };
            TestValidationResult<Parcel> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.State);
        }
        [Test]
        public void ParcelValidator_TackingIdIsNull_Success()
        {
            Parcel model = new Parcel { TrackingId = null };
            TestValidationResult<Parcel> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.TrackingId);
        }
        [Test]
        public void ParcelValidator_VisitedHopsIsNull_ValidationError()
        {
            Parcel model = new Parcel { VisitedHops = null };
            TestValidationResult<Parcel> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.VisitedHops);
        }
        [Test]
        public void ParcelValidator_WeightIsNull_ValidationError()
        {
            Parcel model = new Parcel { Weight = null };
            TestValidationResult<Parcel> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Weight);
        }


        [Test]
        public void ParcelValidator_WeightLessThan0f_ValidatioError()
        {
            Parcel model = new Parcel { Weight = -0.1f };
            TestValidationResult<Parcel> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Weight);
        }

        [Test]
        public void ParcelValidator_WeightGreaterThan0f_Success()
        {
            Parcel model = new Parcel { Weight = 0.1f };
            TestValidationResult<Parcel> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Weight);
        }
        [Test]
        public void ParcelValidator_TackingIdMatchesRegex_Success()
        {
            Parcel model = new Parcel { TrackingId = "ABCDEFGH" };
            TestValidationResult<Parcel> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.TrackingId);
        }
        [Test]
        public void ParcelValidator_TackingIdDoesNotMatchesRegex_ValidationError()
        {
            Parcel model = new Parcel { TrackingId = "ABCDEFH" };
            TestValidationResult<Parcel> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.TrackingId);
        }
    }
}