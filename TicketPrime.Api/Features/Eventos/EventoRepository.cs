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

    public async Task<int> AddAsync(Evento evento)
    {
        const string sql = """
            INSERT INTO Eventos (Nome, Descricao, CapacidadeTotal, DataEvento, PrecoPadrao, LocalEvento)
            VALUES (@Nome, @Descricao, @CapacidadeTotal, @DataEvento, @PrecoPadrao, @LocalEvento);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        return await _db.ExecuteScalarAsync<int>(sql, evento);
    }
}
