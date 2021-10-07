using System.Collections.Generic;

namespace NLSL.SKS.Pacakge.DataAccess.Entities
{
    public class Country
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

        public List<City> Cities
        {
            get;
            set;
        } = new List<City>();
    }
}