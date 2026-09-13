using Banking.Application.Interfaces;
using Banking.Domain;

namespace Banking.Application;
public class AccountService : IAccountService
{
    private readonly Dictionary<Guid, Account> _accounts = new();

    public Account CreateAccount()
    {
        var account = new Account(Guid.NewGuid());
        _accounts[account.Id] = account;
        return account;
    }

    public void Deposit(Guid accountId, decimal amount)
    {
        var account = GetAccount(accountId);
        account.Deposit(amount);
    }

    public void Withdraw(Guid accountId, decimal amount)
    {
        var account = GetAccount(accountId);
        account.Withdraw(amount);
    }

    public void Transfer(Guid fromAccountId, Guid toAccountId, decimal amount)
    {
        var fromAccount = GetAccount(fromAccountId);
        var toAccount = GetAccount(toAccountId);

        fromAccount.Withdraw(amount);
        toAccount.Deposit(amount);
    }

    public decimal GetBalance(Guid accountId)
    {
        var account = GetAccount(accountId);
        return account.Balance;
    }

    private Account GetAccount(Guid accountId)
    {
        if (!_accounts.TryGetValue(accountId, out var account))
            throw new InvalidOperationException("Account not found.");

        return account;
    }
}
