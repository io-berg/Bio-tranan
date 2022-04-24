namespace WebApp.Models
{
    public class MovieViewingViewModelSmall
    {
        public long Id { get; set; }
        public MovieViewModelSmall Movie { get; set; }
        public SalonViewModel Salon { get; set; }
        public DateTime ViewingStart { get; set; }
        public Decimal TicketPrice { get; set; }
        public int ReservedSeats { get; set; }
    }
}