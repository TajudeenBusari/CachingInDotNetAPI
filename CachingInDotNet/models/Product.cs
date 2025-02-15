using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CachingInDotNet.models;

[Table("Products")]
public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid productId { get; set; }
    public string? productName { get; set; }
    public string? productDescription { get; set; }
    public double productPrice { get; set; }
    public int productQuantity { get; set; }
    public string? productCategory { get; set; }
    public DateTime productCreatedDate { get; set; }
    public DateTime ExpiryDateTime { get; set; }
}