USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_UserListReturnsByOrder]    Script Date: 26.12.2025 01:48:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_UserListReturnsByOrder]
    @CustomerID INT,
    @OrderID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        r.ReturnID,
        r.ReturnDate,
        r.ReturnStatus,
        r.Quantity AS ReturnQty,
        r.ReturnReason,
        oi.OrderItemID,
        p.ProductName,
        pv.Color,
        pv.Size
    FROM dbo.[Return] r
    JOIN dbo.OrderItem oi ON oi.OrderItemID = r.OrderItemID
    JOIN dbo.[Order] o ON o.OrderID = oi.OrderID
    JOIN dbo.ProductVariant pv ON pv.VariantID = oi.VariantID
    JOIN dbo.Product p ON p.ProductID = pv.ProductID
    WHERE o.CustomerID = @CustomerID
      AND o.OrderID = @OrderID
    ORDER BY r.ReturnID DESC;
END
GO


