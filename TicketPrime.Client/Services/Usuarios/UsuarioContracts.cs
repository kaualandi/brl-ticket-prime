namespace TicketPrime.Client.Services.Usuarios;

public sealed class UsuarioListItem
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
}

public sealed class SalvarUsuarioRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
}

public sealed class UsuarioResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }

    public static UsuarioResult Ok() => new() { Success = true };

    public static UsuarioResult Fail(string errorMessage) => new()
    {
        Success = false,
        ErrorMessage = errorMessage
    };
}
