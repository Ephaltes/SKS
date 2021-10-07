using System.Drawing;

using FluentValidation;

using NLSL.SKS.Package.BusinessLogic.Entities;

namespace NLSL.SKS.Package.BusinessLogic.Validators
{
    public class ReportHopValidator : BaseValidator<ReportHop>
    {
        public ReportHopValidator()
        {
            RuleFor(x => x.HopCode).NotEmpty();
            RuleFor(x => x.TrackingId).NotNull().SetValidator(new TrackingIdValidator());
        }
    }
}