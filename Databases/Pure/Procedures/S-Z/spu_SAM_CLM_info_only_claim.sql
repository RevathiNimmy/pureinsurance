SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_info_only_claim'
GO


CREATE PROCEDURE spu_SAM_CLM_info_only_claim  
 @Base_claim_id INT,  
 @Info_only BIT = NULL OUTPUT  
AS  
  
SELECT @Info_only = Info_only  
FROM Claim  
WHERE claim_id in (SELECT MAX(claim_id) FROM Claim WHERE base_claim_id =@base_claim_id)





GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
