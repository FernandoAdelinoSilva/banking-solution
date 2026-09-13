using Banking.Application.Interfaces;
using Banking.Domain;

namespace Banking.Application;
public class AccountService : IAccountService
{
    private readonly Dictionary<Guid, Account> _accounts = new();

    public Account CreateAccount()
    {
        throw new NotImplementedException();
    }

    public void Deposit(Guid accountId, decimal amount)
    {
        throw new NotImplementedException();
    }

    public void Withdraw(Guid accountId, decimal amount)
    {
        throw new NotImplementedException();
    }

    public void Transfer(Guid fromAccountId, Guid toAccountId, decimal amount)
    {
        throw new NotImplementedException();
    }

    public decimal GetBalance(Guid accountId)
    {
        throw new NotImplementedException();
    }

    private Account GetAccount(Guid accountId)
    {
        throw new NotImplementedException();
    }
}
