using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

namespace NLSL.SKS.Package.BusinessLogic.Validators
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        public BaseValidator()
        {
            CascadeMode = CascadeMode.Stop;        
        }
    }
}
