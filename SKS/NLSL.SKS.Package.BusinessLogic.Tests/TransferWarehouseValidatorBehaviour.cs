using FluentValidation.TestHelper;

using NetTopologySuite.Geometries;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Validators;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class TransferWarehouseValidatorBehaviour
    {
        private TransferWarehouseValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new TransferWarehouseValidator
                        {
                            CascadeMode = FluentValidation.CascadeMode.Continue
                        };
        }
        [Test]
        public void TransferWarehouseValidator_HopTypeIsNull_ValidationError()
        {
            TransferWarehouse model = new TransferWarehouse { HopType = null };
            TestValidationResult<TransferWarehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.HopType);
        }
        [Test]
        public void TransferWarehouseValidator_CodeIsNull_ValidationError()
        {
            TransferWarehouse model = new TransferWarehouse { Code = null };
            TestValidationResult<TransferWarehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Code);
        }
        [Test]
        public void TransferWarehouseValidator_DescriptionIsNull_ValidationError()
        {
            TransferWarehouse model = new TransferWarehouse { Description = null };
            TestValidationResult<TransferWarehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Description);
        }
        [Test]
        public void TransferWarehouseValidator_ProcessingDelayMinsIsNull_ValidationError()
        {
            TransferWarehouse model = new TransferWarehouse { ProcessingDelayMins = null };
            TestValidationResult<TransferWarehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.ProcessingDelayMins);
        }
        [Test]
        public void TransferWarehouseValidator_LocationNameIsNull_ValidationError()
        {
            TransferWarehouse model = new TransferWarehouse { LocationName = null };
            TestValidationResult<TransferWarehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.LocationName);
        }
        [Test]
        public void TransferWarehouseValidator_LocationCoordinatesIsNull_ValidationError()
        {
            TransferWarehouse model = new TransferWarehouse { LocationCoordinates = null };
            TestValidationResult<TransferWarehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.LocationCoordinates);
        }
        [Test]
        public void TransferWarehouseValidator_DescrptionIsInValidRegex_ValidationError()
        {
            TransferWarehouse model = new TransferWarehouse { Description = "b- ahAah%Aa" };
            TestValidationResult<TransferWarehouse> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Description);
        }

       [Test]
        public void TransferWarehouseValidator_DescrptionIsValidRegex_Success()
        {
            TransferWarehouse model = new TransferWarehouse { Description = "B- ahAahaA" };
            TestValidationResult<TransferWarehouse> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Description);
        }

        [Test]
        public void TransferWarehouseValidator_RegionGeoJsonIsEmpty_ValidationError()
        {
            TransferWarehouse model = new TransferWarehouse { HopType = "warehouse", RegionGeometry = null };
            TestValidationResult<TransferWarehouse> result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(entity => entity.RegionGeometry);
        }
        [Test]
        public void TransferWarehouseValidator_RegionGeoJson_Succes()
        {
            TransferWarehouse model = new TransferWarehouse { HopType = "warehouse", RegionGeometry = new Point(1,1) };
            TestValidationResult<TransferWarehouse> result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(entity => entity.RegionGeometry);
        }
        [Test]
        public void TransferWarehouseValidator_LogisticsPartnerIsEmpty_ValidationError()
        {
            TransferWarehouse model = new TransferWarehouse { HopType = "warehouse", LogisticsPartner = null };
            TestValidationResult<TransferWarehouse> result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(entity => entity.LogisticsPartner);
        }
        [Test]
        public void TransferWarehouseValidator_LogisticsPArtner_Succes()
        {
            TransferWarehouse model = new TransferWarehouse { HopType = "warehouse", LogisticsPartner = "Test" };
            TestValidationResult<TransferWarehouse> result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(entity => entity.LogisticsPartner);
        }
        
        [Test]
        public void TransferWarehouseValidator_LogisticsPartnerUrlIsEmpty_ValidationError()
        {
            TransferWarehouse model = new TransferWarehouse { HopType = "warehouse", LogisticsPartnerUrl = null };
            TestValidationResult<TransferWarehouse> result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(entity => entity.LogisticsPartnerUrl);
        }
        [Test]
        public void TransferWarehouseValidator_LogisticsPartnerUrl_Succes()
        {
            TransferWarehouse model = new TransferWarehouse { HopType = "warehouse", LogisticsPartnerUrl = "Test" };
            TestValidationResult<TransferWarehouse> result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(entity => entity.LogisticsPartnerUrl);
        }
    }
}