using System.Net;
using System.Net.Http.Json;
using ApplicationApi;
using ApplicationApi.Commands;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace ApplicationApiTests;

public class ApplicationPostTests
{
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;
    private IMediator _mediator = null!;

    [SetUp]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(_mediator);
                });
            });
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task PostApplication_Returns400_WhenNoLoanApplicationRequestPassed()
    {
        // Act
        var response = await _client.PostAsJsonAsync<LoanApplicationRequest>("/application", null!);

        // Assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task PostApplication_CallsMediator_WithRequest_AndReturnsOk()
    {
        // Arrange
        var loanApplication = new LoanApplicationRequest(1000, 2000, 750);
        _mediator.Send(Arg.Any<IRequest<Unit>>(), Arg.Any<CancellationToken>()).Returns(Unit.Value);

        // Act
        var response = await _client.PostAsJsonAsync("/application", loanApplication);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await _mediator.Received(1).Send(Arg.Is<LoanApplicationRequest>(r =>
            r.LoanAmount == loanApplication.LoanAmount &&
            r.AssetValue == loanApplication.AssetValue &&
            r.CreditScore == loanApplication.CreditScore), Arg.Any<CancellationToken>());
    }
}
