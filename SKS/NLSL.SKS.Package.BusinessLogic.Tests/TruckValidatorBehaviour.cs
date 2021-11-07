using FluentValidation.TestHelper;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Validators;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class TruckValidatorBehaviour
    {
        private TruckValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new TruckValidator
                        {
                            CascadeMode = FluentValidation.CascadeMode.Continue
                        };
        }
        [Test]
        public void TruckValidator_HopTypeIsNull_ValidationError()
        {
            Truck model = new Truck { HopType = null };
            TestValidationResult<Truck> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.HopType);
        }
        [Test]
        public void TruckValidator_CodeIsNull_ValidationError()
        {
            Truck model = new Truck { Code = null };
            TestValidationResult<Truck> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Code);
        }
        [Test]
        public void TruckValidator_DescriptionIsNull_ValidationError()
        {
            Truck model = new Truck { Description = null };
            TestValidationResult<Truck> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Description);
        }
        [Test]
        public void TruckValidator_ProcessingDelayMinsIsNull_ValidationError()
        {
            Truck model = new Truck { ProcessingDelayMins = null };
            TestValidationResult<Truck> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.ProcessingDelayMins);
        }
        [Test]
        public void TruckValidator_LocationNameIsNull_ValidationError()
        {
            Truck model = new Truck { LocationName = null };
            TestValidationResult<Truck> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.LocationName);
        }
        [Test]
        public void TruckValidator_LocationCoordinatesIsNull_ValidationError()
        {
            Truck model = new Truck { LocationCoordinates = null };
            TestValidationResult<Truck> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.LocationCoordinates);
        }
        [Test]
        public void TruckValidator_DescrptionIsInValidRegex_ValidationError()
        {
            Truck model = new Truck { Description = "b- ahAah%Aa" };
            TestValidationResult<Truck> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Description);
        }

       [Test]
        public void TruckValidator_DescrptionIsValidRegex_Success()
        {
            Truck model = new Truck { Description = "B- ahAahaA" };
            TestValidationResult<Truck> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Description);
        }
        [Test]
        public void TruckValidator_NumberPlateIsEmpty_ValidationError()
        {
            Truck model = new Truck { HopType = "warehouse", NumberPlate = null };
            TestValidationResult<Truck> result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(entity => entity.NumberPlate);
        }
        [Test]
        public void TruckValidator_NumberPlate_Succes()
        {
            Truck model = new Truck { HopType = "warehouse", NumberPlate = "Test" };
            TestValidationResult<Truck> result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(entity => entity.NumberPlate);
        }
        
        [Test]
        public void TruckValidator_RegionGeoJsonIsEmpty_ValidationError()
        {
            Truck model = new Truck { HopType = "warehouse", RegionGeoJson = null };
            TestValidationResult<Truck> result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(entity => entity.RegionGeoJson);
        }
        [Test]
        public void TruckValidator_RegionGeoJson_Succes()
        {
            Truck model = new Truck { HopType = "warehouse", RegionGeoJson = "Test" };
            TestValidationResult<Truck> result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(entity => entity.RegionGeoJson);
        }
    }
}