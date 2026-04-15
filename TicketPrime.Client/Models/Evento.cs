namespace TicketPrime.Client.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int CapacidadeTotal { get; set; }
        public DateTime DataEvento { get; set; }
        public decimal PrecoPadrao { get; set; }
        public string LocalEvento { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
