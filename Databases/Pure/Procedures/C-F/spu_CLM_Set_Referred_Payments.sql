SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Set_Referred_Payments'
GO





CREATE PROCEDURE spu_CLM_Set_Referred_Payments  
    @claimid int,  
    @status smallint  
AS  
  
UPDATE Claim_Payment  
SET Is_Referred = @status  
WHERE Claim_Id = @claimid  
AND is_referred IS NULL  /* i.e. the status is not already set */  









GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
