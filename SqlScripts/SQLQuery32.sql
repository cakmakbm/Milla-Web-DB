USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_ListReviewsByProduct]    Script Date: 26.12.2025 01:47:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_ListReviewsByProduct]
  @ProductID INT
AS
BEGIN
  SET NOCOUNT ON;

  SELECT ReviewID, CustomerID, Rating, ReviewText, ReviewDate
  FROM dbo.Review
  WHERE ProductID=@ProductID
  ORDER BY ReviewDate DESC;
END
GO


