using System.Collections.Generic;

using NLSL.SKS.Pacakge.DataAccess.Entities;

namespace NLSL.SKS.Pacakge.DataAccess.Interfaces
{
    public interface IWarehouseRepository
    {
        public string Create(Warehouse warehouse);
        public void Update(Warehouse warehouse);
        public void Delete(string id);

        //Gets
        public IReadOnlyCollection<Warehouse> GetAllWarehouses();
        public Warehouse? GetWarehouseByCode(string code);
    }
}