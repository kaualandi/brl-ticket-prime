namespace TicketPrime.Client.Services.Cupons;

public interface ICupomService
{
    Task<CriarCupomResult> CriarAsync(CriarCupomRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CupomListItem>> ListarAsync(CancellationToken cancellationToken = default);
    Task<CupomListItem?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CriarCupomResult> AtualizarAsync(int id, AtualizarCupomRequest request, CancellationToken cancellationToken = default);
}
