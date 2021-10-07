namespace NLSL.SKS.Pacakge.DataAccess.Entities
{
    public class LogisticPartner
    {
        /// <summary>
        /// Name of the logistics partner.
        /// </summary>
        /// <value>Name of the logistics partner.</value>
        public string LogisticsPartner
        {
            get;
            set;
        }

        /// <summary>
        /// BaseURL of the logistics partner&#x27;s REST service.
        /// </summary>
        /// <value>BaseURL of the logistics partner&#x27;s REST service.</value>
        public string LogisticsPartnerUrl
        {
            get;
            set;
        }
    }
}