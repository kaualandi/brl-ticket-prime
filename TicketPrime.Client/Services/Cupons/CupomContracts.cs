namespace TicketPrime.Client.Services.Cupons;

public sealed class CriarCupomRequest
{
    public string Codigo { get; set; } = string.Empty;
    public decimal PorcentagemDesconto { get; set; }
    public decimal ValorMinimoRegra { get; set; }
}

public sealed class AtualizarCupomRequest
{
    public string Codigo { get; set; } = string.Empty;
    public decimal PorcentagemDesconto { get; set; }
    public decimal ValorMinimoRegra { get; set; }
}

public sealed class CupomListItem
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public decimal PorcentagemDesconto { get; set; }
    public decimal ValorMinimoRegra { get; set; }
}

public sealed class CriarCupomResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }

    public static CriarCupomResult Ok() => new() { Success = true };

    public static CriarCupomResult Fail(string errorMessage) => new()
    {
        Success = false,
        ErrorMessage = errorMessage
    };
}
