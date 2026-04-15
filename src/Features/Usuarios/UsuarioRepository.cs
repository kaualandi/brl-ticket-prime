using Dapper;
using System.Data;

namespace TicketPrime.Api.Features.Usuarios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IDbConnection _db;

    public UsuarioRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _db.QueryAsync<Usuario>(
            "SELECT Cpf, Nome, Email, Senha FROM Usuarios");
    }

    public async Task<Usuario?> GetByCpfAsync(string cpf)
    {
        return await _db.QueryFirstOrDefaultAsync<Usuario>(
            "SELECT Cpf, Nome, Email, Senha FROM Usuarios WHERE Cpf = @Cpf",
            new { Cpf = cpf });
    }

    public async Task<bool> ExistsByCpfAsync(string cpf)
    {
        var count = await _db.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM Usuarios WHERE Cpf = @Cpf",
            new { Cpf = cpf });
        return count > 0;
    }

    public async Task AddAsync(Usuario usuario)
    {
        await _db.ExecuteAsync(
            "INSERT INTO Usuarios (Cpf, Nome, Email, Senha) VALUES (@Cpf, @Nome, @Email, @Senha)",
            usuario);
    }

    public async Task<int> UpdateAsync(Usuario usuario)
    {
        return await _db.ExecuteAsync(
            "UPDATE Usuarios SET Nome = @Nome, Email = @Email, Senha = @Senha WHERE Cpf = @Cpf",
            usuario);
    }

    public async Task<int> DeleteAsync(string cpf)
    {
        return await _db.ExecuteAsync(
            "DELETE FROM Usuarios WHERE Cpf = @Cpf",
            new { Cpf = cpf });
    }
}
