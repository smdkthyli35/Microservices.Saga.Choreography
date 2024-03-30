using MassTransit;
using Order.API.Enums;
using Order.API.Models.Contexts;
using Shared.Events;

namespace Order.API.Consumers
{
    public class StockNotReservedEventConsumer(OrderAPIDbContext _context) : IConsumer<StockNotReservedEvent>
    {
        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            Models.Order? order = await _context.Orders.FindAsync(context.Message.OrderId);
            ArgumentNullException.ThrowIfNull(order);
            order.OrderStatus = OrderStatus.Fail;
            await _context.SaveChangesAsync();
        }
    }
}
