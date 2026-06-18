SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_GetBaseRecoveryKeysForClaimPeril'
GO

CREATE PROCEDURE spu_SAM_GetBaseRecoveryKeysForClaimPeril

@claim_peril_id int

AS

SELECT recovery.base_recovery_id, recovery.recovery_type_id, recovery_type.code 
FROM recovery 

	
INNER JOIN recovery_type ON 
		recovery_type.recovery_type_id = recovery.recovery_type_id
WHERE claim_peril_id = @claim_peril_id


GO
