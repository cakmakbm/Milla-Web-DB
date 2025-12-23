namespace MillaWeb.Models;

public class AddressRow
{
    public int AddressID { get; set; }
    public string AddressLine { get; set; } = "";
    public string City { get; set; } = "";
    public string? District { get; set; }
    public string? ZipCode { get; set; }
    public string Country { get; set; } = "";
    public bool IsDefault { get; set; }

    public string DisplayText =>
        $"{AddressLine} - {City} {District} {ZipCode} ({Country})" + (IsDefault ? " [Default]" : "");
}
