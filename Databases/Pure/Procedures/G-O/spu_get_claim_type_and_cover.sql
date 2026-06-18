SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_claim_type_and_cover'
GO

CREATE PROCEDURE spu_get_claim_type_and_cover
    @risk_type_id INT
AS  
	SELECT 
	    ctb.code,
	    ccb.code,
		rt.Attach_Claim_Outside_Of_Policy_Period
	FROM risk r JOIN
	risk_type rt on r.risk_type_id=rt.risk_type_id
	JOIN claims_type_basis ctb
	   ON rt.claims_type_basis_id= ctb.claims_type_basis_id
	JOIN claims_cover_basis ccb
	   ON rt.claims_cover_basis_id = ccb.claims_cover_basis_id
	WHERE r.risk_cnt = @risk_type_id  
GO