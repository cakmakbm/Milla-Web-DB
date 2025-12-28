USE [Milla]
GO

/****** Object:  View [dbo].[vw_Returns]    Script Date: 26.12.2025 01:49:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   VIEW [dbo].[vw_Returns]
AS
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
JOIN dbo.Product p ON p.ProductID = pv.ProductID;
GO


