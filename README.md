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

- .NET 8 ou superior
- SQL Server Express
- SQL Server Management Studio (SSMS) ou Azure Data Studio

## Configuração do banco

Execute o script abaixo no SQL Server:

```bash
db/ticketprime.sql

