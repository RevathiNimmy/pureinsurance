SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Product_Causation_Del'
GO


CREATE PROCEDURE spu_Product_Causation_Del
    @product_id int
AS


DELETE FROM Product_Allowed_Causation
WHERE product_id = @product_id
GO


