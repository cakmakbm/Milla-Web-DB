USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_RegisterCustomer]    Script Date: 26.12.2025 01:47:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_RegisterCustomer]
    @Email        nvarchar(255),
    @PasswordHash nvarchar(255),
    @FirstName    nvarchar(100),
    @LastName     nvarchar(100)
AS
BEGIN
    SET NOCOUNT ON;

    -- Email unique kontrolü (AppUser üzerinden)
    IF EXISTS (SELECT 1 FROM dbo.AppUser WHERE Email = @Email)
    BEGIN
        RAISERROR('EMAIL_EXISTS', 16, 1);
        RETURN;
    END

    DECLARE @CustomerID int;

    INSERT INTO dbo.Customer(FirstName, LastName, Email)
    VALUES (@FirstName, @LastName, @Email);

    SET @CustomerID = SCOPE_IDENTITY();

    INSERT INTO dbo.AppUser(Email, PasswordHash, Role, CustomerID)
    VALUES (@Email, @PasswordHash, 'Customer', @CustomerID);

    SELECT @CustomerID AS CustomerID;
END
GO


