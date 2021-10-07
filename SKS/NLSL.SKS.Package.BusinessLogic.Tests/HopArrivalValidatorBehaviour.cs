using FluentValidation;
using FluentValidation.TestHelper;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Validators;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class HopArrivalValidatorBehaviour
    {
        private HopArrivalValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new HopArrivalValidator();
            validator.CascadeMode = CascadeMode.Continue;
        }
        [Test]
        public void GeoCoordinateValidator_DateTimeIsNull_ValidationError()
        {
            HopArrival model = new HopArrival { DateTime = null };
            TestValidationResult<HopArrival> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.DateTime);
        }
        [Test]
        public void GeoCoordinateValidator_DescriptionIsNull_ValidationError()
        {
            HopArrival model = new HopArrival { Description = null };
            TestValidationResult<HopArrival> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Description);
        }
        [Test]
        public void GeoCoordinateValidator_CodeIsNull_ValidationError()
        {
            HopArrival model = new HopArrival { Code = null };
            TestValidationResult<HopArrival> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Code);
        }
        [Test]
        public void GeoCoordinateValidator_CodeMatchesRegexLowEndCase_Success()
        {
            HopArrival model = new HopArrival { Code = "ABCD1" };
            TestValidationResult<HopArrival> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Code);
        }
        [Test]
        public void GeoCoordinateValidator_CodeMatchesRegexHighEndCase_Success()
        {
            HopArrival model = new HopArrival { Code = "ABCD1234" };
            TestValidationResult<HopArrival> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Code);
        }
        [Test]
        public void GeoCoordinateValidator_CodeDoesNotMatchRegex_ValidationError()
        {
            HopArrival model = new HopArrival { Code = "" };
            TestValidationResult<HopArrival> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Code);
        }
    }
}