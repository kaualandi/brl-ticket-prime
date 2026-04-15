using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Reflection;
using TicketPrime.Api.Features.Cupons;
using TicketPrime.Api.Features.Eventos;
using TicketPrime.Api.Features.Usuarios;
using TicketPrime.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddCorsPolicy();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TicketPrime API",
        Version = "v1",
        Description = "API REST para gestão de eventos, cupons e usuários da plataforma TicketPrime.",
        Contact = new OpenApiContact
        {
            Name = "Equipe TicketPrime"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowBlazorClient");
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "TicketPrime API v1");
    options.DocumentTitle = "TicketPrime API Docs";
    options.RoutePrefix = "swagger";
});

var eventos = app.MapGroup("/api/eventos")
    .WithTags("Eventos");

eventos.MapGet("/", async Task<Ok<IEnumerable<Evento>>> (IDbConnection db) =>
    {
        var lista = await db.QueryAsync<Evento>(
            """
            SELECT Id, Nome, Descricao, CapacidadeTotal, DataEvento,
                   PrecoPadrao, LocalEvento, ImageUrl
            FROM   Eventos
            ORDER  BY DataEvento
            """);

        return TypedResults.Ok(lista);
    })
    .WithName("ListarEventos")
    .WithSummary("Lista todos os eventos")
    .WithDescription("Retorna a coleção completa de eventos cadastrados, ordenada pela data de realização.")
    .Produces<IEnumerable<Evento>>(StatusCodes.Status200OK);

eventos.MapGet("/{id:int}", async Task<Results<Ok<Evento>, NotFound>> (int id, IDbConnection db) =>
    {
        var evento = await db.QueryFirstOrDefaultAsync<Evento>(
            """
            SELECT Id, Nome, Descricao, CapacidadeTotal, DataEvento,
                   PrecoPadrao, LocalEvento, ImageUrl
            FROM   Eventos
            WHERE  Id = @Id
            """,
            new { Id = id });

        return evento is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(evento);
    })
    .WithName("ObterEventoPorId")
    .WithSummary("Obtém um evento por ID")
    .WithDescription("Busca um evento específico pelo identificador numérico informado na rota.")
    .Produces<Evento>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

eventos.MapPost("/", async Task<Results<Created, BadRequest<string>>> (CreateEventoRequest req, IDbConnection db) =>
    {
        var erro = ValidarEvento(req);
        if (erro is not null)
            return TypedResults.BadRequest(erro);

        var id = await db.ExecuteScalarAsync<int>(
            """
            INSERT INTO Eventos (Nome, Descricao, CapacidadeTotal, DataEvento, PrecoPadrao, LocalEvento, ImageUrl)
            OUTPUT INSERTED.Id
            VALUES (@Nome, @Descricao, @CapacidadeTotal, @DataEvento, @PrecoPadrao, @LocalEvento, @ImageUrl)
            """,
            new
            {
                Nome = req.Nome.Trim(),
                Descricao = req.Descricao.Trim(),
                req.CapacidadeTotal,
                req.DataEvento,
                req.PrecoPadrao,
                LocalEvento = req.LocalEvento.Trim(),
                ImageUrl = req.ImageUrl?.Trim() ?? string.Empty
            });

        return TypedResults.Created($"/api/eventos/{id}");
    })
    .WithName("CriarEvento")
    .WithSummary("Cria um novo evento")
    .WithDescription("Cadastra um evento com nome, descrição, local, data, capacidade, preço padrão e URL opcional de imagem.")
    .Accepts<CreateEventoRequest>("application/json")
    .Produces(StatusCodes.Status201Created)
    .Produces<string>(StatusCodes.Status400BadRequest, "text/plain");

