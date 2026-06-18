SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Round_Amounts_For_Posting'
GO

CREATE PROCEDURE spu_ACT_Round_Amounts_For_Posting
	@currency_id smallint,    
	@currency_amount numeric(19,4) OUTPUT,
	@base_currency_id smallint,
	@base_amount numeric(19,4) OUTPUT,
	@account_currency_id smallint,
	@account_amount numeric(19,4) OUTPUT,	
	@system_currency_id smallint,
	@system_amount numeric(19,4) OUTPUT	
AS

DECLARE	@rounding_places TINYINT 

SELECT @rounding_places=round_to_places FROM Currency WITH(NOLOCK) WHERE currency_id=@currency_id
SELECT @currency_amount=ROUND(@currency_amount,@rounding_places)

IF @base_currency_id<>@currency_id
	SELECT @rounding_places=round_to_places FROM Currency WITH(NOLOCK) WHERE currency_id=@base_currency_id
SELECT @base_amount=ROUND(@base_amount,@rounding_places)

IF @account_currency_id<>@base_currency_id
	SELECT @rounding_places=round_to_places FROM Currency WITH(NOLOCK) WHERE currency_id=@account_currency_id
SELECT @account_amount=ROUND(@account_amount,@rounding_places)

IF @system_currency_id<>@account_currency_id
	SELECT @rounding_places=round_to_places FROM Currency WITH(NOLOCK) WHERE currency_id=@system_currency_id
SELECT @system_amount=ROUND(@system_amount,@rounding_places)

GO