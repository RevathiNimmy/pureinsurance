SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Budget_Detail'
GO


CREATE PROCEDURE spu_ACT_Add_Budget_Detail
    @budget_detail_id int OUTPUT,
    @budget_id int,
    @budget_sequence int,
    @period_id int,
    @account_id int,
    @budget_amount numeric(19,4),
    @actual_amount numeric(19,4),
    @variance_amount numeric(19,4)
AS

BEGIN
INSERT INTO Budget_Detail (
    budget_id,
    budget_sequence,
    period_id,
    account_id,
    budget_amount,
    actual_amount,
    variance_amount)
VALUES (
    @budget_id,
    @budget_sequence,
    @period_id,
    @account_id,
    @budget_amount,
    @actual_amount,
    @variance_amount )
END
SELECT @budget_detail_id=@@IDENTITY
GO


