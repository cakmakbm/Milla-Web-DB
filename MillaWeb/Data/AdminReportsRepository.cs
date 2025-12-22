using Microsoft.Data.SqlClient;
using MillaWeb.Models;

namespace MillaWeb.Data;

public class AdminReportsRepository
{
    private readonly string _connStr;

    public AdminReportsRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("Default")
                  ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public List<LowStockRow> GetLowStock()
    {
        var list = new List<LowStockRow>();

        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand(@"
SELECT VariantID, ProductID, ProductName, Color, Size, StockQuantity, Sku
FROM dbo.vw_LowStockVariants
ORDER BY StockQuantity ASC, VariantID DESC;", conn);

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new LowStockRow
            {
                VariantID = r.GetInt32(0),
                ProductID = r.GetInt32(1),
                ProductName = r.GetString(2),
                Color = r.GetString(3),
                Size = r.GetString(4),
                StockQuantity = r.GetInt32(5),
                Sku = r.IsDBNull(6) ? null : r.GetString(6),
            });
        }

        return list;
    }
}
