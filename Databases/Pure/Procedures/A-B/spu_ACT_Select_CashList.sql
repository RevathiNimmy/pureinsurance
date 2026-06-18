SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_CashList'
GO


CREATE PROCEDURE spu_ACT_Select_CashList  
    @cashlist_id int  
AS  
  
SELECT  
    cashlist_id,  
    bankaccount_id,  
    cashlisttype_id,  
    cashliststatus_id,  
    cashlist_ref,  
    company_id,  
    currency_id,  
    list_date,  
    control_total,  
    item_count,  
-- *** pkh 07/10/2002 starts - Added for Front Office Receipting module  
    cashlist_drawer_id,  
    batch_id,  
    pmuser_id ,  
    confirm_pmuser_id ,  
    confirm2_pmuser_id,  
    date_approved,  
    banking_total,  
    cash_float_amount,  
    date_deposited,  
-- KG SubBranch 29/07/03  
    sub_branch_id,
    is_split_receipt
-- *** pkh 07/10/2002 ends   - Added for Front Office Receipting module  
FROM CashList  
WHERE cashlist_id = @cashlist_id  
GO


