using System;
using System.Diagnostics.CodeAnalysis;

namespace NLSL.SKS.Package.WebhookManager.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class WebHookManagerExceptionBase : Exception
    {
        public WebHookManagerExceptionBase()
        {
        }

        public WebHookManagerExceptionBase(string message)
            : base(message)
        {
        }

        public WebHookManagerExceptionBase(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}