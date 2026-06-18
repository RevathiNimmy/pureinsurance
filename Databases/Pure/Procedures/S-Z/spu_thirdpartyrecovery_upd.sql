SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_thirdpartyrecovery_upd'
GO


CREATE PROCEDURE spu_thirdpartyrecovery_upd
    @recovery_id int,
    @peril_id int,
    @recovery_type_id int,
    @currency_id int,
    @initial_reserve currency,
    @revised_reserve currency,
    @received_to_date currency,
    @revision_count int,
	@tax_amount currency
AS

--*******************************************************************************************
-- Version      Author  Date        Desc
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting
--
--*******************************************************************************************
DECLARE @AgentUnderwriter varchar(1)
declare @CurrentTaxAmount currency

SELECT  @AgentUnderwriter = value
FROM    hidden_options
WHERE   branch_id = 1 and option_number = 1

IF @AgentUnderwriter is null
    SELECT @AgentUnderwriter = "A"

IF @AgentUnderwriter = ""
    SELECT @AgentUnderwriter = "A"

IF @AgentUnderwriter = "A"
begin
	-- Select existing tax amount, to be able to add the new amount to it
	SELECT 	@CurrentTaxAmount = tax_amount
	FROM 	recovery
    where 	recovery_id= @recovery_id
	IF(@CurrentTaxAmount IS NULL)
		set @CurrentTaxAmount=0
	
    Update recovery
    set
    Claim_peril_id = @peril_id ,
    recovery_type_id=@recovery_type_id,
    currency_id=@currency_id,
    initial_reserve=@initial_reserve,
    revised_reserve =@revised_reserve,
    received_to_date =@received_to_date,
    revision_count =@revision_count,
	tax_amount = @tax_amount + @CurrentTaxAmount
    where
    recovery_id= @recovery_id

    UPDATE Claim 
    SET Last_modified_date = Getdate()
    WHERE Claim_id = 
    (SELECT DISTINCT Claim_id 
    FROM claim_peril 
    WHERE claim_peril_id = @peril_id)
	
end
ELSE
begin
--UNDERWRITING

	-- Select existing tax amount, to be able to add the new amount to it
	SELECT 	@CurrentTaxAmount = tax_amount
	FROM 	work_recovery
    where 	recovery_id= @recovery_id
	IF(@CurrentTaxAmount IS NULL)
		set @CurrentTaxAmount=0

    UPDATE work_recovery
    SET Claim_peril_id = @peril_id ,
        recovery_type_id = @recovery_type_id,
        currency_id = @currency_id,
        initial_reserve = @initial_reserve,
        revised_reserve = @revised_reserve,
        received_to_date = @received_to_date,
        revision_count = @revision_count,
		tax_amount = @tax_amount + @CurrentTaxAmount
    WHERE recovery_id = @recovery_id
end
GO


