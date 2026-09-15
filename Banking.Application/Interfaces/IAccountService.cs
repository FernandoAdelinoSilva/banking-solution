using Banking.Application.DTOs;

namespace Banking.Application.Interfaces;
public interface IAccountService
{
    decimal GetBalance(string accountId);
    object ProcessEvent(EventDTO request);
    void Reset();
}
