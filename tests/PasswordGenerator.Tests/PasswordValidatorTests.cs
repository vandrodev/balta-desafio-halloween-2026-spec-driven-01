using PasswordGenerator.Api.Services;
using Xunit;

namespace PasswordGenerator.Tests;

public sealed class PasswordValidatorTests
{
    [Fact]
    public void ValidPassword_ShouldPassValidation()
    {
        var password = "Abcdef123456!@#$";

        var isValid = PasswordValidator.IsValid(password);

        Assert.True(isValid);
    }

    [Theory]
    [InlineData("Abcdef123456!@#$")]
    [InlineData("P@ssw0rdValid123456")]
    [InlineData("Zebra-1234567890!@")]
    public void ValidPassword_ShouldPassValidation_ForMultipleStrongPasswords(string password)
    {
        var isValid = PasswordValidator.IsValid(password);

        Assert.True(isValid);
    }

    [Theory]
    [InlineData("Abcdef123456!@#")]
    [InlineData("Abcdef123456 ")]
    [InlineData("Abcdef!@#$")]
    [InlineData("abcdefg1234567!")]
    [InlineData("Abcdefghijklmnop!")]
    [InlineData("Abcdefghijklmnop")]
    [InlineData("")]
    [InlineData(null)]
    public void InvalidPassword_ShouldFailValidation(string? password)
    {
        var isValid = PasswordValidator.IsValid(password);

        Assert.False(isValid);
    }

    [Fact]
    public void EnsureValid_ShouldThrow_WhenPasswordIsInvalid()
    {
        var exception = Assert.Throws<ArgumentException>(() => PasswordValidator.EnsureValid("shortpass!"));

        Assert.Contains("senha", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void EnsureValid_ShouldNotThrow_WhenPasswordIsValid()
    {
        var exception = Record.Exception(() => PasswordValidator.EnsureValid("Abcdef123456!@#$"));

        Assert.Null(exception);
    }

    [Theory]
    [InlineData("Abcdef123456!@#")]
    [InlineData("Abcdef123456!")]
    [InlineData("Abcdef123456")]
    public void IsValid_ShouldRejectPasswordsShorterThanMinimumLength(string password)
    {
        var isValid = PasswordValidator.IsValid(password);

        Assert.False(isValid);
    }

    [Theory]
    [InlineData("Abcdefghijklmnop1")]
    [InlineData("Abcdefghijklmnop9")]
    [InlineData("Abcdefghijklmno123")]
    public void IsValid_ShouldRejectPasswordsWithoutSpecialCharacter(string password)
    {
        var isValid = PasswordValidator.IsValid(password);

        Assert.False(isValid);
    }

    [Theory]
    [InlineData("Abcdefg1234567 !")]
    [InlineData("Abcdefg1234567\t!")]
    [InlineData("Abcdefg1234567\n!")]
    public void IsValid_ShouldRejectPasswordsContainingWhitespace(string password)
    {
        var isValid = PasswordValidator.IsValid(password);

        Assert.False(isValid);
    }

    [Theory]
    [InlineData("Abcdefghijklmnop!")]
    [InlineData("Abcdefghijklmno!@#")]
    [InlineData("abcdefghijklmnop!")]
    public void IsValid_ShouldRejectPasswordsWithoutNumericCharacter(string password)
    {
        var isValid = PasswordValidator.IsValid(password);

        Assert.False(isValid);
    }
}
