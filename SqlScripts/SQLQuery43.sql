USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_UserListAddresses]    Script Date: 26.12.2025 01:48:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_UserListAddresses]
    @CustomerID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT AddressID, AddressLine, City, District, ZipCode, Country, IsDefault, CreatedAt
    FROM dbo.Address
    WHERE CustomerID=@CustomerID
    ORDER BY IsDefault DESC, AddressID DESC;
END
GO


