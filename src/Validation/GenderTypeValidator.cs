namespace InsuraNova.Validation
{
    public class GenderTypeValidator : AbstractValidator<GenderType>
    {
        public GenderTypeValidator()
        {
            RuleFor(genderType => genderType.Name)
                .NotEmpty().WithMessage("Gender type is required.")
                .MaximumLength(50).WithMessage("Gender type must be less than 50 characters.");
        }
    }
}