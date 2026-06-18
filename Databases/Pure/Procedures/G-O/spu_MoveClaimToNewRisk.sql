SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_MoveClaimToNewRisk'
GO

CREATE PROCEDURE spu_MoveClaimToNewRisk    
    @InsuranceFileCnt int,    
    @RiskCnt int,    
    @NewInsuranceFileCnt int,    
    @NewRiskCnt int,  
	@ClaimCnt int = NULL    
AS    
    IF @ClaimCnt IS NOT NULL
	UPDATE  Claim
	SET     policy_id = @NewInsuranceFileCnt,
	risk_type_id = @NewRiskCnt
	WHERE   claim_id = @ClaimCnt
ELSE
	UPDATE  Claim
	SET     policy_id = @NewInsuranceFileCnt,
	risk_type_id = @NewRiskCnt
	WHERE   policy_id = @InsuranceFileCnt
	AND     risk_type_id = @RiskCnt
	-- Only move risks within the new policy period
	--AND     loss_from_date >= ( SELECT  cover_start_date
	--	 FROM    insurance_file
	--	 --WHERE   insurance_file_cnt = @NewInsuranceFileCnt )
	--	 WHERE   insurance_file_cnt = @InsuranceFileCnt )

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
