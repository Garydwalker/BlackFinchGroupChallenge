using ApplicationApi.Commands.RejectLoanApplication;
using ApplicationDomain.Domain;
using ApplicationDomain.Events.LoanApplicationComplete;
using ApplicationDomain.Stores;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace ApplicationApiTests.Commands;

[TestFixture]
public class RejectLoanApplicationRequestHandlerTests
{
    private ILoanApplicationStore _loanApplicationStore;
    private RejectLoanApplicationRequestHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _loanApplicationStore = Substitute.For<ILoanApplicationStore>();
        _handler = new RejectLoanApplicationRequestHandler(_loanApplicationStore);
    }

    [Test]
    public async Task Handle_ApplicationNotFound_ThrowsException()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        _loanApplicationStore.Get(applicationId).Returns(Task.FromResult<LoanApplication?>(null));
        var request = new RejectLoanApplicationRequest(applicationId);
        var cancellationToken = new CancellationToken();

        // Act
        Func<Task> act = async () => await _handler.Handle(request, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Application not found");
        await _loanApplicationStore.Received(1).Get(applicationId);
    }

    [Test]
    public async Task Handle_ApplicationFound_CallsApproveAndReturns()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        var loanApplication = LoanApplication.Create(1000m, 5000m, 700, applicationId);
        loanApplication.ClearEvents();

        _loanApplicationStore.Get(applicationId).Returns(Task.FromResult<LoanApplication?>(loanApplication));
        var request = new RejectLoanApplicationRequest(applicationId);
        var cancellationToken = new CancellationToken();

        // Act
        var result = await _handler.Handle(request, cancellationToken);

        // Assert
        loanApplication.Events.Should().HaveCount(1);
        loanApplication.Events[0].Should().BeOfType<LoanApplicationCompleteEvent>();
        result.Should().Be(Unit.Value);
    }
}