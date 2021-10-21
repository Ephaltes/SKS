using System;

using AutoMapper;

using FakeItEasy;

using FluentAssertions;
using NUnit.Framework;
using NLSL.SKS.Package.Services.DTOs;
using NLSL.SKS.Package.Services.Controllers;
using Microsoft.AspNetCore.Mvc;

using NLSL.SKS.Package.BusinessLogic.Interfaces;

namespace NLSL.SKS.Package.Services.Tests
{
    public class SenderApiControllerBehaviour
    {
        private Parcel _testParcel = new();
        private Recipient _testSender = new();
        private Recipient _testRecipient = new();
        private SenderApiController _testController;
        private IMapper _mapper;
        private IParcelLogic _parcelLogic;
        [SetUp]
        public void Setup()
        {
            _parcelLogic = A.Fake<IParcelLogic>();
            _mapper = A.Fake<IMapper>();

            _testController = new SenderApiController(_parcelLogic, _mapper);

            _testParcel = new();
            _testSender = new();
            _testRecipient = new();

            _testSender.City = "testSender.City";
            _testSender.Country = "testSender.Country";
            _testSender.Name = "testSender.Name";
            _testSender.Street = "testSender.Street";
            _testSender.PostalCode = "testSender.PostalCode";

            _testRecipient.City = "testRecipient.City";
            _testRecipient.Country = "testRecipient.Country";
            _testRecipient.Name = "testRecipient.Name";
            _testRecipient.Street = "testRecipient.Street";
            _testRecipient.PostalCode = "testRecipient.PostalCode";

            _testParcel.Weight = 1;
            _testParcel.Sender = _testSender;
            _testParcel.Recipient = _testRecipient;
        }

        [Test]
        public void SubmitParcel_ValidParcel_Success()
        {
            ObjectResult result;
            A.CallTo(() => _parcelLogic.Submit(A<BusinessLogic.Entities.Parcel>.Ignored)).Returns(new BusinessLogic.Entities.Parcel());
            
            result = (ObjectResult) _testController.SubmitParcel(_testParcel);

            result.StatusCode.Should().Be(201);
        }
        [Test]
        public void SubmitParcel_InvalidParcel_BadRequest()
        {
            ObjectResult result;
            A.CallTo(() => _parcelLogic.Submit(A<BusinessLogic.Entities.Parcel>.Ignored)).Returns(null);
            
            result = (ObjectResult) _testController.SubmitParcel(_testParcel);

            result.StatusCode.Should().Be(400);
        }
    }
}
