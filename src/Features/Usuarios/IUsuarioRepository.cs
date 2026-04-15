namespace TicketPrime.Api.Features.Usuarios;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<Usuario?> GetByCpfAsync(string cpf);
    Task<bool> ExistsByCpfAsync(string cpf);
    Task AddAsync(Usuario usuario);
    Task<int> UpdateAsync(Usuario usuario);
    Task<int> DeleteAsync(string cpf);
}
