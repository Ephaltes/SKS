using FluentValidation;
using FluentValidation.TestHelper;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Validators;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class ReportHopValidatorBehaviour
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
            ReportHop model = new ReportHop { HopCode = null };
            TestValidationResult<ReportHop> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.HopCode);
        }
        [Test]
        public void ReportHopValidator_HopCodeIsEmpty_ValidationError()
        {
            ReportHop model = new ReportHop { HopCode = "" };
            TestValidationResult<ReportHop> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.HopCode);
        }
        [Test]
        public void ReportHopValidator_HopCodeIsNotNullAndNotEmpty_Success()
        {
            ReportHop model = new ReportHop { HopCode = "a" };
            TestValidationResult<ReportHop> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.HopCode);
        }
        [Test]
        public void ReportHopValidator_TrackingIdIsNull_ValidationError()
        {
            ReportHop model = new ReportHop { TrackingId = null };
            TestValidationResult<ReportHop> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.HopCode);
        }
    }
}