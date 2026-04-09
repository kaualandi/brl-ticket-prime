namespace TicketPrime.Api.Features.Usuarios;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<Usuario?> GetByIdAsync(int id);
    Task<int> AddAsync(Usuario usuario);
    Task<int> UpdateAsync(Usuario usuario);
    Task<int> DeleteAsync(int id);
}
