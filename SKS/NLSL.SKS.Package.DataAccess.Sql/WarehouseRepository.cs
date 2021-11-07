using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.DataAccess.Entities;
using NLSL.SKS.Package.DataAccess.Interfaces;

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
            _logger.LogDebug("starting, create warehouse");
            _context.Warehouses.Add(warehouse);
            _context.SaveChanges();
            _logger.LogDebug("create warehouse complete");

            return warehouse.Code;
        }
        public void Update(Warehouse warehouse)
        {
            _logger.LogDebug("starting, update warehouse");
            _context.Warehouses.Update(warehouse);
            _context.SaveChanges();
            _logger.LogDebug("update warehouse complete");
        }
        public void Delete(string id)
        {
            _logger.LogDebug("starting, delete warehouse");
            Warehouse temp = new Warehouse
                             { Code = id };

            _context.Warehouses.Remove(temp);
            _context.SaveChanges();
            _logger.LogDebug("delete warehouse complete");
        }
        public IReadOnlyCollection<Warehouse> GetAllWarehouses()
        {
            _logger.LogDebug("starting, get all warehouses");
            List<Warehouse>? warehouses = _context.Warehouses.ToList();
            _logger.LogDebug("get all warehouses complete");

            return warehouses;
        }
        public Warehouse? GetWarehouseByCode(string code)
        {
            _logger.LogDebug("starting, get warehouse by code");
            Warehouse? warehouse = _context.Warehouses.FirstOrDefault(warehouse => warehouse.Code == code);
            _logger.LogDebug("get warehouse by code complete");

            return warehouse;
        }
    }
}