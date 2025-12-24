using Microsoft.Data.SqlClient;
using System.Data;

namespace MillaWeb.Data;

public class AuthRepository
{
    private readonly string _connStr;

    public AuthRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("Default")
                  ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    // LOGIN
    public (int UserID, string Email, string Role, int? CustomerID)? ValidateUser(string email, string passwordHash)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand(@"
SELECT TOP 1 UserID, Email, Role, CustomerID
FROM dbo.AppUser
WHERE Email = @Email AND PasswordHash = @Pwd;
", conn);

        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@Pwd", passwordHash);

        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;

        int? customerId = r.IsDBNull(r.GetOrdinal("CustomerID"))
            ? null
            : r.GetInt32(r.GetOrdinal("CustomerID"));

        return (
            r.GetInt32(r.GetOrdinal("UserID")),
            r.GetString(r.GetOrdinal("Email")),
            r.GetString(r.GetOrdinal("Role")),
            customerId
        );
    }

    // REGISTER (Customer + AppUser)
    // Bu metot:
    // 1) Email zaten var mı kontrol eder
    // 2) Customer insert eder
    // 3) AppUser insert eder (Role='User', CustomerID bağlar)
    // 4) CustomerID döndürür
    public int RegisterCustomer(
    string email,
    string passwordHash,
    string firstName,
    string lastName,
    string phoneNumber)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var tx = conn.BeginTransaction();

        try
        {
            // Email check
            using (var check = new SqlCommand(
                "SELECT COUNT(1) FROM dbo.AppUser WHERE Email=@Email",
                conn, tx))
            {
                check.Parameters.AddWithValue("@Email", email);
                if ((int)check.ExecuteScalar() > 0)
                    throw new Exception("EMAIL_EXISTS");
            }

            // Customer insert
            int customerId;
            using (var cmdC = new SqlCommand(@"
INSERT INTO dbo.Customer (FirstName, LastName, Email, PhoneNumber)
VALUES (@FirstName, @LastName, @Email, @Phone);

SELECT CAST(SCOPE_IDENTITY() AS INT);
", conn, tx))
            {
                cmdC.Parameters.AddWithValue("@FirstName", firstName);
                cmdC.Parameters.AddWithValue("@LastName", lastName);
                cmdC.Parameters.AddWithValue("@Email", email);
                cmdC.Parameters.AddWithValue("@Phone", phoneNumber);

                customerId = (int)cmdC.ExecuteScalar();
            }

            // AppUser insert
            using (var cmdU = new SqlCommand(@"
INSERT INTO dbo.AppUser (Email, PasswordHash, Role, CustomerID)
VALUES (@Email, @Pwd, 'User', @CustomerID);
", conn, tx))
            {
                cmdU.Parameters.AddWithValue("@Email", email);
                cmdU.Parameters.AddWithValue("@Pwd", passwordHash);
                cmdU.Parameters.AddWithValue("@CustomerID", customerId);
                cmdU.ExecuteNonQuery();
            }

            tx.Commit();
            return customerId;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

}
