namespace OrderSystem.Dtos;

public class OrderCreateDto
{
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public List<OrderItemDto> Details { get; set; } = new();
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class OrderResponseDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderDetailResponseDto> Details { get; set; } = new();
}

public class OrderDetailResponseDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Price * Quantity;
}
