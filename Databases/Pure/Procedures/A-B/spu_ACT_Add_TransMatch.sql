SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_TransMatch'
GO


CREATE PROCEDURE spu_ACT_Add_TransMatch
    @transmatch_id int OUTPUT,
    @allocationdetail_id int,
    @transdetail_id int,
    @match_id int,
    @currency_id smallint,
    @base_match_amount numeric(19,4),
    @currency_match_amount numeric(19,4),
    @currency_match_xrate numeric(12,8)
AS

DECLARE @account_id INT
DECLARE @company_id INT
DECLARE @currency_amount_unrounded MONEY
DECLARE @base_amount MONEY
DECLARE @account_amount MONEY
DECLARE @system_amount MONEY
DECLARE @currency_base_xrate FLOAT
DECLARE @account_base_xrate FLOAT
DECLARE @system_base_xrate FLOAT
DECLARE @return_status INT
DECLARE @match_date DATETIME
DECLARE @spare varchar(20) 

/*Get the account and company IDs from the transdetail record.*/
SELECT	@account_id = account_id,
		@company_id = company_id,
		@account_base_xrate = account_base_xrate,
		@system_base_xrate = system_base_xrate
FROM	TransDetail
WHERE	transdetail_id=@transdetail_id

/*Get the match date from the matchgroup record*/
SELECT @match_date = match_date
FROM MatchGroup
WHERE match_id = @match_id

/*Get the converted amounts*/
EXECUTE spu_ACT_Do_Currency_Conversion
	@account_id = @account_id,
	@company_id = @company_id,
	@currency_id = @currency_id,
	@currency_amount_unrounded = @currency_match_amount,
	@mode = 'ALL',
	@base_amount = @base_amount OUTPUT,
	@account_amount = @account_amount OUTPUT,
	@system_amount = @system_amount OUTPUT,
	@currency_base_xrate = @currency_match_xrate OUTPUT,
	@currency_base_date = @match_date OUTPUT,
	@account_base_xrate = @account_base_xrate OUTPUT,
	@system_base_xrate = @system_base_xrate OUTPUT,
	@return_status = @return_status OUTPUT


INSERT INTO TransMatch 
(
    allocationdetail_id,
    transdetail_id,
    match_id,
    currency_id,
    base_match_amount,
    currency_match_amount,
    currency_match_xrate,
    account_match_amount,
    system_match_amount
)
VALUES 
(
    @allocationdetail_id,
    @transdetail_id,
    @match_id,
    @currency_id,
    @base_match_amount,
    @currency_match_amount,
    @currency_match_xrate,
    @account_amount,
    @system_amount
)

SELECT @transmatch_id = @@IDENTITY

IF @allocationdetail_id IS NOT NULL
BEGIN
	/*Update the transdetail record with new outstanding amounts*/
	EXEC spu_ACT_Update_TransDetailOutstanding @transdetail_id, 
			@currency_match_amount, @base_match_amount, @account_amount, @system_amount
END

SELECT @spare = SPARE FROM TransDetail WHERE transdetail_id = @transdetail_id  
SET @spare = Lower(LTrim(RTrim(@spare)))  
  
If @spare like 'rev%'  
BEGIN  
  UPDATE TransMatch  SET is_reversed = 1 WHERE transdetail_id = @transdetail_id  AND transmatch_id = @transmatch_id
END   
ELSE  
  UPDATE TransMatch  SET is_reversed = 0 WHERE transdetail_id = @transdetail_id AND transmatch_id = @transmatch_id


GO


