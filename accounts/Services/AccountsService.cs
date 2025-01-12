using Accounts.Context;
using Accounts.DTO;
using Accounts.Entity;
using Microsoft.EntityFrameworkCore;


namespace Accounts.Services;
public class AccountService
{
    private readonly PostgresDbContext _postgresDbContext;
    private readonly ILogger _log;

    public AccountService(PostgresDbContext postgresDbContext, ILogger<AccountService> logger, PasswordService passwordService)
    {
        _postgresDbContext = postgresDbContext;
        _log = logger;
    }

    public async Task<Account> CreateAccountAsync(Account account)
    {
        await _postgresDbContext.Accounts.AddAsync(account);
        await _postgresDbContext.SaveChangesAsync();
        return account;
    }

    public async Task<Account?> GetAccountByIdAsync(int id)
    {
        return await _postgresDbContext.Accounts.FindAsync(id);
    }
    public async Task<Account?> GetAccountByUserName(string userName)
    {
        return await _postgresDbContext.Accounts.Where(x => x.UserName == userName).FirstOrDefaultAsync();
    }
    public async Task<IList<AccountResponseDTO>> GetAllAsync(int from, int count)
    {

        return await _postgresDbContext.Accounts
            .Include(a => a.AccountRoles)
                .ThenInclude(ar => ar.Role)
            .Select(a => new AccountResponseDTO
            {
                Id = a.Id,
                UserName = a.UserName,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Roles = a.AccountRoles.Select(ar => ar.Role.Name).ToList()
            })
            .Skip(from)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<AccountResponseDTO>> GetAccountsByRoleAsync(string roleName, string nameFilter, int from, int count)
    {
        var query = _postgresDbContext.AccountRoles
                                            .Include(a => a.Account)
                                            .Include(r => r.Role)
                                            .Where(x => x.Role.Name == roleName)
                                            .Skip(from)
                                            .Take(count);
        if (nameFilter is not null)
        {
            query = query.Where(a => EF.Functions.Like(a.Account.FullName, $"%{nameFilter}%"));
        }

        return await query.Select(a => new AccountResponseDTO
        {
            Id = a.Account.Id,
            UserName = a.Account.UserName,
            FirstName = a.Account.FirstName,
            LastName = a.Account.LastName,
        })
                    .ToListAsync();
    }

    public async Task<AccountRole?> GetAccountByRoleAsync(int id, string roleName)
    {
        return await _postgresDbContext.AccountRoles
                                            .Include(a => a.Account)
                                            .Include(r => r.Role)
                                            .Where(x => x.AccountId == id && x.Role.Name == roleName)
                                            .FirstOrDefaultAsync();
    }

    public async Task<Account> UpdateAccountAsync(Account account)
    {

        _postgresDbContext.Accounts.Update(account);
        await _postgresDbContext.SaveChangesAsync();
        return account;
    }

    public async Task<Account> DeleteAccountAsync(Account account)
    {
        account.DeletedAt = DateTime.Now.ToUniversalTime();
        _postgresDbContext.Accounts.Update(account);
        await _postgresDbContext.SaveChangesAsync();
        return account;
    }

    public async Task UpdateUserRolesAsync(int userId, List<string> roleNames)
    {
        var account = await _postgresDbContext.Accounts
                                .Include(ar => ar.AccountRoles)
                                .ThenInclude(r => r.Role)
                                .FirstOrDefaultAsync(u => u.Id == userId);
        HashSet<Role> roles = [];

        if (account is null)
        {
            _log.LogInformation($"Account with id '{userId}' does not exists");
            return;
        }

        foreach (string roleName in roleNames)
        {
            try
            {
                roles.Add(
                    await _postgresDbContext.Roles.FirstAsync(r => r.Name == roleName)
                    );
            }
            catch (InvalidOperationException)
            {
                _log.LogError($"Role with name '{roleName}' does not exits");
            }

        }

        account.AccountRoles.RemoveAll(ar => !roleNames.Contains(ar.Role.Name));

        foreach (Role role in roles)
        {
            if (!account.AccountRoles.Any(x => x.AccountId == account.Id && x.RoleId == role.Id))
            {
                account.AccountRoles.Add(new AccountRole { RoleId = role.Id, AccountId = account.Id });

            }
        }

        _postgresDbContext.Accounts.Update(account);
        await _postgresDbContext.SaveChangesAsync();

        return;
    }

    public async Task<Account?> Authenticate(string userName, string password)
    {
        var account = await _postgresDbContext.Accounts
                                            .Where(a => a.UserName == userName && a.Password == password)
                                            .Include(ar => ar.AccountRoles)
                                            .ThenInclude(r => r.Role)
                                            .FirstOrDefaultAsync();
        return account;
    }

    public async Task<bool> AccountWithRoleExists(int id, string roleName)
    {
        return await _postgresDbContext.AccountRoles
                                    .Include(a => a.Account)
                                    .Include(r => r.Role)
                                    .AnyAsync(a => a.AccountId == id && a.Role.Name == roleName); 
    }


    // public async Task<IdentityResult> 


}