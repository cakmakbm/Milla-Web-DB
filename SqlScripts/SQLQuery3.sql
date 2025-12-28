USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminAnswerQuestion]    Script Date: 26.12.2025 01:44:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_AdminAnswerQuestion]
  @QuestionID INT,
  @SupplierID INT,
  @AnswerText NVARCHAR(1000)
AS
BEGIN
  SET NOCOUNT ON;

  IF @AnswerText IS NULL OR LTRIM(RTRIM(@AnswerText)) = ''
  BEGIN
    RAISERROR('AnswerText required',16,1);
    RETURN;
  END

  IF EXISTS (SELECT 1 FROM dbo.Answer WHERE QuestionID=@QuestionID)
  BEGIN
    UPDATE dbo.Answer
    SET SupplierID=@SupplierID, AnswerText=@AnswerText, AnswerDate=SYSUTCDATETIME()
    WHERE QuestionID=@QuestionID;
  END
  ELSE
  BEGIN
    INSERT INTO dbo.Answer(QuestionID, SupplierID, AnswerText)
    VALUES(@QuestionID, @SupplierID, @AnswerText);
  END

  UPDATE dbo.Question SET IsAnswered=1 WHERE QuestionID=@QuestionID;
END
GO


