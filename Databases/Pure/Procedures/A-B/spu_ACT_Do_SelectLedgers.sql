SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_SelectLedgers'
GO


CREATE PROCEDURE spu_ACT_Do_SelectLedgers
    @CompanyID int,
    @sub_branch_id int = NULL
AS


/* DD 23/08/2002 */
/* Get the Product Option for multi-tree accounting */
DECLARE @Value VARCHAR(20)
SELECT
    @Value=Value
FROM
    Hidden_options
WHERE
    option_number=16

/*
    If Null/0 then there is only one tree.
    Hardcoded for performance reasons
*/
IF @Value IS NULL OR @Value=0
BEGIN
    SELECT @CompanyID=1
    SELECT @sub_branch_id=1
END
ELSE
BEGIN
    IF @sub_branch_id IS NULL
        EXEC spu_sub_branch_default @source_id=@CompanyID, @sub_branch_id=@sub_branch_id OUTPUT
END


SELECT ledger_id,
       ledger_name,
       ledgertype_id,
       ledger_short_name
FROM   Ledger
WHERE  Ledger.sub_branch_id = @sub_branch_id
ORDER BY ledgertype_id

GO


