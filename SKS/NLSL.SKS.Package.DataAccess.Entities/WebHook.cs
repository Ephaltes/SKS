using System;

namespace NLSL.SKS.Package.DataAccess.Entities
{
    public class WebHook
    {
        public long? Id
        {
            get;
            set;
        }

        public string trackingId
        {
            get;
            set;
        }

        public string URL
        {
            get;
            set;
        }

        public DateTime? CreatedAt
        {
            get;
            set;
        }
    }
}