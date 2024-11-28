using LoanApplicationApp.Domain;
using LoanApplicationApp.Events.LoanApprovalRequest;
using LoanApplicationApp.Events.LoanApproved;
using LoanApplicationApp.Events.LoanRejected;
using LoanApplicationApp.LoanApprovalEngine;
using LoanApplicationApp.LoanApprovalEngine.Rules;
using MediatR;
using NSubstitute;

namespace LoanApplicationApp.Tests.Events.LoanApprovalRequest;

[TestFixture]
public class LoanApprovalRequestNotificationHandlerTests
{
    private LoanApplicationApprovalEngine _loanApplicationApprovalEngine = null!;
    private IMediator _mediator = null!;
    private LoanApprovalRequestNotificationHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _loanApplicationApprovalEngine = Substitute.For<LoanApplicationApprovalEngine>(new List<ILoanAcceptanceRule>());
        _mediator = Substitute.For<IMediator>();
        _handler = new LoanApprovalRequestNotificationHandler(_loanApplicationApprovalEngine, _mediator);
    }

    [Test]
    public async Task Handle_ShouldPublishLoanApprovedEvent_WhenLoanIsApproved()
    {
        // Arrange
        var application = LoanApplication.Create(500000, 1000000, 750);
        var notification = new LoanApprovalRequestEvent(application, DateTime.UtcNow);
        _loanApplicationApprovalEngine.Evaluate(application).Returns(true);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        await _mediator.Received(1).Publish(Arg.Is<LoanApprovedEvent>(e => e.Application == application), Arg.Any<CancellationToken>());
        await _mediator.DidNotReceive().Publish(Arg.Any<LoanRejectedEvent>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_ShouldPublishLoanRejectedEvent_WhenLoanIsRejected()
    {
        // Arrange
        var application = LoanApplication.Create(500000, 1000000, 750);
        var notification = new LoanApprovalRequestEvent(application, DateTime.UtcNow);
        _loanApplicationApprovalEngine.Evaluate(application).Returns(false);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        await _mediator.Received(1).Publish(Arg.Is<LoanRejectedEvent>(e => e.Application == application), Arg.Any<CancellationToken>());
        await _mediator.DidNotReceive().Publish(Arg.Any<LoanApprovedEvent>(), Arg.Any<CancellationToken>());
    }
}