using ApplicationApi.Commands.NewLoanApplication;
using ApplicationDomain.Domain;
using ApplicationDomain.Stores;
using NSubstitute;

namespace ApplicationApiTests.Commands;

[TestFixture]
public class LoanApplicationRequestHandlerTests
{
    private ILoanApplicationStore _mockLoanApplicationStore = null!;
    private LoanApplicationRequestHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _mockLoanApplicationStore = Substitute.For<ILoanApplicationStore>();
        _handler = new LoanApplicationRequestHandler(_mockLoanApplicationStore);
    }

    [TestCase(1)]
    [TestCase(50)]
    [TestCase(999)]
    public async Task Handle_ValidRequest_CreatesLoanApplication(int creditScore)
    {
        // Arrange
        var request = new LoanApplicationRequest(Guid.NewGuid(),LoanAmount: 1000, AssetValue: 5000, CreditScore: creditScore);
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
        var request = new LoanApplicationRequest(Guid.NewGuid(), LoanAmount: 1000, AssetValue: 5000, CreditScore: 1000);
        var cancellationToken = new CancellationToken();

        // Act & Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _handler.Handle(request, cancellationToken));
    }
}