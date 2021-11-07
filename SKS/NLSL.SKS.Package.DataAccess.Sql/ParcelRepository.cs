using System.Linq;

using Castle.Core.Logging;

using Microsoft.Extensions.Logging;

using NLSL.SKS.Package.DataAccess.Entities;
using NLSL.SKS.Package.DataAccess.Interfaces;

namespace NLSL.SKS.Package.DataAccess.Sql
{
    public class ParcelRepository : IParcelRepository
    {
        private PackageContext _context;
        private readonly ILogger<ParcelRepository> _logger;
        public ParcelRepository(PackageContext context, ILogger<ParcelRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public int Create(Parcel parcel)
        {
            _logger.LogDebug("starting, create parcel");
            _context.Parcels.Add(parcel);
            _context.SaveChanges();
            
            _logger.LogDebug("create parcel complete");
            return parcel.Id;
        }
        public void Update(Parcel parcel)
        {
            _logger.LogDebug("starting, update parcel");
            _context.Parcels.Update(parcel);
            _context.SaveChanges();
            
            _logger.LogDebug("update parcel complete");
        }
        public void Delete(int id)
        {
            _logger.LogDebug("starting, delete parcel");
            Parcel parcel = new Parcel() { Id = id };
            _context.Parcels.Remove(parcel);
            _context.SaveChanges();
            _logger.LogDebug("delete parcel complete");
        }
        public Parcel? GetParcelByTrackingId(string trackingId)
        {
            _logger.LogDebug("starting, get parcel by trackingId");
            var parcel = _context.Parcels.FirstOrDefault(parcel => parcel.TrackingId == trackingId);
            _logger.LogDebug("get parcel by trackingId complete");
            return parcel;
        }
        public Parcel? GetById(int id)
        {
            _logger.LogDebug("starting, get parcel by trackingId");
            var parcel = _context.Parcels.FirstOrDefault(parcel => parcel.Id == id);
            _logger.LogDebug("get parcel by trackingId complete");
            return parcel;
        }
    }
}