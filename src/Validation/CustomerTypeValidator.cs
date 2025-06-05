namespace InsuraNova.Validation
{
    public class CustomerTypeValidator : AbstractValidator<CustomerType>
    {
        public CustomerTypeValidator()
        {
            RuleFor(customerType => customerType.TypeName)
                .NotEmpty().WithMessage("Customer type is required.")
                .MaximumLength(50).WithMessage("Customer type must be less than 50 characters.");
            
            RuleFor(customerType => customerType.Alias)
               .NotEmpty().WithMessage("Alias is required.")
               .MaximumLength(50).WithMessage("Alias must be less than 50 characters.");

        }
    }
}