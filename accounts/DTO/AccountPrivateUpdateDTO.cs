using System.ComponentModel;
using accounts.Migrations;

namespace accounts.DTO;

public class AccountPrivateUpdateDTO
{

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; }


}