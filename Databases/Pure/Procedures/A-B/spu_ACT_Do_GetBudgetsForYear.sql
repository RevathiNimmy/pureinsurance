SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_GetBudgetsForYear'
GO


CREATE PROCEDURE spu_ACT_Do_GetBudgetsForYear
    @period_id integer
AS

/*
    History:
    201098 - CTAF - Original
*/
BEGIN
    SELECT b.budget_id
    FROM Budget b, Period p
    WHERE p.period_id = @period_id
    AND b.period_year_name = p.year_name
END
GO


