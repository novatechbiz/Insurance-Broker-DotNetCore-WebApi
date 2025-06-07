using FluentValidation;
using InsuraNova.Models;

namespace InsuraNova.Validation
{
    public class PremiumLineValidator : AbstractValidator<PremiumLine>
    {
        public PremiumLineValidator()
        {
            RuleFor(x => x.LineName)
                .NotEmpty().WithMessage("Line name is required.")
                .MaximumLength(100).WithMessage("Line name must be at most 100 characters.");

            
        }
    }
}
