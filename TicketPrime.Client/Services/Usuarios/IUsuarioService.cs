namespace TicketPrime.Client.Services.Usuarios;

public interface IUsuarioService
{
    Task<IReadOnlyList<UsuarioListItem>> ListarAsync(CancellationToken cancellationToken = default);
    Task<UsuarioListItem?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UsuarioResult> CriarAsync(SalvarUsuarioRequest request, CancellationToken cancellationToken = default);
    Task<UsuarioResult> AtualizarAsync(int id, SalvarUsuarioRequest request, CancellationToken cancellationToken = default);
    Task<UsuarioResult> DeletarAsync(int id, CancellationToken cancellationToken = default);
}
