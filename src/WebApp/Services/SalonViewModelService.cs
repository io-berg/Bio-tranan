using MovieTheaterCore.Interfaces;
using MovieTheaterCore.Models;
using WebApp.Models;

namespace WebApp.Services
{
    public class SalonViewModelService
    {
        IRepository<Salon> _repository;

        public SalonViewModelService(IRepository<Salon> repository)
        {
            _repository = repository;
        }

        public async Task<SalonViewModel> GetModelById(long id)
        {
            var salon = await _repository.GetByIdAsync((int)id);

            var model = new SalonViewModel
            {
                Name = salon.Name,
                Seats = salon.Seats
            };

            return model;
        }
    }
}