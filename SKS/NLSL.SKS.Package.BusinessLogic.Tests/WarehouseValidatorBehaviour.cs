using FluentValidation.TestHelper;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Validators;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class WarehouseValidatorBehaviour
    {
        private WarehouseValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new WarehouseValidator();
            validator.CascadeMode = FluentValidation.CascadeMode.Continue;
        }
        [Test]
        public void WarehouseValidator_HopTypeIsNull_ValidationError()
        {
            Warehouse model = new Warehouse { HopType = null };
            TestValidationResult<Warehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.HopType);
        }
        [Test]
        public void WarehouseValidator_CodeIsNull_ValidationError()
        {
            Warehouse model = new Warehouse { Code = null };
            TestValidationResult<Warehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Code);
        }
        [Test]
        public void WarehouseValidator_DescriptionIsNull_ValidationError()
        {
            Warehouse model = new Warehouse { Description = null };
            TestValidationResult<Warehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Description);
        }
        [Test]
        public void WarehouseValidator_ProcessingDelayMinsIsNull_ValidationError()
        {
            Warehouse model = new Warehouse { ProcessingDelayMins = null };
            TestValidationResult<Warehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.ProcessingDelayMins);
        }
        [Test]
        public void WarehouseValidator_LocationNameIsNull_ValidationError()
        {
            Warehouse model = new Warehouse { LocationName = null };
            TestValidationResult<Warehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.LocationName);
        }
        [Test]
        public void WarehouseValidator_LocationCoordinatesIsNull_ValidationError()
        {
            Warehouse model = new Warehouse { LocationCoordinates = null };
            TestValidationResult<Warehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.LocationCoordinates);
        }
        [Test]
        public void WarehouseValidator_DescrptionIsInValidRegex_ValidationError()
        {
            Warehouse model = new Warehouse { Description = "b- ahAah%Aa" };
            TestValidationResult<Warehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Code);
        }

       [Test]
        public void WarehouseValidator_DescrptionIsValidRegex_Success()
        {
            Warehouse model = new Warehouse { Description = "B- ahAahaA" };
            TestValidationResult<Warehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Code);
        }
        [Test]
        public void WarehouseValidator_LevelIsNullByHoptypeWarehouse_ValidationError()
        {
            Warehouse model = new Warehouse { HopType = "warehouse", Level = null };
            TestValidationResult<Warehouse> result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(entity => entity.Level);
        }
        [Test]
        public void WarehouseValidator_NextHopsIsNullByHoptypeWarehouse_ValidationError()
        {
            Warehouse model = new Warehouse { HopType = "warehouse", NextHops = null };
            TestValidationResult<Warehouse> result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(entity => entity.NextHops);
        }
    }
}