# AV2 — Plano de Entrega (alinhado ao enunciado oficial)

> Fonte: `AV2 Avaliação` (enunciado do professor). A nota da AV2 = **4 requisitos de código**
> (agnósticos de domínio) + **20 artefatos de SDD × 0,5 = 10 pontos**. Cada item do SDD é
> verificado **automaticamente pela presença de literais exatos** em arquivos específicos —
> o que está entre `` precisa aparecer *ipsis litteris*. Board: issues **#81–#92**.

## Requisitos de código (agnósticos de domínio)

O domínio é livre; usamos o **motor de reservas** sobre a base da AV1.

| Req | Exigência | Onde | Issue |
|-----|-----------|------|-------|
| A1 | ≥2 endpoints com **regra de negócio** (não CRUD simples de 1 tabela) | `POST /api/reservas` + endpoint com JOIN | #81, #82 |
| A2 | ≥1 endpoint com **JOIN** (`INNER JOIN`/`LEFT JOIN`, 2+ tabelas) | `GET` reservas com evento/usuário | #82 |
| A3 | ≥1 endpoint com **≥3 validações** antes do INSERT/UPDATE → `400` com mensagem específica | validações da reserva | #81 |
| A4 | **Queries parametrizadas** (`@Parametro`, zero concatenação/interpolação) | todo o código de dados | #81, #82 |

## Os 20 itens do SDD (item → arquivo → literais exatos → issue)

| # | Artefato | Arquivo | Literais que o corretor busca | Issue |
|---|----------|---------|-------------------------------|-------|
| 01 | Testes AAA | `/tests` | `// Arrange`, `// Act`, `// Assert` em ≥3 métodos | #83 |
| 02 | Nomenclatura de testes | `/tests` | `Metodo_Cenario_ResultadoEsperado`; **sem** `if/switch/for/foreach/while` | #83 |
| 03 | Padrões arquiteturais | `/docs/analise_arquitetura.md` | 3 cenários; `Trade-off:` ou `Positivo:`/`Negativo:` | #84 |
| 04 | Violações arquiteturais | `/docs/analise_arquitetura.md` | ≥5; `**Problema:**` `**Evidência:**` `**Impacto:**` `**Ação Recomendada:**` | #84 |
| 05 | ADR | `/docs/adrs/001-*.md` | `## Contexto` `## Decisão` `## Consequências`; `Status:`; `Prós:`/`Contras:` | #85 |
| 06 | Registro de dívida técnica | `/docs/registro_divida_tecnica.md` | colunas `ID da Dívida` `Descrição Técnica` `Freq. Alteração` `Risco` `Esforço` `Decisão`; ≥6; `Alto`/`Médio`/`Baixo` | #86 |
| 07 | Priorização de dívida | `/docs/registro_divida_tecnica.md` | `Prioridade 1 (Imediato)` `Prioridade 2 (Próxima Sprint)` `Prioridade 3 (Aceitar/Ignorar)`; ≥1 P1 e ≥1 P3 | #86 |
| 08 | Classificação de manutenção | `/docs/fluxo_manutencao.md` | 12 tickets; `Corretiva`/`Adaptativa`/`Perfectiva`/`Preventiva`; `Ticket N → Tipo` | #87 |
| 09 | Pipeline de liberação segura | `/docs/fluxo_manutencao.md` | `1. Análise de Impacto` `2. Teste como Instrumento Cirúrgico` `3. Feature Toggle` `4. Estratégia de Release e Regressão` | #87 |
| 10 | Plano de iteração | `/docs/plano_iteracao.md` | `Objetivo da Iteração:` `Escopo (Backlog Selecionado):` `Entregáveis (Evidências):` `Risco Principal do Ciclo:` `Definição de Pronto (DoD):` | #88 |
| 11 | Quadro visual + WIP | `/docs/plano_iteracao.md` | ≥4 colunas; `WIP máximo: N` (N ≤ 6) | #88 |
| 12 | Matriz de riscos | `/docs/operacao.md` | colunas `Risco` `Probabilidade` `Impacto` `Estratégia` `Ação Planejada`; ≥5; `Mitigar`/`Transferir`/`Aceitar`/`Evitar` | #89 |
| 13 | Gatilhos de risco | `/docs/operacao.md` | coluna `Gatilho`; toda linha ≥20 chars | #89 |
| 14 | Métrica de fluxo (DORA) | `/docs/operacao.md` | 7 campos (`Nome da Métrica:` … `Ação se Violado:`); `Deploy`/`Lead Time`/`Throughput`/`DORA` | #89 |
| 15 | Métrica de qualidade | `/docs/operacao.md` | mesmos 7 campos; `Falha`/`Erro`/`Teste`/`Change Failure Rate`/`Cobertura` | #89 |
| 16 | SLO | `/docs/operacao.md` | `SLI (Indicador):` `Fórmula de Coleta:` `Fonte do Dado:` `Janela de Medição:` (Nº+`dias`/`horas`) `Alvo (SLO):` (%) | #90 |
| 17 | Error Budget Policy | `/docs/operacao.md` | `Error Budget Policy:`; `Nível 1`/`Nível 2`/`Nível 3`; Nível 3 = `congelamento`/`Feature Freeze`/`Zero novas funcionalidades` | #90 |
| 18 | Segurança no código (SSDF) | `/src/**.cs` | **sem** `Password=`/`Pwd=`/`User Id=`/`ConnectionString=` literais (✅ já OK) | #91 |
| 19 | Threat Model + Gates | `/docs/seguranca_ciclo.md` | `Ativos Protegidos:` `Vetor de Ataque Provável:` `Falha Arquitetural Potencial:` `Controle de Engenharia (Mitigação):`; `Gate 1` `Gate 2` `Gate 3` | #91 |
| 20 | Topologia de times + DoD final | `/docs/topologia_times.md` + `release_checklist_final.md` (raiz) | `Stream-aligned` `Platform` `Enabling` `Complicated-Subsystem`; 7 caixas `[x]` | #92 |

