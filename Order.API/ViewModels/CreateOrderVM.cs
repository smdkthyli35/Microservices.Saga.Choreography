namespace Order.API.ViewModels
{
    public record CreateOrderVM(string BuyerId, List<CreateOrderItemVM> OrderItems);
}
