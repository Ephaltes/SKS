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
    class ErrorValidatorTest
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
                var model = new Error { ErrorMessage = null };
                var result = validator.TestValidate(model);
                result.ShouldHaveValidationErrorFor(entity => entity.ErrorMessage);
        }
        [Test]
        public void ErrorValidator_ErrorMessageIsNotNull_Success()
        {
            var model = new Error { ErrorMessage = "a" };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.ErrorMessage);
        }
    }
}
