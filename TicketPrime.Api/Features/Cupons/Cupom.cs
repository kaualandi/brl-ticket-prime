namespace TicketPrime.Api.Features.Cupons;

public class Cupom
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public decimal PorcentagemDesconto { get; set; }
    public decimal ValorMinimoRegra { get; set; }
}
