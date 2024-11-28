using FluentAssertions;
using LoanApplicationApp.Domain;
using LoanApplicationApp.Events.LoanApplicationComplete;
using LoanApplicationApp.Events.LoanApprovalRequest;

namespace LoanApplicationApp.Tests.Domain;

[TestFixture]
public class LoanApplicationTests
{
    [Test]
    public void Create_ShouldInitializeLoanApplication_WithCorrectValues()
    {
        // Arrange
        const decimal amount = 500000;
        const decimal assetValue = 1000000;
        const int creditScore = 750;

        // Act
        var application = LoanApplication.Create(amount, assetValue, creditScore);

        // Assert
        application.Amount.Should().Be(amount);
        application.AssetValue.Should().Be(assetValue);
        application.CreditScore.Should().Be(creditScore);
        application.ApprovalStatus.Should().BeNull();
        application.LoanToValuePercentage.Should().Be(amount / assetValue * 100);
        application.Events.Should().HaveCount(1);
        application.Events[0].Should().BeOfType<LoanApprovalRequestEvent>();
    }

    [Test]
    public void Approve_ShouldSetApprovalStatusToTrue()
    {
        // Arrange
        var application = LoanApplication.Create(500000, 1000000, 750);

        // Act
        application.Approve();

        // Assert
        application.ApprovalStatus.Should().BeTrue();
        application.Events.Should().HaveCount(2);
        application.Events[1].Should().BeOfType<LoanApplicationCompleteEvent>();
    }

    [Test]
    public void Reject_ShouldSetApprovalStatusToFalse()
    {
        // Arrange
        var application = LoanApplication.Create(500000, 1000000, 750);

        // Act
        application.Reject();

        // Assert
        application.ApprovalStatus.Should().BeFalse();
        application.Events.Should().HaveCount(2);
        application.Events[1].Should().BeOfType<LoanApplicationCompleteEvent>();
    }
}