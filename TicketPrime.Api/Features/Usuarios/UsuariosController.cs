using Microsoft.AspNetCore.Mvc;
using TicketPrime.Api.Features.Usuarios;

namespace TicketPrime.Api.Features.Usuarios;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioRepository _repo;

    public UsuariosController(IUsuarioRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await _repo.GetAllAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var usuario = await _repo.GetByIdAsync(id);
        if (usuario is null)
            return NotFound();
        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CreateUsuarioRequest request)
    {
        if (request == null)
            return BadRequest("Dados inválidos");

        if (string.IsNullOrWhiteSpace(request.Nome))
            return BadRequest("Nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest("Email é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.Senha))
            return BadRequest("Senha é obrigatória.");

        if (string.IsNullOrWhiteSpace(request.Cpf))
            return BadRequest("CPF é obrigatório.");

        var usuario = new Usuario
        {
            Nome = request.Nome.Trim(),
            Email = request.Email.Trim(),
            Senha = request.Senha,
            Cpf = request.Cpf.Trim()
        };

        var id = await _repo.AddAsync(usuario);
        return Created($"/api/usuarios/{id}", null);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] CreateUsuarioRequest request)
    {
        if (request == null)
            return BadRequest("Dados inválidos");

        if (string.IsNullOrWhiteSpace(request.Nome))
            return BadRequest("Nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest("Email é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.Senha))
            return BadRequest("Senha é obrigatória.");

        if (string.IsNullOrWhiteSpace(request.Cpf))
            return BadRequest("CPF é obrigatório.");

        var usuario = new Usuario
        {
            Id = id,
            Nome = request.Nome.Trim(),
            Email = request.Email.Trim(),
            Senha = request.Senha,
            Cpf = request.Cpf.Trim()
        };

        var rows = await _repo.UpdateAsync(usuario);
        if (rows == 0)
            return NotFound();

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deletar(int id)
    {
        var rows = await _repo.DeleteAsync(id);
        if (rows == 0)
            return NotFound();

        return NoContent();
    }
}
