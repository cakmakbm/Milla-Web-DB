USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_UserCreateReturn]    Script Date: 26.12.2025 01:48:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   PROCEDURE [dbo].[sp_UserCreateReturn]
    @OrderItemID   INT,
    @Quantity      INT,
    @ReturnReason  NVARCHAR(300) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Quantity <= 0
    BEGIN
        RAISERROR('Return quantity must be > 0.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.OrderItem WHERE OrderItemID = @OrderItemID)
    BEGIN
        RAISERROR('Invalid OrderItemID.', 16, 1);
        RETURN;
    END

    BEGIN TRY
        BEGIN TRAN;

        DECLARE @BoughtQty INT, @VariantID INT;
        SELECT @BoughtQty = Quantity, @VariantID = VariantID
        FROM dbo.OrderItem WITH (UPDLOCK, ROWLOCK)
        WHERE OrderItemID = @OrderItemID;

        -- Daha önce iade edilen miktarý düþ
        DECLARE @AlreadyReturned INT =
            (SELECT COALESCE(SUM(Quantity), 0) FROM dbo.[Return] WHERE OrderItemID = @OrderItemID);

        IF (@AlreadyReturned + @Quantity) > @BoughtQty
        BEGIN
            RAISERROR('Return quantity exceeds purchased quantity.', 16, 1);
            ROLLBACK TRAN;
            RETURN;
        END

        INSERT INTO dbo.[Return] (OrderItemID, ReturnReason, Quantity, ReturnStatus)
        VALUES (@OrderItemID, @ReturnReason, @Quantity, N'Requested');

        -- stok iade et (basit demo: anýnda iade ekliyoruz)
        UPDATE dbo.ProductVariant
        SET StockQuantity = StockQuantity + @Quantity
        WHERE VariantID = @VariantID;

        COMMIT TRAN;

        SELECT TOP 1 * FROM dbo.[Return] WHERE OrderItemID = @OrderItemID ORDER BY ReturnID DESC;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        THROW;
    END CATCH
END;
GO


