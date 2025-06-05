using FluentValidation;
using InsuraNova.Models;

namespace InsuraNova.Validation
{
    public class SystemFunctionValidator : AbstractValidator<SystemFunction>
    {
        public SystemFunctionValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Function name is required.")
                .MaximumLength(100).WithMessage("Function name must be at most 100 characters.");

            
        }
    }
}
