USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_UserAddOrderItemByVariant]    Script Date: 26.12.2025 01:47:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_UserAddOrderItemByVariant]
    @OrderID   INT,
    @VariantID INT,
    @Quantity  INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Quantity <= 0
    BEGIN
        RAISERROR('Quantity must be > 0.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.[Order] WHERE OrderID = @OrderID)
    BEGIN
        RAISERROR('Invalid OrderID.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.ProductVariant pv
        INNER JOIN dbo.Product p ON p.ProductID = pv.ProductID
        WHERE pv.VariantID = @VariantID AND p.IsActive = 1
    )
    BEGIN
        RAISERROR('Invalid VariantID or product is inactive.', 16, 1);
        RETURN;
    END

    BEGIN TRY
        BEGIN TRAN;

        -- Stok kontrol + düþür (UPDLOCK ile yarýþ durumunu azaltýr)
        DECLARE @Stock INT;
        SELECT @Stock = pv.StockQuantity
        FROM dbo.ProductVariant pv WITH (UPDLOCK, ROWLOCK)
        WHERE pv.VariantID = @VariantID;

        IF @Stock < @Quantity
        BEGIN
            RAISERROR('Insufficient variant stock.', 16, 1);
            ROLLBACK TRAN;
            RETURN;
        END

        UPDATE dbo.ProductVariant
        SET StockQuantity = StockQuantity - @Quantity
        WHERE VariantID = @VariantID;

        -- fiyatý producttan al
        DECLARE @Price DECIMAL(12,2);
        SELECT @Price = p.UnitPrice
        FROM dbo.ProductVariant pv
        INNER JOIN dbo.Product p ON p.ProductID = pv.ProductID
        WHERE pv.VariantID = @VariantID;

        INSERT INTO dbo.OrderItem (OrderID, VariantID, Quantity, UnitPriceAtOrder)
        VALUES (@OrderID, @VariantID, @Quantity, @Price);

        COMMIT TRAN;

        SELECT * FROM dbo.OrderItem WHERE OrderID = @OrderID AND VariantID = @VariantID;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END;
GO


