using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;

namespace MinimalApi.Vehicles.Tests.IntegrationTests;

public class VehiclesApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public VehiclesApiTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task GetHome_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var res = await client.GetAsync("/");
        res.EnsureSuccessStatusCode();
    }
}
