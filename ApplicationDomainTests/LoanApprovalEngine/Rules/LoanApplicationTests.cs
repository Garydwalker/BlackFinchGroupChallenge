using ApplicationDomain.Domain;
using FluentAssertions;

namespace ApplicationDomainTests.LoanApprovalEngine.Rules;

[TestFixture]
public class LoanApplicationTests
{
    [TestCase(500000, 1000000, 50.00)] // LTV = 50.00%
    [TestCase(333333, 1000000, 33.33)] // LTV = 33.33%
    [TestCase(666667, 1000000, 66.67)] // LTV = 66.67%
    [TestCase(123456, 1000000, 12.35)] // LTV = 12.35%
    [TestCase(987654, 1000000, 98.77)] // LTV = 98.77%
    [TestCase(1000000, 3000000, 33.33)] // LTV = 33.33%
    [TestCase(2500000, 3000000, 83.33)] // LTV = 83.33%
    [TestCase(1234567, 2000000, 61.73)] // LTV = 61.73%
    [TestCase(987654, 2000000, 49.38)] // LTV = 49.38%
    public void LoanToValuePercentage_ShouldReturnExpectedResult_WhenValuesAreProvided(decimal amount, decimal assetValue, decimal expectedLtv)
    {
        // Arrange
        var application = LoanApplication.Create(amount, assetValue, 800);

        // Act
        var ltv = application.LoanToValuePercentage;

        // Assert
        ltv.Should().Be(expectedLtv);
    }
}