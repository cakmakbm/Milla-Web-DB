using Microsoft.Data.SqlClient;
using System.Data;
using MillaWeb.Models;

namespace MillaWeb.Data;

public class AdminVariantsRepository
{
    private readonly string _connStr;

    public AdminVariantsRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("Default")
                  ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public List<AdminVariantRow> ListByProduct(int productId)
    {
        var list = new List<AdminVariantRow>();

        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminListVariantsByProduct", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ProductID", productId);

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new AdminVariantRow
            {
                VariantID = r.GetInt32(r.GetOrdinal("VariantID")),
                ProductID = r.GetInt32(r.GetOrdinal("ProductID")),
                ProductName = r.GetString(r.GetOrdinal("ProductName")),
                Color = r.GetString(r.GetOrdinal("Color")),
                Size = r.GetString(r.GetOrdinal("Size")),
                StockQuantity = r.GetInt32(r.GetOrdinal("StockQuantity")),
                Sku = r.IsDBNull(r.GetOrdinal("Sku")) ? null : r.GetString(r.GetOrdinal("Sku"))
            });
        }

        return list;
    }

    public void AddVariant(int productId, string color, string size, int stock, string? sku)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminAddVariant", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ProductID", productId);
        cmd.Parameters.AddWithValue("@Color", color);
        cmd.Parameters.AddWithValue("@Size", size);
        cmd.Parameters.AddWithValue("@StockQuantity", stock);
        cmd.Parameters.AddWithValue("@Sku", (object?)sku ?? DBNull.Value);

        cmd.ExecuteNonQuery();
    }

    public void UpdateStock(int variantId, int newStock)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminUpdateVariantStock", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@VariantID", variantId);
        cmd.Parameters.AddWithValue("@NewStock", newStock);


        cmd.ExecuteNonQuery();
    }

    public void Delete(int variantId)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminDeleteVariant", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@VariantID", variantId);

        cmd.ExecuteNonQuery();
    }

    public List<(int ProductID, string ProductName)> ListProducts()
    {
        var list = new List<(int, string)>();

        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminListProductsForDropdown", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add((r.GetInt32(0), r.GetString(1)));
        }

        return list;
    }

}
