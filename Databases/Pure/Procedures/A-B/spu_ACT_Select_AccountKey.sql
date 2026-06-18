SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_AccountKey'
GO

CREATE PROCEDURE spu_ACT_Select_AccountKey
    @account_key int,
    @company_id int = null
AS

IF NOT EXISTS 
(
	SELECT value
	FROM hidden_options
	WHERE option_number = 16 
	AND value = 1
)
BEGIN
	SELECT @company_id = NULL
END

SELECT account_id
FROM   Account
WHERE  account_key = @account_key
AND ((company_id=@company_id) or (@company_id is null))

GO



