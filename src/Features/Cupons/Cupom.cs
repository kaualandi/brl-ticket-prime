namespace TicketPrime.Api.Features.Cupons;

/// <summary>
/// Representa um cupom de desconto.
/// </summary>
public class Cupom
{
    /// <summary>
    /// Código textual do cupom.
    /// </summary>
    public string Codigo { get; set; } = string.Empty;

    /// <summary>
    /// Percentual de desconto aplicado pelo cupom.
    /// </summary>
    public decimal PorcentagemDesconto { get; set; }

    /// <summary>
    /// Valor mínimo da compra para aplicação da regra.
    /// </summary>
    public decimal ValorMinimoRegra { get; set; }
}
