SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_CashDeposit_AllProducts_Sel'
GO
 CREATE PROCEDURE spu_CashDeposit_AllProducts_Sel  
 @WhereClause VARCHAR(50)    
AS    
  
	BEGIN
		SELECT    
			product_id,            
			description,
			code,
			is_deleted 
		FROM Product  
		WHERE is_deleted=0
		AND  description LIKE @WhereClause  
		ORDER BY description ASC   
	END
