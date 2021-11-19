using AutoMapper;

using FakeItEasy;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.Services.Controllers;

using NUnit.Framework;

namespace NLSL.SKS.Package.Services.Tests
{
    public class RecipientApiControllerBehaviour
    {
        private IMapper _mapper;
        private IParcelLogic _parcelLogic;
        private RecipientApiController _testController;
        private ILogger<RecipientApiController> _logger;

        [SetUp]
        public void Setup()
        {
            _parcelLogic = A.Fake<IParcelLogic>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<RecipientApiController>>();

            _testController = new RecipientApiController(_parcelLogic, _mapper,_logger);
        }

        [Test]
        public void TrackParcel_ParcelFound_Success()
        {
            ObjectResult result;
            A.CallTo(() => _parcelLogic.Track(A<TrackingId>.Ignored)).Returns(new Parcel());

            result = (ObjectResult)_testController.TrackParcel("ABCDEFGHI");

            result.StatusCode.Should().Be(200);
        }
        [Test]
        public void TrackParcel_ParcelNotFound_NotFound()
        {
            ObjectResult result;
            A.CallTo(() => _parcelLogic.Track(A<TrackingId>.Ignored)).Returns(null);

            result = (ObjectResult)_testController.TrackParcel("ABCDEFGHI");

            result.StatusCode.Should().Be(404);
        }
    }
}