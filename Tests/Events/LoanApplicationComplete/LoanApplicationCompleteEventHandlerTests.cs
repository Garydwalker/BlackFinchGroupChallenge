using LoanApplicationApp.Domain;
using LoanApplicationApp.Events.LoanApplicationComplete;
using LoanApplicationApp.Stores;
using NSubstitute;

namespace LoanApplicationApp.Tests.Events.LoanApplicationComplete;

[TestFixture]
public class LoanApplicationCompleteEventHandlerTests
{
    private LoanStatsStore _loanStatsStore = null!;
    private LoanApplicationCompleteEventHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _loanStatsStore = Substitute.For<LoanStatsStore>();
        _handler = new LoanApplicationCompleteEventHandler(_loanStatsStore);
    }

    [Test]
    public async Task Handle_ShouldUpdateLoanStatsStore_WhenEventIsHandled()
    {
        // Arrange
        var application = LoanApplication.Create(500000, 1000000, 750);
        var notification = new LoanApplicationCompleteEvent(application, DateTime.UtcNow);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _loanStatsStore.Received(1).Update(application);
    }
}