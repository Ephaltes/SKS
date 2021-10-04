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
    class RecipientValidatorTest
    {
        private RecipientValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new RecipientValidator();
            validator.CascadeMode = CascadeMode.Continue;
        }
        [Test]
        public void RecipientValidator_CountryIsNull_ValidationError()
        {
                var model = new Recipient { Country = null };
                validator.CascadeMode = CascadeMode.Stop;

                var result = validator.TestValidate(model);
                result.ShouldHaveValidationErrorFor(entity => entity.Country);
        }
        [Test]
        public void RecipientValidator_CityIsNull_ValidationError()
        {
            var model = new Recipient { City = null,Country="Austria" };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.City);
        }
        [Test]
        public void RecipientValidator_StreetIsNull_ValidationError()
        {
            var model = new Recipient { Street = null, Country = "Austria" };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Street);
        }
        [Test]
        public void RecipientValidator_NameIsNull_ValidationError()
        {
            var model = new Recipient { Name = null, Country = "Austria" };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Name);
        }
        [Test]
        public void RecipientValidator_PostalCodeNull_ValidationError()
        {
            var model = new Recipient { PostalCode = null, Country = "Austria" };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.PostalCode);
        }

        [Test]
        public void RecipientValidator_PostalCodeIsValid_Success()
        {
            var model = new Recipient { Country = "Austria", PostalCode = "A-1234" };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.PostalCode);
        }
        [Test]
        public void RecipientValidator_StreetIsValid_Success()
        {
            var model = new Recipient { Country = "Austria", Street = "Hauptstraße 12/12/12" };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Street);
        }
        [Test]
        public void RecipientValidator_NameIsValid_Success()
        {
            var model = new Recipient { Country = "Austria", Name = "Alfred Duck" };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Name);
        }
        [Test]
        public void RecipientValidator_CityIsValid_Success()
        {
            var model = new Recipient { Country = "Austria", City = "Wien" };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.City);
        }

    }
}
