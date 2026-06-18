SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_CashList'
GO


CREATE PROCEDURE spu_ACT_SelAll_CashList
    @company_id int,
    @sub_branch_id int = NULL
AS


IF @sub_branch_id IS NULL
    EXEC spu_sub_branch_default @company_id, @sub_branch_id OUTPUT


SELECT cashlist_id,
       bankaccount_id,
       cashlisttype_id,
       cashliststatus_id,
       cashlist_ref,
       company_id,
       currency_id,
       list_date,
       control_total,
       item_count
FROM   CashList
WHERE  sub_branch_id = @sub_branch_id


GO


