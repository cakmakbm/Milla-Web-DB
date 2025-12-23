using Microsoft.Data.SqlClient;
using System.Data;
using MillaWeb.Models;

namespace MillaWeb.Data;

public class ProductSocialRepository
{
    private readonly string _connStr;
    public ProductSocialRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public List<ReviewRow> ListReviews(int productId)
    {
        var list = new List<ReviewRow>();
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_ListReviewsByProduct", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ProductID", productId);

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new ReviewRow
            {
                ReviewID = r.GetInt32(0),
                CustomerID = r.GetInt32(1),
                Rating = r.GetInt32(2),
                ReviewText = r.IsDBNull(3) ? null : r.GetString(3),
                ReviewDate = r.GetDateTime(4)
            });
        }
        return list;
    }

    public void AddOrUpdateReview(int customerId, int productId, int rating, string? text)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_UserAddOrUpdateReview", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CustomerID", customerId);
        cmd.Parameters.AddWithValue("@ProductID", productId);
        cmd.Parameters.AddWithValue("@Rating", rating);
        cmd.Parameters.AddWithValue("@ReviewText", (object?)text ?? DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    public List<QnaRow> ListQna(int productId)
    {
        var list = new List<QnaRow>();
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_ListQnAByProduct", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ProductID", productId);

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new QnaRow
            {
                QuestionID = r.GetInt32(0),
                CustomerID = r.GetInt32(1),
                QuestionText = r.GetString(2),
                QuestionDate = r.GetDateTime(3),
                IsAnswered = r.GetBoolean(4),
                AnswerID = r.IsDBNull(5) ? null : r.GetInt32(5),
                AnswerText = r.IsDBNull(6) ? null : r.GetString(6),
                AnswerDate = r.IsDBNull(7) ? null : r.GetDateTime(7),
            });
        }
        return list;
    }

    public void AskQuestion(int customerId, int productId, string questionText)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_UserAskQuestion", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CustomerID", customerId);
        cmd.Parameters.AddWithValue("@ProductID", productId);
        cmd.Parameters.AddWithValue("@QuestionText", questionText);
        cmd.ExecuteNonQuery();
    }

    public void AdminAnswer(int questionId, int supplierId, string answerText)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminAnswerQuestion", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@QuestionID", questionId);
        cmd.Parameters.AddWithValue("@SupplierID", supplierId);
        cmd.Parameters.AddWithValue("@AnswerText", answerText);
        cmd.ExecuteNonQuery();
    }
    public bool HasPurchasedProduct(int customerId, int productId)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_UserHasPurchasedProduct", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CustomerID", customerId);
        cmd.Parameters.AddWithValue("@ProductID", productId);

        return (int)cmd.ExecuteScalar() == 1;
    }

}
