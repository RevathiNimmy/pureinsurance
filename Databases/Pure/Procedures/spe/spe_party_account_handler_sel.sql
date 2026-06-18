SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_account_handler_sel'
GO

CREATE PROCEDURE spe_party_account_handler_sel
    @party_cnt int
AS
SELECT
    party_cnt,
    forename,
    initials,
    department_id,
    party_title_code

FROM party_account_handler
WHERE party_cnt = @party_cnt

GO

