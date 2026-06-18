SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Update_CurrencyRate'
GO

CREATE PROCEDURE spu_ACT_Update_CurrencyRate
    @effective_from datetime,
    @rate_against_base numeric(19, 10),
    @currency_id smallint,
    @company_id smallint,
	@user_id int,
	@unique_id varchar(50),
	@screen_hierarchy varchar(500)
AS

UPDATE CurrencyRate
SET	rate_against_base = @rate_against_base,
	UserId = @user_id,
	UniqueId = @unique_id,
	ScreenHierarchy = @screen_hierarchy
WHERE currency_id = @currency_id
AND company_id = @company_id
AND effective_from = @effective_from 


GO