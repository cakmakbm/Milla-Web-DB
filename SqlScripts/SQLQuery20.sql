USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminSupplier_Delete]    Script Date: 26.12.2025 01:46:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_AdminSupplier_Delete]
  @SupplierID INT
AS
BEGIN
  SET NOCOUNT ON;

  IF EXISTS (SELECT 1 FROM dbo.Product WHERE SupplierID=@SupplierID)
  BEGIN
    RAISERROR('SUPPLIER_IN_USE', 16, 1);
    RETURN;
  END

  DELETE FROM dbo.Supplier WHERE SupplierID=@SupplierID;
END
GO


