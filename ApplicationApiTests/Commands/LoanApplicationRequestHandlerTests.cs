using ApplicationApi.Commands;
using ApplicationDomain.Domain;
using ApplicationDomain.Stores;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace ApplicationApiTests.Commands;

[TestFixture]
public class LoanApplicationRequestHandlerTests
{
    private LoanApplicationStore _mockLoanApplicationStore;
    private LoanApplicationRequestHandler _handler;

    [SetUp]
    public void Setup()
    {
        var dbContext = Substitute.For<ApplicationDbContext>();
        _mockLoanApplicationStore = Substitute.For<LoanApplicationStore>(dbContext);
        _handler = new LoanApplicationRequestHandler(_mockLoanApplicationStore);
    }

    [TestCase(1)]
    [TestCase(50)]
    [TestCase(999)]
    public async Task Handle_ValidRequest_CreatesLoanApplication(int creditScore)
    {
        // Arrange
        var request = new LoanApplicationRequest(LoanAmount: 1000, AssetValue: 5000, CreditScore: creditScore);
        var cancellationToken = new CancellationToken();

        // Act
        var result = await _handler.Handle(request, cancellationToken);

        // Assert
        await _mockLoanApplicationStore.Received(1).Create(Arg.Any<LoanApplication>());
    }

    [Test]
    public void Handle_InvalidCreditScore_ThrowsException()
    {
        // Arrange
        var request = new LoanApplicationRequest(LoanAmount: 1000, AssetValue: 5000, CreditScore: 1000);
        var cancellationToken = new CancellationToken();

        // Act & Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _handler.Handle(request, cancellationToken));
    }
}