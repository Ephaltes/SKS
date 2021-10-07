using AutoMapper;

using FakeItEasy;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using NLSL.SKS.Package.BusinessLogic.Entities;
using NLSL.SKS.Package.BusinessLogic.Interfaces;
using NLSL.SKS.Package.Services.Controllers;

using NUnit.Framework;

namespace NLSL.SKS.Package.Services.Tests
{
    public class RecipientApiControllerBehaviour
    {
        private IMapper _mapper;
        private IParcelManagement _parcelManagement;
        private RecipientApiController _testController;

        [SetUp]
        public void Setup()
        {
            _parcelManagement = A.Fake<IParcelManagement>();
            _mapper = A.Fake<IMapper>();

            _testController = new RecipientApiController(_parcelManagement, _mapper);
        }

        [Test]
        public void TrackParcel_ParcelFound_Success()
        {
            ObjectResult result;
            A.CallTo(() => _parcelManagement.Track(A<TrackingId>.Ignored)).Returns(new Parcel());

            result = (ObjectResult)_testController.TrackParcel("ABCDEFGHI");

            result.StatusCode.Should().Be(200);
        }
        [Test]
        public void TrackParcel_ParcelNotFound_BadRequest()
        {
            ObjectResult result;
            A.CallTo(() => _parcelManagement.Track(A<TrackingId>.Ignored)).Returns(null);

            result = (ObjectResult)_testController.TrackParcel("ABCDEFGHI");

            result.StatusCode.Should().Be(400);
        }
    }
}