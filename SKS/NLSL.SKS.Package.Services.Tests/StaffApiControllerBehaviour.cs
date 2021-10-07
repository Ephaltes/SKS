using System;

using FakeItEasy;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.Services.Controllers;

using NUnit.Framework;

namespace NLSL.SKS.Package.Services.Tests
{
    public class StaffApiControllerBehaviour
    {
        private IParcelManagement _parcelManagement;
        private StaffApiController _testController;
        [SetUp]
        public void Setup()
        {
            _parcelManagement = A.Fake<IParcelManagement>();

            _testController = new StaffApiController(_parcelManagement);
        }

        [Test]
        public void ReportHop_ValidHop_Success()
        {
            StatusCodeResult result;
            A.CallTo(() => _parcelManagement.ReportHop(null)).WithAnyArguments().Returns(true);

            result = (StatusCodeResult)_testController.ReportHop("ABCDEFGHI", "ABCD5678");

            result.StatusCode.Should().Be(200);
        }
        [Test]
        public void ReportHop_InvalidHop_StatusCode500()
        {
            ObjectResult result;
            A.CallTo(() => _parcelManagement.ReportHop(null)).WithAnyArguments().Returns(false);

            result = (ObjectResult)_testController.ReportHop("ABCDEFGHI", "ABCD5678");

            result.StatusCode.Should().Be(500);
        }
       
        [Test]
        public void ReportParcelDelivery_ValidReport_Success()
        {
            StatusCodeResult result;
            A.CallTo(() => _parcelManagement.Delivered(null)).WithAnyArguments().Returns(true);

            result = (StatusCodeResult)_testController.ReportParcelDelivery("ABCDEFGHI");

            result.StatusCode.Should().Be(200);
        }

        [Test]
        public void ReportParcelDelivery_TrackingIDNotFound_NotFoundStatusCode()
        {
            ObjectResult result;
            A.CallTo(() => _parcelManagement.Delivered(null)).WithAnyArguments().Returns(null);

            result = (ObjectResult)_testController.ReportParcelDelivery("ABCDEFGHI");

            result.StatusCode.Should().Be(404);
        }

        [Test]
        public void ReportParcelDelivery_SomethingWrong_BadRequest()
        {
            ObjectResult result;
            A.CallTo(() => _parcelManagement.Delivered(null)).WithAnyArguments().Returns(false);

            result = (ObjectResult)_testController.ReportParcelDelivery("ABCDEFGHI");

            result.StatusCode.Should().Be(400);
        }
    }
}