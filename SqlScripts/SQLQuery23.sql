USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminUpdateOrderStatus]    Script Date: 26.12.2025 01:46:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_AdminUpdateOrderStatus]
    @OrderID INT,
    @NewStatus NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.[Order] WHERE OrderID=@OrderID)
    BEGIN
        RAISERROR('Order not found',16,1);
        RETURN;
    END

    IF @NewStatus NOT IN (N'Pending', N'Processing', N'Shipped', N'Delivered', N'Cancelled')
    BEGIN
        RAISERROR('Invalid status',16,1);
        RETURN;
    END

    UPDATE dbo.[Order]
    SET OrderStatus = @NewStatus
    WHERE OrderID = @OrderID;
END
GO


