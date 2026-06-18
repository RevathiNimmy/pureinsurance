SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_clm_Process_Authorise'
GO

CREATE PROCEDURE spu_clm_Process_Authorise  
 @claim_id int  
AS  
  
 /* Update the payment entries as processed */  
  UPDATE claim_payment  
  Set Is_referred = 0  
  WHERE Claim_Id = @Claim_Id  
  AND   Is_Referred = 1  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
