USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminListVariantsByProduct]    Script Date: 26.12.2025 01:45:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_AdminListVariantsByProduct]
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;

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
    WHERE pv.ProductID = @ProductID
    ORDER BY pv.VariantID DESC;
END
GO


