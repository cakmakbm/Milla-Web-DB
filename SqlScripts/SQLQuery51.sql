USE [Milla]
GO

/****** Object:  View [dbo].[vw_VariantStock]    Script Date: 26.12.2025 01:49:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   VIEW [dbo].[vw_VariantStock]
AS
SELECT
    p.ProductID,
    p.ProductName,
    pv.VariantID,
    pv.Size,
    pv.Color,
    pv.StockQuantity,
    p.UnitPrice
FROM dbo.ProductVariant pv
INNER JOIN dbo.Product p ON p.ProductID = pv.ProductID
WHERE p.IsActive = 1;
GO


