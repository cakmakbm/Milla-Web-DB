USE [Milla]
GO

/****** Object:  View [dbo].[vw_OrderSummary]    Script Date: 26.12.2025 01:49:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   VIEW [dbo].[vw_OrderSummary]
AS
SELECT
    o.OrderID,
    o.OrderDate,
    o.OrderStatus,
    o.CustomerID,
    (c.FirstName + N' ' + c.LastName) AS CustomerFullName,
    SUM(oi.LineTotal) AS OrderTotal,
    COUNT(*) AS ItemCount
FROM dbo.[Order] o
INNER JOIN dbo.Customer c ON c.CustomerID = o.CustomerID
INNER JOIN dbo.OrderItem oi ON oi.OrderID = o.OrderID
GROUP BY
    o.OrderID, o.OrderDate, o.OrderStatus, o.CustomerID, (c.FirstName + N' ' + c.LastName);
GO


