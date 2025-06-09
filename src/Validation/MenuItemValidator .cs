using FluentValidation;

namespace InsuraNova.Validation
{
    public class MenuItemValidator : AbstractValidator<MenuItem>
    {
        public MenuItemValidator()
        {
            RuleFor(menuItem => menuItem.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must be less than 100 characters.");

            //RuleFor(menuItem => menuItem.Icon)
            //    .NotEmpty().WithMessage("Icon is required.")
            //    .MaximumLength(50).WithMessage("Icon must be less than 50 characters.");

            //RuleFor(menuItem => menuItem.Route)
            //    .NotEmpty().WithMessage("Route is required.")
            //    .MaximumLength(200).WithMessage("Route must be less than 200 characters.");
        }
    }
}
