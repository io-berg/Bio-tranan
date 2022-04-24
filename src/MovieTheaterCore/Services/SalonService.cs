
using MovieTheaterCore.Interfaces;
using MovieTheaterCore.Models;

namespace MovieTheaterCore.Services
{
    public class SalonService
    {
        private IRepository<Salon> _salonRepository;

        public SalonService(IRepository<Salon> repository)
        {
            _salonRepository = repository;
        }

        public async Task<List<Salon>> GetAllAsync()
        {
            var list = await _salonRepository.ListAsync();

            return (List<Salon>)list;
        }

        public async Task<Salon> GetByIdAsync(long id)
        {
            Salon salon = await _salonRepository.GetByIdAsync(id);

            return salon;
        }

        public async Task AddSalonAsync(Salon salon)
        {
            await _salonRepository.InsertAsync(salon);
        }

        public async Task UpdateSalonAsync(long Id, Salon salon)
        {
            var oldSalon = await _salonRepository.GetByIdAsync(Id);
            oldSalon.Name = salon.Name;
            oldSalon.Seats = salon.Seats;

            await _salonRepository.UpdateAsync(oldSalon);
        }

        public async Task DeleteSalonAsync(Salon salon)
        {
            await _salonRepository.DeleteAsync(salon);
        }
    }
}