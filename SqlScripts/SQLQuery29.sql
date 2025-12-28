USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_Checkout_FromCartItem]    Script Date: 26.12.2025 01:47:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_Checkout_FromCartItem]
    @CustomerID INT,
    @AddressID INT = NULL,
    @VariantID INT,
    @Quantity INT,
    @OrderID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    EXEC dbo.sp_Checkout_CreateOrder
         @CustomerID=@CustomerID,
         @AddressID=@AddressID,
         @Notes=NULL,
         @OrderID=@OrderID OUTPUT;

    EXEC dbo.sp_Checkout_AddOrderItem
         @OrderID=@OrderID,
         @VariantID=@VariantID,
         @Quantity=@Quantity;

    EXEC dbo.sp_Checkout_UpdateOrderTotal
         @OrderID=@OrderID;
END
GO


