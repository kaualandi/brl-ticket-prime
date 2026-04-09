namespace TicketPrime.Client.Services.Eventos;

public interface IEventoService
{
    Task<IReadOnlyList<EventoListItem>> ListarAsync(CancellationToken cancellationToken = default);
    Task<EventoListItem?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<EventoResult> CriarAsync(SalvarEventoRequest request, CancellationToken cancellationToken = default);
    Task<EventoResult> AtualizarAsync(int id, SalvarEventoRequest request, CancellationToken cancellationToken = default);
    Task<EventoResult> DeletarAsync(int id, CancellationToken cancellationToken = default);
}
