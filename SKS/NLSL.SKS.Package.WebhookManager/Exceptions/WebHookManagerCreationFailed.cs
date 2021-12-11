using System;
using System.Diagnostics.CodeAnalysis;

namespace NLSL.SKS.Package.WebhookManager.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class WebHookManagerCreationFailed : Exception
    {
        public WebHookManagerCreationFailed()
        {
        }

        public WebHookManagerCreationFailed(string message)
            : base(message)
        {
        }

        public WebHookManagerCreationFailed(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}