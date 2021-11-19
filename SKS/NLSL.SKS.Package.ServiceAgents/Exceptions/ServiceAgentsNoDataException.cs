using System;

namespace NLSL.SKS.Package.ServiceAgents.Exceptions
{
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