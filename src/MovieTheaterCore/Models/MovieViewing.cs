namespace MovieTheaterCore.Models
{
    public class MovieViewing
    {
        public long Id { get; private set; }
        public long MovieId { get; set; }
        public Movie Movie { get; set; }
        public long SalonId { get; set; }
        public Salon Salon { get; set; }
        public DateTime ViewingStart { get; set; }
        public Decimal TicketPrice { get; set; }
    }
}