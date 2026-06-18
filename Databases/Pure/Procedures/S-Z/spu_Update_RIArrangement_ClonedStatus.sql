SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Update_RIArrangement_ClonedStatus'
GO

CREATE PROCEDURE spu_Update_RIArrangement_ClonedStatus  
(  
 @risk_cnt INT,  
 @cloned INT,
 @insurance_file_cnt INT = NULL  
)  
AS  

IF ISNULL(@insurance_file_cnt,0)=0
	UPDATE RI_Arrangement SET Cloned = @cloned  
	WHERE risk_cnt = @risk_cnt  and Cloned=1
ELSE
	UPDATE RI_Arrangement SET Cloned = @cloned  
	WHERE risk_cnt in (SELECT risk_cnt from insurance_file_risk_link where insurance_file_cnt=@insurance_file_cnt) 
	and Cloned=1


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
