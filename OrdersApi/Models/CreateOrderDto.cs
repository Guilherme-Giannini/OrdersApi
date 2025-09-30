namespace OrdersApi.Models;

public class CreateOrderDto
{
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
}