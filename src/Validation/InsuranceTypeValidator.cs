public class InsuranceTypeValidator : AbstractValidator<InsuranceType>
{
    public InsuranceTypeValidator()
    {
        RuleFor(insuranceType => insuranceType.TypeName)
            .NotEmpty().WithMessage("TypeName is required")
            .MaximumLength(50).WithMessage("TypeName must not exceed 50 characters");
    }
}
