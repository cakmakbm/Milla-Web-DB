namespace MillaWeb.Models;

public class CartItem
{
    public int VariantID { get; set; }
    public int ProductID { get; set; }
    public string ProductName { get; set; } = "";
    public string Color { get; set; } = "";
    public string Size { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
}
