USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_Checkout_CreateOrder]    Script Date: 26.12.2025 01:46:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Checkout_CreateOrder]
    @CustomerID INT,
    @AddressID INT = NULL,
    @OrderID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- AddressID verilmediyse default adresi kullan (varsa)
    IF @AddressID IS NULL
    BEGIN
        SELECT TOP 1 @AddressID = AddressID
        FROM dbo.Address
        WHERE CustomerID = @CustomerID AND IsDefault = 1
        ORDER BY AddressID DESC;
    END

    INSERT INTO dbo.[Order] (CustomerID, AddressID, OrderDate, OrderStatus, Notes, TotalAmount)
    VALUES (@CustomerID, @AddressID, SYSDATETIME(), 'Pending', NULL, 0);

    SET @OrderID = SCOPE_IDENTITY();
END
GO


