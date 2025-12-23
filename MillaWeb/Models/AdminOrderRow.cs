namespace MillaWeb.Models;

public class AdminOrderRow
{
    public int OrderID { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; } = "";
    public decimal TotalAmount { get; set; }

    public int CustomerID { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
}
