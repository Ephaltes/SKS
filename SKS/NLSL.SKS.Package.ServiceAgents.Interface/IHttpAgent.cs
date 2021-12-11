namespace NLSL.SKS.Package.ServiceAgents.Interface
{
    public interface IHttpAgent
    {
        public void SendParcelToLogisticPartnerPost(string logisticPartnerUri, Package.DataAccess.Entities.Parcel parcel);
        public bool PostAsJson(string url, object content);
    }
}