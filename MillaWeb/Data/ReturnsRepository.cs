using Microsoft.Data.SqlClient;
using MillaWeb.Models;
using System.Data;

namespace MillaWeb.Data;

public class ReturnsRepository
{
    private readonly string _connStr;
    public ReturnsRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public void RequestReturn(int customerId, int orderItemId, int quantity, string? reason)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_UserRequestReturn", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@CustomerID", customerId);
        cmd.Parameters.AddWithValue("@OrderItemID", orderItemId);
        cmd.Parameters.AddWithValue("@Quantity", quantity);
        cmd.Parameters.AddWithValue("@Reason", (object?)reason ?? DBNull.Value);

        cmd.ExecuteNonQuery();
    }
    public List<ReturnRow> ListByOrder(int customerId, int orderId)
    {
        var list = new List<ReturnRow>();

        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_UserListReturnsByOrder", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@CustomerID", customerId);
        cmd.Parameters.AddWithValue("@OrderID", orderId);

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new ReturnRow
            {
                ReturnID = r.GetInt32(0),
                ReturnDate = r.GetDateTime(1),
                ReturnStatus = r.GetString(2),
                ReturnQty = r.GetInt32(3),
                ReturnReason = r.IsDBNull(4) ? null : r.GetString(4),
                OrderItemID = r.GetInt32(5),
                ProductName = r.GetString(6),
                Color = r.IsDBNull(7) ? null : r.GetString(7),
                Size = r.IsDBNull(8) ? null : r.GetString(8),
            });
        }

        return list;
    }

}
