using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api.Data;
using PasswordGenerator.Api.Endpoints;
using PasswordGenerator.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<PasswordService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=passwords.db";
builder.Services.AddDbContext<PasswordGeneratorDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Password Generator API v1");
        options.RoutePrefix = "swagger";
    });
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PasswordGeneratorDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.MapPasswordEndpoints();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();

public partial class Program;
