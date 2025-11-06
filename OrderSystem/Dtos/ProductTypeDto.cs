namespace OrderSystem.Dtos;

public class ProductTypeCreateDto
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}

public class ProductTypeResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}
