namespace InsuraNova.Validation
{
    public class CustomerIdentificationTypeValidator : AbstractValidator<CustomerIdentificationType>
    {
        public CustomerIdentificationTypeValidator()
        {
            RuleFor(customerIdentificationType => customerIdentificationType.IdentificationType)
                .NotEmpty().WithMessage("Identification type is required.")
                .MaximumLength(50).WithMessage("Identification type must be less than 50 characters.");
        }
    }
}