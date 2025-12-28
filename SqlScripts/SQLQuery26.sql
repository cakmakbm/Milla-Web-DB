USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminUpdateVariantStock]    Script Date: 26.12.2025 01:46:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_AdminUpdateVariantStock]
    @VariantID     INT,
    @NewStock      INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.ProductVariant WHERE VariantID = @VariantID)
    BEGIN
        RAISERROR('Invalid VariantID.', 16, 1);
        RETURN;
    END

    IF @NewStock < 0
    BEGIN
        RAISERROR('Stock cannot be negative.', 16, 1);
        RETURN;
    END

    UPDATE dbo.ProductVariant
    SET StockQuantity = @NewStock
    WHERE VariantID = @VariantID;

    SELECT * FROM dbo.ProductVariant WHERE VariantID = @VariantID;
END;
GO


