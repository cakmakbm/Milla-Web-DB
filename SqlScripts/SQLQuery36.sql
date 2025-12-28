USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_UserAddOrUpdateReview]    Script Date: 26.12.2025 01:47:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_UserAddOrUpdateReview]
  @CustomerID INT,
  @ProductID INT,
  @Rating INT,
  @ReviewText NVARCHAR(1000) = NULL
AS
BEGIN
  SET NOCOUNT ON;

  IF @Rating NOT BETWEEN 1 AND 5
  BEGIN
    RAISERROR('Rating must be between 1 and 5',16,1);
    RETURN;
  END

  -- ? Satýn alma kontrolü: customer bu product'ý içeren bir orderitem'a sahip mi?
  IF NOT EXISTS (
      SELECT 1
      FROM dbo.[Order] o
      JOIN dbo.OrderItem oi ON oi.OrderID = o.OrderID
      JOIN dbo.ProductVariant pv ON pv.VariantID = oi.VariantID
      WHERE o.CustomerID = @CustomerID
        AND pv.ProductID = @ProductID
        AND o.OrderStatus IN (N'Processing', N'Shipped', N'Delivered')  -- istersen Delivered yap
  )
  BEGIN
    RAISERROR('You can review only products you purchased.',16,1);
    RETURN;
  END

  IF EXISTS (SELECT 1 FROM dbo.Review WHERE CustomerID=@CustomerID AND ProductID=@ProductID)
  BEGIN
    UPDATE dbo.Review
    SET Rating=@Rating, ReviewText=@ReviewText, ReviewDate=SYSUTCDATETIME()
    WHERE CustomerID=@CustomerID AND ProductID=@ProductID;
  END
  ELSE
  BEGIN
    INSERT INTO dbo.Review(CustomerID, ProductID, Rating, ReviewText)
    VALUES(@CustomerID, @ProductID, @Rating, @ReviewText);
  END
END
GO


