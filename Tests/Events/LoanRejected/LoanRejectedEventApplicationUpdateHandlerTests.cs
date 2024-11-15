using FluentAssertions;
using LoanApplicationApp.Domain;
using LoanApplicationApp.Events.LoanRejected;
using LoanApplicationApp.Stores;
using MediatR;
using NSubstitute;

namespace LoanApplicationApp.Tests.Events.LoanRejected;

[TestFixture]
public class LoanRejectedEventApplicationUpdateHandlerTests
{
    private IMediator _mediator;
    private LoanApplicationStore _loanApplicationStore;
    private LoanRejectedEventApplicationUpdateHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
        _loanApplicationStore = Substitute.For<LoanApplicationStore>(_mediator);
        _handler = new LoanRejectedEventApplicationUpdateHandler(_loanApplicationStore);
    }

    [Test]
    public async Task Handle_ShouldRejectAndUpdateApplication_WhenApplicationIsFound()
    {
        // Arrange
        var application = LoanApplication.Create(500000, 1000000, 750);
        var notification = new LoanRejectedEvent(application, DateTime.UtcNow);
        _loanApplicationStore.Get(application.Id).Returns( application);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        application.ApprovalStatus.Should().BeFalse();
        await _loanApplicationStore.Received(1).Update(application);
    }

    [Test]
    public void Handle_ShouldThrowException_WhenApplicationIsNotFound()
    {
        // Arrange
        var application = LoanApplication.Create(500000, 1000000, 750);
        var notification = new LoanRejectedEvent(application, DateTime.UtcNow);
        _loanApplicationStore.Get(application.Id).Returns(Task.FromResult<LoanApplication?>(null));

        // Act + Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(notification, CancellationToken.None));
        exception.Message.Should().Be($"Could not find Application with id : {application.Id}");
    }
}