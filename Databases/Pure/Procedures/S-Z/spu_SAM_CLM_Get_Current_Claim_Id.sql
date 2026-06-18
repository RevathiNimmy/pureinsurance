SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_Current_Claim_Id'
GO

  /*******************************************************************************************************/  
/* spu_CLM_Get_Id_FromBaseID                                                                              */  
/*                                                                                        */  
/* Get the Current Version of Claim Id */  
/*******************************************************************************************************/  

CREATE  PROCEDURE spu_SAM_CLM_Get_Current_Claim_Id 
@BaseClaim_id int  
  
AS  
  
SELECT  TOP 1 Claim_id  
FROM Claim  
WHERE base_claim_id = @baseClaim_id  
AND is_dirty=0
Order by Claim_id  
DESC  
  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
