USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_UserCreatePayment]    Script Date: 26.12.2025 01:48:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_UserCreatePayment]
    @OrderID INT,
    @PaymentMethod NVARCHAR(50),
    @Amount DECIMAL(12,2)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Payment
        (OrderID, PaymentMethod, PaymentDate, Amount)
    VALUES
        (@OrderID, @PaymentMethod, GETDATE(), @Amount);
END
GO


