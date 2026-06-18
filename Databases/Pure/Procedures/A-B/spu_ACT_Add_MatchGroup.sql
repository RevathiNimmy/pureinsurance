SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_MatchGroup'
GO


CREATE PROCEDURE spu_ACT_Add_MatchGroup
    @match_id int OUTPUT,
    @period_id int,
    @company_id smallint,
    @match_date datetime
AS


BEGIN
INSERT INTO MatchGroup (
    period_id,
    company_id,
    match_date)
VALUES (
    @period_id,
    @company_id,
    @match_date)
END
BEGIN
SELECT @match_id = @@IDENTITY
END
GO