eventos.MapPut("/{id:int}", async Task<Results<Ok, NotFound, BadRequest<string>>> (int id, CreateEventoRequest req, IDbConnection db) =>
    {
        var erro = ValidarEvento(req);
        if (erro is not null)
            return TypedResults.BadRequest(erro);

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
                Nome = req.Nome.Trim(),
                Descricao = req.Descricao.Trim(),
                req.CapacidadeTotal,
                req.DataEvento,
                req.PrecoPadrao,
                LocalEvento = req.LocalEvento.Trim(),
                ImageUrl = req.ImageUrl?.Trim() ?? string.Empty
            });

        return rows == 0
            ? TypedResults.NotFound()
            : TypedResults.Ok();
    })
    .WithName("AtualizarEvento")
    .WithSummary("Atualiza um evento existente")
    .WithDescription("Atualiza os dados de um evento já cadastrado a partir do ID informado na rota.")
    .Accepts<CreateEventoRequest>("application/json")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces<string>(StatusCodes.Status400BadRequest, "text/plain");

eventos.MapDelete("/{id:int}", async Task<Results<NoContent, NotFound>> (int id, IDbConnection db) =>
    {
        var rows = await db.ExecuteAsync(
            "DELETE FROM Eventos WHERE Id = @Id", new { Id = id });

        return rows == 0
            ? TypedResults.NotFound()
            : TypedResults.NoContent();
    })
    .WithName("ExcluirEvento")
    .WithSummary("Exclui um evento")
    .WithDescription("Remove definitivamente um evento cadastrado a partir do ID informado.")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

var cupons = app.MapGroup("/api/cupons")
    .WithTags("Cupons");

cupons.MapGet("/", async Task<Ok<IEnumerable<Cupom>>> (IDbConnection db) =>
    {
        var lista = await db.QueryAsync<Cupom>(
            "SELECT Codigo, PorcentagemDesconto, ValorMinimoRegra FROM Cupons ORDER BY Codigo");

        return TypedResults.Ok(lista);
    })
    .WithName("ListarCupons")
    .WithSummary("Lista todos os cupons")
    .WithDescription("Retorna a coleção completa de cupons cadastrados, ordenada pelo código.")
    .Produces<IEnumerable<Cupom>>(StatusCodes.Status200OK);

cupons.MapGet("/{codigo}", async Task<Results<Ok<Cupom>, NotFound>> (string codigo, IDbConnection db) =>
    {
        var cupom = await db.QueryFirstOrDefaultAsync<Cupom>(
            "SELECT Codigo, PorcentagemDesconto, ValorMinimoRegra FROM Cupons WHERE Codigo = @Codigo",
            new { Codigo = codigo });

        return cupom is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(cupom);
    })
    .WithName("ObterCupomPorCodigo")
    .WithSummary("Obtém um cupom pelo código")
    .WithDescription("Busca um cupom específico pelo código informado na rota.")
    .Produces<Cupom>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

cupons.MapPost("/", async Task<Results<Created, BadRequest<string>>> (Cupom cupom, IDbConnection db) =>
    {
        var err = ValidarCupom(cupom);
        if (err is not null)
            return TypedResults.BadRequest(err);

        var codigo = cupom.Codigo.Trim();

        var exists = await db.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM Cupons WHERE Codigo = @Codigo",
            new { Codigo = codigo });

        if (exists > 0)
            return TypedResults.BadRequest("Código de cupom já cadastrado.");

        await db.ExecuteAsync(
            "INSERT INTO Cupons (Codigo, PorcentagemDesconto, ValorMinimoRegra) VALUES (@Codigo, @PorcentagemDesconto, @ValorMinimoRegra)",
            new { Codigo = codigo, cupom.PorcentagemDesconto, cupom.ValorMinimoRegra });

        return TypedResults.Created($"/api/cupons/{codigo}");
    })
    .WithName("CriarCupom")
    .WithSummary("Cria um novo cupom")
    .WithDescription("Cadastra um cupom com código, porcentagem de desconto e valor mínimo da regra.")
    .Accepts<Cupom>("application/json")
    .Produces(StatusCodes.Status201Created)
    .Produces<string>(StatusCodes.Status400BadRequest, "text/plain");

