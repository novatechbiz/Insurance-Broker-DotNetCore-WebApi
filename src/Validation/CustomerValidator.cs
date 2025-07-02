
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");

            RuleFor(customer => customer.EmailAddress)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(150).WithMessage("Email address cannot exceed 150 characters.");

            RuleFor(customer => customer.ContactNo)
                .NotEmpty().WithMessage("Contact number is required.")
                .Matches(@"^\+?[0-9]{7,15}$").WithMessage("Contact number must be a valid format (7–15 digits).");

            RuleFor(customer => customer.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past.");

            RuleFor(customer => customer.CompanyId)
                .GreaterThan(0).WithMessage("Valid company ID is required.");
        }
    }
