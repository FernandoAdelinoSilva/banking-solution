using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain;
public class Event
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public EventType Type { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Timestamp { get; private set; }

    public Event(Guid accountId, EventType type, decimal amount)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        Type = type;
        Amount = amount;
        Timestamp = DateTime.UtcNow;
    }
}
