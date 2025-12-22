using Microsoft.Data.SqlClient;
using MillaWeb.Models;
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

    public List<CatalogItem> GetCatalog()
    {
        var list = new List<CatalogItem>();

        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand(
            "SELECT ProductID, ProductName, CategoryName, BrandName, SupplierName, UnitPrice, ImageUrl, TotalVariantStock, VariantCount " +
            "FROM dbo.vw_FashionCatalog ORDER BY ProductName;",
            conn);

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new CatalogItem
            {
                ProductID = r.GetInt32(r.GetOrdinal("ProductID")),
                ProductName = r.GetString(r.GetOrdinal("ProductName")),
                CategoryName = r.GetString(r.GetOrdinal("CategoryName")),
                BrandName = r.GetString(r.GetOrdinal("BrandName")),
                SupplierName = r.GetString(r.GetOrdinal("SupplierName")),
                UnitPrice = r.GetDecimal(r.GetOrdinal("UnitPrice")),

                ImageUrl = r.IsDBNull(r.GetOrdinal("ImageUrl")) ? null : r.GetString(r.GetOrdinal("ImageUrl")),

                TotalVariantStock = r.IsDBNull(r.GetOrdinal("TotalVariantStock")) ? 0 : r.GetInt32(r.GetOrdinal("TotalVariantStock")),
                VariantCount = r.IsDBNull(r.GetOrdinal("VariantCount")) ? 0 : r.GetInt32(r.GetOrdinal("VariantCount"))
            });
        }

        return list;
    }
}
