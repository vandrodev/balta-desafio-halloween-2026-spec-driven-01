using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PasswordGenerator.Api.Services;

namespace PasswordGenerator.Api.Endpoints;

public static class PasswordEndpoints
{
    public static WebApplication MapPasswordEndpoints(this WebApplication app)
    {
        app.MapPost("/passwords", async Task<Created<PasswordCreatedResponse>> (PasswordService service) =>
            {
                var passwordRecord = await service.GenerateAsync();
                return TypedResults.Created($"/passwords/{passwordRecord.Id}", new PasswordCreatedResponse(passwordRecord.Id));
            })
            .WithName("CreatePassword")
            .WithSummary("Gera e persiste uma nova senha forte.")
            .WithDescription("Cria um novo registro de senha, valida a regra de negócio e retorna o GUID associado.")
            .Produces<PasswordCreatedResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapGet("/passwords/{id}", async Task<Results<Ok<PasswordResponse>, BadRequest<ProblemDetails>, NotFound<ProblemDetails>>> (string id, PasswordService service) =>
            {
                if (!Guid.TryParse(id, out var passwordId))
                {
                    return TypedResults.BadRequest(new ProblemDetails
                    {
                        Title = "Identificador inválido",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "O identificador informado deve ser um GUID válido.",
                        Type = "https://httpstatuses.com/400"
                    });
                }

                try
                {
                    var passwordRecord = await service.GetByIdAsync(passwordId);
                    return TypedResults.Ok(new PasswordResponse(passwordRecord.Id, passwordRecord.Password));
                }
                catch (ArgumentException ex)
                {
                    return TypedResults.BadRequest(new ProblemDetails
                    {
                        Title = "Identificador inválido",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = ex.Message,
                        Type = "https://httpstatuses.com/400"
                    });
                }
                catch (KeyNotFoundException ex)
                {
                    return TypedResults.NotFound(new ProblemDetails
                    {
                        Title = "Senha não encontrada",
                        Status = StatusCodes.Status404NotFound,
                        Detail = ex.Message,
                        Type = "https://httpstatuses.com/404"
                    });
                }
            })
            .WithName("GetPasswordById")
            .WithSummary("Consulta uma senha cadastrada pelo GUID.")
            .WithDescription("Valida o identificador informado e retorna a senha associada quando o registro existe.")
            .Produces<PasswordResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }
}

/// <summary>Representa o identificador gerado para uma senha persistida.</summary>
public sealed record PasswordCreatedResponse(Guid Id);

/// <summary>Representa o resultado da consulta de uma senha por identificador.</summary>
public sealed record PasswordResponse(Guid Id, string Password);
