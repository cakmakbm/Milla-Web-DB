namespace MillaWeb.Models;

public class ReturnRow
{
    public int ReturnID { get; set; }
    public DateTime ReturnDate { get; set; }
    public string ReturnStatus { get; set; } = "";
    public int ReturnQty { get; set; }
    public string? ReturnReason { get; set; }

    public int OrderItemID { get; set; }
    public string ProductName { get; set; } = "";
    public string? Color { get; set; }
    public string? Size { get; set; }
}
