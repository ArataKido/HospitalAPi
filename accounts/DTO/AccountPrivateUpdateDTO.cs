using System.ComponentModel;
using Accounts.Migrations;

namespace Accounts.DTO;

public class AccountPrivateUpdateDTO
{

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; }


}