using System.ComponentModel.DataAnnotations;

namespace MillaWeb.Models;

public class AdminProductEditVM
{
    public int ProductID { get; set; }

    [Required]
    public int CategoryID { get; set; }

    [Required]
    public int BrandID { get; set; }

    [Required]
    public int SupplierID { get; set; }

    [Required, StringLength(200)]
    public string ProductName { get; set; } = "";

    [Range(0, 999999)]
    public decimal UnitPrice { get; set; }

    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }
}
