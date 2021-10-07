using FluentValidation;
using FluentValidation.TestHelper;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Validators;

using NUnit.Framework;

namespace NLSL.SKS.Package.BusinessLogic.Tests
{
    public class RecipientValidatorBehaviour
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
            Recipient model = new Recipient { Country = null };
            validator.CascadeMode = CascadeMode.Stop;

            TestValidationResult<Recipient> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Country);
        }
        [Test]
        public void RecipientValidator_CountryIsValidButNotAustriaOrOesterreich_Success()
        {
            Recipient model = new Recipient { Country = "banane" };
            TestValidationResult<Recipient> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Country);
        }

        [Test]
        public void RecipientValidator_CityIsNull_ValidationError()
        {
            Recipient model = new Recipient { City = null, Country = "Austria" };
            TestValidationResult<Recipient> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.City);
        }
        [Test]
        public void RecipientValidator_StreetIsNull_ValidationError()
        {
            Recipient model = new Recipient { Street = null, Country = "Austria" };
            TestValidationResult<Recipient> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Street);
        }
        [Test]
        public void RecipientValidator_NameIsNull_ValidationError()
        {
            Recipient model = new Recipient { Name = null, Country = "Austria" };
            TestValidationResult<Recipient> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Name);
        }
        [Test]
        public void RecipientValidator_PostalCodeNull_ValidationError()
        {
            Recipient model = new Recipient { PostalCode = null, Country = "Austria" };
            TestValidationResult<Recipient> result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.PostalCode);
        }

        [Test]
        public void RecipientValidator_PostalCodeIsValid_Success()
        {
            Recipient model = new Recipient { Country = "Austria", PostalCode = "A-1234" };
            TestValidationResult<Recipient> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.PostalCode);
        }
        [Test]
        public void RecipientValidator_StreetIsValid_Success()
        {
            Recipient model = new Recipient { Country = "Austria", Street = "Hauptstraße 12/12/12" };
            TestValidationResult<Recipient> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Street);
        }
        [Test]
        public void RecipientValidator_NameIsValid_Success()
        {
            Recipient model = new Recipient { Country = "Austria", Name = "Alfred Duck" };
            TestValidationResult<Recipient> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Name);
        }
        [Test]
        public void RecipientValidator_CityIsValid_Success()
        {
            Recipient model = new Recipient { Country = "Austria", City = "Wien" };
            TestValidationResult<Recipient> result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.City);
        }

    }
}