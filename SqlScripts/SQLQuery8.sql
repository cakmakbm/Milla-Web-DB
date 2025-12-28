USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminDeactivateProduct]    Script Date: 26.12.2025 01:45:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE   PROCEDURE [dbo].[sp_AdminDeactivateProduct]
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Product
    SET IsActive = 0
    WHERE ProductID = @ProductID;
END
GO


