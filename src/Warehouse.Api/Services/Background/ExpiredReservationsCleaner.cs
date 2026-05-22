using Warehouse.Infrastructure.Data;

namespace Warehouse.Api.Services.Background;

public class ExpiredReservationsCleaner(WmsContext context) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
            await CancelExpiredReservations(stoppingToken);
        }
    }

    private async Task CancelExpiredReservations(CancellationToken token)
    {
    }
}