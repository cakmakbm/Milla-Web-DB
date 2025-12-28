USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_ListQnAByProduct]    Script Date: 26.12.2025 01:47:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_ListQnAByProduct]
  @ProductID INT
AS
BEGIN
  SET NOCOUNT ON;

  SELECT
    q.QuestionID, q.CustomerID, q.QuestionText, q.QuestionDate, q.IsAnswered,
    a.AnswerID, a.AnswerText, a.AnswerDate
  FROM dbo.Question q
  LEFT JOIN dbo.Answer a ON a.QuestionID = q.QuestionID
  WHERE q.ProductID=@ProductID
  ORDER BY q.QuestionDate DESC;
END
GO


