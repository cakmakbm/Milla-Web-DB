USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminGetOrderHeader]    Script Date: 26.12.2025 01:45:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_AdminGetOrderHeader]
    @OrderID INT
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
    WHERE o.OrderID = @OrderID;
END
GO


