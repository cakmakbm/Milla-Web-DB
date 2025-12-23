using Microsoft.Data.SqlClient;
using System.Data;
using MillaWeb.Models;

namespace MillaWeb.Data;

public class AdminOrdersRepository
{
    private readonly string _connStr;
    public AdminOrdersRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public List<AdminOrderRow> List(int? customerId, string? status)
    {
        var list = new List<AdminOrderRow>();

        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminListOrders", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@CustomerID", (object?)customerId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Status", (object?)status ?? DBNull.Value);

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new AdminOrderRow
            {
                OrderID = r.GetInt32(0),
                OrderDate = r.GetDateTime(1),
                OrderStatus = r.GetString(2),
                TotalAmount = r.GetDecimal(3),
                CustomerID = r.GetInt32(4),
                FirstName = r.GetString(5),
                LastName = r.GetString(6),
                Email = r.GetString(7),
            });
        }

        return list;
    }
}
