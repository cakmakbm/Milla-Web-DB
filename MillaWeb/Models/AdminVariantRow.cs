namespace MillaWeb.Models;

public class AdminVariantRow
{
    public int VariantID { get; set; }
    public int ProductID { get; set; }
    public string ProductName { get; set; } = "";
    public string Color { get; set; } = "";
    public string Size { get; set; } = "";
    public int StockQuantity { get; set; }
    public string? Sku { get; set; }
}
