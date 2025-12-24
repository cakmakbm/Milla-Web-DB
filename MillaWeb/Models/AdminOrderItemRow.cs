namespace MillaWeb.Models;

public class AdminOrderItemRow
{
    public int OrderItemID { get; set; }
    public int ProductID { get; set; }
    public string ProductName { get; set; } = "";

    public int VariantID { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public string? SKU { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPriceAtOrder { get; set; }
    public decimal LineTotal { get; set; }
}
