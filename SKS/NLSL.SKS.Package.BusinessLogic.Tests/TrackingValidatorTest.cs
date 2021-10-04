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
    class TrackingIdValidatorTest
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
                var model = new TrackingId(null);
                var result = validator.TestValidate(model);
                result.ShouldHaveValidationErrorFor(entity => entity.Id);
        }
        [Test]
        public void TrackingIdValidator_IdIsValid_Success()
        {
            var model = new TrackingId("ABCDEFGHI");
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Id);
        }
        [Test]
        public void TrackingIdValidator_IdIsNotValid_ValidationError()
        {
            var model = new TrackingId("ABCDEFGH");
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Id);
        }
    }
}
