SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_AccountIdFromShortCode'
GO

CREATE PROCEDURE spu_Get_AccountIdFromShortCode
@short_code char(20)
AS
SELECT account_id,company_id    
FROM ACCOUNT 
WHERE short_code=@short_code
GO
