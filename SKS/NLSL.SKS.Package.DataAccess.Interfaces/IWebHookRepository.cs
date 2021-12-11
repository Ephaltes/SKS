using System.Collections.Generic;

using NLSL.SKS.Package.DataAccess.Entities;

namespace NLSL.SKS.Package.DataAccess.Interfaces
{
    public interface IWebHookRepository
    {
        public long? Create(WebHook webhook);
        public void Delete(long? id);
        public IList<WebHook> GetAllWebHooksByTrackingId(string id);
        public WebHook? GetWebHookById(long? id);
    }
}