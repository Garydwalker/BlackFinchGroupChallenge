using FluentAssertions;
using LoanDecisionApi.LoanApprovalEngine;
using LoanDecisionApi.LoanApprovalEngine.Rules;
using NSubstitute;

namespace LoanDecisionApiTests.LoanApprovalEngine;

[TestFixture]
public class LoanApplicationApprovalEngineTests
{
    private ILoanAcceptanceRule _mockRule1 = null!;
    private ILoanAcceptanceRule _mockRule2 = null!;
    private LoanApplicationApprovalEngine _engine = null!;

    [SetUp]
    public void Setup()
    {
        _mockRule1 = Substitute.For<ILoanAcceptanceRule>();
        _mockRule2 = Substitute.For<ILoanAcceptanceRule>();
        var rules = new List<ILoanAcceptanceRule> { _mockRule1, _mockRule2 };
        _engine = new LoanApplicationApprovalEngine(rules);
    }

    [Test]
    public async Task Evaluate_ShouldReturnTrue_WhenRuleReturnsTrue()
    {
        // Arrange
        var rules = new List<ILoanAcceptanceRule> { _mockRule1 };
        var engine = new LoanApplicationApprovalEngine(rules);
        var application = new LoanApplication(Guid.NewGuid(), 500000, 1000000, 750);
        _mockRule1.Evaluate(application).Returns(true);

        // Act
        var result = await engine.Evaluate(application);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task Evaluate_ShouldReturnFalse_WhenRuleReturnsFalse()
    {
        // Arrange
        var rules = new List<ILoanAcceptanceRule> { _mockRule1 };
        var engine = new LoanApplicationApprovalEngine(rules);
        var application = new LoanApplication(Guid.NewGuid(), 500000, 1000000, 750);
        _mockRule1.Evaluate(application).Returns(false);

        // Act
        var result = await engine.Evaluate(application);

        // Assert
        result.Should().BeFalse();
    }


    [Test]
    public async Task Evaluate_ShouldReturnTrue_WhenBothRulesReturnTrue()
    {
        // Arrange
        var application = new LoanApplication(Guid.NewGuid(), 500000, 1000000, 750);
        _mockRule1.Evaluate(application).Returns(true);
        _mockRule2.Evaluate(application).Returns(true);

        // Act
        var result = await _engine.Evaluate(application);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task Evaluate_ShouldReturnFalse_WhenFirstRuleReturnsFalse_AndSecondRuleReturnsTrue()
    {
        // Arrange
        var application = new LoanApplication(Guid.NewGuid(), 500000, 1000000, 750);
        _mockRule1.Evaluate(application).Returns(false);
        _mockRule2.Evaluate(application).Returns(true);

        // Act
        var result = await _engine.Evaluate(application);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task Evaluate_ShouldReturnFalse_WhenFirstRuleReturnsTrue_AndSecondRuleReturnsFalse()
    {
        // Arrange
        var application = new LoanApplication(Guid.NewGuid(), 500000, 1000000, 750);
        _mockRule1.Evaluate(application).Returns(true);
        _mockRule2.Evaluate(application).Returns(false);

        // Act
        var result = await _engine.Evaluate(application);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task Evaluate_ShouldReturnFalse_WhenBothRulesReturnFalse()
    {
        // Arrange
        var application = new LoanApplication(Guid.NewGuid(), 500000, 1000000, 750);
        _mockRule1.Evaluate(application).Returns(false);
        _mockRule2.Evaluate(application).Returns(false);

        // Act
        var result = await _engine.Evaluate(application);

        // Assert
        result.Should().BeFalse();
    }
}