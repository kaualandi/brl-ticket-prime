# Matriz de Seleção de Ciclo

## Contexto do Sistema

O TicketPrime é um backend de vendas de ingressos desenvolvido por alunos de Engenharia de Software. O sistema atende compradores de ingressos e operadores que cadastram eventos e cupons. Ele deve garantir que nenhum ingresso seja vendido além da capacidade do evento, que cambistas sejam bloqueados e que cupons de desconto sejam aplicados corretamente.

## Critérios Determinantes

- Os requisitos são fixos e não negociáveis: rotas, tabelas e regras de negócio já estão definidos pelo professor.
- O contrato exige provas de engenharia além do código: documentação, testes e métricas de confiabilidade são obrigatórios.
- O sistema anterior falhou em produção por ausência de validações, o que exige feedback técnico antes da entrega final.
- A correção é automatizada e case-sensitive, logo erros estruturais precisam ser detectados cedo.

## Maiores Riscos Identificados

- Alto risco de falha nas regras de negócio (overselling, fraude de cupom, cambismo), que já causaram prejuízo real no sistema anterior.
- Alto risco de vulnerabilidade de segurança por SQL Injection se as queries forem escritas com concatenação.
- Alto risco de perda de pontos por erros estruturais (nomes de pastas, arquivos ou rotas com case errado).
- Baixo risco de mudança de escopo, pois os requisitos são imutáveis.
- Baixo risco de segurança de vida, pois o sistema é digital e não envolve operações físicas críticas.

## Modelo de Ciclo Recomendado

Iterativo e Incremental.

## Justificativa Técnica

O modelo iterativo divide o projeto em duas entregas funcionais (AV1 e AV2). Na primeira iteração, a fundação é validada: banco de dados, endpoints básicos, segurança com Dapper e infraestrutura de testes. Só então, na segunda iteração, o motor de vendas com as regras críticas de negócio é construído sobre essa base já verificada. Isso impede que falhas de segurança ou estrutura se propaguem para a parte mais complexa do sistema. Cada iteração gera evidências concretas de engenharia (User Stories, testes com Assert, ADR, Matriz de Riscos), atendendo diretamente à exigência contratual do cliente.

## Pré-requisitos

Para executar o projeto, é necessário ter instalado:

- .NET 10 ou superior
- Docker Desktop

## Configuração do banco

1. Inicie o Docker Desktop e suba o SQL Server:

```bash
docker compose up -d
```

2. Crie o banco e as tabelas:

```bash
docker exec ticketprime-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'TicketPrime@2026' -C -Q "CREATE DATABASE TicketPrime;"

docker exec ticketprime-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'TicketPrime@2026' -C -d TicketPrime -Q "
CREATE TABLE cupons (
    id INT IDENTITY(1,1) PRIMARY KEY,
    codigo VARCHAR(50) NOT NULL UNIQUE,
    percentual_desconto DECIMAL(5,2) NOT NULL,
    valor_minimo_regra DECIMAL(10,2) NOT NULL DEFAULT 0
);

CREATE TABLE Eventos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(200) NOT NULL,
    Descricao VARCHAR(1000) NOT NULL DEFAULT '',
    CapacidadeTotal INT NOT NULL,
    DataEvento DATETIME NOT NULL,
    PrecoPadrao DECIMAL(10,2) NOT NULL,
    LocalEvento VARCHAR(200) NOT NULL DEFAULT '',
    ImageUrl VARCHAR(500) NOT NULL DEFAULT ''
);

CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(200) NOT NULL,
    Email VARCHAR(200) NOT NULL UNIQUE,
    Senha VARCHAR(200) NOT NULL,
    Cpf VARCHAR(14) NOT NULL DEFAULT ''
);
"
```

3. Se o banco já existia sem a coluna `ImageUrl`, execute a migration:

```bash
docker exec ticketprime-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'TicketPrime@2026' -C -d TicketPrime -Q "ALTER TABLE Eventos ADD ImageUrl VARCHAR(500) NOT NULL DEFAULT '';"
```

## Seed de dados

Para popular o banco com 8 eventos de exemplo:

```bash
docker exec -i ticketprime-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'TicketPrime@2026' -C -d TicketPrime < seed.sql
```

## Executando o projeto

Em terminais separados:

```bash
# API (http://localhost:5201)
dotnet watch --project TicketPrime.Api --launch-profile http

# Client Blazor (http://localhost:5272)
dotnet watch --project TicketPrime.Client
```
