using MovieTheaterCore.Models;
using WebApp.Models;

public class DetailsViewModel
{
    public MovieViewingViewModel Viewing { get; set; }
    public ReservationCreationModel ReservationForm { get; set; }
    public IEnumerable<Review> Reviews { get; set; }

    public int remainingSeats
    {
        get
        {
            return Viewing.Salon.Seats - Viewing.ReservedSeats;
        }
    }
}