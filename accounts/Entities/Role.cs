
using Microsoft.EntityFrameworkCore;

namespace accounts.Entity;
[Index(nameof(Id), IsUnique = true)]
public class Role
{
    public int Id;
    public required string Name { get; set; }
    public List<AccountRole> AccountRoles { get; set; } = [];
}