using System.ComponentModel.DataAnnotations;

namespace TicketPrime.Api.Features.Eventos;

/// <summary>
/// Dados necessários para criação ou atualização de um evento.
/// </summary>
public class CreateEventoRequest
{
    /// <summary>
    /// Nome do evento.
    /// </summary>
    [Required]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição detalhada do evento.
    /// </summary>
    [Required]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Local em que o evento será realizado.
    /// </summary>
    [Required]
    public string LocalEvento { get; set; } = string.Empty;

    /// <summary>
    /// Capacidade máxima de participantes.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int CapacidadeTotal { get; set; }

    /// <summary>
    /// Data e hora do evento.
    /// </summary>
    public DateTime DataEvento { get; set; }

    /// <summary>
    /// Preço padrão do ingresso.
    /// </summary>
    [Range(0.01, 999999999, ErrorMessage = "O preco padrao deve ser maior que zero.")]
    public decimal PrecoPadrao { get; set; }

    /// <summary>
    /// URL opcional de imagem de capa.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;
}
