USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_UserRequestReturn]    Script Date: 26.12.2025 01:48:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_UserRequestReturn]
    @CustomerID INT,
    @OrderItemID INT,
    @Quantity INT,
    @Reason NVARCHAR(300) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Quantity IS NULL OR @Quantity <= 0
    BEGIN
        RAISERROR('Quantity must be > 0', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.OrderItem oi
        JOIN dbo.[Order] o ON o.OrderID = oi.OrderID
        WHERE oi.OrderItemID = @OrderItemID
          AND o.CustomerID = @CustomerID
    )
    BEGIN
        RAISERROR('Order item not found for this customer.', 16, 1);
        RETURN;
    END

    DECLARE @BoughtQty INT;
    SELECT @BoughtQty = oi.Quantity
    FROM dbo.OrderItem oi
    WHERE oi.OrderItemID = @OrderItemID;

    DECLARE @AlreadyReturned INT;
    SELECT @AlreadyReturned = ISNULL(SUM(r.Quantity),0)
    FROM dbo.[Return] r
    WHERE r.OrderItemID = @OrderItemID
      AND r.ReturnStatus IN (N'Requested', N'Approved', N'Refunded');

    IF (@AlreadyReturned + @Quantity) > @BoughtQty
    BEGIN
        RAISERROR('Return quantity exceeds purchased quantity.', 16, 1);
        RETURN;
    END

    INSERT INTO dbo.[Return](OrderItemID, Quantity, ReturnReason, ReturnStatus)
    VALUES(@OrderItemID, @Quantity, @Reason, N'Requested');
END
GO


