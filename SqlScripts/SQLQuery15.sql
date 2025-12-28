USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminListProductsForDropdown]    Script Date: 26.12.2025 01:45:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_AdminListProductsForDropdown]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ProductID, ProductName
    FROM dbo.Product
    WHERE IsActive = 1
    ORDER BY ProductName;
END
GO


