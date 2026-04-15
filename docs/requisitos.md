# Requisitos — AV1 TicketPrime

## Épico Principal

**EP-01 — Fundação da API: Cadastros e Infraestrutura Segura**

Construir a base do sistema TicketPrime com endpoints de cadastro funcionais, banco de dados estruturado, conexão segura via Dapper e infraestrutura de testes validada — provando que a API existe, persiste dados corretamente e rejeita entradas inválidas antes de qualquer lógica de venda ser implementada.

---

## Histórias de Usuário

Como operador do sistema, Quero cadastrar um novo evento, Para que ele fique disponível para venda de ingressos.

Como comprador, Quero listar os eventos disponíveis, Para que eu possa escolher qual ingresso quero comprar.

Como operador do sistema, Quero cadastrar um cupom de desconto, Para que ele possa ser aplicado em compras futuras.

Como comprador, Quero me cadastrar no sistema com meu CPF, Para que eu possa realizar reservas de ingressos.

Como desenvolvedor, Quero que a API rejeite cadastros com dados inválidos ou duplicados, Para que o banco de dados não acumule registros inconsistentes.

Como desenvolvedor, Quero que todas as consultas ao banco usem parâmetros Dapper com `@`, Para que o sistema seja imune a ataques de SQL Injection.

Como engenheiro de qualidade, Quero uma suíte de testes automatizados com Assert em cada caso, Para que cada regra de negócio tenha uma prova verificável de que funciona.

Como auditor do contrato, Quero um README.md com os comandos exatos para executar o projeto, Para que qualquer pessoa consiga rodar o sistema sem depender de conhecimento prévio da equipe.

---

## Critérios de Aceitação

### Cadastro de Usuário

Dado que um operador envia POST /api/usuarios com Cpf, Nome e Email válidos
Quando o CPF ainda não existe na tabela Usuarios
Então o sistema insere o registro no banco e retorna 201 Created.

Dado que um operador envia POST /api/usuarios com um Cpf já existente na tabela Usuarios
Quando a API consultar o banco com WHERE Cpf = @Cpf
Então o sistema retorna 400 Bad Request com a mensagem "CPF já cadastrado." sem realizar INSERT.

Dado que um operador envia POST /api/usuarios com o campo Cpf ausente ou vazio
Quando a API receber o payload
Então o sistema retorna 400 Bad Request com a mensagem "CPF é obrigatório." sem realizar nenhuma consulta ao banco.

### Cadastro de Evento

Dado que um operador envia POST /api/eventos com Nome, CapacidadeTotal, DataEvento e PrecoPadrao válidos
Quando todos os campos estiverem presentes e com valores aceitáveis
Então o sistema insere o registro na tabela Eventos e retorna 201 Created com o Id gerado automaticamente.

Dado que um operador envia POST /api/eventos com CapacidadeTotal igual a zero ou negativo
Quando a API validar o payload
Então o sistema retorna 400 Bad Request com a mensagem "CapacidadeTotal deve ser maior que zero." sem realizar INSERT.

Dado que um operador envia POST /api/eventos com PrecoPadrao igual a zero ou negativo
Quando a API validar o payload
Então o sistema retorna 400 Bad Request com a mensagem "PrecoPadrao deve ser maior que zero." sem realizar INSERT.

### Cadastro de Cupom

Dado que um operador envia POST /api/cupons com Codigo, PorcentagemDesconto e ValorMinimoRegra válidos
Quando o Codigo ainda não existir na tabela Cupons
Então o sistema insere o registro no banco e retorna 201 Created.

Dado que um operador envia POST /api/cupons com um Codigo já existente na tabela Cupons
Quando a API consultar o banco com WHERE Codigo = @Codigo
Então o sistema retorna 400 Bad Request com a mensagem "Código de cupom já cadastrado." sem realizar INSERT.

Dado que um operador envia POST /api/cupons com PorcentagemDesconto fora do intervalo de 1 a 100
Quando a API validar o payload
Então o sistema retorna 400 Bad Request com a mensagem "PorcentagemDesconto deve ser um valor entre 1 e 100." sem realizar INSERT.
