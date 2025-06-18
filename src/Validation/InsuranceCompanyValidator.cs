using FluentValidation;
using InsuraNova.Models;

namespace InsuraNova.Validation
{
    public class InsuranceCompanyValidator : AbstractValidator<InsuranceCompany>
    {
        public InsuranceCompanyValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required.")
                .MaximumLength(100).WithMessage("Company name must be at most 100 characters.");

            
        }
    }
}
