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
    class WarehouseNextHopsValidatorTest
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
                var model = new WarehouseNextHops { Hop = null };
                var result = validator.TestValidate(model);
                result.ShouldHaveValidationErrorFor(entity => entity.Hop);
        }

        [Test]
        public void WarehouseNextHopsValidator_TraveltimeMinsIsNull_ValidationError()
        {
            var model = new WarehouseNextHops { TraveltimeMins = null};
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.TraveltimeMins);
        }
    }
}
