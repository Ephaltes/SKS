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
    class ParcelValidatorTest
    {
        private ParcelValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new ParcelValidator();
            validator.CascadeMode  = CascadeMode.Continue;
        }
        [Test]
        public void ParcelValidator_RecipientIsNull_ValidationError()
        {
                var model = new Parcel { Recipient=null};
                var result = validator.TestValidate(model);
                result.ShouldHaveValidationErrorFor(entity => entity.Recipient);
        }
        [Test]
        public void ParcelValidator_SenderIsNull_ValidationError()
        {
            var model = new Parcel {Sender = null};
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Sender);
        }
        [Test]
        public void ParcelValidator_FutureHopsIsNull_ValidationError()
        {
            var model = new Parcel {FutureHops = null};
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.FutureHops);
        }
        [Test]
        public void ParcelValidator_StateIsNull_Success()
        {
            var model = new Parcel {State = null};
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.State);
        }
        [Test]
        public void ParcelValidator_TackingIdIsNull_Success()
        {
            var model = new Parcel {TrackingId = null};
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.TrackingId);
        }
        [Test]
        public void ParcelValidator_VisitedHopsIsNull_ValidationError()
        {
            var model = new Parcel {VisitedHops = null};
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.VisitedHops);
        }
        [Test]
        public void ParcelValidator_WeightIsNull_ValidationError()
        {
            var model = new Parcel {Weight = null };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Weight);
        }


        [Test]
        public void ParcelValidator_WeightLessThan0f_ValidatioError()
        {
            var model = new Parcel { Weight = -0.1f };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.Weight);
        }

        [Test]
        public void ParcelValidator_WeightGreaterThan0f_Success()
        {
            var model = new Parcel { Weight = 0.1f };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(entity => entity.Weight);
        }
        [Test]
        public void ParcelValidator_TackingIdMatchesRegex_Success()
        {
            var model = new Parcel { TrackingId = "ABCDEFGH" };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.TrackingId);
        }
        [Test]
        public void ParcelValidator_TackingIdDoesNotMatchesRegex_ValidationError()
        {
            var model = new Parcel { TrackingId = "ABCDEFH" };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(entity => entity.TrackingId);
        }
    }
}
