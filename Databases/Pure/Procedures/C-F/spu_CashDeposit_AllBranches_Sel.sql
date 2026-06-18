SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXEC DDLDropProcedure 'spu_CashDeposit_AllBranches_Sel'
GO
 CREATE PROCEDURE spu_CashDeposit_AllBranches_Sel    
 @WhereClause VARCHAR(50)  
AS    
  
	BEGIN
		Select Source_id, description, code, is_deleted from source where is_deleted=0  
		AND effective_date<=getdate()
		AND  description LIKE @WhereClause  
	END
