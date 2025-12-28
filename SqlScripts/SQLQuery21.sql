USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminSupplier_List]    Script Date: 26.12.2025 01:46:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_AdminSupplier_List]
AS
BEGIN
  SET NOCOUNT ON;
  SELECT SupplierID, StoreName, ContactEmail, ContactPhone, AddressText
  FROM dbo.Supplier
  ORDER BY StoreName;
END
GO


