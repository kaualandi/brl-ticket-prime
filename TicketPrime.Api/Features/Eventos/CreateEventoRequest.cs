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

    [Range(0.01, 999999999, ErrorMessage = "O preco padrao deve ser maior que zero.")]
    public decimal PrecoPadrao { get; set; }
}
