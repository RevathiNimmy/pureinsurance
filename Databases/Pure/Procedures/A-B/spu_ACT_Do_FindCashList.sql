SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_FindCashList'
GO


CREATE PROCEDURE spu_ACT_Do_FindCashList
    @company_id int,
    @cashliststatus_id int = NULL,
    @cashlisttype_id smallint = NULL,
    @cashlist_ref varchar(25) = NULL,
    @bankaccount_id smallint = NULL,
    @currency_id smallint = NULL,
    @start_date datetime = NULL,
    @end_date datetime = NULL,
    @control_total numeric(19,4) = NULL,
    @item_count int = NULL,
    @sub_branch_id int = NULL
AS


IF @sub_branch_id IS NULL
    EXEC spu_sub_branch_default @company_id, @sub_branch_id OUTPUT


SELECT cashlist_id,
       cashliststatus_id,
       cashlisttype_id,
       cashlist_ref,
       company_id,
       bankaccount_id,
       currency_id,
       list_date,
       control_total,
       item_count
FROM   CashList
WHERE (cashliststatus_id = @cashliststatus_id OR @cashliststatus_id IS NULL)
AND   (cashlisttype_id = @cashlisttype_id OR @cashlisttype_id IS NULL)
AND   (cashlist_ref LIKE @cashlist_ref OR @cashlist_ref IS NULL)
AND   (bankaccount_id = @bankaccount_id OR @bankaccount_id IS NULL)
AND   (currency_id = @currency_id OR @currency_id IS NULL)
AND   (list_date >= @start_date OR @start_date IS NULL)
AND   (list_date <= @end_date OR @end_date IS NULL)
AND   (control_total = @control_total OR @control_total IS NULL)
AND   (item_count = @item_count OR @item_count IS NULL)
AND   (sub_branch_id = @sub_branch_id)

GO


