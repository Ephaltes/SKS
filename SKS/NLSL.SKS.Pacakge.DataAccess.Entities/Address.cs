namespace NLSL.SKS.Pacakge.DataAccess.Entities
{
    public class Address
    {
        
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// Street
        /// </summary>
        /// <value>Street</value>
        public string Street
        {
            get;
            set;
        }

        /// <summary>
        /// Postalcode
        /// </summary>
        /// <value>Postalcode</value>
        public PostalCode PostalCode
        {
            get;
            set;
        }

        /// <summary>
        /// City
        /// </summary>
        /// <value>City</value>
        public City City
        {
            get;
            set;
        }

        /// <summary>
        /// Country
        /// </summary>
        /// <value>Country</value>
        public Country Country
        {
            get;
            set;
        }
    }
}