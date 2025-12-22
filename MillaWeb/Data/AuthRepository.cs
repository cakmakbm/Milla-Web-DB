using Microsoft.Data.SqlClient;

namespace MillaWeb.Data;

public class AuthRepository
{
    private readonly string _connStr;

    public AuthRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("Default")
                  ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public (int UserID, string Email, string Role, int? CustomerID)? ValidateUser(string email, string password)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand(@"
SELECT TOP 1 UserID, Email, Role, CustomerID
FROM dbo.AppUser
WHERE Email = @Email AND PasswordHash = @Pwd;
", conn);

        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@Pwd", password);

        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;

        int? customerId = r.IsDBNull(r.GetOrdinal("CustomerID")) ? null : r.GetInt32(r.GetOrdinal("CustomerID"));

        return (
            r.GetInt32(r.GetOrdinal("UserID")),
            r.GetString(r.GetOrdinal("Email")),
            r.GetString(r.GetOrdinal("Role")),
            customerId
        );
    }
}
