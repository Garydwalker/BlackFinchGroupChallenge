using MediatR;

namespace LoanApplicationApp.Commands;

public record LoanApplicationRequest(string? RequestLoanAmount, string? RequestAssetValue, string? ProvidedCreditScore) : IRequest<Unit>
{
    public decimal? LoanAmount => decimal.TryParse(RequestLoanAmount,out var loanAmountAsDecimal)? loanAmountAsDecimal : null;
    public decimal? AssetValue => decimal.TryParse(RequestAssetValue, out var assetValueAsDecimal) ? assetValueAsDecimal : null;
    public int? CreditScore => int.TryParse(ProvidedCreditScore, out var creditScoreAsInt) ? creditScoreAsInt : null;
}