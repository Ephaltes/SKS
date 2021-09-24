using System;
using FluentAssertions;
using NUnit.Framework;
using NLSL.SKS.Package.Services.DTOs;
using NLSL.SKS.Package.Services.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace NLSL.SKS.Package.Services.Tests
{
    public class StaffApiControllerBehaviour
    {
        private StaffApiController testController = new();
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReportHop_ValidHop_DoesNotThrowException()
        {           
            StatusCodeResult result;

            result = (StatusCodeResult) testController.ReportHop("ABCDEFGHI", "ABCD5678");

            result.StatusCode.Should().Be(200);
        }
        [Test]
        public void ReportHop_InValidHopWithTrackingIDToShort_ArgumentException()
        {
            Action action;

            action = () => testController.ReportHop("ABCDEFGH", "ABCD56789");

            action.Should().Throw<ArgumentException>();
        }
        [Test]
        public void ReportHop_InValidHopWithCodeToShort_ArgumentException()
        {
            Action action;

            action = () => testController.ReportHop("ABCDEFGHI", "ABCD");

            action.Should().Throw<ArgumentException>();
        }
        
        [Test]
        public void ReportParcelDelivery_ValidReport_Success()
        {           
            StatusCodeResult result;

            result = (StatusCodeResult) testController.ReportParcelDelivery("ABCDEFGHI");

            result.StatusCode.Should().Be(200);
        }
        
        [Test]
        public void ReportParcelDelivery_WrongTrackingIdLength_ArgumentException()
        {           
            Action action;

            action = () => testController.ReportParcelDelivery("ABCDE");

            action.Should().Throw<ArgumentException>();
        }
        
        [Test]
        public void ReportParcelDelivery_WrongTrackingIdLength_ArgumentNullException()
        {           
            Action action;

            action = () => testController.ReportParcelDelivery(null);

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
