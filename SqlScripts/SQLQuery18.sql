USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminSetProductActive]    Script Date: 26.12.2025 01:46:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_AdminSetProductActive]
  @ProductID INT,
  @IsActive BIT
AS
BEGIN
  SET NOCOUNT ON;

  UPDATE dbo.Product
  SET IsActive = @IsActive
  WHERE ProductID = @ProductID;
END
GO


