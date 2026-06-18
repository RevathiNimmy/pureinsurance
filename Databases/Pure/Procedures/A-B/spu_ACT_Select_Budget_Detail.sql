SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Budget_Detail'
GO


CREATE PROCEDURE spu_ACT_Select_Budget_Detail
    @budget_detail_id int
AS


SELECT budget_detail_id,
    budget_id,
    budget_sequence,
    period_id,
    account_id,
    budget_amount,
    actual_amount,
    variance_amount
FROM Budget_Detail
WHERE budget_detail_id = @budget_detail_id
GO


