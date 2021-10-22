using NLSL.SKS.Pacakge.DataAccess.Entities;

namespace NLSL.SKS.Pacakge.DataAccess.Interfaces
{
    public interface IParcelRepository
    {
        public int Create(Parcel parcel);
        public void Update(Parcel parcel);
        public void Delete(int id);
        
        //Gets
        public Parcel? GetParcelByTrackingId(string trackingId);
    }
}