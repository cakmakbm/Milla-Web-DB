USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminBrand_List]    Script Date: 26.12.2025 01:44:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_AdminBrand_List]
AS
BEGIN
  SET NOCOUNT ON;
  SELECT BrandID, BrandName, Country
  FROM dbo.Brand
  ORDER BY BrandName;
END
GO


