
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_DT_Delete_Risk_RI_Arrangements'
GO

/*******************************************************************************************************/  
/* spu_SAM_DT_Delete_Risk_RI_Arrangements                                                           		*/  
/*                                                                                         	        */  
/* Delete the Risk RI Arrangement Details 		 							*/  
/*******************************************************************************************************/  

CREATE PROCEDURE spu_SAM_DT_Delete_Risk_RI_Arrangements

@risk_cnt int

AS

DELETE 
FROM 
	RI_Arrangement 
WHERE 
	risk_cnt = @risk_cnt
 
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

