namespace Banking.Application.DTOs;

public class EventDTO
{
    public string Type { get; set; } = string.Empty;
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public decimal Amount { get; set; }
}
