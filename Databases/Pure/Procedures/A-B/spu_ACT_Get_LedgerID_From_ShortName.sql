SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_LedgerID_From_ShortName'
GO


CREATE PROCEDURE spu_ACT_Get_LedgerID_From_ShortName
    @ShortName varchar(2),
    @sub_branch_id integer,
    @LedgerID integer OUTPUT
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
    SELECT @sub_branch_id=1

SELECT  @LedgerID=ledger_id
    FROM Ledger
    WHERE ledger_short_name = @ShortName
    AND sub_branch_id=@sub_branch_id
GO
