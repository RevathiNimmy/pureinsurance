SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_AccountShort'
GO

CREATE PROCEDURE spu_ACT_Select_AccountShort
    @short_code VARCHAR(20),
    @company_id INT = NULL,
	@user_id INT=NULL
AS

BEGIN
    DECLARE @sSql as varchar(500);
	DECLARE @optionvalue as char(2);

	SELECT DISTINCT @optionvalue=value 
	FROM System_Options 
	WHERE option_number =5152;   

	IF @short_code = '%INSURERSUSPENSE%'
		BEGIN
			IF @optionvalue=1 
				BEGIN
					SELECT account_id
					FROM   Account
					WHERE  short_code LIKE @short_code
					AND ((company_id=@company_id) or (@company_id is null))
					AND (company_id IN(SELECT s.source_id
														  FROM Source s left join PMUser_Source u
														  ON s.source_id=u.source_id
														  AND u.user_id=@user_id
														  WHERE u.source_id is	null))
			    END
		    ELSE
			    BEGIN
					SELECT account_id
					FROM   Account
					WHERE  short_code LIKE @short_code
					AND ((company_id=@company_id) or (@company_id is null))					
			    END
		END
	ELSE
		BEGIN
		    IF @optionvalue=1 
				BEGIN
					SELECT account_id
					FROM   Account
					WHERE  short_code = @short_code
					AND ((company_id=@company_id) or (@company_id is null))
					AND (company_id IN(SELECT s.source_id
														  FROM Source s left join PMUser_Source u
														  ON s.source_id=u.source_id
														  AND u.user_id=@user_id
														  WHERE u.source_id is	null))
				END
			ELSE
			    BEGIN
					SELECT account_id
					FROM   Account
					WHERE  short_code = @short_code
					AND ((company_id=@company_id) or (@company_id is null))					
				END
		END
END
GO
GO


