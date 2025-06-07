namespace InsuraNova.Validation
{
    public class UserTypeValidator : AbstractValidator<UserType>
    {
        public UserTypeValidator()
        {
            RuleFor(userType => userType.Name)
                .NotEmpty().WithMessage("User type name is required.")
                .MaximumLength(50).WithMessage("User type name must be less than 50 characters.");
        }
    }
}