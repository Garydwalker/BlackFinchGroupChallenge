using ApplicationDomain.Domain;
using ApplicationDomain.LoanApprovalEngine.Rules;
using FluentAssertions;

namespace ApplicationDomainTests.LoanApprovalEngine.Rules;

[TestFixture]
public class MillionPoundLoanAcceptanceRuleTests
{
    private MillionPoundLoanAcceptanceRule _rule = null!;

    [SetUp]
    public void Setup()
    {
        _rule = new MillionPoundLoanAcceptanceRule();
    }

    [TestCase(999999)]
    [TestCase(500000)]
    public void Evaluate_ShouldAlwaysReturnTrue_WhenLoanValueIsUnderOneMillion(decimal amount)
    {
        // Arrange
        var application = LoanApplication.Create(amount, 1000000, 800);

        // Act
        var result = _rule.Evaluate(application);

        // Assert
        result.Should().BeTrue();
    }

    [TestCase(1000000, 1800000, 950, true)] // LTV < 60.00%
    [TestCase(1000000, 1666667, 950, true)] // LTV = 60.00%
    [TestCase(1000000, 1666667, 949, false)] // LTV = 60.00%
    [TestCase(1500000, 2500001, 950, true)] // LTV = 60.00%, Credit Score < 950
    [TestCase(1500000, 2500000, 949, false)] // LTV = 60.00%
    [TestCase(1000000, 1666900, 950, true)] // LTV < 60.00%


    public void Evaluate_ShouldReturnExpectedResult_WhenLoanValueIsOneMillionOrMore(decimal amount, decimal assetValue, int creditScore, bool expected)
    {
        // Arrange
        var application = LoanApplication.Create(amount, assetValue, creditScore);

        // Act
        var result = _rule.Evaluate(application);

        // Assert
        result.Should().Be(expected);
    }
}