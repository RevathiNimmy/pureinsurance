SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PFMediaTypeHistory_Add_SetupEntry'
GO


CREATE PROCEDURE spu_PFMediaTypeHistory_Add_SetupEntry

@pfprem_finance_cnt int,
@pfprem_finance_version int

As

BEGIN
	DECLARE @count int

	-- determine if we need to store the original details as the setup line in the history
	SELECT @count = count(*) FROM pfMediaTypeHistory   
	WHERE pfprem_finance_cnt = @pfprem_finance_cnt  
	AND pfprem_finance_version = @pfprem_finance_version  
	  
	IF @Count = 0   
	BEGIN
		-- save existing media details before updating them
		EXEC spu_PFMediaTypeHistory_add @pfprem_finance_cnt,@pfprem_finance_version, 'Setup'  
	END
	
END




GO
