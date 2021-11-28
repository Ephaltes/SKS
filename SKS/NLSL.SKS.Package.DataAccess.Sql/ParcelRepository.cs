using System;
using System.Linq;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.DataAccess.Entities;
using NLSL.SKS.Package.DataAccess.Interfaces;
using NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos;

namespace NLSL.SKS.Package.DataAccess.Sql
{
    public class ParcelRepository : IParcelRepository
    {
        private readonly ILogger<ParcelRepository> _logger;
        private readonly PackageContext _context;
        private static Random _random = new Random();
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int Length = 9;


        public ParcelRepository(PackageContext context, ILogger<ParcelRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public int Create(Parcel parcel)
        {
            try
            {
                _logger.LogDebug("starting, create parcel");
                _context.Parcels.Add(parcel);
                _context.SaveChanges();

                _logger.LogDebug("create parcel complete");

                return parcel.Id;
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("Db Concurrency error", e);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("error during saving", e);
            }
            catch (SqlException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("Error during Sql Connection", e);
            }
        }

        public void Update(Parcel parcel)
        {
            try
            {
                _logger.LogDebug("starting, update parcel");
                _context.Parcels.Update(parcel);
                _context.SaveChanges();

                _logger.LogDebug("update parcel complete");
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("Db Concurrency error", e);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("error during saving", e);
            }
            catch (SqlException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("Error during Sql Connection", e);
            }
        }

        public void Delete(int id)
        {
            try
            {
                _logger.LogDebug("starting, delete parcel");
                Parcel parcel = new()
                                { Id = id };
                _context.Parcels.Remove(parcel);
                _context.SaveChanges();
                _logger.LogDebug("delete parcel complete");
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("Db Concurrency error", e);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("error during saving", e);
            }
            catch (SqlException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("Error during Sql Connection", e);
            }
        }

        public Parcel? GetParcelByTrackingId(string trackingId)
        {
            try
            {
                _logger.LogDebug("starting, get parcel by trackingId");
                Parcel? parcel = _context.Parcels.FirstOrDefault(parcel => parcel.TrackingId == trackingId);
                _logger.LogDebug("get parcel by trackingId complete");
                return parcel;
            }
            catch (SqlException e)
            {
                throw new DataAccessExceptionBase("Error during Sql Connection", e);
            }
        }
        public string GenerateTrackingId()
        {
            try
            {
                string newTrackingId = "";
                do
                {
                    newTrackingId = new string(Enumerable.Repeat(Chars, Length)
                        .Select(s => s[_random.Next(s.Length)]).ToArray());


                } while (_context.Parcels.FirstOrDefault(x => x.TrackingId == newTrackingId) != null);

                return newTrackingId;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,$"{exception.Message}");
                throw new DataAccessExceptionBase("Error during Sql Connection", exception);
            }
        }

        public Parcel? GetById(int id)
        {
            try
            {
                _logger.LogDebug("starting, get parcel by trackingId");
                Parcel? parcel = _context.Parcels.FirstOrDefault(parcel => parcel.Id == id);
                _logger.LogDebug("get parcel by trackingId complete");
                return parcel;
            }
            catch (SqlException e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new DataAccessExceptionBase("Error during Sql Connection", e);
            }
        }
    }
}