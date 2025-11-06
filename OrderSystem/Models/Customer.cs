using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
