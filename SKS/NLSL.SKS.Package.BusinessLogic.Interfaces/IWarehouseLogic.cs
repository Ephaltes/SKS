using System.Collections.Generic;

using NLSL.SKS.Package.BusinessLogic.Entities;

namespace NLSL.SKS.Package.BusinessLogic.Interfaces
{
    public interface IWarehouseLogic
    {
        public Hop? Get(WarehouseCode warehouseCode);
        public IReadOnlyCollection<Warehouse> GetAll();
        public bool ReplaceHierarchy(Warehouse warehouse);
    }
}