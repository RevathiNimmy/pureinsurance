SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_ProductDocument_option'
GO
CREATE PROC spu_SAM_Get_ProductDocument_option
@product_id INT
AS
SELECT produce_certificate,produce_schedule,produce_debit_note 
FROM product
WHERE product_id=@product_id

