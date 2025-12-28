USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminUpdateProductPriceAndInfo]    Script Date: 26.12.2025 01:46:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_AdminUpdateProductPriceAndInfo]
    @ProductID    INT,
    @ProductName  NVARCHAR(200) = NULL,
    @UnitPrice    DECIMAL(12,2) = NULL,
    @Description  NVARCHAR(1000) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Product WHERE ProductID = @ProductID)
    BEGIN
        RAISERROR('Invalid ProductID.', 16, 1);
        RETURN;
    END

    UPDATE dbo.Product
    SET
        ProductName = COALESCE(@ProductName, ProductName),
        UnitPrice   = COALESCE(@UnitPrice, UnitPrice),
        Description = COALESCE(@Description, Description)
    WHERE ProductID = @ProductID;

    SELECT * FROM dbo.Product WHERE ProductID = @ProductID;
END;
GO


