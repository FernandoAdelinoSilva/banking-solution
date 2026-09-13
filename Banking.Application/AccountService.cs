using Banking.Application.Interfaces;
using Banking.Domain;
using Banking.Infrastructure.Interfaces;

namespace Banking.Application;
public class AccountService : IAccountService
{
    private readonly IAccountStore _accountStore;

    public AccountService(IAccountStore accountStore)
    {
        _accountStore = accountStore;
    }

    public Account CreateAccount()
    {
        var account = new Account(Guid.NewGuid());
        _accountStore.Save(account);
        return account;
    }

    public void Deposit(Guid accountId, decimal amount)
    {
        var account = GetAccount(accountId);
        account.Deposit(amount);
        _accountStore.Save(account);
    }

    public void Withdraw(Guid accountId, decimal amount)
    {
        var account = GetAccount(accountId);
        account.Withdraw(amount);
        _accountStore.Save(account);
    }

    public void Transfer(Guid fromAccountId, Guid toAccountId, decimal amount)
    {
        var fromAccount = GetAccount(fromAccountId);
        var toAccount = GetAccount(toAccountId);

        fromAccount.Withdraw(amount);
        toAccount.Deposit(amount);

        _accountStore.Save(fromAccount);
        _accountStore.Save(toAccount);
    }

    public decimal GetBalance(Guid accountId)
    {
        var account = GetAccount(accountId);
        return account.Balance;
    }

    private Account GetAccount(Guid accountId)
    {
        var account = _accountStore.Get(accountId);
        if (account is null)
            throw new InvalidOperationException("Account not found.");

        return account;
    }
}
