namespace TicketPrime.Client.Services.Eventos;

public sealed class EventoListItem
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int CapacidadeTotal { get; set; }
    public DateTime DataEvento { get; set; }
    public decimal PrecoPadrao { get; set; }
    public string LocalEvento { get; set; } = string.Empty;
}

public sealed class SalvarEventoRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string LocalEvento { get; set; } = string.Empty;
    public int CapacidadeTotal { get; set; }
    public DateTime DataEvento { get; set; }
    public decimal PrecoPadrao { get; set; }
}

public sealed class EventoResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }

    public static EventoResult Ok() => new() { Success = true };

    public static EventoResult Fail(string errorMessage) => new()
    {
        Success = false,
        ErrorMessage = errorMessage
    };
}
