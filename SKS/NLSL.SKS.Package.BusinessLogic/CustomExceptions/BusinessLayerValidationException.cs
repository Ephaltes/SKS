using System;

namespace NLSL.SKS.Package.BusinessLogic.CustomExceptions
{
    public class BusinessLayerValidationException : Exception
    {
        public BusinessLayerValidationException() 
        {
        }

        public BusinessLayerValidationException(string message)
            : base(message)
        {
        }

        public BusinessLayerValidationException(string message, Exception inner)
            : base(message, inner)
        {
        } 
    }
}