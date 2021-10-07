using FluentValidation.TestHelper;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Validators;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class WarehouseCodeValidatorBehaviour
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
            WarehouseCode model = new WarehouseCode(null);
            TestValidationResult<WarehouseCode> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Code);
        }
        [Test]
        public void WarehouseCode_ErrorMessageIsValid_Success()
        {
            WarehouseCode model = new WarehouseCode("A");
            TestValidationResult<WarehouseCode> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Code);
        }
    }
}