USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminUpdateReturnStatus]    Script Date: 26.12.2025 01:46:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_AdminUpdateReturnStatus]
    @ReturnID INT,
    @NewStatus NVARCHAR(50)   -- Approved / Rejected / Refunded
AS
BEGIN
    SET NOCOUNT ON;

    IF @NewStatus NOT IN (N'Approved', N'Rejected', N'Refunded')
    BEGIN
        RAISERROR('Invalid status.', 16, 1);
        RETURN;
    END

    DECLARE @OldStatus NVARCHAR(50);
    DECLARE @OrderItemID INT;
    DECLARE @Qty INT;
    DECLARE @VariantID INT;

    SELECT
        @OldStatus = ReturnStatus,
        @OrderItemID = OrderItemID,
        @Qty = Quantity
    FROM dbo.[Return]
    WHERE ReturnID = @ReturnID;

    IF @OldStatus IS NULL
    BEGIN
        RAISERROR('Return not found.', 16, 1);
        RETURN;
    END

    IF @OldStatus = N'Refunded'
    BEGIN
        RAISERROR('Already refunded.', 16, 1);
        RETURN;
    END

    BEGIN TRAN;

    UPDATE dbo.[Return]
    SET ReturnStatus = @NewStatus
    WHERE ReturnID = @ReturnID;

    IF @NewStatus = N'Refunded'
    BEGIN
        SELECT @VariantID = oi.VariantID
        FROM dbo.OrderItem oi
        WHERE oi.OrderItemID = @OrderItemID;

        UPDATE dbo.ProductVariant
        SET StockQuantity = StockQuantity + @Qty
        WHERE VariantID = @VariantID;
    END

    COMMIT;
END
GO


