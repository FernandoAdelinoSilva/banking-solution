using Banking.Domain;

namespace Banking.Infrastructure.Interfaces;
public interface IAccountStore
{
    void Save(Account account);
    Account? Get(string accountId);
    IEnumerable<Account> GetAll();
}
