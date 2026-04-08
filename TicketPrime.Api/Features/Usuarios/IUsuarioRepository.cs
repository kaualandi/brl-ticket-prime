namespace TicketPrime.Api.Features.Usuarios;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<int> AddAsync(Usuario usuario);
}
