using System;

namespace NLSL.SKS.Package.ServiceAgents.Exceptions
{
    public class ServiceAgentHttpRequestFailed : Exception
    {
        public ServiceAgentHttpRequestFailed() 
        {
        }

        public ServiceAgentHttpRequestFailed(string message)
            : base(message)
        {
        }

        public ServiceAgentHttpRequestFailed(string message, Exception inner)
            : base(message, inner)
        {
        } 
    }
}