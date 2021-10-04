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
    class HopArrivalValidatorTest
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
                var model = new HopArrival { DateTime = null};
                var result = validator.TestValidate(model);
                result.ShouldHaveValidationErrorFor(entity => entity.DateTime);
        }
        [Test]
        public void GeoCoordinateValidator_DescriptionIsNull_ValidationError()
        {
            var model = new HopArrival {Description = null };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Description);
        }
        [Test]
        public void GeoCoordinateValidator_CodeIsNull_ValidationError()
        {
            var model = new HopArrival {Code = null};
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Code);
        }
        [Test]
        public void GeoCoordinateValidator_CodeMatchesRegexLowEndCase_Success()
        {
            var model = new HopArrival {Code = "ABCD1"};
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Code);
        }
        [Test]
        public void GeoCoordinateValidator_CodeMatchesRegexHighEndCase_Success()
        {
            var model = new HopArrival {Code = "ABCD1234"};
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Code);
        }
        [Test]
        public void GeoCoordinateValidator_CodeDoesNotMatchRegex_ValidationError()
        {
            var model = new HopArrival {Code = ""};
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Code);
        }
    }
}
