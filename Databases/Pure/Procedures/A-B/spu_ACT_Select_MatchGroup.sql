SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_MatchGroup'
GO


CREATE PROCEDURE spu_ACT_Select_MatchGroup
    @match_id int
AS


SELECT
    match_id,
    period_id,
    company_id,
    match_date
FROM MatchGroup
WHERE match_id = @match_id
GO


