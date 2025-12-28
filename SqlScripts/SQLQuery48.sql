USE [Milla]
GO

/****** Object:  View [dbo].[vw_LowStockVariants]    Script Date: 26.12.2025 01:48:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   VIEW [dbo].[vw_LowStockVariants]
AS
SELECT
    pv.VariantID,
    pv.ProductID,
    p.ProductName,
    pv.Color,
    pv.Size,
    pv.StockQuantity,
    pv.Sku
FROM dbo.ProductVariant pv
INNER JOIN dbo.Product p ON p.ProductID = pv.ProductID
WHERE p.IsActive = 1
  AND pv.StockQuantity <= 5;
GO


