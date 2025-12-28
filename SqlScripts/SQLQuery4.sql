USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminBrand_Add]    Script Date: 26.12.2025 01:44:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_AdminBrand_Add]
  @BrandName NVARCHAR(120),
  @Country   NVARCHAR(100) = NULL
AS
BEGIN
  SET NOCOUNT ON;

  IF EXISTS (SELECT 1 FROM dbo.Brand WHERE BrandName = @BrandName)
  BEGIN
    RAISERROR('BRAND_EXISTS', 16, 1);
    RETURN;
  END

  INSERT INTO dbo.Brand(BrandName, Country)
  VALUES(@BrandName, @Country);
END
GO


