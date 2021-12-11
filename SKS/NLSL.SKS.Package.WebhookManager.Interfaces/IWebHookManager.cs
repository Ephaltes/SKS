using NLSL.SKS.Package.WebhookManager.Entities;

namespace NLSL.SKS.Package.WebhookManager.Interfaces
{
    public interface IWebHookManager
    {
        public WebhookResponse SubscribeNewWebHook(WebHook webHook);
        public void UnSubscribeWebHook(long? id);
        public void ParcelStateChanged(Parcel parcel);
        public WebhookResponses GetAllByTrackingId(string trackingId);
    }
}