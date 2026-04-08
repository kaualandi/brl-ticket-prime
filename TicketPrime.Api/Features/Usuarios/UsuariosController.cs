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

        var usuario = new Usuario
        {
            Nome = request.Nome.Trim(),
            Email = request.Email.Trim(),
            Senha = request.Senha
        };

        var id = await _repo.AddAsync(usuario);
        return Created($"/api/usuarios/{id}", null);
    }
}
