using FluentAssertions;
using LoanApplicationApp.Domain;
using LoanApplicationApp.LoanApprovalEngine.Rules;

namespace LoanApplicationApp.Tests.LoanApprovalEngine.Rules;

[TestFixture]
public class SubMillionPoundLoanAcceptanceRuleTests
{
    private SubMillionPoundLoanAcceptanceRule _rule = null!;

    [SetUp]
    public void Setup()
    {
        _rule = new SubMillionPoundLoanAcceptanceRule();
    }

    [TestCase(1000001)]
    [TestCase(1500000)]
    [TestCase(2000000)]
    public void Evaluate_ShouldReturnTrue_WhenLoanAmountIsOverOneMillion(decimal amount)
    {
        // Arrange
        var application = LoanApplication.Create(amount, 2000000, 800);

        // Act
        var result = _rule.Evaluate(application);

        // Assert
        result.Should().BeTrue();
    }


    [TestCase(500000, 1000000, 750, true)] // LTV = 50.00%, Credit Score = 750
    [TestCase(500000, 1000000, 751, true)] // LTV = 50.00%, Credit Score > 750
    [TestCase(500000, 1000000, 749, false)] // LTV = 50.00%, Credit Score < 750
    [TestCase(599900, 1000000, 749, false)] // LTV = 59.99%, Credit Score < 750
    [TestCase(599900, 1000000, 750, true)] // LTV = 59.99%, Credit Score = 750
    public void Evaluate_ShouldReturnExpectedResult_WhenLTVIsUnder60PercentAndCreditScoreIsAtLeast750(decimal amount, decimal assetValue, int creditScore, bool expected)
    {
        // Arrange
        var application = LoanApplication.Create(amount, assetValue, creditScore);

        // Act
        var result = _rule.Evaluate(application);

        // Assert
        result.Should().Be(expected);
    }
    [TestCase(600000, 1000000, 800, true)] // LTV = 60.00%, Credit Score = 800
    [TestCase(600000, 1000000, 799, false)] // LTV = 60.00%, Credit Score<= 800
    [TestCase(700000, 1000000, 800, true)] // LTV = 70.00%, Credit Score = 800
    [TestCase(700000, 1000000, 801, true)] // LTV = 70.00%, Credit Score > 800
    [TestCase(700000, 1000000, 799, false)] // LTV = 70.00%, Credit Score < 800
    [TestCase(799900, 1000000, 800, true)] // LTV = 79.99%, Credit Score = 800
    [TestCase(799900, 1000000, 799, false)] // LTV = 79.99%, Credit Score < 800
    public void Evaluate_ShouldReturnExpectedResult_WhenLTVIsAbove60PercentAndLessThan80PercentAndCreditScoreIsAtLeast800(decimal amount, decimal assetValue, int creditScore, bool expected)
    {
        // Arrange
        var application = LoanApplication.Create(amount, assetValue, creditScore);

        // Act
        var result = _rule.Evaluate(application);

        // Assert
        result.Should().Be(expected);
    }

    [TestCase(800000, 1000000, 899, false)] // LTV = 80.00%, Credit Score < 900
    [TestCase(800000, 1000000, 900, true)] // LTV = 80.00%, Credit Score = 900
    [TestCase(800000, 1000000, 901, true)] // LTV = 80.00%, Credit Score > 900
    [TestCase(850000, 1000000, 899, false)] // LTV = 85.00%, Credit Score < 900
    [TestCase(850000, 1000000, 900, true)] // LTV = 85.00%, Credit Score = 900
    [TestCase(850000, 1000000, 901, true)] // LTV = 85.00%, Credit Score > 900
    [TestCase(899900, 1000000, 899, false)] // LTV = 89.99%, Credit Score < 900
    [TestCase(899900, 1000000, 900, true)] // LTV = 89.99%, Credit Score = 900
    [TestCase(899900, 1000000, 901, true)] // LTV = 89.99%, Credit Score > 900
    public void Evaluate_ShouldReturnExpectedResult_WhenLTVIs80PercentOrMoreAndUnder90PercentAndCreditScoreIsAtLeast900(decimal amount, decimal assetValue, int creditScore, bool expected)
    {
        // Arrange
        var application = LoanApplication.Create(amount, assetValue, creditScore);

        // Act
        var result = _rule.Evaluate(application);

        // Assert
        result.Should().Be(expected);
    }

    [TestCase(900000, 1000000)] // LTV = 90.00%
    [TestCase(950000, 1000000)] // LTV = 95.00%
    [TestCase(1000000, 1000000)] // LTV = 100.00%
    public void Evaluate_ShouldReturnFalse_WhenLTVIs90PercentOrMore(decimal amount, decimal assetValue)
    {
        // Arrange
        var application = LoanApplication.Create(amount, assetValue, 999);

        // Act
        var result = _rule.Evaluate(application);

        // Assert
        result.Should().BeFalse();
    }
}