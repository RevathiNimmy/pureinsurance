SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_XOL_Count'
GO

CREATE Procedure spu_CLM_Get_XOL_Count 
	@claim_ID int
AS
    SELECT Count(*) FROM Claim_RI_Arrangement_line (nolock)
    WHERE Claim_id IN (SELECT clmb.Claim_id
		FROM claim clm 
		INNER JOIN Transaction_Type tt ON clm.transaction_type_id = tt.transaction_type_id 
		INNER JOIN Claim clmb ON clm.base_claim_id = clmb.base_claim_id 
		INNER JOIN Claim_Payment clp ON clmb.Claim_id = clp.claim_id 
		WHERE clm.Claim_id = @claim_ID AND tt.code = 'C_CP' AND clp.is_referred = 1)  
	AND Type IN ('FX','TX','X') AND (this_payment <> 0 OR this_reserve <> 0)
