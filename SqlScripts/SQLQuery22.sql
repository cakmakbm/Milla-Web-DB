USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminSupplier_Update]    Script Date: 26.12.2025 01:46:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_AdminSupplier_Update]
  @SupplierID   INT,
  @StoreName    NVARCHAR(150),
  @ContactEmail NVARCHAR(255)=NULL,
  @ContactPhone NVARCHAR(20)=NULL,
  @AddressText  NVARCHAR(250)=NULL
AS
BEGIN
  SET NOCOUNT ON;

  IF NOT EXISTS (SELECT 1 FROM dbo.Supplier WHERE SupplierID=@SupplierID)
  BEGIN
    RAISERROR('SUPPLIER_NOT_FOUND', 16, 1);
    RETURN;
  END

  IF EXISTS (SELECT 1 FROM dbo.Supplier WHERE StoreName=@StoreName AND SupplierID<>@SupplierID)
  BEGIN
    RAISERROR('SUPPLIER_EXISTS', 16, 1);
    RETURN;
  END

  UPDATE dbo.Supplier
  SET StoreName=@StoreName,
      ContactEmail=@ContactEmail,
      ContactPhone=@ContactPhone,
      AddressText=@AddressText
  WHERE SupplierID=@SupplierID;
END
GO


