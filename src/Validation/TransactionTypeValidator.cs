
    public class TransactionTypeValidator : AbstractValidator<TransactionType>
    {
        public TransactionTypeValidator()
        {
            RuleFor(transactionType => transactionType.TypeName)
                .NotEmpty().WithMessage("Type name is required.")
                .MaximumLength(50).WithMessage("Type name must not exceed 50 characters.");
        }
    }

