using Accounts.DTO;
using Accounts.Entity;
using Accounts.Services;
using Microsoft.AspNetCore.Mvc;


namespace Accounts.Controllers;

[Route("api/doctors")]
[ApiController]
[Produces("application/json")]

public class DoctorsController : ControllerBase
{
    private readonly AccountService _accountService;

    public DoctorsController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDoctors(string? nameFilter, int from = 0, int count = 100)
    {
        return Ok(await _accountService.GetAccountsByRoleAsync(
                                                        roleName: "doctor",
                                                        nameFilter: nameFilter,
                                                        from: from,
                                                        count: count));
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDoctor(int id)
    {
        var doctor = await _accountService.GetAccountByRoleAsync(id: id, roleName: "doctor");
        if (doctor is not null)
        {
            return Ok(doctor);
        }
        return NotFound();
    }
}