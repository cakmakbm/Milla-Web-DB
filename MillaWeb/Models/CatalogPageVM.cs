namespace MillaWeb.Models;

public class CatalogPageVM
{
    public List<CatalogItem> Items { get; set; } = new();

    public List<string> Categories { get; set; } = new();
    public List<string> Brands { get; set; } = new();
    public List<string> Sizes { get; set; } = new();

    public List<string> SelectedCategories { get; set; } = new();
    public List<string> SelectedBrands { get; set; } = new();
    public List<string> SelectedSizes { get; set; } = new();

    public string Q { get; set; } = "";

    public int Page { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int TotalCount { get; set; } = 0;
}
