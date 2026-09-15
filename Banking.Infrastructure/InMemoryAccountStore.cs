using Banking.Domain;
using Banking.Infrastructure.Interfaces;
using System.Collections.Concurrent;

namespace Banking.Infrastructure;

public class InMemoryAccountStore : IAccountStore
{
    private readonly ConcurrentDictionary<string, Account> _accounts = new();

    public void Save(Account account)
    {
        _accounts[account.Id] = account;
    }

    public Account? Get(string accountId)
    {
        _accounts.TryGetValue(accountId, out var account);
        return account;
    }

    public IEnumerable<Account> GetAll()
    {
        return _accounts.Values;
    }
    public void Clear()
    {
        _accounts.Clear();
    }
}
