namespace InsuraNova.Validation
{
    public class UserValidator : AbstractValidator<UserProfile>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must be less than 50 characters.");

            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username must be less than 50 characters.");

        }
    }
}
