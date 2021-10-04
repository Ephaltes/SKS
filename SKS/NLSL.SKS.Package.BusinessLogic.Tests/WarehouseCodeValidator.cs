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
    class WarehouseCodeValidatorTest
    {
        private WarehouseCodeValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new WarehouseCodeValidator();
        }
        [Test]
        public void WarehouseCode_ErrorMessageIsNull_ValidationError()
        {
                var model = new WarehouseCode(null);
                var result = validator.TestValidate(model);
                result.ShouldHaveValidationErrorFor(entity => entity.Code);
        }
        [Test]
        public void WarehouseCode_ErrorMessageIsValid_Success()
        {
            var model = new WarehouseCode("A");
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Code);
        }
    }
}
