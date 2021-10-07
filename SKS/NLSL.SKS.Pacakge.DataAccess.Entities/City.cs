using System.Collections.Generic;

namespace NLSL.SKS.Pacakge.DataAccess.Entities
{
    public class City
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public List<PostalCode> PostalCodes
        {
            get;
            set;
        } = new List<PostalCode>();
    }
}