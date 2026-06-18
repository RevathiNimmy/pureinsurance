SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Period'
GO


CREATE PROCEDURE spu_ACT_Select_Period
    @period_id int
AS


SELECT
    period_id,
    company_id,
    sub_branch_id,
    year_name,
    period_name,
    period_end_date,
    period_end_complete
FROM Period
WHERE period_id = @period_id
GO


