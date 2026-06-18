SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SAM_CLM_Check_ClaimPerilKey'
GO

CREATE PROCEDURE spu_SAM_CLM_Check_ClaimPerilKey
    	@Claim_cnt INT,
	@Claim_Peril_cnt INT
AS

-- ******************************************************************************************************
-- Stored Procedure spu_Check_ClaimPerilKey
-- ******************************************************************************************************
-- Revision             Description of Modification                                     Date        Who 
-- --------             ---------------------------                                     ----        ---
-- 1.0                  RKS 28082006 - Created                                
-- ******************************************************************************************************

BEGIN
    SELECT count(*) FROM Claim_Peril
    	WHERE Claim_id = @Claim_cnt AND Claim_Peril_id = @Claim_Peril_cnt
    
END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
