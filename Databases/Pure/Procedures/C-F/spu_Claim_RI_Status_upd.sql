
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_RI_Status_upd'
GO


CREATE PROCEDURE spu_Claim_RI_Status_upd    
 @ClaimId INT,    
 @status INT    
AS    
    
 UPDATE Claim_RI_Arrangement  
 SET cloned = @status -- Deleted/ Complete    
 WHERE claim_id = @ClaimId    
   
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
