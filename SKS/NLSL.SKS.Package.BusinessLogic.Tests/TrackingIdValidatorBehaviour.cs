using FluentValidation.TestHelper;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Validators;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class TrackingIdValidatorBehaviour
    {
        private TrackingIdValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new TrackingIdValidator();
        }
        [Test]
        public void TrackingIdValidator_IdIsNull_ValidationError()
        {
            TrackingId model = new TrackingId(null);
            TestValidationResult<TrackingId> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Id);
        }
        [Test]
        public void TrackingIdValidator_IdIsValid_Success()
        {
            TrackingId model = new TrackingId("ABCDEFGHI");
            TestValidationResult<TrackingId> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Id);
        }
        [Test]
        public void TrackingIdValidator_IdIsNotValid_ValidationError()
        {
            TrackingId model = new TrackingId("ABCDEFGH");
            TestValidationResult<TrackingId> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Id);
        }
    }
}