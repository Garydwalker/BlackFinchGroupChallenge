using FluentAssertions;
using LoanApplicationApp.Domain;
using LoanApplicationApp.Events.LoanApproved;
using LoanApplicationApp.Stores;
using MediatR;
using NSubstitute;

namespace LoanApplicationApp.Tests.Events.LoanApproved;

[TestFixture]
public class LoanApprovedEventApplicationUpdateHandlerTests
{
    private IMediator _mediator;
    private LoanApplicationStore _loanApplicationStore;
    private LoanApprovedEventApplicationUpdateHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
        _loanApplicationStore = Substitute.For<LoanApplicationStore>(_mediator);
        _handler = new LoanApprovedEventApplicationUpdateHandler(_loanApplicationStore);
    }

    [Test]
    public async Task Handle_ShouldApproveAndUpdateApplication_WhenApplicationIsFound()
    {
        // Arrange
        var application = LoanApplication.Create(500000, 1000000, 750);
        var notification = new LoanApprovedEvent(application, DateTime.UtcNow);
        _loanApplicationStore.Get(application.Id).Returns(application);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        application.ApprovalStatus.Should().BeTrue();
        await _loanApplicationStore.Received(1).Update(application);
    }

    [Test]
    public void Handle_ShouldThrowException_WhenApplicationIsNotFound()
    {
        // Arrange
        var application = LoanApplication.Create(500000, 1000000, 750);
        var notification = new LoanApprovedEvent(application, DateTime.UtcNow);
        _loanApplicationStore.Get(application.Id).Returns((LoanApplication?)null);

        // Act + Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(notification, CancellationToken.None));
        exception.Message.Should().Be($"Could not find Application with id : {application.Id}");
    }
}