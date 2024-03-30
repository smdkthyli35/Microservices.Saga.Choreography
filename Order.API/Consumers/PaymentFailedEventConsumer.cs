using MassTransit;
using Order.API.Enums;
using Order.API.Models.Contexts;
using Shared.Events;

namespace Order.API.Consumers
{
    public class PaymentFailedEventConsumer(OrderAPIDbContext _context) : IConsumer<PaymentFailedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            Models.Order? order = await _context.Orders.FindAsync(context.Message.OrderId);
            ArgumentNullException.ThrowIfNull(order);
            order.OrderStatus = OrderStatus.Fail;
            await _context.SaveChangesAsync();
        }
    }
}
