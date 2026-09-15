using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _service;

    public AccountController(IAccountService service)
    {
        _service = service;
    }

    [HttpGet("balance")]
    public IActionResult GetBalance([FromQuery] string accountId)
    {
        try
        {
            var balance = _service.GetBalance(accountId);
            return Ok(balance);
        }
        catch
        {
            return NotFound(0);
        }
    }

    [HttpPost("event")]
    public IActionResult HandleEvent([FromBody] EventDTO request)
    {
        try
        {
            var result = _service.ProcessEvent(request);
            return Created("", result);
        }
        catch
        {
            return NotFound(0);
        }
    }

    [HttpPost("reset")]
    public IActionResult Reset()
    {
        _service.Reset();
        return Ok();
    }
}