# CLAUDE.md — TicketPrime

Guia para agentes trabalhando neste repositório. Leia antes de editar.

## O que é

Projeto acadêmico de Engenharia de Software: **TicketPrime**, um backend de venda de
ingressos. A entrega é dividida em duas iterações:

- **AV1 (entregue):** fundação — CRUD de Eventos, Cupons e Usuários, banco estruturado,
  acesso seguro via Dapper, Swagger e suíte de testes de validação.
- **AV2 (a fazer):** **motor de vendas** — endpoint de reservas/compra com as regras de
  negócio críticas (anti-overselling, bloqueio de cambista, aplicação de cupom). Ver
  `docs/av2-tarefas.md`.

A correção é **automatizada e case-sensitive**: nomes de rotas, tabelas, colunas e
mensagens de erro importam. Não renomeie nada sem necessidade.

## Stack e estrutura

- **.NET 10**. Solução: `TicketPrime.slnx`.
- `src/` — **API** (`TicketPrime.Api`). Minimal API, **todos os endpoints em `Program.cs`**
  agrupados por `app.MapGroup("/api/...")`. Acesso a dados com **Dapper** sobre **SQL Server**.
- `TicketPrime.Client/` — **Blazor WebAssembly** + MudBlazor. Consome a API. Serviços em
  `Services/{Feature}/` (interface + impl Api + mock + contracts).
- `tests/` — **xUnit**. `ValidacaoTests.cs` espelha a lógica de validação dos endpoints
  e a verifica com `Assert`, sem banco nem HTTP.
- `db/ticketprime.sql` — schema (DDL). `seed.sql` — dados de exemplo (8 eventos).
- `docker-compose.yml` — sobe SQL Server 2022 em `localhost:1433` (sa / `TicketPrime@2026`).
- `docs/` — requisitos e planos. `requisitos.md` = spec da AV1.

### Convenções de código (API)

- **Feature folders** em `src/Features/{Feature}/`: entidade, `Create{Feature}Request`,
  responses. Os endpoints ficam em `Program.cs` (não há controllers).
- **Sempre Dapper parametrizado** (`@param` + objeto anônimo). Nunca concatenar SQL —
  é critério de segurança (SQL Injection) e de nota.
- **Validação** em funções `static string? Validar...(...)` no fim de `Program.cs`,
  retornando a mensagem de erro (PT-BR) ou `null`.
- **Erros de negócio** → `TypedResults.BadRequest(string)` (texto puro, PT-BR), 400.
  Sucesso de criação → `TypedResults.Created(...)`, 201. Cada endpoint declara
  `.WithName/.WithSummary/.WithDescription/.Produces(...)` para o Swagger.
- Mensagens de erro em **português** e exatamente como nos critérios de aceitação.

## Comandos

```bash
# 1. Subir o banco
docker compose up -d

# 2. Criar schema + seed (container: ticketprime-sqlserver, senha: TicketPrime@2026)
docker exec -i ticketprime-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'TicketPrime@2026' -C -Q "CREATE DATABASE TicketPrime;"
docker exec -i ticketprime-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'TicketPrime@2026' -C -d TicketPrime < db/ticketprime.sql
docker exec -i ticketprime-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'TicketPrime@2026' -C -d TicketPrime < seed.sql

# 3. Build / testes / run
dotnet build TicketPrime.slnx
dotnet test tests
dotnet watch --project src --launch-profile http   # API  -> http://localhost:5201 (+ /swagger)
dotnet watch --project TicketPrime.Client           # UI   -> http://localhost:5272
```

> O schema em `db/ticketprime.sql` já inclui `CREATE DATABASE`/`USE`. O passo `CREATE DATABASE`
> separado do README é redundante quando se roda o arquivo inteiro — confira antes de duplicar.

## Modelo de dados (resumo)

- `Usuarios(Cpf PK, Nome, Email UNIQUE, Senha)`
- `Eventos(Id PK identity, Nome, CapacidadeTotal, DataEvento, PrecoPadrao, Descricao, LocalEvento, ImageUrl)`
- `Cupons(Codigo PK, PorcentagemDesconto, ValorMinimoRegra)`
- `Reservas(Id PK identity, UsuarioCpf FK, EventoId FK, CupomUtilizado FK NULL, ValorFinalPago)`
  — **1 reserva = 1 ingresso**. Base do motor de vendas da AV2 (ainda sem endpoint).

## Fluxo de trabalho

- **Branch por feature/fix** (`feat/...`, `fix/...`), baseada na `master` atualizada.
  Nunca commitar direto na `master`.
- Antes de commitar: `dotnet build` + `dotnet test` precisam passar.
- Commit: prefixo conventional-commit em **inglês** + título/corpo em **português**.
- **Nunca** adicionar co-autoria do Claude em commits ou PRs.
- Push do branch e oferecer link do PR; o merge é do usuário.
