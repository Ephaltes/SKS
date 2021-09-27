using System;

namespace NLSL.SKS.Package.BusinessLogic.Interfaces
{
    public interface IParcelManagement
    {
        public Parcel Transition(Parcel parcel);

        public Parcel Track(string trackingId);

        public Parcel Submit(Parcel parcel);

        public bool Delivered(string trackingID);

        public bool ReportHop(string trackingId, string hopCode);
    }
}
