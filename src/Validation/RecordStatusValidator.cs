namespace InsuraNova.Validation
{

    public class RecordStatusValidator : AbstractValidator<RecordStatus>
    {
        public RecordStatusValidator()
        {
            RuleFor(recordStatus => recordStatus.StatusValue)
                .NotEmpty().WithMessage("Status Value is required.");
                

            RuleFor(recordStatus => recordStatus.StatusName)
                .NotEmpty().WithMessage("Status Name is required.")
                .MaximumLength(50).WithMessage("Status Name cannot exceed 50 characters.");

        }
    }
}
