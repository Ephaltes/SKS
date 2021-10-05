using FluentValidation;
using FluentValidation.TestHelper;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Validators;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class WarehouseNextHopsValidatorBehaviour
    {
        private WarehouseNextHopsValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new WarehouseNextHopsValidator();
            validator.CascadeMode = CascadeMode.Continue;
        }
        [Test]
        public void WarehouseNextHopsValidator_HopIsNull_ValidationError()
        {
            WarehouseNextHops model = new WarehouseNextHops { Hop = null };
            TestValidationResult<WarehouseNextHops> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Hop);
        }

        [Test]
        public void WarehouseNextHopsValidator_TraveltimeMinsIsNull_ValidationError()
        {
            WarehouseNextHops model = new WarehouseNextHops { TraveltimeMins = null };
            TestValidationResult<WarehouseNextHops> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.TraveltimeMins);
        }
    }
}