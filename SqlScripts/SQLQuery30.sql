USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_Checkout_UpdateOrderTotal]    Script Date: 26.12.2025 01:47:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Checkout_UpdateOrderTotal]
    @OrderID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Total DECIMAL(18,2);

    SELECT @Total = ISNULL(SUM(LineTotal), 0)
    FROM dbo.OrderItem
    WHERE OrderID = @OrderID;

    UPDATE dbo.[Order]
    SET TotalAmount = @Total
    WHERE OrderID = @OrderID;

    -- Ýstersen debug için döndürür:
    SELECT @Total AS TotalAmount;
END
GO


