namespace TicketPrime.Client.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime Data { get; set; }
        public string Local { get; set; }
        public string Descricao { get; set; }
    }
}
