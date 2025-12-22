namespace MillaWeb.Models;

public class ProductDetailVM
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    public string BrandName { get; set; } = "";
    public string CategoryName { get; set; } = "";
    public string SupplierName { get; set; } = "";

    public List<ProductVariantVM> Variants { get; set; } = new();
}

public class ProductVariantVM
{
    public int VariantID { get; set; }
    public string Color { get; set; } = "";
    public string Size { get; set; } = "";
    public int StockQuantity { get; set; }
    public string? Sku { get; set; }
}
