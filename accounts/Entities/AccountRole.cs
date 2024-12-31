
namespace Accounts.Entity;

public class AccountRole
{
    public required int AccountId;
    public required int RoleId;
    public Account? Account;
    public Role? Role;

}