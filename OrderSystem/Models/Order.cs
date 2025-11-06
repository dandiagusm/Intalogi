using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderSystem.Models;

public class Order
{
    [Key]
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
