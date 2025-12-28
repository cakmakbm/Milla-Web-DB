USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminSupplier_Add]    Script Date: 26.12.2025 01:46:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_AdminSupplier_Add]
  @StoreName    NVARCHAR(150),
  @ContactEmail NVARCHAR(255)=NULL,
  @ContactPhone NVARCHAR(20)=NULL,
  @AddressText  NVARCHAR(250)=NULL
AS
BEGIN
  SET NOCOUNT ON;

  IF EXISTS (SELECT 1 FROM dbo.Supplier WHERE StoreName=@StoreName)
  BEGIN
    RAISERROR('SUPPLIER_EXISTS', 16, 1);
    RETURN;
  END

  INSERT INTO dbo.Supplier(StoreName, ContactEmail, ContactPhone, AddressText)
  VALUES(@StoreName, @ContactEmail, @ContactPhone, @AddressText);
END
GO


