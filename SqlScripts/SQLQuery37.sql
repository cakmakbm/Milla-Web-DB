USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_UserAskQuestion]    Script Date: 26.12.2025 01:47:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_UserAskQuestion]
  @CustomerID INT,
  @ProductID INT,
  @QuestionText NVARCHAR(1000)
AS
BEGIN
  SET NOCOUNT ON;

  IF @QuestionText IS NULL OR LTRIM(RTRIM(@QuestionText)) = ''
  BEGIN
    RAISERROR('QuestionText required',16,1);
    RETURN;
  END

  INSERT INTO dbo.Question(CustomerID, ProductID, QuestionText)
  VALUES(@CustomerID, @ProductID, @QuestionText);
END
GO


