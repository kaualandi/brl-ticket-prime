using System.ComponentModel.DataAnnotations;

namespace TicketPrime.Api.Features.Eventos;

public class CreateEventoRequest
{
    [Required]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    public string LocalEvento { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CapacidadeTotal { get; set; }

    public DateTime DataEvento { get; set; }

    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal PrecoPadrao { get; set; }
}
