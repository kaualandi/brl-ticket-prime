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

    public async Task<int> AddAsync(Usuario usuario)
    {
        const string sql = """
            INSERT INTO Usuarios (Nome, Email, Senha)
            VALUES (@Nome, @Email, @Senha);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        return await _db.ExecuteScalarAsync<int>(sql, usuario);
    }
}
