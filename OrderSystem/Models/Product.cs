using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderSystem.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public int ProductTypeId { get; set; }

    [ForeignKey("ProductTypeId")]
    public ProductType? ProductType { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
