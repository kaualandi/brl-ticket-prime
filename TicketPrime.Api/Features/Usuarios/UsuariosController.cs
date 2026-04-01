using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    [HttpPost]
    public IActionResult Criar([FromBody] CreateUsuarioRequest request)
    {
        if (request == null)
            return BadRequest("Dados inválidos");

        var usuario = new Usuario
        {
            Nome = request.Nome,
            Email = request.Email,
            Senha = request.Senha
        };

        return Created("", usuario);
    }
}
