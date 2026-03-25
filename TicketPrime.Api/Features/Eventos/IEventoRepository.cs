namespace TicketPrime.Api.Features.Eventos;

public interface IEventoRepository
{
    Task<IEnumerable<Evento>> GetAllAsync();
    Task<int> AddAsync(Evento evento);
}
