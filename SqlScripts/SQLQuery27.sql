USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_Checkout_AddOrderItem]    Script Date: 26.12.2025 01:46:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Checkout_AddOrderItem]
    @OrderID INT,
    @VariantID INT,
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Quantity IS NULL OR @Quantity <= 0
    BEGIN
        RAISERROR('Quantity must be > 0', 16, 1);
        RETURN;
    END

    DECLARE @UnitPrice DECIMAL(12,2);
    DECLARE @CurrentStock INT;

    -- Stok satýrýný kilitle (race condition olmasýn)
    SELECT
        @CurrentStock = pv.StockQuantity,
        @UnitPrice = p.UnitPrice
    FROM dbo.ProductVariant pv WITH (UPDLOCK, ROWLOCK)
    JOIN dbo.Product p ON p.ProductID = pv.ProductID
    WHERE pv.VariantID = @VariantID
      AND p.IsActive = 1;

    IF @CurrentStock IS NULL
    BEGIN
        RAISERROR('Invalid VariantID or product inactive.', 16, 1);
        RETURN;
    END

    IF @CurrentStock < @Quantity
    BEGIN
        RAISERROR('Insufficient stock.', 16, 1);
        RETURN;
    END

    -- Stok düþ
    UPDATE dbo.ProductVariant
    SET StockQuantity = StockQuantity - @Quantity
    WHERE VariantID = @VariantID;

    -- OrderItem ekle
    INSERT INTO dbo.OrderItem(OrderID, VariantID, Quantity, UnitPriceAtOrder)
    VALUES(@OrderID, @VariantID, @Quantity, @UnitPrice);
END
GO