cupons.MapPut("/{codigo}", async Task<Results<Ok, NotFound, BadRequest<string>>> (string codigo, Cupom cupom, IDbConnection db) =>
    {
        var err = ValidarCupom(cupom);
        if (err is not null)
            return TypedResults.BadRequest(err);

        var codigoNovo = cupom.Codigo.Trim();

        if (!codigoNovo.Equals(codigo, StringComparison.OrdinalIgnoreCase))
        {
            var conflict = await db.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Cupons WHERE Codigo = @CodigoNovo",
                new { CodigoNovo = codigoNovo });

            if (conflict > 0)
                return TypedResults.BadRequest("Código de cupom já cadastrado.");
        }

        var rows = await db.ExecuteAsync(
            """
            UPDATE Cupons
            SET    Codigo = @CodigoNovo, PorcentagemDesconto = @PorcentagemDesconto,
                   ValorMinimoRegra = @ValorMinimoRegra
            WHERE  Codigo = @Codigo
            """,
            new { Codigo = codigo, CodigoNovo = codigoNovo, cupom.PorcentagemDesconto, cupom.ValorMinimoRegra });

        return rows == 0
            ? TypedResults.NotFound()
            : TypedResults.Ok();
    })
    .WithName("AtualizarCupom")
    .WithSummary("Atualiza um cupom existente")
    .WithDescription("Atualiza o código, a porcentagem de desconto e o valor mínimo da regra de um cupom já cadastrado.")
    .Accepts<Cupom>("application/json")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces<string>(StatusCodes.Status400BadRequest, "text/plain");

cupons.MapDelete("/{codigo}", async Task<Results<NoContent, NotFound>> (string codigo, IDbConnection db) =>
    {
        var rows = await db.ExecuteAsync(
            "DELETE FROM Cupons WHERE Codigo = @Codigo", new { Codigo = codigo });

        return rows == 0
            ? TypedResults.NotFound()
            : TypedResults.NoContent();
    })
    .WithName("ExcluirCupom")
    .WithSummary("Exclui um cupom")
    .WithDescription("Remove definitivamente um cupom cadastrado a partir do código informado.")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

var usuarios = app.MapGroup("/api/usuarios")
    .WithTags("Usuários");

usuarios.MapGet("/", async Task<Ok<IEnumerable<UsuarioListItemResponse>>> (IDbConnection db) =>
    {
        var lista = await db.QueryAsync<UsuarioListItemResponse>(
            "SELECT Cpf, Nome, Email FROM Usuarios");

        return TypedResults.Ok(lista);
    })
    .WithName("ListarUsuarios")
    .WithSummary("Lista todos os usuários")
    .WithDescription("Retorna a coleção de usuários cadastrados com CPF, nome e e-mail.")
    .Produces<IEnumerable<UsuarioListItemResponse>>(StatusCodes.Status200OK);

usuarios.MapGet("/{cpf}", async Task<Results<Ok<UsuarioDetailResponse>, NotFound>> (string cpf, IDbConnection db) =>
    {
        var usuario = await db.QueryFirstOrDefaultAsync<UsuarioDetailResponse>(
            "SELECT Cpf, Nome, Email, Senha FROM Usuarios WHERE Cpf = @Cpf",
            new { Cpf = cpf });

        return usuario is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(usuario);
    })
    .WithName("ObterUsuarioPorCpf")
    .WithSummary("Obtém um usuário pelo CPF")
    .WithDescription("Busca um usuário específico pelo CPF informado na rota.")
    .Produces<UsuarioDetailResponse>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

usuarios.MapPost("/", async Task<Results<Created, BadRequest<string>>> (CreateUsuarioRequest req, IDbConnection db) =>
    {
        var erro = ValidarUsuario(req);
        if (erro is not null)
            return TypedResults.BadRequest(erro);

        var cpf = req.Cpf.Trim();

        var exists = await db.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM Usuarios WHERE Cpf = @Cpf",
            new { Cpf = cpf });

        if (exists > 0)
            return TypedResults.BadRequest("CPF já cadastrado.");

        await db.ExecuteAsync(
            "INSERT INTO Usuarios (Cpf, Nome, Email, Senha) VALUES (@Cpf, @Nome, @Email, @Senha)",
            new { Cpf = cpf, Nome = req.Nome.Trim(), Email = req.Email.Trim(), Senha = req.Senha });

        return TypedResults.Created($"/api/usuarios/{cpf}");
    })
    .WithName("CriarUsuario")
    .WithSummary("Cria um novo usuário")
    .WithDescription("Cadastra um novo usuário com CPF, nome, e-mail e senha.")
    .Accepts<CreateUsuarioRequest>("application/json")
    .Produces(StatusCodes.Status201Created)
    .Produces<string>(StatusCodes.Status400BadRequest, "text/plain");

