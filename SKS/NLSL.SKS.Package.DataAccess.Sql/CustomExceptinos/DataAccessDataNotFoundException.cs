using System;
using System.Diagnostics.CodeAnalysis;

namespace NLSL.SKS.Package.DataAccess.Sql.CustomExceptinos
{
    [ExcludeFromCodeCoverage]
    public class DataAccessDataNotFoundException : Exception
    {
        public DataAccessDataNotFoundException() 
        {
        }

        public DataAccessDataNotFoundException(string message)
            : base(message)
        {
        }

        public DataAccessDataNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        } 
    }
}