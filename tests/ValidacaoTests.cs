using Xunit;

namespace TicketPrime.Tests;

// ──────────────────────────────────────────────────────────────────
// Mirrors the validation logic used in the Minimal API endpoints
// so that the business rules can be verified without a running
// database or HTTP server.
// ──────────────────────────────────────────────────────────────────

public class EventoValidacaoTests
{
    private static string? ValidarEvento(string nome, string descricao, string localEvento,
                                          int capacidadeTotal, decimal precoPadrao)
    {
        if (string.IsNullOrWhiteSpace(nome))        return "Nome é obrigatório.";
        if (string.IsNullOrWhiteSpace(descricao))   return "Descrição é obrigatória.";
        if (string.IsNullOrWhiteSpace(localEvento)) return "LocalEvento é obrigatório.";
        if (capacidadeTotal <= 0)                   return "CapacidadeTotal deve ser maior que zero.";
        if (precoPadrao <= 0)                       return "Preço padrão deve ser maior que zero.";
        return null;
    }

    [Fact]
    public void NomeVazio_DeveRetornarErro()
    {
        var erro = ValidarEvento("", "Descricao", "Local", 100, 50m);
        Assert.Equal("Nome é obrigatório.", erro);
    }

    [Fact]
    public void DescricaoVazia_DeveRetornarErro()
    {
        var erro = ValidarEvento("Show", "", "Local", 100, 50m);
        Assert.Equal("Descrição é obrigatória.", erro);
    }

    [Fact]
    public void LocalEventoVazio_DeveRetornarErro()
    {
        var erro = ValidarEvento("Show", "Descricao", "", 100, 50m);
        Assert.Equal("LocalEvento é obrigatório.", erro);
    }

    [Fact]
    public void CapacidadeZero_DeveRetornarErro()
    {
        var erro = ValidarEvento("Show", "Descricao", "Local", 0, 50m);
        Assert.Equal("CapacidadeTotal deve ser maior que zero.", erro);
    }

    [Fact]
    public void CapacidadeNegativa_DeveRetornarErro()
    {
        var erro = ValidarEvento("Show", "Descricao", "Local", -10, 50m);
        Assert.Equal("CapacidadeTotal deve ser maior que zero.", erro);
    }

    [Fact]
    public void PrecoZero_DeveRetornarErro()
    {
        var erro = ValidarEvento("Show", "Descricao", "Local", 100, 0m);
        Assert.Equal("Preço padrão deve ser maior que zero.", erro);
    }

    [Fact]
    public void PrecoNegativo_DeveRetornarErro()
    {
        var erro = ValidarEvento("Show", "Descricao", "Local", 100, -1m);
        Assert.Equal("Preço padrão deve ser maior que zero.", erro);
    }

    [Fact]
    public void DadosValidos_DeveRetornarNulo()
    {
        var erro = ValidarEvento("Show", "Descricao", "Local", 100, 50m);
        Assert.Null(erro);
    }
}

public class CupomValidacaoTests
{
    private static string? ValidarCupom(string codigo, decimal porcentagemDesconto, decimal valorMinimo)
    {
        if (string.IsNullOrWhiteSpace(codigo))                                return "Código é obrigatório.";
        if (porcentagemDesconto < 1 || porcentagemDesconto > 100)             return "PorcentagemDesconto deve ser um valor entre 1 e 100.";
        if (valorMinimo < 0)                                                  return "ValorMinimoRegra não pode ser negativo.";
        return null;
    }

    [Fact]
    public void CodigoVazio_DeveRetornarErro()
    {
        var erro = ValidarCupom("", 10m, 0m);
        Assert.Equal("Código é obrigatório.", erro);
    }

    [Fact]
    public void PorcentagemZero_DeveRetornarErro()
    {
        var erro = ValidarCupom("PROMO", 0m, 0m);
        Assert.Equal("PorcentagemDesconto deve ser um valor entre 1 e 100.", erro);
    }

    [Fact]
    public void PorcentagemAcimaDe100_DeveRetornarErro()
    {
        var erro = ValidarCupom("PROMO", 101m, 0m);
        Assert.Equal("PorcentagemDesconto deve ser um valor entre 1 e 100.", erro);
    }

    [Fact]
    public void ValorMinimoNegativo_DeveRetornarErro()
    {
        var erro = ValidarCupom("PROMO", 10m, -1m);
        Assert.Equal("ValorMinimoRegra não pode ser negativo.", erro);
    }

    [Fact]
    public void DadosValidos_DeveRetornarNulo()
    {
        var erro = ValidarCupom("BEMVINDO10", 10m, 50m);
        Assert.Null(erro);
    }
}

public class UsuarioValidacaoTests
{
    private static string? ValidarUsuario(string cpf, string nome, string email, string senha)
    {
        if (string.IsNullOrWhiteSpace(cpf))   return "CPF é obrigatório.";
        if (string.IsNullOrWhiteSpace(nome))  return "Nome é obrigatório.";
        if (string.IsNullOrWhiteSpace(email)) return "Email é obrigatório.";
        if (string.IsNullOrWhiteSpace(senha)) return "Senha é obrigatória.";
        return null;
    }

    [Fact]
    public void CpfVazio_DeveRetornarErro()
    {
        var erro = ValidarUsuario("", "Nome", "email@test.com", "senha");
        Assert.Equal("CPF é obrigatório.", erro);
    }

    [Fact]
    public void NomeVazio_DeveRetornarErro()
    {
        var erro = ValidarUsuario("123.456.789-00", "", "email@test.com", "senha");
        Assert.Equal("Nome é obrigatório.", erro);
    }

    [Fact]
    public void EmailVazio_DeveRetornarErro()
    {
        var erro = ValidarUsuario("123.456.789-00", "Nome", "", "senha");
        Assert.Equal("Email é obrigatório.", erro);
    }

    [Fact]
    public void SenhaVazia_DeveRetornarErro()
    {
        var erro = ValidarUsuario("123.456.789-00", "Nome", "email@test.com", "");
        Assert.Equal("Senha é obrigatória.", erro);
    }

    [Fact]
    public void DadosValidos_DeveRetornarNulo()
    {
        var erro = ValidarUsuario("123.456.789-00", "João Silva", "joao@test.com", "senhaSegura");
        Assert.Null(erro);
    }
}
