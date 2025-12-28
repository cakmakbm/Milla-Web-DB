USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminDeleteVariant]    Script Date: 26.12.2025 01:45:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_AdminDeleteVariant]
    @VariantID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.ProductVariant
    WHERE VariantID = @VariantID;
END
GO


