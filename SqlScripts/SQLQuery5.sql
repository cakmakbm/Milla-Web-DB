USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminBrand_Delete]    Script Date: 26.12.2025 01:44:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_AdminBrand_Delete]
  @BrandID INT
AS
BEGIN
  SET NOCOUNT ON;

  -- Brand'a baðlý ürün varsa silmeyi engelle (FK zaten engeller, ama mesaj için)
  IF EXISTS (SELECT 1 FROM dbo.Product WHERE BrandID=@BrandID)
  BEGIN
    RAISERROR('BRAND_IN_USE', 16, 1);
    RETURN;
  END

  DELETE FROM dbo.Brand WHERE BrandID=@BrandID;
END
GO


