using System.Collections.Generic;

using NLSL.SKS.Package.BusinessLogic.Entities;

namespace NLSL.SKS.Package.BusinessLogic.Interfaces
{
    public interface IWarehouseManagement
    {
        public Warehouse? Get(WarehouseCode warehouseCode);
        public IReadOnlyCollection<Warehouse> GetAll();
        public bool Add(Warehouse warehouse);
    }
}