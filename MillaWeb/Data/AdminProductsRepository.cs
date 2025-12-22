using Microsoft.Data.SqlClient;
using MillaWeb.Models;

namespace MillaWeb.Data;

public class AdminProductsRepository
{
    private readonly string _connStr;

    public AdminProductsRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("Default")
                  ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public List<AdminProductRow> ListProducts()
    {
        var list = new List<AdminProductRow>();

        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminListProducts", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new AdminProductRow
            {
                ProductID = r.GetInt32(r.GetOrdinal("ProductID")),
                ProductName = r.GetString(r.GetOrdinal("ProductName")),
                CategoryName = r.GetString(r.GetOrdinal("CategoryName")),
                BrandName = r.GetString(r.GetOrdinal("BrandName")),
                SupplierName = r.GetString(r.GetOrdinal("SupplierName")),
                UnitPrice = r.GetDecimal(r.GetOrdinal("UnitPrice")),
                IsActive = r.GetBoolean(r.GetOrdinal("IsActive")),
                ImageUrl = r.IsDBNull(r.GetOrdinal("ImageUrl")) ? null : r.GetString(r.GetOrdinal("ImageUrl")),
            });
        }

        return list;
    }

    public int AddProduct(int categoryId, int brandId, int supplierId, string productName, decimal unitPrice, string? description, string? imageUrl)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminAddProduct", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@CategoryID", categoryId);
        cmd.Parameters.AddWithValue("@BrandID", brandId);
        cmd.Parameters.AddWithValue("@SupplierID", supplierId);
        cmd.Parameters.AddWithValue("@ProductName", productName);
        cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
        cmd.Parameters.AddWithValue("@Description", (object?)description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ImageUrl", (object?)imageUrl ?? DBNull.Value);

        var newIdObj = cmd.ExecuteScalar();
        return Convert.ToInt32(newIdObj);
    }

    public void DeactivateProduct(int productId)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminDeactivateProduct", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ProductID", productId);

        cmd.ExecuteNonQuery();
    }
}
