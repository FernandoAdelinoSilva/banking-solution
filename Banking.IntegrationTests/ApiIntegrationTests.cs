using System.Net;
using System.Text;

namespace Banking.IntegrationTests;

public class ApiIntegrationTests
{
    private readonly HttpClient _client;

    public ApiIntegrationTests()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5203") };
    }

    [Fact]
    public async Task Reset_ShouldReturn200()
    {
        var response = await _client.PostAsync("/Account/reset", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetBalance_NonExistingAccount_ShouldReturn404()
    {
        var response = await _client.GetAsync("/Account/balance?account_id=1234");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Equal("0", body);
    }

    [Fact]
    public async Task Deposit_ShouldCreateAccountWithInitialBalance()
    {
        var payload = new StringContent(
            "{\"type\":\"deposit\",\"destination\":\"100\",\"amount\":10}",
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/Account/event", payload);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"balance\":10", body);
    }

    [Fact]
    public async Task Deposit_ShouldIncreaseBalanceForExistingAccount()
    {
        var payload = new StringContent(
            "{\"type\":\"deposit\",\"destination\":\"100\",\"amount\":10}",
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/Account/event", payload);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"balance\":20", body);
    }

    [Fact]
    public async Task GetBalance_ExistingAccount_ShouldReturn200()
    {
        var response = await _client.GetAsync("/Account/balance?account_id=100");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Equal("20", body);
    }

    [Fact]
    public async Task Withdraw_NonExistingAccount_ShouldReturn404()
    {
        var payload = new StringContent(
            "{\"type\":\"withdraw\",\"origin\":\"200\",\"amount\":10}",
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/Account/event", payload);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Equal("0", body);
    }

    [Fact]
    public async Task Withdraw_ExistingAccount_ShouldDecreaseBalance()
    {
        var payload = new StringContent(
            "{\"type\":\"withdraw\",\"origin\":\"100\",\"amount\":5}",
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/Account/event", payload);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"balance\":15", body);
    }

    [Fact]
    public async Task Transfer_ExistingAccount_ShouldMoveFunds()
    {
        var payload = new StringContent(
            "{\"type\":\"transfer\",\"origin\":\"100\",\"amount\":15,\"destination\":\"300\"}",
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/Account/event", payload);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"origin\"", body);
        Assert.Contains("\"balance\":0", body);
        Assert.Contains("\"destination\"", body);
        Assert.Contains("\"balance\":15", body);
    }

    [Fact]
    public async Task Transfer_NonExistingOrigin_ShouldReturn404()
    {
        var payload = new StringContent(
            "{\"type\":\"transfer\",\"origin\":\"200\",\"amount\":15,\"destination\":\"300\"}",
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/Account/event", payload);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Equal("0", body);
    }
}