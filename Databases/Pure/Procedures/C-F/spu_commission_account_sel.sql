SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXEC DDLDropProcedure 'spu_commission_account_sel'
GO

CREATE PROCEDURE spu_commission_account_sel
    @source_id int,
    @risk_code_id int = NULL,
    @account_executive_cnt int = NULL
AS

BEGIN
    -- SJP (CMG) 11/04/2003 PS235
    IF (@account_executive_cnt IS NULL)
    BEGIN
        SELECT DISTINCT [pty].party_cnt
        FROM   party [pty],
               risk_code [rkc],
               risk_group [rkg],
               risk_by_source [rbs]
        WHERE  [pty].party_cnt = [rbs].commission_cnt
        AND    [rbs].risk_group_id = [rkc].risk_group_id
        AND    [rkg].risk_group_id = [rkc].risk_group_id
        AND    [rkc].risk_code_id = @risk_code_id
        AND    ([rbs].source_id = @source_id
                OR [rbs].source_id = 0)
    END
    ELSE
    BEGIN
        SELECT DISTINCT [pty].party_cnt
        FROM   party [pty],
               party_consultant [ptc] 
        WHERE  [pty].party_cnt = [ptc].commission_cnt
        AND    [ptc].party_cnt = @account_executive_cnt 
    END
END
GO


