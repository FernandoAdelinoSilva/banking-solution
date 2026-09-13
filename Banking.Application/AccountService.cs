using Banking.Application.DTOs;
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

    public decimal GetBalance(string accountId)
    {
        var account = GetAccount(accountId);
        return account.Balance;
    }

    public object ProcessEvent(EventDTO request)
    {
        switch (request.Type.ToLower())
        {
            case "deposit":
                Deposit(request.Destination!, request.Amount);
                var destBalance = GetBalance(request.Destination!);
                return new { destination = new { id = request.Destination, balance = destBalance } };

            case "withdraw":
                Withdraw(request.Origin!, request.Amount);
                var originBalance = GetBalance(request.Origin!);
                return new { origin = new { id = request.Origin, balance = originBalance } };

            case "transfer":
                Transfer(request.Origin!, request.Destination!, request.Amount);
                var fromBalance = GetBalance(request.Origin!);
                var toBalance = GetBalance(request.Destination!);
                return new
                {
                    origin = new { id = request.Origin, balance = fromBalance },
                    destination = new { id = request.Destination, balance = toBalance }
                };

            default:
                throw new InvalidOperationException("Invalid event type");
        }
    }

    private void Deposit(string accountId, decimal amount)
    {
        var account = _accountStore.Get(accountId);

        if (account == null)
            account = CreateAccount(accountId);

        account.Deposit(amount);
        _accountStore.Save(account);
    }

    private void Withdraw(string accountId, decimal amount)
    {
        var account = GetAccount(accountId);
        account.Withdraw(amount);
        _accountStore.Save(account);
    }

    private void Transfer(string fromAccountId, string toAccountId, decimal amount)
    {
        var fromAccount = GetAccount(fromAccountId);

        var toAccount = _accountStore.Get(toAccountId);
        if (toAccount == null)
            toAccount = CreateAccount(toAccountId);

        fromAccount.Withdraw(amount);
        toAccount.Deposit(amount);

        _accountStore.Save(fromAccount);
        _accountStore.Save(toAccount);
    }

    private Account GetAccount(string accountId)
    {
        var account = _accountStore.Get(accountId);
        if (account is null)
            throw new InvalidOperationException("Account not found.");

        return account;
    }

    private Account CreateAccount(string accountId)
    {
        var account = new Account(accountId);
        _accountStore.Save(account);
        return account;
    }
}
