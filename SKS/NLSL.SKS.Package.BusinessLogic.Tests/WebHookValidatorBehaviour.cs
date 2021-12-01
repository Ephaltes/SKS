using FluentValidation.TestHelper;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Validators;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class WebHookValidatorBehaviour
    {
        private WebHookValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new WebHookValidator();
        }
        [Test]
        public void WebHookValidator_TrackingIdIsNull_ValidationError()
        {
            WebHook model = new WebHook();
            TestValidationResult<WebHook> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Id);
        }
        [Test]
        public void WebHookValidator_TrackingIdIsValid_Success()
        {
            WebHook model = new WebHook();
            model.trackingId = "ABCDEFGHI";
            TestValidationResult<WebHook> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Id);
        }
        [Test]
        public void WebHookValidator_TrackingIdIsNotValid_ValidationError()
        {
            WebHook model = new WebHook();
            model.trackingId = "ABCDEFGH";
            TestValidationResult<WebHook> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Id);
        }
    }
}