using System;
using System.Diagnostics.CodeAnalysis;

namespace NLSL.SKS.Package.WebhookManager.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class WebHookManagerDeleteFailed : Exception
    {
        public WebHookManagerDeleteFailed()
        {
        }

        public WebHookManagerDeleteFailed(string message)
            : base(message)
        {
        }

        public WebHookManagerDeleteFailed(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}