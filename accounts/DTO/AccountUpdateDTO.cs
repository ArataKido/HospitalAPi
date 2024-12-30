using System.ComponentModel;
using accounts.Migrations;

namespace accounts.DTO;

public class AccountUpdateDTO
{

    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; }
    [DefaultValue(new[] { "user" })]
    public List<string> Roles { get; set; } = new List<string> { "user" };


}