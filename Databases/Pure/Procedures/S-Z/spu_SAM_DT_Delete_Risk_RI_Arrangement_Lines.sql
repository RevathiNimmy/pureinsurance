
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_DT_Delete_Risk_RI_Arrangement_Lines'
GO

/*******************************************************************************************************/  
/* spu_SAM_DT_Delete_Risk_RI_Arrangement_Lines                                                           		*/  
/*                                                                                         	        */  
/* Delete the Risk RI Arrangement Line Details 		 							*/  
/*******************************************************************************************************/  

CREATE PROCEDURE spu_SAM_DT_Delete_Risk_RI_Arrangement_Lines

@risk_cnt int

AS

DELETE RI_Arrangement_Line
FROM 
	RI_Arrangement_Line 
INNER JOIN RI_Arrangement 
ON 
	RI_Arrangement_Line.ri_arrangement_id = RI_Arrangement.ri_arrangement_id
WHERE 
	RI_Arrangement.risk_cnt = @risk_cnt
 
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

