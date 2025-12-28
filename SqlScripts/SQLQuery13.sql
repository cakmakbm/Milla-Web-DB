USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminListOrders]    Script Date: 26.12.2025 01:45:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_AdminListOrders]
    @CustomerID INT = NULL,
    @Status NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        o.OrderID,
        o.OrderDate,
        o.OrderStatus,
        ISNULL(o.TotalAmount, 0) AS TotalAmount,
        o.CustomerID,
        c.FirstName,
        c.LastName,
        c.Email
    FROM dbo.[Order] o
    JOIN dbo.Customer c ON c.CustomerID = o.CustomerID
    WHERE (@CustomerID IS NULL OR o.CustomerID = @CustomerID)
      AND (@Status IS NULL OR o.OrderStatus = @Status)
    ORDER BY o.OrderID DESC;
END
GO