> ⚠️ **Case-sensitive.** Copie os literais exatamente (acentuação, dois-pontos, `**negrito:**`,
> nomes de arquivo e de pasta). Errar o literal = perder os 0,5 do item.

## Distribuição (Sprint 2 — 2 issues por integrante)

| Issue | Responsável | Itens cobertos |
|-------|-------------|----------------|
| #81 Endpoint de reserva | @coco-lucas | A1, A3, A4 |
| #82 Endpoint com JOIN | @Guilherme-Sequeira | A2 |
| #83 Testes AAA + nomenclatura | @Guilherme-Sequeira | 01, 02 |
| #84 analise_arquitetura.md | @murilothunder | 03, 04 |
| #85 ADR | @kaualandi | 05 |
| #86 registro_divida_tecnica.md | @Natan18s | 06, 07 |
| #87 fluxo_manutencao.md | @coco-lucas | 08, 09 |
| #88 plano_iteracao.md | @Natan18s | 10, 11 |
| #89 operacao.md (riscos+métricas) | @kaualandi | 12, 13, 14, 15 |
| #90 operacao.md (SLO+budget) | @lucasanes | 16, 17 |
| #91 segurança (SSDF + ciclo) | @murilothunder | 18, 19 |
| #92 topologia + checklist | @lucasanes | 20 |

> #89 e #90 escrevem no **mesmo arquivo** `/docs/operacao.md` — combinar para evitar conflito.

---

# Validação — "está tudo feito?"

### A. Código (rode com a API funcionando)
- [ ] `dotnet build TicketPrime.slnx` sem erros e `dotnet test tests` 100% verde.
- [ ] Existem ≥2 endpoints de negócio; ≥1 com `JOIN`; ≥1 com ≥3 validações → `400`.
- [ ] Nenhuma query com concatenação/interpolação — tudo `@Parametro` (conferir o diff).

### B. SDD — checagem por literais (greps de exemplo, rodar na raiz)
```bash
grep -rE "// Arrange|// Act|// Assert" tests | wc -l        # item 01: ≥3 de cada
grep -E "Trade-off:|Positivo:|Negativo:" docs/analise_arquitetura.md   # 03
grep -E "\*\*Problema:\*\*|\*\*Ação Recomendada:\*\*" docs/analise_arquitetura.md  # 04
ls docs/adrs/ && grep -E "## Contexto|## Decisão|## Consequências|Status:" docs/adrs/*.md  # 05
grep -E "Prioridade 1 \(Imediato\)|Prioridade 3 \(Aceitar/Ignorar\)" docs/registro_divida_tecnica.md  # 07
grep -E "1\. Análise de Impacto|2\. Teste como Instrumento Cirúrgico|3\. Feature Toggle" docs/fluxo_manutencao.md  # 09
grep -E "WIP máximo:" docs/plano_iteracao.md                # 11
grep -E "Mitigar|Transferir|Aceitar|Evitar" docs/operacao.md && grep -E "Gatilho" docs/operacao.md  # 12,13
grep -E "Error Budget Policy:|Feature Freeze|congelamento" docs/operacao.md  # 17
grep -rnE "Password=|Pwd=|User Id=|ConnectionString=" src --include="*.cs" || echo "SSDF ok"  # 18
grep -E "Ativos Protegidos:|Gate 1|Gate 2|Gate 3" docs/seguranca_ciclo.md   # 19
grep -E "Stream-aligned|Platform|Enabling|Complicated-Subsystem" docs/topologia_times.md  # 20
grep -E "\[x\]" release_checklist_final.md | wc -l          # 20: 7 caixas
```
- [ ] Cada comando acima retorna o(s) literal(is) esperado(s) do item correspondente.
- [ ] Os 20 itens marcados; todas as issues #81–#92 e sub-issues em **Done**.

### C. Entrega
- [ ] Branch(es) `feat/...` com commits focados, sem co-autoria do Claude.
- [ ] PR(s) abertos/oferecidos; merge a cargo do usuário.

> Tudo marcado = AV2 (código + SDD completo) entregue. Detalhe de design do domínio
> (regras do motor de reservas, que são livres) em `docs/av2-decisoes.md`.
