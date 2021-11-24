using System.Collections.Generic;

using NetTopologySuite.Geometries;

using NLSL.SKS.Package.DataAccess.Entities;

namespace NLSL.SKS.Package.DataAccess.Interfaces
{
    public interface IWarehouseRepository
    {
        public string Create(Warehouse warehouse);
        public void Update(Warehouse warehouse);
        public void Delete(string id);

        //Gets
        public IReadOnlyCollection<Warehouse> GetAllWarehouses();
        public Warehouse? GetWarehouseByCode(string code);
        
        public void DeleteHierarchy();
        
        public Warehouse? GetRootWarehouse();

        public Hop? GetHopForPoint(Point point);

        public Warehouse? GetParentOfHopByCode(string code);
    }
}