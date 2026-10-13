namespace PasswordGenerator.Api.Services;

public static class PasswordValidator
{
    public const int MinimumLength = 16;
    public const string AllowedSpecialCharacters = "!@#$%^&*()-_=+[]{};:,.<>/?|~`";

    public static bool IsValid(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        if (password.Length < MinimumLength)
            return false;

        if (password.Any(char.IsWhiteSpace))
            return false;

        if (!password.Any(char.IsDigit))
            return false;

        if (!password.Any(character => AllowedSpecialCharacters.Contains(character)))
            return false;

        return true;
    }

    public static void EnsureValid(string? password)
    {
        if (!IsValid(password))
        {
            throw new ArgumentException(
                "A senha informada é inválida. Ela deve ter pelo menos 16 caracteres, conter números e um caractere especial, e não pode conter espaços.",
                nameof(password));
        }
    }
}
