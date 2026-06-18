SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_risk_id'
GO

  /*******************************************************************************************************/  
/* spu_SAM_Get_risk_id                                                                              */  
/*                                                                                        */  
/* Get the Risk Id */  
/*******************************************************************************************************/  


CREATE PROCEDURE spu_SAM_Get_risk_id    
@Base_Claim_id INT,    
@Version_id INT,  
@Risk_id INT = NULL OUTPUT  
  
AS    
    
SELECT @Risk_id = Risk_type_id  
FROM   Claim  
WHERE  Base_claim_id = @Base_claim_id    
AND    Version_id = @Version_id     
      
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
