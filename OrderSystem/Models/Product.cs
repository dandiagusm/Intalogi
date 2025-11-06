namespace OrderSystem.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public float ItemPrice { get; set; }

        public int TypeId { get; set; }
        public ProductType? ProductType { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
