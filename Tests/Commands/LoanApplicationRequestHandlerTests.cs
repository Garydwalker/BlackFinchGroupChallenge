using FluentAssertions;
using LoanApplicationApp.Commands;
using LoanApplicationApp.Domain;
using LoanApplicationApp.Stores;
using MediatR;
using NSubstitute;

namespace LoanApplicationApp.Tests.Commands;

[TestFixture]
public class LoanApplicationRequestHandlerTests
{
    private LoanApplicationStore _loanApplicationStore;
    private LoanApplicationRequestHandler _handler;

    [SetUp]
    public void Setup()
    {
        _loanApplicationStore = Substitute.For<LoanApplicationStore>(Substitute.For<IMediator>());
        _handler = new LoanApplicationRequestHandler(_loanApplicationStore);
    }

    [Test]
    public async Task Handle_ShouldCreateLoanApplication_WhenRequestIsValid()
    {
        // Arrange
        var request = new LoanApplicationRequest("500000", "1000000", "750");
       
        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        await _loanApplicationStore.Received(1).Create(Arg.Is<LoanApplication>(app =>
            app.Amount == 500000 &&
            app.AssetValue == 1000000 &&
            app.CreditScore == 750));
    }

    [Test]
    public void Handle_ShouldThrowArgumentNullException_WhenLoanAmountIsNull()
    {
        // Arrange
        var request = new LoanApplicationRequest(null, "1000000", "750");
       

        // Act + Assert
        var exception =
            Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Contain("LoanAmount");
    }

    [Test]
    public void Handle_ShouldThrowArgumentNullException_WhenAssetValueIsNull()
    {
        // Arrange
        var request = new LoanApplicationRequest("500000", null, "750");

        // Act + Assert
        var exception =
            Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Contain("AssetValue");
    }

    [Test]
    public void Handle_ShouldThrowArgumentNullException_WhenCreditScoreIsNull()
    {
        // Arrange
        var request = new LoanApplicationRequest("500000", "1000000", null);


        // Act + Assert
        var exception =
            Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Contain("CreditScore");
    }

    [Test]
    public void Handle_ShouldThrowArgumentOutOfRangeException_WhenCreditScoreIsOutOfRange()
    {
        // Arrange
        var request = new LoanApplicationRequest("500000", "1000000", "1000");

        // Act + Assert
        var exception =
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Contain("CreditScore");

    }
}