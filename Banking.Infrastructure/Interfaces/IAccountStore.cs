using Banking.Domain;

namespace Banking.Infrastructure.Interfaces;
public interface IAccountStore
{
    void Save(Account account);
    Account? Get(Guid accountId);
    IEnumerable<Account> GetAll();
}
