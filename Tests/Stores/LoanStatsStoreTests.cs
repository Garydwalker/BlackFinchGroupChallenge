using FluentAssertions;
using LoanApplicationApp.Domain;
using LoanApplicationApp.Stores;

namespace LoanApplicationApp.Tests.Stores;

[TestFixture]
public class LoanStatsStoreTests
{
    private LoanStatsStore _store;

    [SetUp]
    public void Setup()
    {
        _store = new LoanStatsStore();
    }
    
    [Test]
    public void Update_ShouldUpdateStats_WhenSecondLoanApplicationIsApproved()
    {
        // Arrange
        var application = LoanApplication.Create(500000, 1000000, 750);
        application.Approve();
        _store.Update(application);
        var application2 = LoanApplication.Create(500000, 1000000, 750);
        application2.Approve();

        // Act
        _store.Update(application2);
        var stats = _store.GetStats();

        // Assert
        stats.TotalApplications.Should().Be(2);
        stats.SuccessfulApplications.Should().Be(2);
        stats.UnsuccessfulApplications.Should().Be(0);
        stats.TotalValueOfLoans.Should().Be(1000000);
        stats.AverageLtv.Should().Be(50);
    }

    [Test]
    public void Update_ShouldUpdateStats_WhenLoanApplicationIsApproved()
    {
        // Arrange
        var application = LoanApplication.Create(500000, 1000000, 750);
        application.Approve();

        // Act
        _store.Update(application);
        var stats = _store.GetStats();

        // Assert
        stats.TotalApplications.Should().Be(1);
        stats.SuccessfulApplications.Should().Be(1);
        stats.UnsuccessfulApplications.Should().Be(0);
        stats.TotalValueOfLoans.Should().Be(500000);
        stats.AverageLtv.Should().Be(50);
    }

    [Test]
    public void Update_ShouldUpdateStats_WhenLoanApplicationIsRejected()
    {
        // Arrange
        var application = LoanApplication.Create(500000, 1000000, 750);
        application.Reject();

        // Act
        _store.Update(application);
        var stats = _store.GetStats();

        // Assert
        stats.TotalApplications.Should().Be(1);
        stats.SuccessfulApplications.Should().Be(0);
        stats.UnsuccessfulApplications.Should().Be(1);
        stats.TotalValueOfLoans.Should().Be(0);
        stats.AverageLtv.Should().Be(50);
    }

    [Test]
    public void GetStats_ShouldReturnInitialStats_WhenNoApplicationsAreUpdated()
    {
        // Act
        var stats = _store.GetStats();

        // Assert
        stats.TotalApplications.Should().Be(0);
        stats.SuccessfulApplications.Should().Be(0);
        stats.UnsuccessfulApplications.Should().Be(0);
        stats.TotalValueOfLoans.Should().Be(0);
        stats.AverageLtv.Should().Be(0);
    }
}