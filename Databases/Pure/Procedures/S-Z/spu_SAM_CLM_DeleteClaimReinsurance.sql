SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_DeleteClaimReinsurance'
GO

CREATE PROCEDURE spu_SAM_CLM_DeleteClaimReinsurance

@claim_id int

AS 

BEGIN

DELETE FROM claim_ri_arrangement_line where claim_id = @claim_id 

DELETE FROM claim_ri_arrangement where claim_id = @claim_id

END



GO
