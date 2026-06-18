SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'Spu_CLM_Get_Recovery_Revised_Amount'
GO

CREATE Procedure Spu_CLM_Get_Recovery_Revised_Amount
	@claim_id int,
	@peril_type_code VARCHAR(30),
	@recovery_type_code VARCHAR(30)
AS

SELECT revised_reserve,received_to_date,Initial_reserve 
FROM 
	recovery r
	INNER JOIN recovery_type rt
		ON r.recovery_type_id = rt.recovery_type_id
	INNER JOIN claim_peril cp 
		ON cp.Claim_Peril_id = r.claim_Peril_id
	INNER JOIN Peril_Type pt
		ON cp.Peril_type_id = pt.peril_type_id
	INNER JOIN claim c 
		ON c.Claim_id = cp.Claim_id
	WHERE
		c.Claim_id = @claim_id
		AND pt.code = @peril_type_code
		AND rt.code = @recovery_type_code

GO