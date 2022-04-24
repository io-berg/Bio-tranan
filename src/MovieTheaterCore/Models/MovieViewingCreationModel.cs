namespace MovieTheaterCore.Models
{
    public class MovieViewingCreationModel
    {
        public long MovieId { get; set; }
        public long SalonId { get; set; }
        public DateTime ViewingStart { get; set; }
        public Decimal TicketPrice { get; set; }
    }
}