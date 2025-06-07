
    public class CurrencyValidator : AbstractValidator<Currency>
    {
        public CurrencyValidator()
        {
            RuleFor(currency => currency.CurrencyName)
                .NotEmpty().WithMessage("Currency name is required.")
                .MaximumLength(50).WithMessage("Currency name cannot exceed 50 characters.");

            RuleFor(currency => currency.CurrencyCode)
                .NotEmpty().WithMessage("Currency code is required.")
                .MaximumLength(50).WithMessage("Currency code cannot exceed 50 characters.");
        }
   }

