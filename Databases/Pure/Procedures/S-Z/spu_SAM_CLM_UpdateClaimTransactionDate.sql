SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_UpdateClaimTransactionDate'
GO

CREATE PROCEDURE spu_SAM_CLM_UpdateClaimTransactionDate

@claim_id int,
@transaction_date datetime

AS


BEGIN

UPDATE claim 
SET create_date = @transaction_date,
last_modified_date = @transaction_date
WHERE claim_id = @claim_id

END




GO
