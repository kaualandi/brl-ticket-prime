namespace TicketPrime.Api.Features.Usuarios;

/// <summary>
/// Representa um usuário completo persistido na base.
/// </summary>
public class Usuario
{
    /// <summary>
    /// CPF do usuário.
    /// </summary>
    public string Cpf { get; set; } = string.Empty;

    /// <summary>
    /// Nome completo do usuário.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// E-mail do usuário.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usuário.
    /// </summary>
    public string Senha { get; set; } = string.Empty;
}
