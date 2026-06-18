SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Ledger'
GO


CREATE PROCEDURE spu_ACT_SelAll_Ledger
    @company_id int,
    @sub_branch_id int = NULL
AS

/* DD 23/08/2002 */
/* Get the Product Option for multi-tree accounting */
DECLARE @Value VARCHAR(20)
SELECT
    @Value = Value
FROM
    Hidden_options
WHERE
    option_number = 16

/*
    If Null/0 then there is only one tree.
    Hardcoded for performance reasons
*/
IF ISNULL(@Value, 0) = 0
BEGIN
    SELECT @company_id = 1
    SELECT @sub_branch_id = 1
END
ELSE
BEGIN
    IF @sub_branch_id IS NULL
        EXEC spu_sub_branch_default @source_id=@company_id, @sub_branch_id=@sub_branch_id OUTPUT
END

SELECT ledger_id,
       sequence,
       company_id,
       sub_branch_id,
       ledger_name,
       ledger_short_name,
       mapping_id,
       ledgertype_id,
       current_period_id,
       is_deletable
FROM   Ledger
WHERE  sub_branch_id = @sub_branch_id


GO


