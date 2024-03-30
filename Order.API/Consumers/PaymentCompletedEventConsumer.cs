using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Enums;
using Order.API.Models.Contexts;
using Shared.Events;

namespace Order.API.Consumers
{
    public class PaymentCompletedEventConsumer(OrderAPIDbContext _context) : IConsumer<PaymentCompletedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            Order.API.Models.Order? order = await _context.Orders.FindAsync(context.Message.OrderId);
            ArgumentNullException.ThrowIfNull(order);
            order.OrderStatus = OrderStatus.Completed;
            await _context.SaveChangesAsync();
        }
    }
}