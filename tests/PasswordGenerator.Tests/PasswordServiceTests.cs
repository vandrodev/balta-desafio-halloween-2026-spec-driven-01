using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api.Data;
using PasswordGenerator.Api.Models;
using PasswordGenerator.Api.Services;
using Xunit;

namespace PasswordGenerator.Tests;

public sealed class PasswordServiceTests
{
    [Fact]
    public async Task GenerateAsync_ShouldCreateAndPersistStrongPassword()
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
            var passwordRecord = await service.GenerateAsync();

            Assert.NotEqual(Guid.Empty, passwordRecord.Id);
            Assert.True(PasswordValidator.IsValid(passwordRecord.Password));

            var persisted = await context.Passwords.SingleAsync();
            Assert.Equal(passwordRecord.Id, persisted.Id);
            Assert.Equal(passwordRecord.Password, persisted.Password);
        }
    }
}
