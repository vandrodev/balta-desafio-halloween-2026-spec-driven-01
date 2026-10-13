using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api.Data;
using PasswordGenerator.Api.Models;

namespace PasswordGenerator.Api.Services;

public sealed class PasswordService
{
    private const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
    private const string Digits = "0123456789";
    private const string SeedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+[]{};:,.<>/?|~`";

    private readonly PasswordGeneratorDbContext _dbContext;

    public PasswordService(PasswordGeneratorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PasswordRecord> GenerateAsync()
    {
        var password = GenerateStrongPassword();
        var passwordRecord = new PasswordRecord
        {
            Id = Guid.NewGuid(),
            Password = password,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Passwords.Add(passwordRecord);
        await _dbContext.SaveChangesAsync();

        return passwordRecord;
    }

    public async Task<PasswordRecord> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("O GUID informado é inválido.", nameof(id));
        }

        var passwordRecord = await _dbContext.Passwords
            .SingleOrDefaultAsync(record => record.Id == id);

        if (passwordRecord is null)
        {
            throw new KeyNotFoundException($"A senha com o GUID '{id}' não foi encontrado.");
        }

        return passwordRecord;
    }

    private static string GenerateStrongPassword()
    {
        var passwordChars = new List<char>(PasswordValidator.MinimumLength)
        {
            UppercaseLetters[Random.Shared.Next(UppercaseLetters.Length)],
            LowercaseLetters[Random.Shared.Next(LowercaseLetters.Length)],
            Digits[Random.Shared.Next(Digits.Length)],
            PasswordValidator.AllowedSpecialCharacters[Random.Shared.Next(PasswordValidator.AllowedSpecialCharacters.Length)]
        };

        while (passwordChars.Count < PasswordValidator.MinimumLength)
        {
            passwordChars.Add(SeedCharacters[Random.Shared.Next(SeedCharacters.Length)]);
        }

        var password = new string(passwordChars.OrderBy(_ => Guid.NewGuid()).ToArray());

        if (!PasswordValidator.IsValid(password))
        {
            throw new InvalidOperationException("A senha gerada não atende aos critérios mínimos de segurança.");
        }

        return password;
    }
}
