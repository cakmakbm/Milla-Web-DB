USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminAddVariant]    Script Date: 26.12.2025 01:44:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_AdminAddVariant]
    @ProductID INT,
    @Color NVARCHAR(60),
    @Size NVARCHAR(20),
    @StockQuantity INT,
    @Sku NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.ProductVariant
        (ProductID, Color, Size, StockQuantity, Sku)
    VALUES
        (@ProductID, @Color, @Size, @StockQuantity, @Sku);

    SELECT SCOPE_IDENTITY() AS NewVariantID;
END
GO


