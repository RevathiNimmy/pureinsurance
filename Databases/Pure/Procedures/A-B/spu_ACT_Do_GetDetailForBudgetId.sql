SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_GetDetailForBudgetId'
GO


CREATE PROCEDURE spu_ACT_Do_GetDetailForBudgetId
    @budget_id integer
AS

/*
    The specified title should have been sp_ACT_DoGetDetailsForBudgetId but this was too long
    History:
    201098 - CTAF - Original
*/
BEGIN
SELECT budget_sequence,
    period_id,
    account_id,
    budget_amount,
    budget_detail_id
FROM Budget_Detail
WHERE budget_id = @budget_id
END
GO


