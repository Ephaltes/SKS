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
    class ReportHopValidatorTest
    {
        private ReportHopValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new ReportHopValidator();
            validator.CascadeMode = CascadeMode.Continue;
        }
        [Test]
        public void ReportHopValidator_HopCodeIsNull_ValidationError()
        {
                var model = new ReportHop { HopCode = null };
                var result = validator.TestValidate(model);
                result.ShouldHaveValidationErrorFor(entity => entity.HopCode);
        }
        [Test]
        public void ReportHopValidator_HopCodeIsEmpty_ValidationError()
        {
            var model = new ReportHop { HopCode = "" };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.HopCode);
        }
        [Test]
        public void ReportHopValidator_HopCodeIsNotNullAndNotEmpty_Success()
        {
            var model = new ReportHop { HopCode = "a" };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.HopCode);
        }
        [Test]
        public void ReportHopValidator_TrackingIdIsNull_ValidationError()
        {
            var model = new ReportHop { TrackingId = null };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.HopCode);
        }
    }
}
