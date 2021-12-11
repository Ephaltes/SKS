using System;
using System.Diagnostics.CodeAnalysis;

namespace NLSL.SKS.Package.WebhookManager.Entities
{
    [ExcludeFromCodeCoverage]
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