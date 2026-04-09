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

    // GET /api/eventos/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var evento = await _repo.GetByIdAsync(id);
        if (evento is null)
            return NotFound();
        return Ok(evento);
    }

    // PUT /api/eventos/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateEventoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            return BadRequest("Informe o nome do evento.");

        if (string.IsNullOrWhiteSpace(request.Descricao))
            return BadRequest("Informe a descricao do evento.");

        if (string.IsNullOrWhiteSpace(request.LocalEvento))
            return BadRequest("Informe o local do evento.");

        if (request.CapacidadeTotal <= 0)
            return BadRequest("A capacidade total deve ser maior que zero.");

        if (request.DataEvento == default)
            return BadRequest("Informe a data do evento.");

        if (request.PrecoPadrao <= 0)
            return BadRequest("O preco padrao deve ser maior que zero.");

        var evento = new Evento
        {
            Id = id,
            Nome = request.Nome.Trim(),
            Descricao = request.Descricao.Trim(),
            LocalEvento = request.LocalEvento.Trim(),
            CapacidadeTotal = request.CapacidadeTotal,
            DataEvento = request.DataEvento,
            PrecoPadrao = request.PrecoPadrao
        };

        var rows = await _repo.UpdateAsync(evento);
        if (rows == 0)
            return NotFound();

        return Ok();
    }

    // DELETE /api/eventos/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var rows = await _repo.DeleteAsync(id);
        if (rows == 0)
            return NotFound();

        return NoContent();
    }
}
