SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'Spu_Sir_insurance_file_SelectRenewalProduct'
GO

Create Procedure Spu_Sir_insurance_file_SelectRenewalProduct
@ifileCnt INT
AS
BEGIN

	SELECT code,Product.Product_id
	FROM product
	INNER JOIN insurance_file ON insurance_file.Renewal_product_id=product.product_id 
	WHERE Insurance_file_cnt = @ifileCnt

END
GO