using MovieTheaterCore.Services;

namespace WebApp.Workers
{
    public class ReservationCleaningWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReservationCleaningWorker> _logger;

        public ReservationCleaningWorker(IServiceProvider serviceProvider, ILogger<ReservationCleaningWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                using (var scope = _serviceProvider.CreateScope())
                {
                    var reservationService = scope.ServiceProvider.GetRequiredService<ReservationService>();
                    int cleaned = await reservationService.CleanExpiredReservations();

                    _logger.LogInformation("Cleaned {count} expired reservations", cleaned);
                }
                await Task.Delay(600000, stoppingToken);
            }

            _logger.LogInformation("ReservationCleaningWorker is stopping.");
        }
    }
}

