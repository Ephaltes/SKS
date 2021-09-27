using System.Collections.Generic;

namespace NLSL.SKS.Package.BusinessLogic.Interfaces
{
    public interface IWarehouseManagement
    {
        public Warehouse Get(string warehouseCode);
        public IReadOnlyCollection<Warehouse> GetAll();
        public bool Add(Warehouse);
    }
}