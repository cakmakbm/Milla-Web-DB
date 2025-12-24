namespace MillaWeb.Models;

public class AdminSupplierRow
{
    public int SupplierID { get; set; }
    public string StoreName { get; set; } = "";
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? AddressText { get; set; }
}
