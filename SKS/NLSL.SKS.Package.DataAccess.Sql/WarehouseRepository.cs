using System.Collections.Generic;
using System.Linq;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NetTopologySuite.Geometries;

using NLSL.SKS.Package.DataAccess.Entities;
using NLSL.SKS.Package.DataAccess.Interfaces;
using NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos;
using NLSL.SKS.Package.DataAccess.Sql.Extensions;

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


        public void Delete(string id)
        {
            try
            {
                _logger.LogDebug("starting, delete warehouse");
                Warehouse temp = new Warehouse
                                 { Code = id };

                _context.Warehouses.Remove(temp);
                _context.SaveChanges();
                _logger.LogDebug("delete warehouse complete");
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
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("Error during Sql Connection", e);
            }
        }


        public Hop? GetWarehouseByCode(string code)
        {
            try
            {
                _logger.LogDebug("starting, get warehouse by code");
                Hop? maybeWarehouse = _context.Warehouses.FirstOrDefault(warehouse => warehouse.Code == code);
                if (maybeWarehouse is not null)
                {
                    _logger.LogDebug("get warehouse by code complete");
                    return maybeWarehouse;
                }
                Hop? maybeTruck = _context.Trucks.FirstOrDefault(warehouse => warehouse.Code == code);
                if (maybeTruck is not null)
                {
                    _logger.LogDebug("get warehouse by code complete");
                    return maybeTruck;
                }
                Hop? maybeTransferwarehouse = _context.Transferwarehouses.FirstOrDefault(warehouse => warehouse.Code == code);
                _logger.LogDebug("get warehouse by code complete");
                return maybeTransferwarehouse;
            }
            catch (SqlException e)
            {
                _logger.LogError(e, $"{e.Message}");

                throw new DataAccessExceptionBase("Error during Sql Connection", e);
            }
        }
        public void DeleteHierarchy()
        {
            _context.Database.ExecuteSqlRaw(_context.WarehouseNextHops.GetSqlDeleteStatementForTable());
            _context.Database.ExecuteSqlRaw(_context.Warehouses.GetSqlDeleteStatementForTable());
            // _context.WarehouseNextHops.RemoveRange(_context.WarehouseNextHops);
            // _context.Warehouses.RemoveRange(_context.Warehouses);
            // _context.Transferwarehouses.RemoveRange(_context.Transferwarehouses);
            // _context.Trucks.RemoveRange(_context.Trucks);
            // _context.WarehouseNextHops.RemoveRange(_context.WarehouseNextHops);

            _context.SaveChanges();
        }
        //Gets a single warehouse which is not referenced by WarehouseNextHops
        public Warehouse? GetRootWarehouse()
        {
            /*
                Warehouse? warehouse = _context.Warehouses
                    .SingleOrDefault(warehouse => !_context.WarehouseNextHops
                                        .Any(wh => wh.Hop.Code == warehouse.Code));
            */

            Warehouse? warehouse = _context.Warehouses.SingleOrDefault(x => x.Level == 0);

            return warehouse;
        }

        public Hop? GetHopForPoint(Point point)
        {
            return _context.Trucks.FirstOrDefault(x => x.RegionGeometry.Contains(point)) ??
                   (Hop?)_context.Transferwarehouses.FirstOrDefault(x => x.RegionGeometry.Contains(point));

        }

        public Warehouse? GetParentOfHopByCode(string code)
        {
            WarehouseNextHops warehouse = _context.WarehouseNextHops.First(x => x.Hop.Code == code);
            return _context.Warehouses.FirstOrDefault(x => x.NextHops.Contains(warehouse));
        }
    }
}