SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_MatchGroup'
GO


CREATE PROCEDURE spu_ACT_SelAll_MatchGroup
AS


SELECT
    match_id,
    period_id,
    company_id,
    match_date
FROM MatchGroup
GO


