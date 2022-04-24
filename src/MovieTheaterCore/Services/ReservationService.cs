using MovieTheaterCore.Interfaces;
using MovieTheaterCore.Models;

namespace MovieTheaterCore.Services
{
    public class ReservationService
    {
        IRepository<Reservation> _reservationRepository;
        MovieViewingService _movieViewingService;

        public ReservationService(IRepository<Reservation> repository, MovieViewingService movieViewingService)
        {
            _reservationRepository = repository;
            _movieViewingService = movieViewingService;
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            var list = await _reservationRepository.ListAsync();

            return (List<Reservation>)list;
        }

        public async Task<List<Reservation>> GetByDate(DateTime date)
        {
            var list = await _reservationRepository.ListAsync(r => r.MovieViewing.ViewingStart.Date == date.Date);

            return (List<Reservation>)list;
        }

        public async Task<int> CleanExpiredReservations()
        {
            var toBeRemoved = await _reservationRepository.ListAsync(r => r.MovieViewing.ViewingStart < DateTime.Now.AddDays(-14));
            foreach (var reservation in toBeRemoved)
            {
                await _reservationRepository.DeleteAsync(reservation);
            }

            return toBeRemoved.Count();
        }

        public async Task<Reservation> GetByIdAsync(long id)
        {
            Reservation reservation = await _reservationRepository.GetByIdAsync(id);

            return reservation;
        }

        public async Task<Reservation> GetByCode(string reservationCode)
        {
            var reservation = _reservationRepository.ListAsync(r => r.ReservationCode == reservationCode).Result.FirstOrDefault();
            if (reservation == null) return null;
            reservation.MovieViewing = await _movieViewingService.GetByIdAsync(reservation.MovieViewingId);
            return reservation;
        }

        public async Task<Reservation> AddReservationAsync(ReservationCreationModel reservationModel)
        {
            if (reservationModel.Seats == 0) throw new Exception("Invalid number of seats");

            Reservation reservation = await CreateReservation(reservationModel);
            if (!await IsValidReservation(reservation)) throw new Exception("Not enough free seats");

            await _reservationRepository.InsertAsync(reservation);
            return reservation;
        }

        public async Task<IEnumerable<Reservation>> GetByViewingId(long viewingId)
        {
            return await _reservationRepository.ListAsync(r => r.MovieViewing.Id == viewingId);
        }

        public async Task DeleteReservationAsync(Reservation reservation)
        {
            await _reservationRepository.DeleteAsync(reservation);
        }

        private async Task<Reservation> CreateReservation(ReservationCreationModel reservationModel)
        {
            var movieViewing = await _movieViewingService.GetByIdAsync(reservationModel.MovieViewingId);
            var reservation = new Reservation
            {
                MovieViewing = movieViewing,
                Seats = reservationModel.Seats,
                ReservationCode = GenerateCode()
            };

            return reservation;
        }

        private string GenerateCode()
        {
            string base64Guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return base64Guid.Replace("=", "").Replace("+", "").Replace("/", "");
        }

        private async Task<Boolean> IsValidReservation(Reservation reservation)
        {
            var salonSeats = reservation.MovieViewing.Salon.Seats;
            var viewingReservations = await _reservationRepository.ListAsync(r => r.MovieViewing.Id == reservation.MovieViewing.Id);
            var reservedSeats = viewingReservations.Sum(r => r.Seats);

            return salonSeats >= reservedSeats + reservation.Seats;
        }
    }
}