using NLSL.SKS.Package.DataAccess.Entities;

namespace NLSL.SKS.Package.DataAccess.Interfaces
{
    public interface IParcelRepository
    {
        public int Create(Parcel parcel);
        public void Update(Parcel parcel);
        public void Delete(int id);
        public Parcel? GetById(int id);
        
        //Gets
        public Parcel? GetParcelByTrackingId(string trackingId);
    }
}