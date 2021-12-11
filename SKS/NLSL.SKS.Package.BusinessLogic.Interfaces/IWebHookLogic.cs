using NLSL.SKS.Package.BusinessLogic.Entities;

namespace NLSL.SKS.Package.BusinessLogic.Interfaces
{
    public interface IWebHookLogic
    {
        public WebhookResponse? Add(WebHook webHook);
        public void Remove(long? id);
        public Entities.WebhookResponses GetByTrackingId(TrackingId id);
    }
}