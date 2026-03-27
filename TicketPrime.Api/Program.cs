using Dapper;
using System.Data;
using TicketPrime.Api.Features.Cupons;
using TicketPrime.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddCorsPolicy();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowBlazorClient");
app.UseAuthorization();

app.MapGet("/api/cupons", async (IDbConnection db) =>
{
	var cupons = await db.QueryAsync<Cupom>(
		"SELECT Codigo, PorcentagemDesconto, ValorMinimoRegra FROM Cupons ORDER BY Codigo");

	return Results.Ok(cupons);
});

app.MapGet("/api/cupons/{codigo}", async (string codigo, IDbConnection db) =>
{
	var cupom = await db.QueryFirstOrDefaultAsync<Cupom>(
		"SELECT Codigo, PorcentagemDesconto, ValorMinimoRegra FROM Cupons WHERE Codigo = @Codigo",
		new { Codigo = codigo.Trim() });

	return cupom is null ? Results.NotFound() : Results.Ok(cupom);
});

app.MapPost("/api/cupons", async (Cupom cupom, IDbConnection db) =>
{
	var validationError = ValidateCupom(cupom);

	if (validationError is not null)
	{
		return Results.BadRequest(validationError);
	}

	var codigo = cupom.Codigo.Trim();

	var count = await db.ExecuteScalarAsync<int>(
		"SELECT COUNT(*) FROM Cupons WHERE Codigo = @Codigo",
		new { Codigo = codigo });

	if (count > 0)
	{
		return Results.BadRequest("Código de cupom já cadastrado.");
	}

	await db.ExecuteAsync(
		"INSERT INTO Cupons (Codigo, PorcentagemDesconto, ValorMinimoRegra) VALUES (@Codigo, @PorcentagemDesconto, @ValorMinimoRegra)",
		new
		{
			Codigo = codigo,
			cupom.PorcentagemDesconto,
			cupom.ValorMinimoRegra
		});

	return Results.Created($"/api/cupons/{codigo}", null);
});

app.MapPut("/api/cupons/{codigo}", async (string codigo, Cupom cupom, IDbConnection db) =>
{
	var validationError = ValidateCupom(cupom);

	if (validationError is not null)
	{
		return Results.BadRequest(validationError);
	}

	var codigoAtual = codigo.Trim();
	var codigoNovo = cupom.Codigo.Trim();

	var codigoDuplicado = await db.ExecuteScalarAsync<int>(
		"SELECT COUNT(*) FROM Cupons WHERE Codigo = @CodigoNovo AND Codigo <> @CodigoAtual",
		new { CodigoNovo = codigoNovo, CodigoAtual = codigoAtual });

	if (codigoDuplicado > 0)
	{
		return Results.BadRequest("Código de cupom já cadastrado.");
	}

	var rowsAffected = await db.ExecuteAsync(
		"UPDATE Cupons SET Codigo = @Codigo, PorcentagemDesconto = @PorcentagemDesconto, ValorMinimoRegra = @ValorMinimoRegra WHERE Codigo = @CodigoAtual",
		new
		{
			Codigo = codigoNovo,
			cupom.PorcentagemDesconto,
			cupom.ValorMinimoRegra,
			CodigoAtual = codigoAtual
		});

	if (rowsAffected == 0)
	{
		return Results.NotFound();
	}

	return Results.Ok();
});

static string? ValidateCupom(Cupom cupom)
{
	if (string.IsNullOrWhiteSpace(cupom.Codigo))
	{
		return "Codigo é obrigatório.";
	}

	if (cupom.PorcentagemDesconto < 1 || cupom.PorcentagemDesconto > 100)
	{
		return "PorcentagemDesconto deve ser um valor entre 1 e 100.";
	}

	if (cupom.ValorMinimoRegra < 0)
	{
		return "ValorMinimoRegra não pode ser negativo.";
	}

	return null;
}

app.MapControllers();

app.Run();
