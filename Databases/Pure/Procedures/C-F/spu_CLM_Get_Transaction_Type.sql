SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_transaction_type'
GO

CREATE Procedure spu_CLM_Get_transaction_type   

    @claim_ID int = 0,
	@Peril_ID int = 0
AS


If @Claim_ID <> 0 BEGIN
SELECT tt.code FROM Claim c

INNER JOIN transaction_type tt

ON tt.transaction_type_id = c.transaction_type_id

WHERE claim_id = @claim_id
END
ELSE IF @Peril_ID <> 0 BEGIN
SELECT tt.code FROM Claim c

INNER JOIN transaction_type tt

ON tt.transaction_type_id = c.transaction_type_id

WHERE claim_id = (SELECT cp.claim_id FROM claim_peril cp WHERE claim_peril_id=@Peril_ID)
END

