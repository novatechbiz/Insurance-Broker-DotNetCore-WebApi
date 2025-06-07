using FluentValidation;
using InsuraNova.Models;

namespace InsuraNova.Validation
{
    public class CompanyTypeValidator : AbstractValidator<CompanyType>
    {
        public CompanyTypeValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Type name is required.")
                .MaximumLength(100).WithMessage("Type name must be at most 100 characters.");

           
        }
    }
}
