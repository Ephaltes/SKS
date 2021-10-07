using FluentValidation.TestHelper;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Validators;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class ErrorValidatorBehaviour
    {
        private ErrorValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new ErrorValidator();
        }
        [Test]
        public void ErrorValidator_ErrorMessageIsNull_ValidationError()
        {
            Error model = new Error { ErrorMessage = null };
            TestValidationResult<Error> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.ErrorMessage);
        }
        [Test]
        public void ErrorValidator_ErrorMessageIsNotNull_Success()
        {
            Error model = new Error { ErrorMessage = "a" };
            TestValidationResult<Error> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.ErrorMessage);
        }
    }
}