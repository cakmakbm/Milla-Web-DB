USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_UserCreateOrder]    Script Date: 26.12.2025 01:47:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_UserCreateOrder]
    @CustomerID INT,
    @AddressID  INT,
    @Notes      NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Customer WHERE CustomerID = @CustomerID)
    BEGIN
        RAISERROR('Invalid CustomerID.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.Address WHERE AddressID = @AddressID AND CustomerID = @CustomerID)
    BEGIN
        RAISERROR('Address does not belong to the customer.', 16, 1);
        RETURN;
    END

    INSERT INTO dbo.[Order] (CustomerID, AddressID, OrderStatus, Notes)
    VALUES (@CustomerID, @AddressID, N'Pending', @Notes);

    SELECT SCOPE_IDENTITY() AS NewOrderID;
END;
GO


