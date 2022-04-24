namespace WebApp.Models
{
    public class MovieViewingViewModel
    {
        public long Id { get; set; }
        public MovieViewModel Movie { get; set; }
        public SalonViewModel Salon { get; set; }
        public DateTime ViewingStart { get; set; }
        public Decimal TicketPrice { get; set; }
        public int ReservedSeats { get; set; }
        public string imgLink { get; set; }
    }
}