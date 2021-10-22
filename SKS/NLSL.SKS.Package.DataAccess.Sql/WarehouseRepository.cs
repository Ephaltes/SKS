using System;
using System.Collections.Generic;
using System.Linq;

using NLSL.SKS.Package.DataAccess.Entities;
using NLSL.SKS.Package.DataAccess.Interfaces;

namespace NLSL.SKS.Package.DataAccess.Sql
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly PackageContext _context;
        public WarehouseRepository(PackageContext context)
        {
            _context = context;
        }
        public string Create(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            _context.SaveChanges();

            return warehouse.Code;
        }
        public void Update(Warehouse warehouse)
        {
            _context.Warehouses.Update(warehouse);
            _context.SaveChanges();
        }
        public void Delete(string id)
        {
            Warehouse temp = new Warehouse
                              { Code = id };

            _context.Warehouses.Remove(temp);
            _context.SaveChanges();
        }
        public IReadOnlyCollection<Warehouse> GetAllWarehouses()
        {
            return _context.Warehouses.ToList();
        }
        public Warehouse? GetWarehouseByCode(string code)
        {
            return _context.Warehouses.FirstOrDefault(warehouse => warehouse.Code == code);
        }
    }
}