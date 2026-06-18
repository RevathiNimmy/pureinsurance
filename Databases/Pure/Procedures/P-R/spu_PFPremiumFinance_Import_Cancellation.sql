SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PFPremiumFinance_Import_Cancellation'
GO


CREATE PROCEDURE spu_PFPremiumFinance_Import_Cancellation

@pfprem_finance_cnt int,
@pfprem_finance_version int,
@action_code varchar(50),
@dd_cancelled tinyint, 
@cc_cancelled tinyint, 
@statusInd varchar(3)

AS

BEGIN

	-- store the existing history as setup if no previous history item exists
	EXEC spu_PFMediaTypeHistory_Add_SetupEntry @pfprem_finance_cnt,@pfprem_finance_version 

	-- update the current details to reflect the new data
	UPDATE pfpremiumfinance
	
	SET
		dd_cancelled = @dd_cancelled,
		cc_cancelled = @cc_cancelled,
		statusInd = @statusInd
	
	WHERE pfprem_finance_cnt = @pfprem_finance_cnt
	AND pfprem_finance_version = @pfprem_finance_version

	-- save these changes to the history table
	EXEC spu_PFMediaTypeHistory_Add @pfprem_finance_cnt,@pfprem_finance_version ,@action_code

END





GO
