using Banking.Application;
using Banking.Application.DTOs;
using Banking.Infrastructure;

namespace Banking.Tests;

public class AccountServiceTests
{
    private readonly AccountService _service;

    public AccountServiceTests()
    {
        var store = new InMemoryAccountStore();
        _service = new AccountService(store);
    }

    [Fact]
    public void GetBalance_NonExistingAccount_ShouldReturnNotFound()
    {
        Assert.Throws<InvalidOperationException>(() => _service.GetBalance("1234"));
    }

    [Fact]
    public void Deposit_ShouldCreateAccountWithInitialBalance()
    {
        var request = new EventDTO { Type = "deposit", Destination = "100", Amount = 10 };
        var result = _service.ProcessEvent(request);

        var balance = _service.GetBalance("100");
        Assert.Equal(10, balance);
    }

    [Fact]
    public void Deposit_ShouldIncreaseBalanceForExistingAccount()
    {
        var request1 = new EventDTO { Type = "deposit", Destination = "100", Amount = 10 };
        _service.ProcessEvent(request1);

        var request2 = new EventDTO { Type = "deposit", Destination = "100", Amount = 10 };
        _service.ProcessEvent(request2);

        var balance = _service.GetBalance("100");
        Assert.Equal(20, balance);
    }

    [Fact]
    public void Withdraw_NonExistingAccount_ShouldThrow()
    {
        var request = new EventDTO { Type = "withdraw", Origin = "200", Amount = 10 };
        Assert.Throws<InvalidOperationException>(() => _service.ProcessEvent(request));
    }

    [Fact]
    public void Withdraw_ShouldDecreaseBalance()
    {
        var deposit = new EventDTO { Type = "deposit", Destination = "100", Amount = 20 };
        _service.ProcessEvent(deposit);

        var withdraw = new EventDTO { Type = "withdraw", Origin = "100", Amount = 5 };
        _service.ProcessEvent(withdraw);

        var balance = _service.GetBalance("100");
        Assert.Equal(15, balance);
    }

    [Fact]
    public void Transfer_ShouldMoveFundsBetweenAccounts()
    {
        var deposit = new EventDTO { Type = "deposit", Destination = "100", Amount = 15 };
        _service.ProcessEvent(deposit);

        var transfer = new EventDTO { Type = "transfer", Origin = "100", Destination = "300", Amount = 15 };
        _service.ProcessEvent(transfer);

        Assert.Equal(0, _service.GetBalance("100"));
        Assert.Equal(15, _service.GetBalance("300"));
    }

    [Fact]
    public void Transfer_NonExistingOrigin_ShouldThrow()
    {
        var transfer = new EventDTO { Type = "transfer", Origin = "200", Destination = "300", Amount = 15 };
        Assert.Throws<InvalidOperationException>(() => _service.ProcessEvent(transfer));
    }
}