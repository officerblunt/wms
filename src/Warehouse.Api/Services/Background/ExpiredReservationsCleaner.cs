using Warehouse.Infrastructure.Data;

namespace Warehouse.Api.Services.Background;

public class ExpiredReservationsCleaner(IServiceProvider serviceProvider) : BackgroundService
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
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WmsContext>();
        var expiredReservations =
            context.StockReservations.Where(reservation =>
                reservation.ExpiresAt <= DateTime.UtcNow && reservation.CancelledAt == null);

        foreach (var expiredReservation in expiredReservations)
        {
            expiredReservation.CancelledAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(token);
    }
}