using System;

namespace NLSL.SKS.Package.BusinessLogic.CustomExceptions
{
    public class BusinessLayerDataNotFoundException : Exception
    {
        public BusinessLayerDataNotFoundException() 
        {
        }

        public BusinessLayerDataNotFoundException(string message)
            : base(message)
        {
        }

        public BusinessLayerDataNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        } 
    }
}