using System.Runtime.InteropServices;
using Accounts.DTO;
using Accounts.Entity;
using Accounts.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Controllers;

[Route("api/authentication")]
[ApiController]
[Produces("application/json")]
public class AuthenticationController : ControllerBase
{
    private readonly AccountService _accountService;
    private readonly AuthenticationService _authenticationService;
    private readonly PasswordService _passwordService;
    private readonly IMapper _mapper;

    public AuthenticationController(AccountService accountService, AuthenticationService authenticationService, IMapper mapper, PasswordService passwordService)
    {
        _accountService = accountService;
        _authenticationService = authenticationService;
        _mapper = mapper;
        _passwordService = passwordService;
    }



    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp(AccountCreateDTO accountCreateDTO)
    {
        if(await _accountService.GetAccountByUserName(accountCreateDTO.UserName) is not null)
        {
            return BadRequest("User with such username already exists");
        }

        Account account = _mapper.Map<Account>(accountCreateDTO);
        account.Password = _passwordService.HashPassword(accountCreateDTO.Password.Trim());

        await _accountService.CreateAccountAsync(account);

        if (accountCreateDTO.Roles is not null)
        {
            await _accountService.UpdateUserRolesAsync(account.Id, accountCreateDTO.Roles);
        }
        return Ok(_mapper.Map<AccountResponseDTO>(account));
    }

    [HttpPost("SignIn")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn(string username, string password)
    {
        var account = await _accountService.GetAccountByUserName(userName: username);
        if (account is null || account.DeletedAt is not null || !_passwordService.VerifyPassword(account.Password, password.Trim()))
        {
            return Unauthorized("Invalid username or password");
        }
        return Ok(_authenticationService.GenerateJwtToken(account));
    }
}