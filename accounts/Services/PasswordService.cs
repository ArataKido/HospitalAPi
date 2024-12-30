using Microsoft.AspNetCore.Identity;

namespace accounts.Services;

public class PasswordService
{
    private readonly PasswordHasher<object> _passwordHasher = new();

    public string HashPassword(string plainPassword)
    {
        return _passwordHasher.HashPassword(null, plainPassword);
    }

    public bool VerifyPassword(string hashedPassword, string plainPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, plainPassword);
        return result == PasswordVerificationResult.Success;
    }
}
