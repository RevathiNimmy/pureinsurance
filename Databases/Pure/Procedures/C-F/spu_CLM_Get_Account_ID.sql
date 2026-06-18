SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Account_ID'
GO

CREATE PROCEDURE spu_CLM_Get_Account_ID

@short_code varchar(20),
@account_key int

AS 

IF ISNULL(@account_key,0) <> 0 
	SELECT account_id from account where account_key = @account_key
ELSE
	SELECT account_id from account where short_code = @short_code




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
