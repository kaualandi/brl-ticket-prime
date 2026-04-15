using Dapper;
using System.Data;
using TicketPrime.Api.Features.Cupons;
using TicketPrime.Api.Features.Eventos;
using TicketPrime.Api.Features.Usuarios;
using TicketPrime.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddCorsPolicy();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowBlazorClient");

// ═══════════════════════════════════════════════════════
//  EVENTOS
// ═══════════════════════════════════════════════════════

app.MapGet("/api/eventos", async (IDbConnection db) =>
{
    var lista = await db.QueryAsync<Evento>(
        """
        SELECT Id, Nome, Descricao, CapacidadeTotal, DataEvento,
               PrecoPadrao, LocalEvento, ImageUrl
        FROM   Eventos
        ORDER  BY DataEvento
        """);
    return Results.Ok(lista);
});

app.MapGet("/api/eventos/{id:int}", async (int id, IDbConnection db) =>
{
    var evento = await db.QueryFirstOrDefaultAsync<Evento>(
        """
        SELECT Id, Nome, Descricao, CapacidadeTotal, DataEvento,
               PrecoPadrao, LocalEvento, ImageUrl
        FROM   Eventos
        WHERE  Id = @Id
        """,
        new { Id = id });
    return evento is null ? Results.NotFound() : Results.Ok(evento);
});

app.MapPost("/api/eventos", async (CreateEventoRequest req, IDbConnection db) =>
{
    if (string.IsNullOrWhiteSpace(req.Nome))        return Results.BadRequest("Nome é obrigatório.");
    if (string.IsNullOrWhiteSpace(req.Descricao))   return Results.BadRequest("Descrição é obrigatória.");
    if (string.IsNullOrWhiteSpace(req.LocalEvento)) return Results.BadRequest("LocalEvento é obrigatório.");
    if (req.CapacidadeTotal <= 0)                   return Results.BadRequest("CapacidadeTotal deve ser maior que zero.");
    if (req.DataEvento == default)                  return Results.BadRequest("DataEvento é obrigatória.");
    if (req.PrecoPadrao <= 0)                       return Results.BadRequest("Preço padrão deve ser maior que zero.");

    var id = await db.ExecuteScalarAsync<int>(
        """
        INSERT INTO Eventos (Nome, Descricao, CapacidadeTotal, DataEvento, PrecoPadrao, LocalEvento, ImageUrl)
        OUTPUT INSERTED.Id
        VALUES (@Nome, @Descricao, @CapacidadeTotal, @DataEvento, @PrecoPadrao, @LocalEvento, @ImageUrl)
        """,
        new
        {
            Nome        = req.Nome.Trim(),
            Descricao   = req.Descricao.Trim(),
            req.CapacidadeTotal,
            req.DataEvento,
            req.PrecoPadrao,
            LocalEvento = req.LocalEvento.Trim(),
            ImageUrl    = req.ImageUrl?.Trim() ?? string.Empty
        });

    return Results.Created($"/api/eventos/{id}", null);
});

app.MapPut("/api/eventos/{id:int}", async (int id, CreateEventoRequest req, IDbConnection db) =>
{
    if (string.IsNullOrWhiteSpace(req.Nome))        return Results.BadRequest("Nome é obrigatório.");
    if (string.IsNullOrWhiteSpace(req.Descricao))   return Results.BadRequest("Descrição é obrigatória.");
    if (string.IsNullOrWhiteSpace(req.LocalEvento)) return Results.BadRequest("LocalEvento é obrigatório.");
    if (req.CapacidadeTotal <= 0)                   return Results.BadRequest("CapacidadeTotal deve ser maior que zero.");
    if (req.DataEvento == default)                  return Results.BadRequest("DataEvento é obrigatória.");
    if (req.PrecoPadrao <= 0)                       return Results.BadRequest("Preço padrão deve ser maior que zero.");

    var rows = await db.ExecuteAsync(
        """
        UPDATE Eventos
        SET    Nome = @Nome, Descricao = @Descricao, CapacidadeTotal = @CapacidadeTotal,
               DataEvento = @DataEvento, PrecoPadrao = @PrecoPadrao,
               LocalEvento = @LocalEvento, ImageUrl = @ImageUrl
        WHERE  Id = @Id
        """,
        new
        {
            Id = id,
            Nome        = req.Nome.Trim(),
            Descricao   = req.Descricao.Trim(),
            req.CapacidadeTotal,
            req.DataEvento,
            req.PrecoPadrao,
            LocalEvento = req.LocalEvento.Trim(),
            ImageUrl    = req.ImageUrl?.Trim() ?? string.Empty
        });

    return rows == 0 ? Results.NotFound() : Results.Ok();
});

