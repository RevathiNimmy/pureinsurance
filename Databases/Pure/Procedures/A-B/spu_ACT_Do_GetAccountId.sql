SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_GetAccountId'
GO


CREATE PROCEDURE spu_ACT_Do_GetAccountId
    @CompanyID smallint,
    @LedgerID smallint,
    @FullKey varchar(64),
    @ShortCode varchar(20),
    @AccountID int OUTPUT,
    @sub_branch_id int = NULL
AS


IF @sub_branch_id IS NULL
    EXEC spu_sub_branch_default @CompanyID, @sub_branch_id OUTPUT


SELECT @AccountID = account_id
FROM   Account
WHERE  sub_branch_id = @sub_branch_id
AND   (ledger_id = @LedgerID OR @LedgerID = -1)
AND   (short_code = @ShortCode OR @ShortCode = '')


GO


