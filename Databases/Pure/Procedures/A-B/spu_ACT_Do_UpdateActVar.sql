SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_UpdateActVar'
GO


CREATE PROCEDURE spu_ACT_Do_UpdateActVar
    @period_id integer,
    @budget_detail_id integer
AS

/*
    History:
    201098 - CTAF - Original
*/
DECLARE @base_amount numeric(19,4),
    @variance_amount numeric(19,4),
    @budget_amount numeric(19,4)
BEGIN
    /* Work out the actual amount for the budget detail */
    SELECT @base_amount = sum(td.currency_amount)
    FROM TransDetail td, Budget_Detail bd
    WHERE td.period_id = @period_id
    AND td.account_id = bd.account_id
    AND bd.budget_detail_id = @budget_detail_id
    /* Update the actual_amount */
    IF @base_amount = null
        SELECT @base_amount = 0
    UPDATE Budget_Detail
    SET actual_amount = @base_amount
    WHERE budget_detail_id = @budget_detail_id
    /* Get the budget_amount */
    SELECT @budget_amount = budget_amount
    FROM Budget_Detail
    WHERE budget_detail_id = @budget_detail_id
    /* Derive the variance (actual - budget) */
    SELECT @variance_amount = @base_amount - @budget_amount
    /* Update the variance */
    UPDATE Budget_Detail
    SET variance_amount = @variance_amount
    WHERE budget_detail_id = @budget_detail_id
END
GO


