namespace TicketPrime.Api.Features.Usuarios;

/// <summary>
/// Representa um usuário na listagem administrativa.
/// </summary>
public sealed class UsuarioListItemResponse
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
}

/// <summary>
/// Representa os dados completos de um usuário.
/// </summary>
public sealed class UsuarioDetailResponse
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
    /// Senha atualmente cadastrada para o usuário.
    /// </summary>
    public string Senha { get; set; } = string.Empty;
}
