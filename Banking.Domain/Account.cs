namespace Banking.Domain;
public class Account
{
    public string Id { get; private set; }
    public decimal Balance { get; private set; }

    public Account(string id)
    {
        Id = id;
        Balance = 0m;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive.");

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdraw amount must be positive.");

        if (Balance < amount)
            throw new InvalidOperationException("Insufficient funds.");

        Balance -= amount;
    }
}
