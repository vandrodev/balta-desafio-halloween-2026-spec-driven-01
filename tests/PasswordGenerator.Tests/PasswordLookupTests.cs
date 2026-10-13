using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api.Data;
using PasswordGenerator.Api.Services;
using Xunit;

namespace PasswordGenerator.Tests;

public sealed class PasswordLookupTests
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnPassword_WhenRecordExists()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<PasswordGeneratorDbContext>()
            .UseSqlite(connection)
            .Options;

        await using (var context = new PasswordGeneratorDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();

            var passwordRecord = new PasswordGenerator.Api.Models.PasswordRecord
            {
                Id = Guid.NewGuid(),
                Password = "Abcdef123456!@#",
                CreatedAt = DateTime.UtcNow
            };

            context.Passwords.Add(passwordRecord);
            await context.SaveChangesAsync();

            var service = new PasswordService(context);
            var result = await service.GetByIdAsync(passwordRecord.Id);

            Assert.NotNull(result);
            Assert.Equal(passwordRecord.Id, result.Id);
            Assert.Equal(passwordRecord.Password, result.Password);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrow_WhenGuidIsInvalid()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<PasswordGeneratorDbContext>()
            .UseSqlite(connection)
            .Options;

        await using (var context = new PasswordGeneratorDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            var service = new PasswordService(context);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.GetByIdAsync(Guid.Empty));
            Assert.Contains("GUID", exception.Message, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrow_WhenRecordDoesNotExist()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<PasswordGeneratorDbContext>()
            .UseSqlite(connection)
            .Options;

        await using (var context = new PasswordGeneratorDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            var service = new PasswordService(context);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetByIdAsync(Guid.NewGuid()));
            Assert.Contains("não foi encontrado", exception.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
