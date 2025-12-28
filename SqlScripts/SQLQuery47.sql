USE [Milla]
GO

/****** Object:  View [dbo].[vw_FashionCatalog]    Script Date: 26.12.2025 01:48:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   VIEW [dbo].[vw_FashionCatalog]
AS
SELECT
    p.ProductID,
    p.ProductName,
    c.CategoryName,
    b.BrandName,
    s.StoreName AS SupplierName,
    p.UnitPrice,
    p.ImageUrl,
    SUM(pv.StockQuantity) AS TotalVariantStock,
    COUNT(*) AS VariantCount
FROM dbo.Product p
INNER JOIN dbo.Category c ON c.CategoryID = p.CategoryID
INNER JOIN dbo.Brand b ON b.BrandID = p.BrandID
INNER JOIN dbo.Supplier s ON s.SupplierID = p.SupplierID
INNER JOIN dbo.ProductVariant pv ON pv.ProductID = p.ProductID
WHERE p.IsActive = 1
GROUP BY
    p.ProductID, p.ProductName, c.CategoryName, b.BrandName, s.StoreName, p.UnitPrice, p.ImageUrl;
GO


