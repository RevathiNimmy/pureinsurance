SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Add_Claim_Peril_DT_Link'
GO

CREATE PROCEDURE spu_SAM_CLM_Add_Claim_Peril_DT_Link  
  
@siriusBaseClaimPerilKey int,  
@samstagingClaimPerilKey int,  
@keytype int,
@versionid int 
  
AS  
  
BEGIN    

DECLARE @claim_peril_id int

SELECT @claim_peril_id = claim_peril_id
FROM claim_peril 
WHERE base_claim_peril_id = @siriusBaseClaimPerilKey 
AND version_id = @versionid

EXEC spu_SAM_DT_ADDDTLINK @claim_peril_id, @samstagingclaimperilkey, @keytype

END  


GO
