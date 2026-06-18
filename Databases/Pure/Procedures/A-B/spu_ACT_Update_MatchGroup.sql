SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_MatchGroup'
GO


CREATE PROCEDURE spu_ACT_Update_MatchGroup
    @match_id int,
    @period_id int,
    @company_id smallint,
    @match_date datetime
AS


BEGIN
UPDATE MatchGroup
    SET
    period_id=@period_id,
    company_id=@company_id,
    match_date=@match_date
WHERE match_id = @match_id
END
GO


