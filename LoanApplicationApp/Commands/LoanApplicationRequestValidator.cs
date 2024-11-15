using FluentValidation;

namespace LoanApplicationApp.Commands;

public class LoanApplicationRequestValidator : AbstractValidator<LoanApplicationRequest>
{
    public LoanApplicationRequestValidator()
    {
        RuleFor(x => x.LoanAmount).NotEmpty().WithMessage("Loan amount is required.");

        RuleFor(x => x.AssetValue).NotEmpty().WithMessage("Asset value is required.");

        RuleFor(x => x.CreditScore)
            .NotEmpty().WithMessage("Credit score is required.")
            .Must(BeInRange).WithMessage("Credit score must be between 1 and 999.");
    }

    private static bool BeInRange(int? x) => x is >= 1 and <=999;
}