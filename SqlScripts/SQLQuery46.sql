USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_UserUpdateAddress]    Script Date: 26.12.2025 01:48:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_UserUpdateAddress]
    @CustomerID INT,
    @AddressID INT,
    @AddressLine NVARCHAR(200),
    @City NVARCHAR(100),
    @District NVARCHAR(100) = NULL,
    @ZipCode NVARCHAR(20) = NULL,
    @Country NVARCHAR(100) = N'Türkiye',
    @IsDefault BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Address WHERE AddressID=@AddressID AND CustomerID=@CustomerID)
    BEGIN
        RAISERROR('Address not found for this customer.', 16, 1);
        RETURN;
    END

    BEGIN TRAN;

    IF @IsDefault = 1
    BEGIN
        UPDATE dbo.Address
        SET IsDefault = 0
        WHERE CustomerID = @CustomerID;
    END

    UPDATE dbo.Address
    SET AddressLine=@AddressLine,
        City=@City,
        District=@District,
        ZipCode=@ZipCode,
        Country=@Country,
        IsDefault=@IsDefault
    WHERE AddressID=@AddressID AND CustomerID=@CustomerID;

    COMMIT;
END
GO