app.MapDelete("/api/eventos/{id:int}", async (int id, IDbConnection db) =>
{
    var rows = await db.ExecuteAsync(
        "DELETE FROM Eventos WHERE Id = @Id", new { Id = id });
    return rows == 0 ? Results.NotFound() : Results.NoContent();
});

// ═══════════════════════════════════════════════════════
//  CUPONS
// ═══════════════════════════════════════════════════════

app.MapGet("/api/cupons", async (IDbConnection db) =>
{
    var lista = await db.QueryAsync<Cupom>(
        "SELECT Codigo, PorcentagemDesconto, ValorMinimoRegra FROM Cupons ORDER BY Codigo");
    return Results.Ok(lista);
});

app.MapGet("/api/cupons/{codigo}", async (string codigo, IDbConnection db) =>
{
    var cupom = await db.QueryFirstOrDefaultAsync<Cupom>(
        "SELECT Codigo, PorcentagemDesconto, ValorMinimoRegra FROM Cupons WHERE Codigo = @Codigo",
        new { Codigo = codigo });
    return cupom is null ? Results.NotFound() : Results.Ok(cupom);
});

app.MapPost("/api/cupons", async (Cupom cupom, IDbConnection db) =>
{
    var err = ValidarCupom(cupom);
    if (err is not null) return Results.BadRequest(err);

    var codigo = cupom.Codigo.Trim();

    var exists = await db.ExecuteScalarAsync<int>(
        "SELECT COUNT(*) FROM Cupons WHERE Codigo = @Codigo",
        new { Codigo = codigo });
    if (exists > 0) return Results.BadRequest("Código de cupom já cadastrado.");

    await db.ExecuteAsync(
        "INSERT INTO Cupons (Codigo, PorcentagemDesconto, ValorMinimoRegra) VALUES (@Codigo, @PorcentagemDesconto, @ValorMinimoRegra)",
        new { Codigo = codigo, cupom.PorcentagemDesconto, cupom.ValorMinimoRegra });

    return Results.Created($"/api/cupons/{codigo}", null);
});

app.MapPut("/api/cupons/{codigo}", async (string codigo, Cupom cupom, IDbConnection db) =>
{
    var err = ValidarCupom(cupom);
    if (err is not null) return Results.BadRequest(err);

    var codigoNovo = cupom.Codigo.Trim();

    if (!codigoNovo.Equals(codigo, StringComparison.OrdinalIgnoreCase))
    {
        var conflict = await db.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM Cupons WHERE Codigo = @CodigoNovo",
            new { CodigoNovo = codigoNovo });
        if (conflict > 0) return Results.BadRequest("Código de cupom já cadastrado.");
    }

    var rows = await db.ExecuteAsync(
        """
        UPDATE Cupons
        SET    Codigo = @CodigoNovo, PorcentagemDesconto = @PorcentagemDesconto,
               ValorMinimoRegra = @ValorMinimoRegra
        WHERE  Codigo = @Codigo
        """,
        new { Codigo = codigo, CodigoNovo = codigoNovo, cupom.PorcentagemDesconto, cupom.ValorMinimoRegra });

    return rows == 0 ? Results.NotFound() : Results.Ok();
});

