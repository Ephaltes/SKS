using System;
using FluentAssertions;
using NUnit.Framework;
using NLSL.SKS.Package.Services.DTOs;
using NLSL.SKS.Package.Services.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace NLSL.SKS.Package.Services.Tests
{
    public class RecipientApiControllerBehaviour
    {
        private RecipientApiController testController = new();
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TrackParcel_ValidTrackingID_Success()
        {
            StatusCodeResult result;

            result = (StatusCodeResult) testController.TrackParcel("ABCDEFGHI");

            result.StatusCode.Should().Be(200);
        }
        [Test]
        public void TrackParcel_InValidTrackingID_ArgumentException()
        {
            Action action;
            
            action = () => testController.TrackParcel("ABCDEFGH");

            action.Should().Throw<ArgumentException>();
        }
    }
}
