namespace MillaWeb.Models;

public class OrderListRow
{
    public int OrderID { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; } = "";
    public decimal TotalAmount { get; set; }
}

public class OrderDetailVM
{
    public int OrderID { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; } = "";
    public decimal TotalAmount { get; set; }

    public List<OrderItemRow> Items { get; set; } = new();
}

public class OrderItemRow
{
    public int OrderItemID { get; set; }
    public int ProductID { get; set; }
    public string ProductName { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPriceAtOrder { get; set; }
    public decimal LineTotal { get; set; }
}
