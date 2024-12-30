
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace accounts.DTO;


public class AccountCreateDTO
{
    public required string UserName { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
    [DefaultValue(new[] { "user" })]
    public List<string> Roles { get; set; } = ["user"];

}