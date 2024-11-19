using System.Net;
using ApplicationApi;
using ApplicationApi.Commands.ApproveLoan;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace ApplicationApiTests.Endpoints;

[TestFixture]
public class ApplicationApprovedEndpointTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    private IMediator _mediator;

    [SetUp]
    public void SetUp()
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
    public async Task ApplicationApproved_ValidRequest_ReturnsOk()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        _mediator.Send(Arg.Any<ApproveLoanApplicationRequest>()).Returns(Unit.Value);

        // Act
        var response = await _client.PostAsync($"/application/{applicationId}/events/approve", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await _mediator.Received(1).Send(Arg.Is<ApproveLoanApplicationRequest>(r => r.ApplicationId == applicationId));
    }
}