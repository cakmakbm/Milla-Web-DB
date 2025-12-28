USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminAddProduct]    Script Date: 26.12.2025 01:44:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_AdminAddProduct]
    @CategoryID INT,
    @BrandID INT,
    @SupplierID INT,
    @ProductName NVARCHAR(200),
    @UnitPrice DECIMAL(12,2),
    @Description NVARCHAR(1000) = NULL,
    @ImageUrl NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Product
        (CategoryID, BrandID, SupplierID, ProductName, UnitPrice, Description, IsActive, ImageUrl, CreatedAt)
    VALUES
        (@CategoryID, @BrandID, @SupplierID, @ProductName, @UnitPrice, @Description, 1, @ImageUrl, SYSUTCDATETIME());

    SELECT SCOPE_IDENTITY() AS NewProductID;
END
GO


