using FluentValidation;

namespace ApplicationApi.Commands;

public class LoanApplicationRequestValidator : AbstractValidator<LoanApplicationRequest>
{
    public LoanApplicationRequestValidator()
    {
        RuleFor(x => x.LoanAmount).GreaterThan(0).WithMessage("Loan amount is required.");

        RuleFor(x => x.AssetValue).GreaterThan(0).WithMessage("Asset value is required.");

        RuleFor(x => x.CreditScore)
            .Must(BeInRange).WithMessage("Credit score must be between 1 and 999.");
    }

    private static bool BeInRange(int x) => x is >= 1 and <= 999;
}