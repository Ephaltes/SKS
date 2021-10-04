using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NUnit.Framework;
using FluentValidation;
using FluentValidation.TestHelper;
using NLSL.SKS.Package.BusinessLogic.Validators;
using NLSL.SKS.Package.BusinessLogic.Entities;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    class GeoCoordinateValidatorTest
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
                var model = new GeoCoordinate { Lon = null};
                var result = validator.TestValidate(model);
                result.ShouldHaveValidationErrorFor(entity => entity.Lon);
        }
        [Test]
        public void GeoCoordinateValidator_LatIsNull_ValidationError()
        {
            var model = new GeoCoordinate { Lat = null};
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Lat);
        }
        [Test]
        public void GeoCoordinateValidator_LonIsValid_Success()
        {
            var model = new GeoCoordinate { Lon = 1 };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Lon);
        }
        [Test]
        public void GeoCoordinateValidator_LatIsValid_Success()
        {
            var model = new GeoCoordinate { Lat = 1 };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Lat);
        }
    }
}
