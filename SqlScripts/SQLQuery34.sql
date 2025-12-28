USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_UserAddAddress]    Script Date: 26.12.2025 01:47:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_UserAddAddress]
    @CustomerID INT,
    @AddressLine NVARCHAR(200),
    @City NVARCHAR(100),
    @District NVARCHAR(100) = NULL,
    @ZipCode NVARCHAR(20) = NULL,
    @Country NVARCHAR(100) = N'Türkiye',
    @IsDefault BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRAN;

    IF @IsDefault = 1
        UPDATE dbo.Address SET IsDefault=0 WHERE CustomerID=@CustomerID;

    INSERT INTO dbo.Address(CustomerID, AddressLine, City, District, ZipCode, Country, IsDefault)
    VALUES(@CustomerID, @AddressLine, @City, @District, @ZipCode, @Country, @IsDefault);

    COMMIT;
END
GO


