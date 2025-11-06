namespace OrderSystem.Dtos;

public class ProductCreateDto
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int ProductTypeId { get; set; }
}

public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int ProductTypeId { get; set; }
    public string? ProductTypeName { get; set; }
}