usuarios.MapPut("/{cpf}", async Task<Results<Ok, NotFound, BadRequest<string>>> (string cpf, CreateUsuarioRequest req, IDbConnection db) =>
    {
        var erro = ValidarUsuarioParaAtualizacao(req);
        if (erro is not null)
            return TypedResults.BadRequest(erro);

        var rows = await db.ExecuteAsync(
            "UPDATE Usuarios SET Nome = @Nome, Email = @Email, Senha = @Senha WHERE Cpf = @Cpf",
            new { Cpf = cpf, Nome = req.Nome.Trim(), Email = req.Email.Trim(), Senha = req.Senha });

        return rows == 0
            ? TypedResults.NotFound()
            : TypedResults.Ok();
    })
    .WithName("AtualizarUsuario")
    .WithSummary("Atualiza um usuário existente")
    .WithDescription("Atualiza os dados cadastrais de um usuário identificado pelo CPF informado na rota.")
    .Accepts<CreateUsuarioRequest>("application/json")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces<string>(StatusCodes.Status400BadRequest, "text/plain");

usuarios.MapDelete("/{cpf}", async Task<Results<NoContent, NotFound>> (string cpf, IDbConnection db) =>
    {
        var rows = await db.ExecuteAsync(
            "DELETE FROM Usuarios WHERE Cpf = @Cpf", new { Cpf = cpf });

        return rows == 0
            ? TypedResults.NotFound()
            : TypedResults.NoContent();
    })
    .WithName("ExcluirUsuario")
    .WithSummary("Exclui um usuário")
    .WithDescription("Remove definitivamente um usuário cadastrado a partir do CPF informado.")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

app.Run();

static string? ValidarEvento(CreateEventoRequest req)
{
    if (string.IsNullOrWhiteSpace(req.Nome))
        return "Nome é obrigatório.";
    if (string.IsNullOrWhiteSpace(req.Descricao))
        return "Descrição é obrigatória.";
    if (string.IsNullOrWhiteSpace(req.LocalEvento))
        return "Local do evento é obrigatório.";
    if (req.CapacidadeTotal <= 0)
        return "Capacidade total deve ser maior que zero.";
    if (req.DataEvento == default)
        return "Data do evento é obrigatória.";
    if (req.PrecoPadrao <= 0)
        return "Preço padrão deve ser maior que zero.";
    return null;
}

static string? ValidarCupom(Cupom c)
{
    if (string.IsNullOrWhiteSpace(c.Codigo))
        return "Código é obrigatório.";
    if (c.PorcentagemDesconto < 1 || c.PorcentagemDesconto > 100)
        return "Porcentagem de desconto deve ser um valor entre 1 e 100.";
    if (c.ValorMinimoRegra < 0)
        return "Valor mínimo da regra não pode ser negativo.";
    return null;
}

static string? ValidarUsuario(CreateUsuarioRequest req)
{
    if (string.IsNullOrWhiteSpace(req.Cpf))
        return "CPF é obrigatório.";
    if (string.IsNullOrWhiteSpace(req.Nome))
        return "Nome é obrigatório.";
    if (string.IsNullOrWhiteSpace(req.Email))
        return "E-mail é obrigatório.";
    if (string.IsNullOrWhiteSpace(req.Senha))
        return "Senha é obrigatória.";
    return null;
}

static string? ValidarUsuarioParaAtualizacao(CreateUsuarioRequest req)
{
    if (string.IsNullOrWhiteSpace(req.Nome))
        return "Nome é obrigatório.";
    if (string.IsNullOrWhiteSpace(req.Email))
        return "E-mail é obrigatório.";
    if (string.IsNullOrWhiteSpace(req.Senha))
        return "Senha é obrigatória.";
    return null;
}
