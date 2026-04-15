namespace TicketPrime.Client.Services.Usuarios;

public interface IUsuarioService
{
    Task<IReadOnlyList<UsuarioListItem>> ListarAsync(CancellationToken cancellationToken = default);
    Task<UsuarioListItem?> ObterPorCpfAsync(string cpf, CancellationToken cancellationToken = default);
    Task<UsuarioResult> CriarAsync(SalvarUsuarioRequest request, CancellationToken cancellationToken = default);
    Task<UsuarioResult> AtualizarAsync(string cpf, SalvarUsuarioRequest request, CancellationToken cancellationToken = default);
    Task<UsuarioResult> DeletarAsync(string cpf, CancellationToken cancellationToken = default);
}
