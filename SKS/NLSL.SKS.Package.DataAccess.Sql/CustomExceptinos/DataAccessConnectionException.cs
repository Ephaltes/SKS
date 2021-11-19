using System;

namespace NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos
{
    public class DataAccessConnectionException : Exception
    {
        public DataAccessConnectionException() 
        {
        }

        public DataAccessConnectionException(string message)
            : base(message)
        {
        }

        public DataAccessConnectionException(string message, Exception inner)
            : base(message, inner)
        {
        } 
    }
}