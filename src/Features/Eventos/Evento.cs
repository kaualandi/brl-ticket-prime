namespace TicketPrime.Api.Features.Eventos;

/// <summary>
/// Representa um evento cadastrado na plataforma.
/// </summary>
public class Evento
{
    /// <summary>
    /// Identificador único do evento.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome de exibição do evento.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição completa do evento.
    /// </summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Quantidade máxima de pessoas permitidas no evento.
    /// </summary>
    public int CapacidadeTotal { get; set; }

    /// <summary>
    /// Data e hora de realização do evento.
    /// </summary>
    public DateTime DataEvento { get; set; }

    /// <summary>
    /// Preço base do ingresso.
    /// </summary>
    public decimal PrecoPadrao { get; set; }

    /// <summary>
    /// Local onde o evento ocorrerá.
    /// </summary>
    public string LocalEvento { get; set; } = string.Empty;

    /// <summary>
    /// URL opcional de imagem de capa do evento.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;
}
