using Microsoft.Data.SqlClient;
using System.Data;
using MillaWeb.Models;

namespace MillaWeb.Data;

public class UserAddressRepository
{
    private readonly string _connStr;
    public UserAddressRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public List<AddressRow> List(int customerId)
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

    public AddressFormVM? Get(int customerId, int addressId)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand(@"
SELECT AddressID, AddressLine, City, District, ZipCode, Country, IsDefault
FROM dbo.Address
WHERE AddressID=@aid AND CustomerID=@cid;", conn);

        cmd.Parameters.AddWithValue("@aid", addressId);
        cmd.Parameters.AddWithValue("@cid", customerId);

        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;

        return new AddressFormVM
        {
            AddressID = r.GetInt32(0),
            AddressLine = r.GetString(1),
            City = r.GetString(2),
            District = r.IsDBNull(3) ? null : r.GetString(3),
            ZipCode = r.IsDBNull(4) ? null : r.GetString(4),
            Country = r.GetString(5),
            IsDefault = r.GetBoolean(6),
        };
    }

    public void Add(int customerId, AddressFormVM vm)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_UserAddAddress", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@CustomerID", customerId);
        cmd.Parameters.AddWithValue("@AddressLine", vm.AddressLine);
        cmd.Parameters.AddWithValue("@City", vm.City);
        cmd.Parameters.AddWithValue("@District", (object?)vm.District ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ZipCode", (object?)vm.ZipCode ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Country", vm.Country ?? "Türkiye");
        cmd.Parameters.AddWithValue("@IsDefault", vm.IsDefault);

        cmd.ExecuteNonQuery();
    }

    public void Update(int customerId, AddressFormVM vm)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_UserUpdateAddress", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@CustomerID", customerId);
        cmd.Parameters.AddWithValue("@AddressID", vm.AddressID!.Value);
        cmd.Parameters.AddWithValue("@AddressLine", vm.AddressLine);
        cmd.Parameters.AddWithValue("@City", vm.City);
        cmd.Parameters.AddWithValue("@District", (object?)vm.District ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ZipCode", (object?)vm.ZipCode ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Country", vm.Country ?? "Türkiye");
        cmd.Parameters.AddWithValue("@IsDefault", vm.IsDefault);

        cmd.ExecuteNonQuery();
    }

    public void Delete(int customerId, int addressId)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_UserDeleteAddress", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CustomerID", customerId);
        cmd.Parameters.AddWithValue("@AddressID", addressId);

        cmd.ExecuteNonQuery();
    }
}
