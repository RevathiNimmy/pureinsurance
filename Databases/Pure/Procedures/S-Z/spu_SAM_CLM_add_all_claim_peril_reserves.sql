SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_add_all_claim_peril_reserves'
GO

CREATE PROCEDURE spu_SAM_CLM_add_all_claim_peril_reserves

@claim_id int, 
@insurance_file_cnt int, 
@risk_cnt int

AS

BEGIN

DECLARE @claim_peril_id int 

DECLARE claim_peril_cursor CURSOR FAST_FORWARD FOR 
	SELECT claim_peril_id
	FROM claim_peril
	WHERE claim_id = @claim_id

OPEN claim_peril_cursor

FETCH NEXT FROM claim_peril_cursor 
	INTO @claim_peril_id

WHILE @@FETCH_STATUS = 0
BEGIN
 
   EXEC spu_get_reserve_details  
		    @perilid = @claim_peril_id,  
		    @siriusproduct ='U',  
		    @policyid = @insurance_file_cnt,  
		    @riskid = @risk_cnt  

   -- Get the next author.
   FETCH NEXT FROM claim_peril_cursor 
   INTO @claim_peril_id

END

CLOSE claim_peril_cursor
DEALLOCATE claim_peril_cursor

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
