USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminListReturns]    Script Date: 26.12.2025 01:45:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_AdminListReturns]
    @Status NVARCHAR(50) = NULL  -- NULL => hepsi, 'Requested' => sadece bekleyen
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
        o.OrderID,
        o.CustomerID,
        pv.VariantID,
        p.ProductName,
        pv.Color,
        pv.Size
    FROM dbo.[Return] r
    JOIN dbo.OrderItem oi ON oi.OrderItemID = r.OrderItemID
    JOIN dbo.[Order] o ON o.OrderID = oi.OrderID
    JOIN dbo.ProductVariant pv ON pv.VariantID = oi.VariantID
    JOIN dbo.Product p ON p.ProductID = pv.ProductID
    WHERE (@Status IS NULL OR r.ReturnStatus = @Status)
    ORDER BY r.ReturnID DESC;
END
GO


