USE [Milla]
GO

/****** Object:  StoredProcedure [dbo].[sp_AdminDeleteProduct]    Script Date: 26.12.2025 01:45:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_AdminDeleteProduct]
  @ProductID INT
AS
BEGIN
  SET NOCOUNT ON;

  -- Sipariþlerde kullanýlmýþ mý?
  IF EXISTS (
      SELECT 1
      FROM dbo.OrderItem oi
      JOIN dbo.ProductVariant pv ON pv.VariantID = oi.VariantID
      WHERE pv.ProductID = @ProductID
  )
  BEGIN
      RAISERROR('PRODUCT_IN_USE_ORDER', 16, 1);
      RETURN;
  END

  -- Review var mý?
  IF EXISTS (SELECT 1 FROM dbo.Review WHERE ProductID=@ProductID)
  BEGIN
      RAISERROR('PRODUCT_IN_USE_REVIEW', 16, 1);
      RETURN;
  END

  -- Question var mý?
  IF EXISTS (SELECT 1 FROM dbo.Question WHERE ProductID=@ProductID)
  BEGIN
      RAISERROR('PRODUCT_IN_USE_QNA', 16, 1);
      RETURN;
  END

  -- Önce child tablolarý sil (FK varsa)
  DELETE FROM dbo.Answer
  WHERE QuestionID IN (SELECT QuestionID FROM dbo.Question WHERE ProductID=@ProductID);

  DELETE FROM dbo.Question WHERE ProductID=@ProductID;
  DELETE FROM dbo.Review   WHERE ProductID=@ProductID;

  DELETE FROM dbo.ProductVariant WHERE ProductID=@ProductID;

  -- En son Product
  DELETE FROM dbo.Product WHERE ProductID=@ProductID;
END
GO


