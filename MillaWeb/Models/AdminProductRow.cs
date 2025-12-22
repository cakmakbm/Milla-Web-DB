namespace MillaWeb.Models;

public class AdminProductRow
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = "";
    public string CategoryName { get; set; } = "";
    public string BrandName { get; set; } = "";
    public string SupplierName { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; }
}
