using Banking.Domain;

namespace Banking.Application.Interfaces;
public interface IAccountService
{
    Account CreateAccount();
    void Deposit(Guid accountId, decimal amount);
    void Withdraw(Guid accountId, decimal amount);
    void Transfer(Guid fromAccountId, Guid toAccountId, decimal amount);
    decimal GetBalance(Guid accountId);
}
