using System;
using FluentAssertions;
using NUnit.Framework;
using NLSL.SKS.Package.Services.DTOs;
using NLSL.SKS.Package.Services.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace NLSL.SKS.Package.Services.Tests
{
    public class SenderApiControllerBehaviour
    {
        private Parcel testParcel = new();
        private Recipient testSender = new();
        private Recipient testRecipient = new();
        private SenderApiController testController = new();
        [SetUp]
        public void Setup()
        {
            testParcel = new();
            testSender = new();
            testRecipient = new();

            testSender.City = "testSender.City";
            testSender.Country = "testSender.Country";
            testSender.Name = "testSender.Name";
            testSender.Street = "testSender.Street";
            testSender.PostalCode = "testSender.PostalCode";

            testRecipient.City = "testRecipient.City";
            testRecipient.Country = "testRecipient.Country";
            testRecipient.Name = "testRecipient.Name";
            testRecipient.Street = "testRecipient.Street";
            testRecipient.PostalCode = "testRecipient.PostalCode";

            testParcel.Weight = 1;
            testParcel.Sender = testSender;
            testParcel.Recipient = testRecipient;
        }

        [Test]
        public void SubmitParcel_ValidParcel_Success()
        {
            StatusCodeResult result;
            
            result = (StatusCodeResult) testController.SubmitParcel(testParcel);

            result.StatusCode.Should().Be(201);
        }
        [Test]
        public void SubmitParcel_InValidParcelWithWeightIsNull_ArgumentNullException()
        {
            Action action;

            testParcel.Weight = null;         
            action = () => testController.SubmitParcel(testParcel);

            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(testParcel.Weight));
        }
    }
}
