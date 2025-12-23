using Microsoft.Data.SqlClient;
using System.Data;
using MillaWeb.Models;

namespace MillaWeb.Data;

public class AdminReturnsRepository
{
    private readonly string _connStr;
    public AdminReturnsRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public List<AdminReturnRow> List(string? status)
    {
        var list = new List<AdminReturnRow>();

        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminListReturns", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Status", (object?)status ?? DBNull.Value);

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new AdminReturnRow
            {
                ReturnID = r.GetInt32(0),
                ReturnDate = r.GetDateTime(1),
                ReturnStatus = r.GetString(2),
                ReturnQty = r.GetInt32(3),
                ReturnReason = r.IsDBNull(4) ? null : r.GetString(4),
                OrderItemID = r.GetInt32(5),
                OrderID = r.GetInt32(6),
                CustomerID = r.GetInt32(7),
                VariantID = r.GetInt32(8),
                ProductName = r.GetString(9),
                Color = r.IsDBNull(10) ? null : r.GetString(10),
                Size = r.IsDBNull(11) ? null : r.GetString(11),
            });
        }

        return list;
    }

    public void UpdateStatus(int returnId, string newStatus)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminUpdateReturnStatus", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ReturnID", returnId);
        cmd.Parameters.AddWithValue("@NewStatus", newStatus);

        cmd.ExecuteNonQuery();
    }
}
