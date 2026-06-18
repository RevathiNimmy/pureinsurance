SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'Spu_CLM_Get_Reserve_Revised_Amount'
GO

CREATE Procedure Spu_CLM_Get_Reserve_Revised_Amount
	@claim_id int,
	@peril_type_code VARCHAR(30),
	@reserve_type_code VARCHAR(30)
AS

SELECT Initial_reserve+ISNULL(Revised_reserve,0)-ISNULL(Paid_to_date,0) Revised_reserve, paid_to_date
FROM 
	Reserve r
	INNER JOIN Reserve_type rt
		ON r.Reserve_type_id = rt.Reserve_type_id
	INNER JOIN claim_peril cp 
		ON cp.Claim_Peril_id = r.claim_Peril_id
	INNER JOIN Peril_Type pt
		ON cp.Peril_type_id = pt.peril_type_id
	INNER JOIN claim c 
		ON c.Claim_id = cp.Claim_id
	WHERE
		c.Claim_id = @claim_id
		AND pt.code = @peril_type_code
		AND rt.name = @reserve_type_code

GO