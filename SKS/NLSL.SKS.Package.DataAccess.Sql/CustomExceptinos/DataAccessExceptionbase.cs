using System;

namespace NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos
{
    public class DataAccessExceptionbase : Exception
    {
        public DataAccessExceptionbase() 
        {
        }

        public DataAccessExceptionbase(string message)
            : base(message)
        {
        }

        public DataAccessExceptionbase(string message, Exception inner)
            : base(message, inner)
        {
        } 
    }
}