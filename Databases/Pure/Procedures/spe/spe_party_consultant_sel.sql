SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_consultant_sel'
GO

CREATE PROCEDURE spe_party_consultant_sel
    @party_cnt int
AS
SELECT
    party_cnt,
    forename,
    initials,
    department_id,
    party_title_code,
    commission_cnt
 FROM party_consultant
WHERE party_cnt = @party_cnt

GO

