using Microsoft.Data.SqlClient;
using System.Data;
using MillaWeb.Models;

namespace MillaWeb.Data;

public class AdminBrandsRepository
{
    private readonly string _connStr;
    public AdminBrandsRepository(IConfiguration cfg)
    {
        _connStr = cfg.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public List<AdminBrandRow> List()
    {
        var list = new List<AdminBrandRow>();
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminBrand_List", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new AdminBrandRow
            {
                BrandID = r.GetInt32(0),
                BrandName = r.GetString(1),
                Country = r.IsDBNull(2) ? null : r.GetString(2)
            });
        }
        return list;
    }

    public void Add(string brandName, string? country)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminBrand_Add", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BrandName", brandName);
        cmd.Parameters.AddWithValue("@Country", (object?)country ?? DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    public void Update(int brandId, string brandName, string? country)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminBrand_Update", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BrandID", brandId);
        cmd.Parameters.AddWithValue("@BrandName", brandName);
        cmd.Parameters.AddWithValue("@Country", (object?)country ?? DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    public void Delete(int brandId)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminBrand_Delete", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BrandID", brandId);
        cmd.ExecuteNonQuery();
    }
}
