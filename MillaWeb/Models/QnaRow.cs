namespace MillaWeb.Models;

public class QnaRow
{
    public int QuestionID { get; set; }
    public int CustomerID { get; set; }
    public string QuestionText { get; set; } = "";
    public DateTime QuestionDate { get; set; }
    public bool IsAnswered { get; set; }

    public int? AnswerID { get; set; }
    public string? AnswerText { get; set; }
    public DateTime? AnswerDate { get; set; }
}
