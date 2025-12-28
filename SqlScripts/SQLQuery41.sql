USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_UserDeleteAddress]    Script Date: 26.12.2025 01:48:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_UserDeleteAddress]
    @CustomerID INT,
    @AddressID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Address
    WHERE AddressID=@AddressID AND CustomerID=@CustomerID;
END
GO


