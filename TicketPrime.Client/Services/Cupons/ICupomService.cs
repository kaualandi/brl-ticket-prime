namespace TicketPrime.Client.Services.Cupons;

public interface ICupomService
{
    Task<CriarCupomResult> CriarAsync(CriarCupomRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CupomListItem>> ListarAsync(CancellationToken cancellationToken = default);
    Task<CupomListItem?> ObterPorCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<CriarCupomResult> AtualizarAsync(string codigo, AtualizarCupomRequest request, CancellationToken cancellationToken = default);
    Task<CriarCupomResult> DeletarAsync(string codigo, CancellationToken cancellationToken = default);
}
