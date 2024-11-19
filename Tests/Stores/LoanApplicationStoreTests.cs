using FluentAssertions;
using LoanApplicationApp.Domain;
using LoanApplicationApp.Stores;
using MediatR;
using NSubstitute;

namespace LoanApplicationApp.Tests.Stores;

[TestFixture]
public class LoanApplicationStoreTests
{
    private IMediator _mediator = null!;
    private LoanApplicationStore _store =null!;

    [SetUp]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
        _store = new LoanApplicationStore(_mediator);
    }

    [Test]
    public async Task Create_ShouldAddApplicationAndPublishEvents()
    {
        // Arrange
        var application = LoanApplication.Create(500000, 1000000, 750);
        application.ClearEvents();
        application.AddEvent(Substitute.For<INotification>());

        // Act
        await _store.Create(application);
        var retrievedApplication = await _store.Get(application.Id);

        // Assert
        retrievedApplication.Should().NotBeNull();
        retrievedApplication.Should().Be(application);
        await _mediator.Received(1).Publish(Arg.Any<INotification>());
    }

    [Test]
    public async Task Update_ShouldUpdateApplicationAndPublishEvents()
    {
        // Arrange
        var application = LoanApplication.Create(500000, 1000000, 750);
        await _store.Create(application);
        application.Approve();
        application.ClearEvents();
        application.AddEvent(Substitute.For<INotification>());
        _mediator.ClearReceivedCalls();

        // Act
        await _store.Update(application);
        var retrievedApplication = await _store.Get(application.Id);

        // Assert
        retrievedApplication.Should().NotBeNull();
        retrievedApplication!.ApprovalStatus.Should().BeTrue();
        await _mediator.Received(1).Publish(Arg.Any<INotification>());
    }

    [Test]
    public void Update_ShouldThrowException_WhenApplicationNotFound()
    {
        // Arrange
        var application = LoanApplication.Create(500000, 1000000, 750);

        // Act + Assert
        Assert.ThrowsAsync<InvalidOperationException>(()=> _store.Update(application));
    }

    [Test]
    public async Task Get_ShouldReturnNull_WhenApplicationNotFound()
    {
        // Act
        var result = await _store.Get(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }
}