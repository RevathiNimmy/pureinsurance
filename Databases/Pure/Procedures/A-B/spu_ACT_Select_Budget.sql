SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Budget'
GO


CREATE PROCEDURE spu_ACT_Select_Budget
    @budget_id int
AS


SELECT
    budget_id,
    budget_ref,
    period_id,
    budget_description,
    period_year_name,
    revises_budget_id,
    budget_status_id
FROM Budget
WHERE budget_id = @budget_id
GO


