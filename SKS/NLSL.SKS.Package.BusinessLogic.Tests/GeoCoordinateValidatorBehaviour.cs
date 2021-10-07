using FluentValidation;
using FluentValidation.TestHelper;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Validators;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class GeoCoordinateValidatorBehaviour
    {
        private GeoCoordinateValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new GeoCoordinateValidator();
            validator.CascadeMode = CascadeMode.Continue;
        }
        [Test]
        public void GeoCoordinateValidator_LonIsNull_ValidationError()
        {
            GeoCoordinate model = new GeoCoordinate { Lon = null };
            TestValidationResult<GeoCoordinate> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Lon);
        }
        [Test]
        public void GeoCoordinateValidator_LatIsNull_ValidationError()
        {
            GeoCoordinate model = new GeoCoordinate { Lat = null };
            TestValidationResult<GeoCoordinate> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Lat);
        }
        [Test]
        public void GeoCoordinateValidator_LonIsValid_Success()
        {
            GeoCoordinate model = new GeoCoordinate { Lon = 1 };
            TestValidationResult<GeoCoordinate> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Lon);
        }
        [Test]
        public void GeoCoordinateValidator_LatIsValid_Success()
        {
            GeoCoordinate model = new GeoCoordinate { Lat = 1 };
            TestValidationResult<GeoCoordinate> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Lat);
        }
    }
}