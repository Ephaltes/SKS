using System.Collections.Generic;

using FakeItEasy;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.DataAccess.Entities;
using NLSL.SKS.Package.DataAccess.Sql;

using NUnit.Framework;

namespace NLSL.SKS.Package.DataAccess.Tests
{
    public class WebHookRepositoryTests
    {
        private PackageContext _context;
        private ILogger<WebHookRepository> _logger;
        private WebHookRepository _repository;

        [SetUp]
        public void Setup()
        {
            DbContextOptions<PackageContext> options = new();
            _context = A.Fake<PackageContext>(x =>
                                                  x.WithArgumentsForConstructor(() => new PackageContext(options)));
            _logger = A.Fake<ILogger<WebHookRepository>>();
            _repository = new WebHookRepository(_context, _logger);

            A.CallTo(() => _context.Database.EnsureCreated()).Returns(true);
        }

        [Test]
        public void Create_Valid_longID()
        {
            A.CallTo(() => _context.WebHooks).Returns(DbContextMock.GetQueryableMockDbSet(new List<WebHook>()));
            A.CallTo(() => _context.WebHooks.Add(null)).WithAnyArguments().Returns(null);
            A.CallTo(() => _context.SaveChanges()).Returns(1);

            WebHook? newWarehouse = new()
                                    {
                                        trackingId = "testWarehouse"
                                    };


            long? result = _repository.Create(newWarehouse);

            A.CallTo(() => _context.WebHooks.Add(null)).WithAnyArguments().MustHaveHappenedOnceExactly();
            A.CallTo(() => _context.SaveChanges()).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void Delete_Valid_longID()
        {
            A.CallTo(() => _context.WebHooks).Returns(DbContextMock.GetQueryableMockDbSet(new List<WebHook>()));
            A.CallTo(() => _context.WebHooks.Remove(null)).WithAnyArguments().Returns(null);
            A.CallTo(() => _context.SaveChanges()).Returns(1);

            WebHook? newWarehouse = new()
                                    {
                                        trackingId = "testWarehouse"
                                    };


            _repository.Delete(1);

            A.CallTo(() => _context.WebHooks.Remove(null)).WithAnyArguments().MustHaveHappenedOnceExactly();
            A.CallTo(() => _context.SaveChanges()).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetAllWebHooksByTrackingId_Valid_returns2webhooks()
        {
            WebHook? a = new()
                         {
                             Id = 1,
                             trackingId = "a"
                         };
            WebHook? b = new()
                         {
                             Id = 2,
                             trackingId = "a"
                         };

            A.CallTo(() => _context.WebHooks).Returns(DbContextMock.GetQueryableMockDbSet(new List<WebHook>
                                                                                          {
                                                                                              a, b,
                                                                                              new()
                                                                                              {
                                                                                                  Id = 3,
                                                                                                  trackingId = "2"
                                                                                              }
                                                                                          }));

            IList<WebHook>? result = _repository.GetAllWebHooksByTrackingId("a");

            result.Should().Contain(a);
            result.Should().Contain(b);
            result.Count.Should().Be(2);
        }


        [Test]
        public void GetWebHookById_valid_returnsonewebhook()
        {
            A.CallTo(() => _context.WebHooks).Returns(DbContextMock.GetQueryableMockDbSet(new List<WebHook>
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
                                                                                          }));

            WebHook? result = _repository.GetWebHookById(3);

            result.Id.Should().Be(3);
            result.trackingId.Should().Be("2");
        }
    }
}