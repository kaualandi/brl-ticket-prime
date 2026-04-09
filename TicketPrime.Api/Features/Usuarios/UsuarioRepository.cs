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
        return await _db.QueryAsync<Usuario>("SELECT * FROM Usuarios");
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _db.QueryFirstOrDefaultAsync<Usuario>(
            "SELECT * FROM Usuarios WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> AddAsync(Usuario usuario)
    {
        const string sql = """
            INSERT INTO Usuarios (Nome, Email, Senha, Cpf)
            VALUES (@Nome, @Email, @Senha, @Cpf);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        return await _db.ExecuteScalarAsync<int>(sql, usuario);
    }

    public async Task<int> UpdateAsync(Usuario usuario)
    {
        const string sql = """
            UPDATE Usuarios
            SET Nome = @Nome, Email = @Email, Senha = @Senha, Cpf = @Cpf
            WHERE Id = @Id
            """;

        return await _db.ExecuteAsync(sql, usuario);
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await _db.ExecuteAsync(
            "DELETE FROM Usuarios WHERE Id = @Id", new { Id = id });
    }
}
