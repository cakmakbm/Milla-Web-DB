USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_UserHasPurchasedProduct]    Script Date: 26.12.2025 01:48:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_UserHasPurchasedProduct]
  @CustomerID INT,
  @ProductID INT
AS
BEGIN
  SET NOCOUNT ON;

  SELECT CASE WHEN EXISTS(
    SELECT 1
    FROM dbo.[Order] o
    JOIN dbo.OrderItem oi ON oi.OrderID = o.OrderID
    JOIN dbo.ProductVariant pv ON pv.VariantID = oi.VariantID
    WHERE o.CustomerID = @CustomerID
      AND pv.ProductID = @ProductID
  ) THEN 1 ELSE 0 END AS HasPurchased;
END
GO


