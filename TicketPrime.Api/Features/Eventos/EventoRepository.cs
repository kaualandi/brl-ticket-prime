using Dapper;
using System.Data;

namespace TicketPrime.Api.Features.Eventos;

public class EventoRepository : IEventoRepository
{
    private readonly IDbConnection _db;

    public EventoRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Evento>> GetAllAsync()
    {
        return await _db.QueryAsync<Evento>("SELECT * FROM Eventos");
    }

    public async Task<Evento?> GetByIdAsync(int id)
    {
        return await _db.QueryFirstOrDefaultAsync<Evento>(
            "SELECT * FROM Eventos WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> AddAsync(Evento evento)
    {
        const string sql = """
            INSERT INTO Eventos (Nome, Descricao, CapacidadeTotal, DataEvento, PrecoPadrao, LocalEvento)
            VALUES (@Nome, @Descricao, @CapacidadeTotal, @DataEvento, @PrecoPadrao, @LocalEvento);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        return await _db.ExecuteScalarAsync<int>(sql, evento);
    }

    public async Task<int> UpdateAsync(Evento evento)
    {
        const string sql = """
            UPDATE Eventos
            SET Nome = @Nome, Descricao = @Descricao, CapacidadeTotal = @CapacidadeTotal,
                DataEvento = @DataEvento, PrecoPadrao = @PrecoPadrao, LocalEvento = @LocalEvento
            WHERE Id = @Id
            """;

        return await _db.ExecuteAsync(sql, evento);
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await _db.ExecuteAsync(
            "DELETE FROM Eventos WHERE Id = @Id", new { Id = id });
    }
}
