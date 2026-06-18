SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Account_Details_by_Short_Code'
GO



CREATE PROCEDURE spu_CLM_Get_Account_Details_by_Short_Code

 @short_code varchar(60)

AS

BEGIN

	SELECT 
		currency_id, 
		account_id 
	FROM account 
	WHERE short_code = @short_code

END







GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
