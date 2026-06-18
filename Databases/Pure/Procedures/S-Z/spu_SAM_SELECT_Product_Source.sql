
EXECUTE DDLDropProcedure 'spu_SAM_SELECT_Product_Source'
GO
CREATE PROCEDURE spu_SAM_SELECT_Product_Source 
	@user_id INT
	AS	
	SELECT  P.product_id,Prod.Code ProductCode, prod.description ProductDescription, s.code Sourcecode,
		   s.DESCRIPTION SourceDescription
	FROM   SOURCE s
	JOIN   product_source p
		ON p.source_id = s.source_id
	JOIN Product Prod ON P.product_id =  Prod.product_id
		 WHERE  ( p.source_id NOT IN (SELECT source_id FROM   PMUser_Source  WHERE  user_id = @user_id))
	ORDER  BY p.product_id 