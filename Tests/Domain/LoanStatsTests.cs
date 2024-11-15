using FluentAssertions;
using LoanApplicationApp.Domain;

namespace LoanApplicationApp.Tests.Domain;

[TestFixture]
public class LoanStatsTests
{
    [Test]
    public void UpdateStats_ShouldUpdateSuccessfulApplicationsAndTotalValueOfLoans_WhenLoanIsApproved()
    {
        // Arrange
        var loanStats = new LoanStats();
        var application = LoanApplication.Create(500000, 1000000, 750);
        application.Approve();

        // Act
        loanStats.UpdateStats(application);

        // Assert
        loanStats.SuccessfulApplications.Should().Be(1);
        loanStats.UnsuccessfulApplications.Should().Be(0);
        loanStats.TotalValueOfLoans.Should().Be(500000);
        loanStats.AverageLtv.Should().Be(application.LoanToValuePercentage);
    }

    [Test]
    public void UpdateStats_ShouldUpdateUnsuccessfulApplications_WhenLoanIsRejected()
    {
        // Arrange
        var loanStats = new LoanStats();
        var application = LoanApplication.Create(500000, 1000000, 750);
        application.Reject();

        // Act
        loanStats.UpdateStats(application);

        // Assert
        loanStats.SuccessfulApplications.Should().Be(0);
        loanStats.UnsuccessfulApplications.Should().Be(1);
        loanStats.TotalValueOfLoans.Should().Be(0);
        loanStats.AverageLtv.Should().Be(application.LoanToValuePercentage);
    }

    [Test]
    public void UpdateStats_ShouldCorrectlyCalculateAverageLtv()
    {
        // Arrange
        var loanStats = new LoanStats();
        var application1 = LoanApplication.Create(500000, 1000000, 750);
        application1.Approve();
        loanStats.UpdateStats(application1);

        var application2 = LoanApplication.Create(300000, 900000, 700);
        application2.Approve();

        // Act
        loanStats.UpdateStats(application2);

        // Assert
        loanStats.SuccessfulApplications.Should().Be(2);
        loanStats.UnsuccessfulApplications.Should().Be(0);
        loanStats.TotalValueOfLoans.Should().Be(800000);
        loanStats.AverageLtv.Should().Be(Math.Round((application1.LoanToValuePercentage + application2.LoanToValuePercentage) / 2, 2));
    }
}