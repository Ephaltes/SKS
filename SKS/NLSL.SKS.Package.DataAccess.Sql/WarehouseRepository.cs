using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.DataAccess.Entities;
using NLSL.SKS.Package.DataAccess.Interfaces;
using NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos;

namespace NLSL.SKS.Package.DataAccess.Sql
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly PackageContext _context;
        private readonly ILogger<WarehouseRepository> _logger;

        public WarehouseRepository(PackageContext context, ILogger<WarehouseRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public string Create(Warehouse warehouse)
        {
            try
            {
                _logger.LogDebug("starting, create warehouse");
                _context.Warehouses.Add(warehouse);
                _context.SaveChanges();
                _logger.LogDebug("create warehouse complete");

                return warehouse.Code;
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new DataAccessExceptionbase("Db Concurrency error", e);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new DataAccessExceptionbase("error during saving", e);
            }
            catch (SqlException e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new DataAccessExceptionbase("Error during Sql Connection", e);
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"{e.Message}");

                throw new DataAccessExceptionbase("something went wrong", e);
            }
        }


        public void Update(Warehouse warehouse)
        {
            try
            {
                _logger.LogDebug("starting, update warehouse");
                _context.Warehouses.Update(warehouse);
                _context.SaveChanges();
                _logger.LogDebug("update warehouse complete");
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new DataAccessExceptionbase("Db Concurrency error", e);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new DataAccessExceptionbase("error during saving", e);
            }
            catch (SqlException e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new DataAccessExceptionbase("Error during Sql Connection", e);
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"{e.Message}");

                throw new DataAccessExceptionbase("something went wrong", e);
            }
        }


        public void Delete(string id)
        {
            try
            {
                _logger.LogDebug("starting, delete warehouse");
                Warehouse temp = new()
                                 {Code = id};

                _context.Warehouses.Remove(temp);
                _context.SaveChanges();
                _logger.LogDebug("delete warehouse complete");
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new DataAccessExceptionbase("Db Concurrency error", e);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new DataAccessExceptionbase("error during saving", e);
            }
            catch (SqlException e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new DataAccessExceptionbase("Error during Sql Connection", e);
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"{e.Message}");

                throw new DataAccessExceptionbase("something went wrong", e);
            }
        }


        public IReadOnlyCollection<Warehouse> GetAllWarehouses()
        {
            try
            {
                _logger.LogDebug("starting, get all warehouses");
                List<Warehouse>? warehouses = _context.Warehouses.ToList();
                _logger.LogDebug("get all warehouses complete");

                return warehouses;
            }
            catch (SqlException e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new DataAccessExceptionbase("Error during Sql Connection", e);
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"{e.Message}");

                throw new DataAccessExceptionbase("something went wrong", e);
            }
        }


        public Warehouse? GetWarehouseByCode(string code)
        {
            try
            {
                _logger.LogDebug("starting, get warehouse by code");
                Warehouse? warehouse = _context.Warehouses.FirstOrDefault(warehouse => warehouse.Code == code);
                _logger.LogDebug("get warehouse by code complete");

                return warehouse;
            }
            catch (SqlException e)
            {
                _logger.LogError(e,$"{e.Message}");
                throw new DataAccessExceptionbase("Error during Sql Connection", e);
            }
        }
    }
}