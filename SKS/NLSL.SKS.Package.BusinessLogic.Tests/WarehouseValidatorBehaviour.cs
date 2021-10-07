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
        /*
         * RuleFor(p => p.HopType).NotNull().WithMessage("{PropertyName} was null");
            

            RuleFor(p => p.Code).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.Description).NotNull().WithMessage("{PropertyName} was null")
                .Matches("^[A-Za-z0-9öÖäÄüÜ- ]*$").WithMessage("{PropertyName} does not Match ^[A-Za-z0-9öÖäÄüÜ- ]*$ Regex");
            RuleFor(p => p.ProcessingDelayMins).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.LocationName).NotNull().WithMessage("{PropertyName} was null");
            RuleFor(p => p.LocationCoordinates).NotNull().WithMessage("{PropertyName} was null");
        */
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
        /*
         * 
            When(p => (p.HopType == "truck"), () => {
                RuleFor(p => p.RegionGeoJson).NotNull().WithMessage("{PropertyName} was null");
                RuleFor(p => p.NumberPlate).NotNull().WithMessage("{PropertyName} was null");
            });
            When(p => (p.HopType == "warehouse"), () => {
                RuleFor(p => p.Level).NotNull().WithMessage("{PropertyName} was null");
                RuleFor(p => p.NextHops).NotNull().WithMessage("{PropertyName} was null");
            });
            When(p => (p.HopType == "transferwarehouse"), () => {
                RuleFor(p => p.RegionGeoJson).NotNull().WithMessage("{PropertyName} was null");
                RuleFor(p => p.LogisticsPartner).NotNull().WithMessage("{PropertyName} was null");
                RuleFor(p => p.LogisticsPartnerUrl).NotNull().WithMessage("{PropertyName} was null");
            });
         */
        [Test]
        public void WarehouseValidator_RegionGeoJsonIsNullByHoptypeTruck_ValidationError()
        {
            Warehouse model = new Warehouse { HopType = "truck", RegionGeoJson = null };
            TestValidationResult<Warehouse> result = validator.TestValidate(model);
            
            result.ShouldHaveValidationErrorFor(entity => entity.Code);
        }
        [Test]
        public void WarehouseValidator_NumberplateIsNullByHoptypeTruck_ValidationError()
        {
            Warehouse model = new Warehouse { HopType = "truck", NumberPlate = null };
            TestValidationResult<Warehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.NumberPlate);
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
        [Test]
        public void WarehouseValidator_RegionGeoJsonIsNullByHoptypeTransferwarehouse_ValidationError()
        {
            Warehouse model = new Warehouse { HopType = "transferwarehouse", RegionGeoJson = null };
            TestValidationResult<Warehouse> result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(entity => entity.RegionGeoJson);
        }
        [Test]
        public void WarehouseValidator_LogisticsPartnerIsNullByHoptypeTransferwarehouse_ValidationError()
        {
            Warehouse model = new Warehouse { HopType = "transferwarehouse", LogisticsPartner = null };
            TestValidationResult<Warehouse> result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(entity => entity.LogisticsPartner);
        }
        [Test]
        public void WarehouseValidator_LogisticsPartnerUrlIsNullByHoptypeTransferwarehouse_ValidationError()
        {
            Warehouse model = new Warehouse { HopType = "transferwarehouse", LogisticsPartnerUrl = null };
            TestValidationResult<Warehouse> result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(entity => entity.LogisticsPartnerUrl);
        }
    }
}