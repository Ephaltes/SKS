using System;
using System.Diagnostics.CodeAnalysis;

namespace NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos
{
    [ExcludeFromCodeCoverage]
    public class DataAccessExceptionBase : Exception
    {
        public DataAccessExceptionBase() 
        {
        }

        public DataAccessExceptionBase(string message)
            : base(message)
        {
        }

        public DataAccessExceptionBase(string message, Exception inner)
            : base(message, inner)
        {
        } 
    }
}