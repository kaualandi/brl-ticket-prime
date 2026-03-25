using Microsoft.AspNetCore.Mvc;

namespace TicketPrime.Api.Features.Eventos;

[ApiController]
[Route("api/eventos")]
public class EventosController : ControllerBase
{
    private readonly IEventoRepository _repo;

    public EventosController(IEventoRepository repo)
    {
        _repo = repo;
    }

    // GET /api/eventos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var eventos = await _repo.GetAllAsync();
        return Ok(eventos);
    }
}
