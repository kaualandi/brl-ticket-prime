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
}
