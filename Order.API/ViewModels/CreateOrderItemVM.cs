namespace Order.API.ViewModels
{
    public record CreateOrderItemVM(string ProductId, int Count, decimal Price);
}
