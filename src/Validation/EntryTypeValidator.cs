
    public class EntryTypeValidator : AbstractValidator<EntryType>
    {
        public EntryTypeValidator()
        {
            RuleFor(entryType => entryType.TypeName)
                .NotEmpty().WithMessage("Type name is required.")
                .MaximumLength(50).WithMessage("Type name must not exceed 50 characters.");
        }
    }

