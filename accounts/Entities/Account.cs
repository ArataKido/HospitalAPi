using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace accounts.Entity;

[Index(nameof(Id), IsUnique = true)]
[Index(nameof(UserName), IsUnique = true)]
[Index(nameof(FullName))]
public class Account
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string FullName { get; private set; }
    public required string Password { get; set; }
    [DefaultValue("null")]
    public DateTime? DeletedAt { get; set; } = null;

    public List<AccountRole> AccountRoles { get; set; } = [];
    public void UpdateFullName()
    {
        FullName = $"{FirstName} {LastName}".Trim();
    }





}
