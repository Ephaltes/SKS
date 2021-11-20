using System;
using System.Diagnostics.CodeAnalysis;

namespace NLSL.SKS.Package.BusinessLogic.CustomExceptions
{
    [ExcludeFromCodeCoverage]
    public class BusinessLayerExceptionBase : Exception
    {
        public BusinessLayerExceptionBase() 
        {
        }

        public BusinessLayerExceptionBase(string message)
            : base(message)
        {
        }

        public BusinessLayerExceptionBase(string message, Exception inner)
            : base(message, inner)
        {
        } 
    }
}