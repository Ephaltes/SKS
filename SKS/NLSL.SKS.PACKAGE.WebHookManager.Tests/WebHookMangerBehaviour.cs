using System.Collections.Generic;

using AutoMapper;

using FakeItEasy;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.DataAccess.Interfaces;
using NLSL.SKS.Package.ServiceAgents.Interface;
using NLSL.SKS.Package.WebhookManager;
using NLSL.SKS.Package.WebhookManager.Entities;
using NLSL.SKS.Package.WebhookManager.Entities.Enums;
using NLSL.SKS.Package.WebhookManager.Exceptions;

using NUnit.Framework;

namespace NLSL.SKS.PACKAGE.WebHookManager.Tests
{
    public class Tests
    {
        private IHttpAgent _httpAgent;
        private ILogger<WebhookManager> _logger;
        private IMapper _mapper;
        private WebhookManager _webHookManager;
        private IWebHookRepository _webHookRepository;

        [SetUp]
        public void Setup()
        {
            _httpAgent = A.Fake<IHttpAgent>();
            _logger = A.Fake<ILogger<WebhookManager>>();
            _mapper = A.Fake<IMapper>();
            _webHookRepository = A.Fake<IWebHookRepository>();
            _webHookManager = new(_webHookRepository, _httpAgent, _logger, _mapper);
        }

        [Test]
        public void SubscribeNewWebhook_ValidWebhook_success()
        {
            WebHook WebHook = new();
            WebhookResponse Response = new()
                                       {
                                           TrackingId = "ABCDEFGHI"
                                       };
            Package.DataAccess.Entities.WebHook WebHookDal = new();

            A.CallTo(_mapper).Where(call => call.Method.Name == "Map").WithReturnType<Package.DataAccess.Entities.WebHook>().WithAnyArguments().Returns(WebHookDal);
            A.CallTo(_mapper).Where(call => call.Method.Name == "Map").WithReturnType<WebhookResponse>().WithAnyArguments().Returns(Response);

            A.CallTo(() => _webHookRepository.Create(null)).WithAnyArguments().Returns(1L);

            WebhookResponse result = _webHookManager.SubscribeNewWebHook(WebHook);

            A.CallTo(() => _webHookRepository.Create(null)).WithAnyArguments().MustHaveHappenedOnceExactly();
            result.TrackingId.Should().Be("ABCDEFGHI");
        }

        [Test]
        public void UnSubscribeNewWebhook_ValidWebhook_success()
        {
            _webHookManager.UnSubscribeWebHook(1L);

            A.CallTo(() => _webHookRepository.Delete(null)).WithAnyArguments().MustHaveHappenedOnceExactly();
        }

        [Test]
        public void ParcelStateChanged_ValidWebhook_success()
        {
            A.CallTo(() => _webHookRepository.GetAllWebHooksByTrackingId(null)).WithAnyArguments().Returns(new List<Package.DataAccess.Entities.WebHook>
                                                                                                           {
                                                                                                               new()
                                                                                                               {
                                                                                                                   trackingId = "a"
                                                                                                               }
                                                                                                           });
            //A.CallTo(() => _httpAgent.PostAsJson(null, null)).WithAnyArguments().

            _webHookManager.ParcelStateChanged(new Parcel
                                               {
                                                   TrackingId = "a",
                                                   State = StateEnum.InTransport
                                               });
            A.CallTo(() => _httpAgent.PostAsJson(null, null)).WithAnyArguments().MustHaveHappenedOnceExactly();
            A.CallTo(() => _webHookRepository.Delete(null)).WithAnyArguments().MustNotHaveHappened();
        }

        [Test]
        public void ParcelStateChanged_ValidWebhookWithTranserred_success()
        {
            A.CallTo(() => _webHookRepository.GetAllWebHooksByTrackingId(null)).WithAnyArguments().Returns(new List<Package.DataAccess.Entities.WebHook>
                                                                                                           {
                                                                                                               new()
                                                                                                               {
                                                                                                                   trackingId = "a"
                                                                                                               }
                                                                                                           });
            A.CallTo(() => _httpAgent.PostAsJson(null, null)).WithAnyArguments().Returns(true);

            _webHookManager.ParcelStateChanged(new Parcel
                                               {
                                                   TrackingId = "a",
                                                   State = StateEnum.Delivered
                                               });

            A.CallTo(() => _httpAgent.PostAsJson(null, null)).WithAnyArguments().MustHaveHappenedOnceExactly();
            A.CallTo(() => _webHookRepository.Delete(null)).WithAnyArguments().MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetAllByTrackingId_ValidWebhook_returnsWebhooks()
        {
            A.CallTo(() => _webHookRepository.GetAllWebHooksByTrackingId(null)).WithAnyArguments().Returns(new List<Package.DataAccess.Entities.WebHook>
                                                                                                           {
                                                                                                               new()
                                                                                                               {
                                                                                                                   Id = 1,
                                                                                                                   trackingId = "a"
                                                                                                               },
                                                                                                               new()
                                                                                                               {
                                                                                                                   Id = 2,
                                                                                                                   trackingId = "a"
                                                                                                               },
                                                                                                               new()
                                                                                                               {
                                                                                                                   Id = 3,
                                                                                                                   trackingId = "2"
                                                                                                               }
                                                                                                           });
            var x = new WebhookResponses();
            
            x.Add(new WebhookResponse()
                      {
                          Id = 1,
                          TrackingId = "a"
                      });
            x.Add(new WebhookResponse()
                  {
                      Id = 2,
                      TrackingId = "a"
                  });
            A.CallTo(_mapper).Where(call => call.Method.Name == "Map").WithReturnType<WebhookResponses>().WithAnyArguments().Returns(x);

            var result = _webHookManager.GetAllByTrackingId("a");
            result.Count.Should().Be(2);

            A.CallTo(() => _webHookRepository.GetAllWebHooksByTrackingId(null)).WithAnyArguments().MustHaveHappenedOnceExactly();
        }
    }
}