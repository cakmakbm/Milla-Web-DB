namespace MillaWeb.Models;

public class ReviewRow
{
    public int ReviewID { get; set; }
    public int CustomerID { get; set; }
    public int Rating { get; set; }
    public string? ReviewText { get; set; }
    public DateTime ReviewDate { get; set; }
}
