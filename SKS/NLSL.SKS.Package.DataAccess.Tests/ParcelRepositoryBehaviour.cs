using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Castle.Core.Logging;

using NUnit.Framework;
using FakeItEasy;

using FizzWare.NBuilder;

using FluentAssertions;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.DataAccess.Entities;
using NLSL.SKS.Package.DataAccess.Sql;
using NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos;

using NUnit.Framework.Constraints;

using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace NLSL.SKS.Package.DataAccess.Tests
{
    public class ParcelRepositoryBehaviour
    {
        private ParcelRepository _repository;
        private PackageContext _context;
        private ILogger<ParcelRepository> _logger;
        
        [SetUp]
        public void Setup()
        {
            DbContextOptions<PackageContext> options = new DbContextOptions<PackageContext>();
            _context = A.Fake<PackageContext>(x=> 
                                                  x.WithArgumentsForConstructor(() => new PackageContext(options)));
            _logger = A.Fake<ILogger<ParcelRepository>>();
            
            _repository = new ParcelRepository(_context,_logger);
            
            A.CallTo(() => _context.Database.EnsureCreated()).Returns(true);
        }
        [Test]
        public void Create_PackageIsValid_intId()
        {
            A.CallTo(() => _context.Parcels).Returns(DbContextMock.GetQueryableMockDbSet(new List<Parcel>()));
            A.CallTo(() => _context.Parcels.Add(null)).WithAnyArguments().Returns(null);
            A.CallTo(() => _context.SaveChanges()).Returns(1);
            
            var newParcel = new Parcel()
                            {
                                Id = 1
                            };

            
            var result = _repository.Create(newParcel);
            
            A.CallTo(() => _context.Parcels.Add(null)).WithAnyArguments().MustHaveHappenedOnceExactly();
            A.CallTo(() => _context.SaveChanges()).MustHaveHappenedOnceExactly();
            result.Should().Be(1);
        }
        [Test]
        public void Update_PackageIsValid_intId()
        {
            A.CallTo(() => _context.Parcels).Returns(DbContextMock.GetQueryableMockDbSet(new List<Parcel>()));
            A.CallTo(() => _context.Parcels.Update(null)).WithAnyArguments().Returns(null);
            A.CallTo(() => _context.SaveChanges()).Returns(1);
            
            var newParcel = new Parcel()
                            {
                                Id = 1
                            };

            
            _repository.Update(newParcel);
            
            A.CallTo(() => _context.Parcels.Update(null)).WithAnyArguments().MustHaveHappenedOnceExactly();
            A.CallTo(() => _context.SaveChanges()).MustHaveHappenedOnceExactly();
        }
        [Test]
        public void Remove_PackageIsValid_intId()
        {
            A.CallTo(() => _context.Parcels).Returns(DbContextMock.GetQueryableMockDbSet(new List<Parcel>()));
            A.CallTo(() => _context.Parcels.Remove(null)).WithAnyArguments().Returns(null);
            A.CallTo(() => _context.SaveChanges()).Returns(1);
            
            var newParcel = new Parcel()
                            {
                                Id = 1
                            };

            
            _repository.Delete(1);
            
            A.CallTo(() => _context.Parcels.Remove(null)).WithAnyArguments().MustHaveHappenedOnceExactly();
            A.CallTo(() => _context.SaveChanges()).MustHaveHappenedOnceExactly();
        }
        [Test]
        public void GetParcelByTrackingId_PackageExists_Package()
        {
            A.CallTo(() => _context.Parcels).Returns(DbContextMock.GetQueryableMockDbSet(new List<Parcel>()
                                                                                         {
                                                                                             new Parcel()
                                                                                             {
                                                                                                 Id =1,
                                                                                                 TrackingId = "1"
                                                                                             },
                                                                                             new Parcel()
                                                                                             {
                                                                                                 Id =2,
                                                                                                 TrackingId = "2"
                                                                                             }
                                                                                         }));

            var result = _repository.GetParcelByTrackingId("1");

            result.TrackingId.Should().Be("1");
        }
        [Test]
        public void GetParcelByTrackingId_PackageDoesNotExist_null()
        {
            A.CallTo(() => _context.Parcels).Returns(DbContextMock.GetQueryableMockDbSet(new List<Parcel>()));

            var result =  _repository.GetParcelByTrackingId("aaaaaaaaaaaaaa");
            
            result.Should().Be(null);
        }
        [Test]
        public void GetById_PackageExists_Package()
        {
            A.CallTo(() => _context.Parcels).Returns(DbContextMock.GetQueryableMockDbSet(new List<Parcel>()
                                                                                         {
                                                                                             new Parcel()
                                                                                             {
                                                                                                 Id =1,
                                                                                                 TrackingId = "1"
                                                                                             },
                                                                                             new Parcel()
                                                                                             {
                                                                                                 Id =2,
                                                                                                 TrackingId = "2"
                                                                                             }
                                                                                         }));

            var result = _repository.GetById(1);

            result.Id.Should().Be(1);
        }
        [Test]
        public void GetById_PackageDoesNotExist_null()
        {
            A.CallTo(() => _context.Parcels).Returns(DbContextMock.GetQueryableMockDbSet(new List<Parcel>()));


            var result =  _repository.GetById(1);
            
            result.Should().Be(null);
        }

        [Test]
        public void Create_Throws_DbUpdateConcurencyException()
        {
            A.CallTo(() => _context.Parcels).Returns(DbContextMock.GetQueryableMockDbSet(new List<Parcel>()));
            A.CallTo(() => _context.Parcels.Add(null)).WithAnyArguments()
                .Throws<DbUpdateConcurrencyException>();
            
            Action action = () => _repository.Create(null);
            
            action.Should().Throw<DataAccessExceptionBase>().WithInnerException<DbUpdateConcurrencyException>();
        }
        
        [Test]
        public void Create_Throws_DbUpdateException()
        {
            A.CallTo(() => _context.Parcels).Returns(DbContextMock.GetQueryableMockDbSet(new List<Parcel>()));
            A.CallTo(() => _context.Parcels.Add(null)).WithAnyArguments()
                .Throws<DbUpdateException>();
            
            Action action = () => _repository.Create(null);
            
            action.Should().Throw<DataAccessExceptionBase>().WithInnerException<DbUpdateException>();
        }
        
        [Test]
        public void Update_Throws_DbUpdateConcurencyException()
        {
            A.CallTo(() => _context.Parcels).Returns(DbContextMock.GetQueryableMockDbSet(new List<Parcel>()));
            A.CallTo(() => _context.Parcels.Update(null)).WithAnyArguments()
                .Throws<DbUpdateConcurrencyException>();
            
            Action action = () => _repository.Update(null);
            
            action.Should().Throw<DataAccessExceptionBase>().WithInnerException<DbUpdateConcurrencyException>();
        }
        
        [Test]
        public void Update_Throws_DbUpdateException()
        {
            A.CallTo(() => _context.Parcels).Returns(DbContextMock.GetQueryableMockDbSet(new List<Parcel>()));
            A.CallTo(() => _context.Parcels.Update(null)).WithAnyArguments()
                .Throws<DbUpdateException>();
            
            Action action = () => _repository.Update(null);
            
            action.Should().Throw<DataAccessExceptionBase>().WithInnerException<DbUpdateException>();
        }
        
        [Test]
        public void Delete_Throws_DbUpdateConcurencyException()
        {
            A.CallTo(() => _context.Parcels).Returns(DbContextMock.GetQueryableMockDbSet(new List<Parcel>()));
            A.CallTo(() => _context.Parcels.Remove(null)).WithAnyArguments()
                .Throws<DbUpdateConcurrencyException>();
            
            Action action = () => _repository.Delete(1);
            
            action.Should().Throw<DataAccessExceptionBase>().WithInnerException<DbUpdateConcurrencyException>();
        }
        
        [Test]
        public void Delete_Throws_DbUpdateException()
        {
            A.CallTo(() => _context.Parcels).Returns(DbContextMock.GetQueryableMockDbSet(new List<Parcel>()));
            A.CallTo(() => _context.Parcels.Remove(null)).WithAnyArguments()
                .Throws<DbUpdateException>();
            
            Action action = () => _repository.Delete(1);
            
            action.Should().Throw<DataAccessExceptionBase>().WithInnerException<DbUpdateException>();
        }
        
       
    }
}