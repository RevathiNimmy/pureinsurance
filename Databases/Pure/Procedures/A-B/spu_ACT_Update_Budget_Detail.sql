SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Budget_Detail'
GO


CREATE PROCEDURE spu_ACT_Update_Budget_Detail
    @budget_detail_id int,
    @budget_id int,
    @budget_sequence int,
    @period_id int,
    @account_id int,
    @budget_amount numeric(19,4),
    @actual_amount numeric(19,4),
    @variance_amount numeric(19,4)
AS


BEGIN
UPDATE Budget_Detail
    SET
    budget_id = @budget_id,
    budget_sequence = @budget_sequence,
    period_id = @period_id,
    account_id = @account_id,
    budget_amount = @budget_amount,
    actual_amount = @actual_amount,
    variance_amount = @variance_amount
WHERE budget_detail_id = @budget_detail_id
END
GO


