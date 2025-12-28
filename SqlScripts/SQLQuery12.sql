USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminGetOrderItems]    Script Date: 26.12.2025 01:45:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_AdminGetOrderItems]
    @OrderID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        oi.OrderItemID,
        p.ProductID,
        p.ProductName,
        pv.VariantID,
        pv.Color,
        pv.Size,
        pv.SKU,
        oi.Quantity,
        oi.UnitPriceAtOrder,
        oi.LineTotal
    FROM dbo.OrderItem oi
    JOIN dbo.ProductVariant pv ON pv.VariantID = oi.VariantID
    JOIN dbo.Product p ON p.ProductID = pv.ProductID
    WHERE oi.OrderID = @OrderID
    ORDER BY oi.OrderItemID;
END
GO


