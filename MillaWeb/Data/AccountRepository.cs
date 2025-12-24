using Microsoft.Data.SqlClient;
using System.Data;

namespace MillaWeb.Data;

public class AccountRepository
{
    private readonly string _connStr;
    public AccountRepository(IConfiguration cfg)
    {
        _connStr = cfg.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public int RegisterCustomer(string email, string passwordHash, string firstName, string lastName)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_RegisterCustomer", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
        cmd.Parameters.AddWithValue("@FirstName", firstName);
        cmd.Parameters.AddWithValue("@LastName", lastName);

        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    
}
