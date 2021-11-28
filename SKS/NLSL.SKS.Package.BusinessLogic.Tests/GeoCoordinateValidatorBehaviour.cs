using FluentValidation;
using FluentValidation.TestHelper;

using NetTopologySuite.Geometries;

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
        public void GeoCoordinateValidator_LocationIsNull_ValidationError()
        {
            GeoCoordinate model = new GeoCoordinate { Location = null};
            TestValidationResult<GeoCoordinate> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Location);
        }
        [Test]
        public void GeoCoordinateValidator_LonIsValid_Success()
        {
            GeoCoordinate model = new GeoCoordinate { Location = new Point(1,1) };
            TestValidationResult<GeoCoordinate> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Location);
        }
    }
}