using Microsoft.Data.SqlClient;
using System.Data;
using MillaWeb.Models;

namespace MillaWeb.Data;

public class OrdersRepository
{
    private readonly string _connStr;
    public OrdersRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public int Checkout(int customerId, List<CartItem> cart, int? addressId = null, string paymentMethod = "Card")
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var tx = conn.BeginTransaction();

        try
        {
            int orderId;

            using (var cmd = new SqlCommand("dbo.sp_Checkout_CreateOrder", conn, tx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", customerId);

                
                cmd.Parameters.AddWithValue("@AddressID", (object?)addressId ?? DBNull.Value);

                var outId = new SqlParameter("@OrderID", SqlDbType.Int)
                { Direction = ParameterDirection.Output };

                cmd.Parameters.Add(outId);

                cmd.ExecuteNonQuery();
                orderId = (int)outId.Value;
            }

            foreach (var item in cart)
            {
                using var cmd2 = new SqlCommand("dbo.sp_Checkout_AddOrderItem", conn, tx);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@OrderID", orderId);
                cmd2.Parameters.AddWithValue("@VariantID", item.VariantID);
                cmd2.Parameters.AddWithValue("@Quantity", item.Quantity);
                cmd2.ExecuteNonQuery();
            }

            using (var cmd3 = new SqlCommand("dbo.sp_Checkout_UpdateOrderTotal", conn, tx))
            {
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@OrderID", orderId);
                cmd3.ExecuteNonQuery();
            }

            // 2) Order total'ı DB'den çek
            decimal totalAmount;
            using (var cmdGetTotal = new SqlCommand(@"
    SELECT TotalAmount
    FROM dbo.[Order]
    WHERE OrderID = @OrderID;", conn, tx))
            {
                cmdGetTotal.Parameters.AddWithValue("@OrderID", orderId);
                var obj = cmdGetTotal.ExecuteScalar();
                totalAmount = obj == null || obj == DBNull.Value ? 0m : Convert.ToDecimal(obj);
            }

            // 3) Payment kaydı oluştur
            using (var cmd4 = new SqlCommand("dbo.sp_UserCreatePayment", conn, tx))
            {
                cmd4.CommandType = CommandType.StoredProcedure;
                cmd4.Parameters.AddWithValue("@OrderID", orderId);
                cmd4.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                cmd4.Parameters.AddWithValue("@Amount", totalAmount);
                cmd4.ExecuteNonQuery();
            }


            tx.Commit();
            return orderId;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }


    public List<OrderListRow> ListOrders(int customerId)
    {
        var list = new List<OrderListRow>();

        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand(@"
SELECT OrderID, OrderDate, OrderStatus, ISNULL(TotalAmount, 0) AS TotalAmount
FROM dbo.[Order]
WHERE CustomerID=@cid
ORDER BY OrderID DESC;", conn);

        cmd.Parameters.AddWithValue("@cid", customerId);

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new OrderListRow
            {
                OrderID = r.GetInt32(0),
                OrderDate = r.GetDateTime(1),
                OrderStatus = r.GetString(2),
                TotalAmount = r.GetDecimal(3),
            });
        }

        return list;
    }

    public OrderDetailVM? GetOrderDetail(int customerId, int orderId)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand(@"
SELECT OrderID, OrderDate, OrderStatus, ISNULL(TotalAmount, 0) AS TotalAmount
FROM dbo.[Order]
WHERE OrderID=@oid AND CustomerID=@cid;", conn);

        cmd.Parameters.AddWithValue("@oid", orderId);
        cmd.Parameters.AddWithValue("@cid", customerId);

        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;

        var vm = new OrderDetailVM
        {
            OrderID = r.GetInt32(0),
            OrderDate = r.GetDateTime(1),
            OrderStatus = r.GetString(2),
            TotalAmount = r.GetDecimal(3),
        };
        r.Close();

        using var cmd2 = new SqlCommand(@"
SELECT oi.OrderItemID,
       p.ProductID,
       p.ProductName,
       oi.Quantity,
       oi.UnitPriceAtOrder,
       oi.LineTotal
FROM dbo.OrderItem oi
JOIN dbo.ProductVariant pv ON pv.VariantID = oi.VariantID
JOIN dbo.Product p ON p.ProductID = pv.ProductID
WHERE oi.OrderID=@oid
ORDER BY oi.OrderItemID;", conn);

        cmd2.Parameters.AddWithValue("@oid", orderId);

        using var r2 = cmd2.ExecuteReader();
        while (r2.Read())
        {
            vm.Items.Add(new OrderItemRow
            {
                OrderItemID = r2.GetInt32(0),
                ProductID = r2.GetInt32(1),
                ProductName = r2.GetString(2),
                Quantity = r2.GetInt32(3),
                UnitPriceAtOrder = r2.GetDecimal(4),
                LineTotal = r2.GetDecimal(5),
            });
        }

        return vm;
    }
    public List<AddressRow> ListAddresses(int customerId)
    {
        var list = new List<AddressRow>();

        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_UserListAddresses", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CustomerID", customerId);

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new AddressRow
            {
                AddressID = r.GetInt32(0),
                AddressLine = r.GetString(1),
                City = r.GetString(2),
                District = r.IsDBNull(3) ? null : r.GetString(3),
                ZipCode = r.IsDBNull(4) ? null : r.GetString(4),
                Country = r.GetString(5),
                IsDefault = r.GetBoolean(6),
            });
        }

        return list;
    }

}
