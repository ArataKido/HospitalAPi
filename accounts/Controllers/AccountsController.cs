
using System.Security.Claims;
using Accounts.DTO;
using Accounts.Entity;
using Accounts.Services;
using Accounts.Utils.Mappers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Controllers;

[Route("api/accounts")]
[ApiController]
[Produces("application/json")]
public class AccountsController : ControllerBase
{
    private readonly AccountService _accountService;
    private readonly PasswordService _passwordService;
    private readonly IMapper _mapper;

    public AccountsController(AccountService accountService, IMapper mapper, PasswordService passwordService)
    {
        _accountService = accountService;
        _mapper = mapper;
        _passwordService = passwordService;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    /// <summary>
    /// Admin only.
    /// </summary>
    public async Task<IActionResult> GetAccounts(int from = 0, int count = 100)
    {
        IList<AccountResponseDTO> accounts = await _accountService.GetAllAsync(from, count);
        return Ok(accounts);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    /// <summary>
/// Admin only.
/// </summary>
    public async Task<IActionResult> CreateAccount(AccountCreateDTO accountCreateDTO)
    {
        if(await _accountService.GetAccountByUserName(accountCreateDTO.UserName) is not null)
        {
            return BadRequest("User with such username already exists");
        }

        Account account = _mapper.Map<Account>(accountCreateDTO);
        account.Password = _passwordService.HashPassword(accountCreateDTO.Password);

        await _accountService.CreateAccountAsync(account);

        if (accountCreateDTO.Roles is not null)
        {
            await _accountService.UpdateUserRolesAsync(account.Id, accountCreateDTO.Roles);
        }

        AccountResponseDTO accountDTO = _mapper.Map<AccountResponseDTO>(account);
        return CreatedAtAction(nameof(CreateAccount), accountDTO);
    }


    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
/// <summary>
/// Admin only.
/// </summary>
    public async Task<IActionResult> UpdateAccount(int id, AccountUpdateDTO accountUpdateDTO)
    {
        var account = await _accountService.GetAccountByIdAsync(id);
        if (account is null) return NotFound();
        if(accountUpdateDTO.UserName is not null)
        {
            var userName = await _accountService.GetAccountByUserName(accountUpdateDTO.UserName);
            if(userName is not null && userName.Id != account.Id)
            return BadRequest("User with such username already exists");
        }

        if (accountUpdateDTO.Password is not null && 
            !_passwordService.VerifyPassword(account.Password, accountUpdateDTO.Password)
            )
        {
            accountUpdateDTO.Password = _passwordService.HashPassword(accountUpdateDTO.Password);
        }
        _mapper.Map(accountUpdateDTO, account);

        await _accountService.UpdateAccountAsync(account);

        await _accountService.UpdateUserRolesAsync(account.Id, accountUpdateDTO.Roles);

        return Ok(_mapper.Map<AccountResponseDTO>(account));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    /// <summary>
/// Admin only.
/// </summary>

    public async Task<IActionResult> DeleteAccount(int id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);

        if (account is null)
        {
            return NotFound();
        }
        return Ok(await _accountService.DeleteAccountAsync(account));
    }

    [HttpPut("Update")]
    [Authorize]
    public async Task<IActionResult> UpdateAccount(AccountPrivateUpdateDTO accountUpdateDTO)
    {
        var account = await _accountService.GetAccountByIdAsync(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));

        if(account is null) return NotFound();

        if(accountUpdateDTO.Password is not null && 
            !_passwordService.VerifyPassword(account.Password, accountUpdateDTO.Password)
            )
        {
            accountUpdateDTO.Password = _passwordService.HashPassword(accountUpdateDTO.Password);
        }

        _mapper.Map(accountUpdateDTO, account);

        await _accountService.UpdateAccountAsync(account);

        return Ok(_mapper.Map<AccountResponseDTO>(account));
    }

    [HttpGet("Me")]
    [Authorize]
    public async Task<IActionResult> GetAccount()
    {
        var account = await _accountService.GetAccountByIdAsync(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        if (account is null) return NotFound();
        return Ok(_mapper.Map<AccountResponseDTO>(account));
    }
}