using FluentAssertions;
using LoanApplicationApp.Commands;

namespace LoanApplicationApp.Tests.Commands;

[TestFixture]
public class LoanApplicationRequestTests
{
    [Test]
    public void LoanAmount_ShouldReturnDecimal_WhenRequestLoanAmountIsValid()
    {
        // Arrange
        var request = new LoanApplicationRequest("500000", "1000000", "750");

        // Act
        var loanAmount = request.LoanAmount;

        // Assert
        loanAmount.Should().Be(500000);
    }

    [Test]
    public void LoanAmount_ShouldReturnNull_WhenRequestLoanAmountIsInvalid()
    {
        // Arrange
        var request = new LoanApplicationRequest("invalid", "1000000", "750");

        // Act
        var loanAmount = request.LoanAmount;

        // Assert
        loanAmount.Should().BeNull();
    }

    [Test]
    public void AssetValue_ShouldReturnDecimal_WhenRequestAssetValueIsValid()
    {
        // Arrange
        var request = new LoanApplicationRequest("500000", "1000000", "750");

        // Act
        var assetValue = request.AssetValue;

        // Assert
        assetValue.Should().Be(1000000);
    }

    [Test]
    public void AssetValue_ShouldReturnNull_WhenRequestAssetValueIsInvalid()
    {
        // Arrange
        var request = new LoanApplicationRequest("500000", "invalid", "750");

        // Act
        var assetValue = request.AssetValue;

        // Assert
        assetValue.Should().BeNull();
    }

    [Test]
    public void CreditScore_ShouldReturnInt_WhenProvidedCreditScoreIsValid()
    {
        // Arrange
        var request = new LoanApplicationRequest("500000", "1000000", "750");

        // Act
        var creditScore = request.CreditScore;

        // Assert
        creditScore.Should().Be(750);
    }

    [Test]
    public void CreditScore_ShouldReturnNull_WhenProvidedCreditScoreIsInvalid()
    {
        // Arrange
        var request = new LoanApplicationRequest("500000", "1000000", "invalid");

        // Act
        var creditScore = request.CreditScore;

        // Assert
        creditScore.Should().BeNull();
    }
}