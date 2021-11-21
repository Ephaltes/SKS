using System;

using FakeItEasy;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.BusinessLogic.CustomExceptions;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos;
using NLSL.SKS.Package.Services.Controllers;

using NUnit.Framework;

namespace NLSL.SKS.Package.Services.Tests
{
    public class StaffApiControllerBehaviour
    {
        private IParcelLogic _parcelLogic;
        private StaffApiController _testController;
        private ILogger<StaffApiController> _logger;
        [SetUp]
        public void Setup()
        {
            _parcelLogic = A.Fake<IParcelLogic>();
            _logger = A.Fake<ILogger<StaffApiController>>();

            _testController = new StaffApiController(_parcelLogic,_logger);
        }

        [Test]
        public void ReportHop_ValidHop_Success()
        {
            StatusCodeResult result;
            A.CallTo(() => _parcelLogic.ReportHop(null)).WithAnyArguments().Returns(true);

            result = (StatusCodeResult)_testController.ReportHop("ABCDEFGHI", "ABCD5678");

            result.StatusCode.Should().Be(200);
        }
        [Test]
        public void ReportHop_InvalidHop_StatusCode500()
        {
            ObjectResult result;
            A.CallTo(() => _parcelLogic.ReportHop(null)).WithAnyArguments().Returns(false);

            result = (ObjectResult)_testController.ReportHop("ABCDEFGHI", "ABCD5678");

            result.StatusCode.Should().Be(500);
        }
       
        [Test]
        public void ReportParcelDelivery_ValidReport_Success()
        {
            StatusCodeResult result;
            A.CallTo(() => _parcelLogic.Delivered(null)).WithAnyArguments().Returns(true);

            result = (StatusCodeResult)_testController.ReportParcelDelivery("ABCDEFGHI");

            result.StatusCode.Should().Be(200);
        }

        [Test]
        public void ReportParcelDelivery_TrackingIDNotFound_NotFoundStatusCode()
        {
            ObjectResult result;
            A.CallTo(() => _parcelLogic.Delivered(null)).WithAnyArguments().Returns(null);

            result = (ObjectResult)_testController.ReportParcelDelivery("ABCDEFGHI");

            result.StatusCode.Should().Be(404);
        }

        [Test]
        public void ReportParcelDelivery_SomethingWrong_BadRequest()
        {
            ObjectResult result;
            A.CallTo(() => _parcelLogic.Delivered(null)).WithAnyArguments().Returns(false);

            result = (ObjectResult)_testController.ReportParcelDelivery("ABCDEFGHI");

            result.StatusCode.Should().Be(400);
        }
        
        
            [Test]
        public void ReportHop_BadRequest_FromBusinessLayerDataNotFoundException()
        {
            ObjectResult result;
            BusinessLayerExceptionBase exception = new BusinessLayerExceptionBase("test",new BusinessLayerDataNotFoundException());
            A.CallTo(() => _parcelLogic.ReportHop(null)).WithAnyArguments()
                .Throws(exception);
            
            
            
            result = (ObjectResult) _testController.ReportHop("","");

            result.StatusCode.Should().Be(500);
        }
        
        [Test]
        public void ReportHop_BadRequest_FromBusinessLayerValidationException()
        {
            ObjectResult result;
            BusinessLayerExceptionBase exception = new BusinessLayerExceptionBase("test",new BusinessLayerValidationException());
            A.CallTo(() => _parcelLogic.ReportHop(null)).WithAnyArguments()
                .Throws(exception);
            
            
            
            result = (ObjectResult) _testController.ReportHop("","");

            result.StatusCode.Should().Be(500);
        }
        
        [Test]
        public void ReportHop_BadRequest_FromDataAccessException()
        {
            ObjectResult result;
            BusinessLayerExceptionBase exception = new BusinessLayerExceptionBase("test",new DataAccessExceptionBase());
            A.CallTo(() => _parcelLogic.ReportHop(null)).WithAnyArguments()
                .Throws(exception);
            
            
            
            result = (ObjectResult) _testController.ReportHop("","");
            result.StatusCode.Should().Be(500);
        }
        
        [Test]
        public void ReportHop_BadRequest_FromException()
        {
            ObjectResult result;
            A.CallTo(() => _parcelLogic.ReportHop(null)).WithAnyArguments()
                .Throws<Exception>();
            
            result = (ObjectResult) _testController.ReportHop("","");

            result.StatusCode.Should().Be(500);
        }
        
        [Test]
        public void ReportParcelDelivery_BadRequest_FromBusinessLayerDataNotFoundException()
        {
            ObjectResult result;
            BusinessLayerExceptionBase exception = new BusinessLayerExceptionBase("test",new BusinessLayerDataNotFoundException());
            A.CallTo(() => _parcelLogic.Delivered(null)).WithAnyArguments()
                .Throws(exception);
            
            
            
            result = (ObjectResult) _testController.ReportParcelDelivery("");

            result.StatusCode.Should().Be(404);
        }
        
        [Test]
        public void ReportParcelDelivery_BadRequest_FromBusinessLayerValidationException()
        {
            ObjectResult result;
            BusinessLayerExceptionBase exception = new BusinessLayerExceptionBase("test",new BusinessLayerValidationException());
            A.CallTo(() => _parcelLogic.Delivered(null)).WithAnyArguments()
                .Throws(exception);
            
            
            
            result = (ObjectResult) _testController.ReportParcelDelivery("");

            result.StatusCode.Should().Be(400);
        }
        
        [Test]
        public void ReportParcelDelivery_BadRequest_FromDataAccessException()
        {
            ObjectResult result;
            BusinessLayerExceptionBase exception = new BusinessLayerExceptionBase("test",new DataAccessExceptionBase());
            A.CallTo(() => _parcelLogic.Delivered(null)).WithAnyArguments()
                .Throws(exception);
            
            
            
            result = (ObjectResult) _testController.ReportParcelDelivery("");
            result.StatusCode.Should().Be(400);
        }
        
        [Test]
        public void ReportParcelDelivery_BadRequest_FromException()
        {
            ObjectResult result;
            A.CallTo(() => _parcelLogic.Delivered(null)).WithAnyArguments()
                .Throws<Exception>();
            
            result = (ObjectResult) _testController.ReportParcelDelivery("");

            result.StatusCode.Should().Be(400);
        }
    }
}