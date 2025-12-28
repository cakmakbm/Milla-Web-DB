USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminBrand_Update]    Script Date: 26.12.2025 01:45:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_AdminBrand_Update]
  @BrandID   INT,
  @BrandName NVARCHAR(120),
  @Country   NVARCHAR(100) = NULL
AS
BEGIN
  SET NOCOUNT ON;

  IF NOT EXISTS (SELECT 1 FROM dbo.Brand WHERE BrandID = @BrandID)
  BEGIN
    RAISERROR('BRAND_NOT_FOUND', 16, 1);
    RETURN;
  END

  IF EXISTS (SELECT 1 FROM dbo.Brand WHERE BrandName=@BrandName AND BrandID<>@BrandID)
  BEGIN
    RAISERROR('BRAND_EXISTS', 16, 1);
    RETURN;
  END

  UPDATE dbo.Brand
  SET BrandName=@BrandName, Country=@Country
  WHERE BrandID=@BrandID;
END
GO


