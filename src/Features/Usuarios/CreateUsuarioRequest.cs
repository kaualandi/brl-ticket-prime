namespace TicketPrime.Api.Features.Usuarios;

/// <summary>
/// Dados necessários para criação ou atualização de um usuário.
/// </summary>
public class CreateUsuarioRequest
{
    /// <summary>
    /// Nome completo do usuário.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Endereço de e-mail do usuário.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha do usuário.
    /// </summary>
    public string Senha { get; set; } = string.Empty;

    /// <summary>
    /// CPF do usuário.
    /// </summary>
    public string Cpf { get; set; } = string.Empty;
}
