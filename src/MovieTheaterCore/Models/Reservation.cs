namespace MovieTheaterCore.Models
{
    public class Reservation
    {
        public long Id { get; set; }
        public long MovieViewingId { get; set; }
        public MovieViewing MovieViewing { get; set; }
        public string ReservationCode { get; set; }
        public int Seats { get; set; }
    }
}