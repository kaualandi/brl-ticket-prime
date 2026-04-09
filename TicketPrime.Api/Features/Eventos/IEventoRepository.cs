namespace TicketPrime.Api.Features.Eventos;

public interface IEventoRepository
{
    Task<IEnumerable<Evento>> GetAllAsync();
    Task<Evento?> GetByIdAsync(int id);
    Task<int> AddAsync(Evento evento);
    Task<int> UpdateAsync(Evento evento);
    Task<int> DeleteAsync(int id);
}
