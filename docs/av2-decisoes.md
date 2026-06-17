# AV2 — Notas de design do domínio (motor de reservas)

> ⚠️ **Não é um artefato avaliado.** O enunciado oficial da AV2 diz que o **domínio é livre**
> ("um sistema de agenda, controle de tarefas, marketplace… todos têm o mesmo peso"). As
> regras abaixo são **escolha da equipe** e existem apenas para satisfazer os **requisitos de
> código** A1–A4 (ver `docs/av2-tarefas.md`). Mensagens, limites e códigos podem mudar à
> vontade — não há literal obrigatório aqui.

## Endpoints (requisitos de código)

- **A1/A3 — `POST /api/reservas`** (issue #81): endpoint com regra de negócio + **≥3
  validações** antes do INSERT. Cada falha → `400 Bad Request` com mensagem específica.
- **A2 — `GET` de reservas com JOIN** (issue #82): `INNER`/`LEFT JOIN` entre `Reservas`,
  `Eventos` e `Usuarios`, retornando dados das 2+ tabelas (ex.: `GET /api/usuarios/{cpf}/reservas`).
- **A4 —** todas as queries com Dapper **parametrizado** (`@Parametro`).

## Validações de negócio propostas (≥3, para o item A3)

Modelo: **1 reserva = 1 ingresso**. Sugestão de regras (ajustável):

1. Evento existe → senão `400`.
2. Evento não esgotado: `COUNT(Reservas do evento) < CapacidadeTotal` → senão `400`.
3. Limite por usuário (anti-cambismo): `COUNT(Reservas do CPF no evento) < N` → senão `400`.
4. Cupom (se enviado) existe e atinge `ValorMinimoRegra`; aplica
   `ValorFinalPago = Round(PrecoPadrao * (1 - PorcentagemDesconto/100), 2)`.

> As mensagens de erro são livres — escolher textos claros em PT-BR. O importante para a nota
> é **haver ≥3 verificações** retornando `400` e as queries serem parametrizadas.

## Banco

- A tabela `Reservas` já existe em `db/ticketprime.sql`. Índice de apoio sugerido:
  `IX_Reservas_Evento(EventoId, UsuarioCpf)`. Ampliar `seed.sql` com usuários e cupons.
