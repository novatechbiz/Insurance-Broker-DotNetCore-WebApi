namespace InsuraNova.Validation
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.CompanyId)
                .NotEmpty().WithMessage("Company ID is required.");

            RuleFor(customer => customer.CustomerIdentificationTypeId)
                .NotEmpty().WithMessage("Customer Identification Type ID is required.");

            RuleFor(customer => customer.CustomerTypeId)
                .NotEmpty().WithMessage("Customer Type ID is required.");

            RuleFor(customer => customer.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(50).WithMessage("Customer name must be less than or equal to 150 characters.");

            RuleFor(customer => customer.IdentificationNo)
                .MaximumLength(50).WithMessage("Identification number must be less than or equal to 100 characters.");

            RuleFor(customer => customer.FullName)
                .MaximumLength(50).WithMessage("Full name must be less than or equal to 150 characters.");

            RuleFor(customer => customer.ContactNo)
                .MaximumLength(50).WithMessage("Contact number must be less than or equal to 20 characters.");

            RuleFor(customer => customer.WhatsAppNo)
                .MaximumLength(50).WithMessage("WhatsApp number must be less than or equal to 20 characters.");

            RuleFor(customer => customer.EmailAddress)
                .MaximumLength(50).WithMessage("Email address must be less than or equal to 100 characters.")
                .EmailAddress().WithMessage("Email address is not valid.")
                .When(customer => !string.IsNullOrEmpty(customer.EmailAddress));

            //GenderTypeId
            //RecordStatusId 

            // RuleFor(customer => customer.EnteredBy)
            //     .MaximumLength(100).WithMessage("EnteredBy must be less than or equal to 100 characters.");

            //EnteredDate

            // RuleFor(customer => customer.ModifiedBy)
            //     .MaximumLength(100).WithMessage("ModifiedBy must be less than or equal to 100 characters.");

            //ModifiedDate

            // validate DateOfBirth if needed (e.g., not in the future)
            RuleFor(customer => customer.DateOfBirth)
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Date of birth cannot be in the future.")
                .When(customer => customer.DateOfBirth.HasValue);

        }
    }
}