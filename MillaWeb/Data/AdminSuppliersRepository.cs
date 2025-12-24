using Microsoft.Data.SqlClient;
using System.Data;
using MillaWeb.Models;

namespace MillaWeb.Data;

public class AdminSuppliersRepository
{
    private readonly string _connStr;
    public AdminSuppliersRepository(IConfiguration cfg)
    {
        _connStr = cfg.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public List<AdminSupplierRow> List()
    {
        var list = new List<AdminSupplierRow>();
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminSupplier_List", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new AdminSupplierRow
            {
                SupplierID = r.GetInt32(0),
                StoreName = r.GetString(1),
                ContactEmail = r.IsDBNull(2) ? null : r.GetString(2),
                ContactPhone = r.IsDBNull(3) ? null : r.GetString(3),
                AddressText = r.IsDBNull(4) ? null : r.GetString(4),
            });
        }

        return list;
    }

    public void Add(string storeName, string? email, string? phone, string? address)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminSupplier_Add", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@StoreName", storeName);
        cmd.Parameters.AddWithValue("@ContactEmail", (object?)email ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ContactPhone", (object?)phone ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@AddressText", (object?)address ?? DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    public void Update(int supplierId, string storeName, string? email, string? phone, string? address)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminSupplier_Update", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@SupplierID", supplierId);
        cmd.Parameters.AddWithValue("@StoreName", storeName);
        cmd.Parameters.AddWithValue("@ContactEmail", (object?)email ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ContactPhone", (object?)phone ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@AddressText", (object?)address ?? DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int supplierId)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminSupplier_Delete", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@SupplierID", supplierId);
        cmd.ExecuteNonQuery();
    }
}
