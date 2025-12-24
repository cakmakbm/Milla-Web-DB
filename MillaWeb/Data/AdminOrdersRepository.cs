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
    public void UpdateStatus(int orderId, string newStatus)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminUpdateOrderStatus", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@OrderID", orderId);
        cmd.Parameters.AddWithValue("@NewStatus", newStatus);

        cmd.ExecuteNonQuery();
    }
    public MillaWeb.Models.AdminOrderDetailVM? GetDetail(int orderId)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        // Header
        using var cmd1 = new SqlCommand("dbo.sp_AdminGetOrderHeader", conn);
        cmd1.CommandType = CommandType.StoredProcedure;
        cmd1.Parameters.AddWithValue("@OrderID", orderId);

        using var r1 = cmd1.ExecuteReader();
        if (!r1.Read()) return null;

        var vm = new MillaWeb.Models.AdminOrderDetailVM
        {
            OrderID = r1.GetInt32(0),
            OrderDate = r1.GetDateTime(1),
            OrderStatus = r1.GetString(2),
            TotalAmount = r1.GetDecimal(3),
            CustomerID = r1.GetInt32(4),
            FirstName = r1.GetString(5),
            LastName = r1.GetString(6),
            Email = r1.GetString(7),
        };
        r1.Close();

        // Items
        using var cmd2 = new SqlCommand("dbo.sp_AdminGetOrderItems", conn);
        cmd2.CommandType = CommandType.StoredProcedure;
        cmd2.Parameters.AddWithValue("@OrderID", orderId);

        using var r2 = cmd2.ExecuteReader();
        while (r2.Read())
        {
            vm.Items.Add(new MillaWeb.Models.AdminOrderItemRow
            {
                OrderItemID = r2.GetInt32(0),
                ProductID = r2.GetInt32(1),
                ProductName = r2.GetString(2),
                VariantID = r2.GetInt32(3),
                Color = r2.IsDBNull(4) ? null : r2.GetString(4),
                Size = r2.IsDBNull(5) ? null : r2.GetString(5),
                SKU = r2.IsDBNull(6) ? null : r2.GetString(6),
                Quantity = r2.GetInt32(7),
                UnitPriceAtOrder = r2.GetDecimal(8),
                LineTotal = r2.GetDecimal(9),
            });
        }

        return vm;
    }


}
