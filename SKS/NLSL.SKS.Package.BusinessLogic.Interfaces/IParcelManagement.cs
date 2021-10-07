using System;

using NLSL.SKS.Package.BusinessLogic.Entities;

namespace NLSL.SKS.Package.BusinessLogic.Interfaces
{
    public interface IParcelManagement
    {
        public Parcel? Transition(Parcel parcel);

        public Parcel? Track(TrackingId trackingId);

        public Parcel? Submit(Parcel parcel);

        public bool? Delivered(TrackingId trackingId);

        public bool ReportHop(ReportHop reportHop);
    }
}
