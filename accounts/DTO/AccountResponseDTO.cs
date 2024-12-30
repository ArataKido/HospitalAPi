
using System.ComponentModel;
using accounts.Migrations;

namespace accounts.DTO;

public class AccountResponseDTO
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public List<string> Roles { get; set; } = new List<string> { "user" };


}