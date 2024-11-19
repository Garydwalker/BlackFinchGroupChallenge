using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using System.Net.Http.Json;
using System.Net;
using Dapr.Client;
using FluentAssertions;
using LoanDecisionApi.Events.LoanApproved;
using LoanDecisionApi.Events.LoanRejected;
using LoanDecisionApi.LoanApprovalEngine;
using LoanDecisionApi.LoanApprovalEngine.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace LoanDecisionApiTests
{
    [TestFixture]
    public class LoanReviewEndpointTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private LoanApplicationApprovalEngine _loanApplicationApprovalEngine;
        private DaprClient _daprClient;

        [SetUp]
        public void SetUp()
        {
            _daprClient = Substitute.For<DaprClient>();
            _loanApplicationApprovalEngine = Substitute.For<LoanApplicationApprovalEngine>(new List<ILoanAcceptanceRule>());
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddSingleton(_daprClient);
                        services.AddSingleton(_loanApplicationApprovalEngine);
                    });
                });
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
            _daprClient.Dispose();
        }

        [Test]
        public async Task LoanReview_Approved_PublishesLoanApprovedEvent()
        {
            // Arrange
            var loanApplication = new LoanApplication(Guid.NewGuid(), 1000, 5000, 700);
            _loanApplicationApprovalEngine.Evaluate(loanApplication).Returns(Task.FromResult(true));

            // Act
            var response = await _client.PostAsJsonAsync("loanReview", loanApplication);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            await _daprClient.Received(1).PublishEventAsync(
                "pubsub",
                "loan-approved",
                Arg.Is<LoanApprovedEvent>(e => e.ApplicationId == loanApplication.ApplicationId),
                Arg.Any<Dictionary<string, string>>(),
                Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task LoanReview_Rejected_PublishesLoanRejectedEvent()
        {
            // Arrange
            var loanApplication = new LoanApplication(Guid.NewGuid(), 1000, 5000, 500);
            _loanApplicationApprovalEngine.Evaluate(loanApplication).Returns(Task.FromResult(false));

            // Act
            var response = await _client.PostAsJsonAsync("loanReview", loanApplication);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            await _daprClient.Received(1).PublishEventAsync(
                "pubsub",
                "loan-rejected",
                Arg.Is<LoanRejectedEvent>(e => e.ApplicationId == loanApplication.ApplicationId),
                Arg.Any<Dictionary<string, string>>(),
                Arg.Any<CancellationToken>());
        }
    }
}
