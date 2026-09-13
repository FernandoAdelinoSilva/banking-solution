using Banking.Application;
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
    public void CreateAccount_ShouldReturnNewAccount()
    {
        var account = _service.CreateAccount();
        Assert.NotNull(account);
        Assert.Equal(0, account.Balance);
    }

    [Fact]
    public void Deposit_ShouldIncreaseBalance()
    {
        var account = _service.CreateAccount();
        _service.Deposit(account.Id, 100);
        Assert.Equal(100, _service.GetBalance(account.Id));
    }

    [Fact]
    public void Withdraw_ShouldDecreaseBalance()
    {
        var account = _service.CreateAccount();
        _service.Deposit(account.Id, 200);
        _service.Withdraw(account.Id, 50);
        Assert.Equal(150, _service.GetBalance(account.Id));
    }

    [Fact]
    public void Transfer_ShouldMoveFundsBetweenAccounts()
    {
        var from = _service.CreateAccount();
        var to = _service.CreateAccount();
        _service.Deposit(from.Id, 300);

        _service.Transfer(from.Id, to.Id, 100);

        Assert.Equal(200, _service.GetBalance(from.Id));
        Assert.Equal(100, _service.GetBalance(to.Id));
    }
}