SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_salvagerecovery_add'
GO


CREATE PROCEDURE spu_salvagerecovery_add
    @recovery_id int OUTPUT,
    @peril_id int,
    @recovery_type_id int,
    @currency_id int,
    @initial_reserve currency,
    @revised_reserve currency,
    @received_to_date currency,
    @revision_count int,
	@Tax_Amount currency
AS

--*******************************************************************************************
-- Version      Author  Date        Desc
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting
--
--*******************************************************************************************
DECLARE @AgentUnderwriter varchar(1)

SELECT  @AgentUnderwriter = value
FROM    hidden_options
WHERE   branch_id = 1 and option_number = 1

IF @AgentUnderwriter is null
    SELECT @AgentUnderwriter = "A"

IF @AgentUnderwriter = ""
    SELECT @AgentUnderwriter = "A"

IF @AgentUnderwriter = "A"
BEGIN
    INSERT INTO recovery (
     claim_peril_id ,
     recovery_type_id,
     currency_id,
     initial_reserve ,
     revised_reserve,
     received_to_date,
     revision_count,
	 tax_amount
     )
    VALUES (
     @peril_id ,
     @recovery_type_id ,
     @currency_id ,
     @initial_reserve,
     @revised_reserve,
     @received_to_date ,
     @revision_count,
	 @Tax_Amount
     )
UPDATE Claim 
SET Last_modified_date = Getdate()
WHERE Claim_id = 
(SELECT DISTINCT Claim_id 
FROM claim_peril 
WHERE claim_peril_id = @peril_id)
END
ELSE
--UNDERWRITING
    INSERT INTO work_recovery
    (
    claim_peril_id ,
    recovery_type_id,
    currency_id,
    initial_reserve ,
    revised_reserve,
    received_to_date,
    revision_count,
	tax_amount
    )
    VALUES
    (
    @peril_id ,
    @recovery_type_id ,
    @currency_id ,
    @initial_reserve,
    @revised_reserve,
    @received_to_date ,
    @revision_count,
	@Tax_Amount
    )

SELECT @recovery_id = @@IDENTITY
GO