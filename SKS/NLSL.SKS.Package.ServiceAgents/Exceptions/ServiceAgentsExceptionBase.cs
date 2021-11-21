using System;
using System.Diagnostics.CodeAnalysis;

namespace NLSL.SKS.Package.ServiceAgents.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ServiceAgentsExceptionBase : Exception
    {
        public ServiceAgentsExceptionBase() 
        {
        }

        public ServiceAgentsExceptionBase(string message)
            : base(message)
        {
        }

        public ServiceAgentsExceptionBase(string message, Exception inner)
            : base(message, inner)
        {
        } 
    }
}