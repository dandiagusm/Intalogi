namespace OrderSystem.Dtos;

public class OrderDetailCreateDto
{
    public int OrderId { get; set; }   
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class OrderDetailResponseFlatDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Price * Quantity;
}
