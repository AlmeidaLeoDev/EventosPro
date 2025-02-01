using EventosPro.Services.Interfaces;
using Quartz;

namespace EventosPro.Services.Implementations
{
    public class DatabaseCleanupJob : IJob 
    {
        private readonly IDatabaseCleanupService _cleanupService;
        private readonly ILogger<DatabaseCleanupJob> _logger;

        public DatabaseCleanupJob(
            IDatabaseCleanupService cleanupService,
            ILogger<DatabaseCleanupJob> logger)
        {
            _cleanupService = cleanupService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation("Starting database cleanup");

                await _cleanupService.CleanupUnconfirmedUsersAsync();

                await _cleanupService.CleanupExpiredTokensAsync();

                _logger.LogInformation("Database cleanup completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during cleaning job execution");
                throw;
            }
        }

    }
}
