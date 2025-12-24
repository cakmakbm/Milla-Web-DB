using Microsoft.Data.SqlClient;
using System.Data;
using MillaWeb.Models;

namespace MillaWeb.Data;

public class CatalogRepository
{
    private readonly string _connStr;

    public CatalogRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public (List<string> Categories, List<string> Brands, List<string> Sizes) GetFilterOptions()
    {
        var cats = new List<string>();
        var brands = new List<string>();
        var sizes = new List<string>();

        using var conn = new SqlConnection(_connStr);
        conn.Open();

        // Categories
        using (var cmd = new SqlCommand("SELECT CategoryName FROM dbo.Category ORDER BY CategoryName;", conn))
        using (var r = cmd.ExecuteReader())
            while (r.Read()) cats.Add(r.GetString(0));

        // Brands
        using (var cmd = new SqlCommand("SELECT BrandName FROM dbo.Brand ORDER BY BrandName;", conn))
        using (var r = cmd.ExecuteReader())
            while (r.Read()) brands.Add(r.GetString(0));

        // Sizes (ProductVariant)
        using (var cmd = new SqlCommand("SELECT DISTINCT Size FROM dbo.ProductVariant WHERE Size IS NOT NULL ORDER BY Size;", conn))
        using (var r = cmd.ExecuteReader())
            while (r.Read()) sizes.Add(r.GetString(0));

        return (cats, brands, sizes);
    }

    public (List<CatalogItem> Items, int TotalCount) GetCatalogFiltered(
        string? q,
        List<string>? categories,
        List<string>? brands,
        List<string>? sizes,
        int page,
        int pageSize)
    {
        var list = new List<CatalogItem>();
        int total = 0;

        string? cats = (categories != null && categories.Count > 0) ? string.Join("|", categories) : null;
        string? brs = (brands != null && brands.Count > 0) ? string.Join("|", brands) : null;
        string? szs = (sizes != null && sizes.Count > 0) ? string.Join("|", sizes) : null;

        using var conn = new SqlConnection(_connStr);
        conn.Open();

        // COUNT
        using (var cmdCount = new SqlCommand(@"
SELECT COUNT(DISTINCT p.ProductID)
FROM dbo.Product p
JOIN dbo.Category c ON c.CategoryID = p.CategoryID
JOIN dbo.Brand b ON b.BrandID = p.BrandID
JOIN dbo.Supplier s ON s.SupplierID = p.SupplierID
WHERE p.IsActive = 1
  AND (@q IS NULL OR p.ProductName LIKE '%'+@q+'%' OR c.CategoryName LIKE '%'+@q+'%' OR b.BrandName LIKE '%'+@q+'%' OR s.StoreName LIKE '%'+@q+'%')
  AND (@cats IS NULL OR c.CategoryName IN (SELECT value FROM string_split(@cats, '|')))
  AND (@brands IS NULL OR b.BrandName IN (SELECT value FROM string_split(@brands, '|')))
  AND (@sizes IS NULL OR EXISTS (
        SELECT 1 FROM dbo.ProductVariant pv
        WHERE pv.ProductID = p.ProductID
          AND pv.Size IN (SELECT value FROM string_split(@sizes, '|'))
  ));", conn))
        {
            cmdCount.Parameters.AddWithValue("@q", (object?)q ?? DBNull.Value);
            cmdCount.Parameters.AddWithValue("@cats", (object?)cats ?? DBNull.Value);
            cmdCount.Parameters.AddWithValue("@brands", (object?)brs ?? DBNull.Value);
            cmdCount.Parameters.AddWithValue("@sizes", (object?)szs ?? DBNull.Value);

            total = Convert.ToInt32(cmdCount.ExecuteScalar());
        }

        int offset = (page - 1) * pageSize;

        // PAGE DATA
        using (var cmd = new SqlCommand(@"
SELECT 
    p.ProductID,
    p.ProductName,
    c.CategoryName,
    b.BrandName,
    s.StoreName AS SupplierName,
    p.UnitPrice,
    p.ImageUrl,
    ISNULL((
        SELECT SUM(pv.StockQuantity) 
        FROM dbo.ProductVariant pv 
        WHERE pv.ProductID = p.ProductID
    ), 0) AS TotalVariantStock,
    ISNULL((
        SELECT COUNT(*) 
        FROM dbo.ProductVariant pv 
        WHERE pv.ProductID = p.ProductID
    ), 0) AS VariantCount
FROM dbo.Product p
JOIN dbo.Category c ON c.CategoryID = p.CategoryID
JOIN dbo.Brand b ON b.BrandID = p.BrandID
JOIN dbo.Supplier s ON s.SupplierID = p.SupplierID
WHERE p.IsActive = 1
  AND (@q IS NULL OR p.ProductName LIKE '%'+@q+'%' OR c.CategoryName LIKE '%'+@q+'%' OR b.BrandName LIKE '%'+@q+'%' OR s.StoreName LIKE '%'+@q+'%')
  AND (@cats IS NULL OR c.CategoryName IN (SELECT value FROM string_split(@cats, '|')))
  AND (@brands IS NULL OR b.BrandName IN (SELECT value FROM string_split(@brands, '|')))
  AND (@sizes IS NULL OR EXISTS (
        SELECT 1 FROM dbo.ProductVariant pv
        WHERE pv.ProductID = p.ProductID
          AND pv.Size IN (SELECT value FROM string_split(@sizes, '|'))
  ))
ORDER BY p.ProductName
OFFSET @off ROWS FETCH NEXT @ps ROWS ONLY;", conn))
        {
            cmd.Parameters.AddWithValue("@q", (object?)q ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@cats", (object?)cats ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@brands", (object?)brs ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@sizes", (object?)szs ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@off", offset);
            cmd.Parameters.AddWithValue("@ps", pageSize);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new CatalogItem
                {
                    ProductID = r.GetInt32(0),
                    ProductName = r.GetString(1),
                    CategoryName = r.GetString(2),
                    BrandName = r.GetString(3),
                    SupplierName = r.GetString(4),
                    UnitPrice = r.GetDecimal(5),
                    ImageUrl = r.IsDBNull(6) ? null : r.GetString(6),
                    TotalVariantStock = r.GetInt32(7),
                    VariantCount = r.GetInt32(8)
                });
            }
        }

        return (list, total);
    }
}
