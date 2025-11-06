namespace OrderSystem.Models
{
    public class ProductType
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
