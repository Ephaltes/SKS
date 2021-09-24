using System;
using FluentAssertions;
using NUnit.Framework;
using NLSL.SKS.Package.Services.DTOs;
using NLSL.SKS.Package.Services.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace NLSL.SKS.Package.Services.Tests
{
    public class LogisticsParnterApiControllerBehaviour 
    {
        private Parcel testParcel = new();
        private Recipient testSender = new();
        private Recipient testRecipient = new();
        private LogisticsPartnerApiController testController = new();
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
        public void TransitionParcel_ValidParcel_Success()
        {
            StatusCodeResult result;
            
            result = (StatusCodeResult) testController.TransitionParcel(testParcel, "ABCDEFGHI");

            result.StatusCode.Should().Be(200);
        }
        [Test]
        public void TransitionParcel_InValidParcelWithWeightIsNull_ArgumentNullException()
        {
            Action action;

            testParcel.Weight = null;         
            action = () => testController.TransitionParcel(testParcel, "ABCDEFGHI");

            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(testParcel.Weight));
        }
    }
}
