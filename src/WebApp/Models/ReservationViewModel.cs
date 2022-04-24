namespace WebApp.Models
{
    public class ReservationViewModel
    {
        public MovieViewingViewModelSmall Viewing { get; set; }
        public string ReservationCode { get; set; }
        public int Seats { get; set; }
        public decimal TotalPrice { get; set; }
    }
}