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

    // POST /api/eventos
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
        {
            return BadRequest("Informe o nome do evento.");
        }

        if (string.IsNullOrWhiteSpace(request.Descricao))
        {
            return BadRequest("Informe a descricao do evento.");
        }

        if (string.IsNullOrWhiteSpace(request.LocalEvento))
        {
            return BadRequest("Informe o local do evento.");
        }

        if (request.CapacidadeTotal <= 0)
        {
            return BadRequest("A capacidade total deve ser maior que zero.");
        }

        if (request.DataEvento == default)
        {
            return BadRequest("Informe a data do evento.");
        }

        if (request.PrecoPadrao <= 0)
        {
            return BadRequest("O preco padrao deve ser maior que zero.");
        }

        var evento = new Evento
        {
            Nome = request.Nome.Trim(),
            Descricao = request.Descricao.Trim(),
            LocalEvento = request.LocalEvento.Trim(),
            CapacidadeTotal = request.CapacidadeTotal,
            DataEvento = request.DataEvento,
            PrecoPadrao = request.PrecoPadrao
        };

        evento.Id = await _repo.AddAsync(evento);

        return Created($"/api/eventos/{evento.Id}", evento);
    }
}
