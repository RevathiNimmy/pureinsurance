SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_SAM_Claim_Check_BaseRecoverykey'
GO

CREATE PROCEDURE spu_SAM_Claim_Check_BaseRecoverykey
     @BaseRecoveryKey int,
@baseClaimPerilKey int, 
@count int OUTPUT
AS
SELECT @count=count(*) FROM Recovery R
JOIN Claim_Peril CP
ON CP.Claim_peril_id=R.Claim_peril_id

WHERE base_recovery_id = @BaseRecoveryKey and CP.Base_claim_Peril_id=@baseClaimPerilKey

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 

GO