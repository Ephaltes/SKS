using System;
using System.Diagnostics.CodeAnalysis;

namespace NLSL.SKS.Package.ServiceAgents.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ServiceAgentsNoDataException : Exception
    {
        public ServiceAgentsNoDataException() 
        {
        }

        public ServiceAgentsNoDataException(string message)
            : base(message)
        {
        }

        public ServiceAgentsNoDataException(string message, Exception inner)
            : base(message, inner)
        {
        } 
    }
}