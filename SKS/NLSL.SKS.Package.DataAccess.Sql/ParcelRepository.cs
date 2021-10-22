using System.Linq;

using NLSL.SKS.Pacakge.DataAccess.Entities;
using NLSL.SKS.Pacakge.DataAccess.Interfaces;

namespace NLSL.SKS.Pacakge.DataAccess.Sql
{
    public class ParcelRepository : IParcelRepository
    {
        private PackageContext _context;
        public ParcelRepository(PackageContext context)
        {
            _context = context;
        }
        public int Create(Parcel parcel)
        {
            _context.Parcels.Add(parcel);
            _context.SaveChanges();

            return parcel.Id;
        }
        public void Update(Parcel parcel)
        {
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            Parcel parcel = new Parcel() { Id = id };
            _context.Parcels.Remove(parcel);
            _context.SaveChanges();
        }
        public Parcel? GetParcelByTrackingId(string trackingId)
        {
            return _context.Parcels.FirstOrDefault(parcel => parcel.TrackingId == trackingId);
        }
    }
}