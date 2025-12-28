USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminListProducts]    Script Date: 26.12.2025 01:45:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_AdminListProducts]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.ProductID,
        p.ProductName,
        c.CategoryName,
        b.BrandName,
        s.StoreName AS SupplierName,
        p.UnitPrice,
        p.IsActive,
        p.ImageUrl
    FROM dbo.Product p
    INNER JOIN dbo.Category c ON c.CategoryID = p.CategoryID
    INNER JOIN dbo.Brand b ON b.BrandID = p.BrandID
    INNER JOIN dbo.Supplier s ON s.SupplierID = p.SupplierID
    ORDER BY p.ProductID DESC;
END
GO