app.MapDelete("/api/cupons/{codigo}", async (string codigo, IDbConnection db) =>
{
    var rows = await db.ExecuteAsync(
        "DELETE FROM Cupons WHERE Codigo = @Codigo", new { Codigo = codigo });
    return rows == 0 ? Results.NotFound() : Results.NoContent();
});

// ═══════════════════════════════════════════════════════
//  USUARIOS
// ═══════════════════════════════════════════════════════

app.MapGet("/api/usuarios", async (IDbConnection db) =>
{
    var lista = await db.QueryAsync<object>(
        "SELECT Cpf, Nome, Email FROM Usuarios");
    return Results.Ok(lista);
});

app.MapGet("/api/usuarios/{cpf}", async (string cpf, IDbConnection db) =>
{
    var usuario = await db.QueryFirstOrDefaultAsync<object>(
        "SELECT Cpf, Nome, Email, Senha FROM Usuarios WHERE Cpf = @Cpf",
        new { Cpf = cpf });
    return usuario is null ? Results.NotFound() : Results.Ok(usuario);
});

app.MapPost("/api/usuarios", async (CreateUsuarioRequest req, IDbConnection db) =>
{
    if (string.IsNullOrWhiteSpace(req.Cpf))   return Results.BadRequest("CPF é obrigatório.");
    if (string.IsNullOrWhiteSpace(req.Nome))  return Results.BadRequest("Nome é obrigatório.");
    if (string.IsNullOrWhiteSpace(req.Email)) return Results.BadRequest("Email é obrigatório.");
    if (string.IsNullOrWhiteSpace(req.Senha)) return Results.BadRequest("Senha é obrigatória.");

    var cpf = req.Cpf.Trim();

    var exists = await db.ExecuteScalarAsync<int>(
        "SELECT COUNT(*) FROM Usuarios WHERE Cpf = @Cpf",
        new { Cpf = cpf });
    if (exists > 0) return Results.BadRequest("CPF já cadastrado.");

    await db.ExecuteAsync(
        "INSERT INTO Usuarios (Cpf, Nome, Email, Senha) VALUES (@Cpf, @Nome, @Email, @Senha)",
        new { Cpf = cpf, Nome = req.Nome.Trim(), Email = req.Email.Trim(), Senha = req.Senha });

    return Results.Created($"/api/usuarios/{cpf}", null);
});

app.MapPut("/api/usuarios/{cpf}", async (string cpf, CreateUsuarioRequest req, IDbConnection db) =>
{
    if (string.IsNullOrWhiteSpace(req.Nome))  return Results.BadRequest("Nome é obrigatório.");
    if (string.IsNullOrWhiteSpace(req.Email)) return Results.BadRequest("Email é obrigatório.");
    if (string.IsNullOrWhiteSpace(req.Senha)) return Results.BadRequest("Senha é obrigatória.");

    var rows = await db.ExecuteAsync(
        "UPDATE Usuarios SET Nome = @Nome, Email = @Email, Senha = @Senha WHERE Cpf = @Cpf",
        new { Cpf = cpf, Nome = req.Nome.Trim(), Email = req.Email.Trim(), Senha = req.Senha });

    return rows == 0 ? Results.NotFound() : Results.Ok();
});

app.MapDelete("/api/usuarios/{cpf}", async (string cpf, IDbConnection db) =>
{
    var rows = await db.ExecuteAsync(
        "DELETE FROM Usuarios WHERE Cpf = @Cpf", new { Cpf = cpf });
    return rows == 0 ? Results.NotFound() : Results.NoContent();
});

// ─── helpers ────────────────────────────────────────────
static string? ValidarCupom(Cupom c)
{
    if (string.IsNullOrWhiteSpace(c.Codigo))
        return "Código é obrigatório.";
    if (c.PorcentagemDesconto < 1 || c.PorcentagemDesconto > 100)
        return "PorcentagemDesconto deve ser um valor entre 1 e 100.";
    if (c.ValorMinimoRegra < 0)
        return "ValorMinimoRegra não pode ser negativo.";
    return null;
}

app.Run();
