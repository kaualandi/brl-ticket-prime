using Dapper;
using System.Data;
using TicketPrime.Api.Features.Cupons;
using TicketPrime.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

DefaultTypeMap.MatchNamesWithUnderscores = true;

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
		"""
		SELECT
			id AS Id,
			codigo AS Codigo,
			percentual_desconto AS PorcentagemDesconto,
			valor_minimo_regra AS ValorMinimoRegra
		FROM cupons
		ORDER BY codigo
		""");

	return Results.Ok(cupons);
});

app.MapGet("/api/cupons/{id:int}", async (int id, IDbConnection db) =>
{
	var cupom = await db.QueryFirstOrDefaultAsync<Cupom>(
		"""
		SELECT
			id AS Id,
			codigo AS Codigo,
			percentual_desconto AS PorcentagemDesconto,
			valor_minimo_regra AS ValorMinimoRegra
		FROM cupons
		WHERE id = @Id
		""",
		new { Id = id });

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
		"SELECT COUNT(*) FROM cupons WHERE codigo = @Codigo",
		new { Codigo = codigo });

	if (count > 0)
	{
		return Results.BadRequest("Código de cupom já cadastrado.");
	}

	var cupomId = await db.ExecuteScalarAsync<int>(
		"""
		INSERT INTO cupons (codigo, percentual_desconto, valor_minimo_regra)
		OUTPUT INSERTED.id
		VALUES (@Codigo, @PorcentagemDesconto, @ValorMinimoRegra)
		""",
		new
		{
			Codigo = codigo,
			cupom.PorcentagemDesconto,
			cupom.ValorMinimoRegra
		});

	return Results.Created($"/api/cupons/{cupomId}", null);
});

app.MapPut("/api/cupons/{id:int}", async (int id, Cupom cupom, IDbConnection db) =>
{
	var validationError = ValidateCupom(cupom);

	if (validationError is not null)
	{
		return Results.BadRequest(validationError);
	}

	var codigoNovo = cupom.Codigo.Trim();

	var codigoDuplicado = await db.ExecuteScalarAsync<int>(
		"SELECT COUNT(*) FROM cupons WHERE codigo = @CodigoNovo AND id <> @Id",
		new { CodigoNovo = codigoNovo, Id = id });

	if (codigoDuplicado > 0)
	{
		return Results.BadRequest("Código de cupom já cadastrado.");
	}

	var rowsAffected = await db.ExecuteAsync(
		"UPDATE cupons SET codigo = @Codigo, percentual_desconto = @PorcentagemDesconto, valor_minimo_regra = @ValorMinimoRegra WHERE id = @Id",
		new
		{
			Codigo = codigoNovo,
			cupom.PorcentagemDesconto,
			cupom.ValorMinimoRegra,
			Id = id
		});

	if (rowsAffected == 0)
	{
		return Results.NotFound();
	}

	return Results.Ok();
});

app.MapDelete("/api/cupons/{id:int}", async (int id, IDbConnection db) =>
{
	var rowsAffected = await db.ExecuteAsync(
		"DELETE FROM cupons WHERE id = @Id", new { Id = id });

	if (rowsAffected == 0)
	{
		return Results.NotFound();
	}

	return Results.NoContent();
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
