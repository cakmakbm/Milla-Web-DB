namespace MillaWeb.Models;

public class AddressFormVM
{
    public int? AddressID { get; set; }
    public string AddressLine { get; set; } = "";
    public string City { get; set; } = "";
    public string? District { get; set; }
    public string? ZipCode { get; set; }
    public string Country { get; set; } = "Türkiye";
    public bool IsDefault { get; set; }
}
