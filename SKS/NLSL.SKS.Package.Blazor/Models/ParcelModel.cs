namespace NLSL.SKS.Package.Blazor.Models
{
    public class ParcelModel
    {
        public double Weight
        {
            get;set; 
        }

        public PersonModel Sender
        {
            get;
            set;
        } = new PersonModel();
        public PersonModel Recipient
        {
            get; set;
        }= new PersonModel();
    }
}
