using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api.Data;
using PasswordGenerator.Api.Models;
using Xunit;

namespace PasswordGenerator.Tests;

public sealed class PasswordGeneratorDbContextTests
{
    [Fact]
    public async Task CanCreateAndStorePasswordRecord()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<PasswordGeneratorDbContext>()
            .UseSqlite(connection)
            .Options;

        await using (var context = new PasswordGeneratorDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();

            var password = new PasswordRecord
            {
                Id = Guid.NewGuid(),
                Password = "Abcdef123456!@",
                CreatedAt = DateTime.UtcNow
            };

            context.Passwords.Add(password);
            await context.SaveChangesAsync();
        }

        await using (var context = new PasswordGeneratorDbContext(options))
        {
            var saved = await context.Passwords.SingleAsync();
            Assert.Equal("Abcdef123456!@", saved.Password);
        }
    }
}
