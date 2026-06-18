SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_CashList'
GO

CREATE  PROCEDURE spu_ACT_Add_CashList
    @cashlist_id int OUTPUT,
    @bankaccount_id int,
    @cashlisttype_id int,
    @cashliststatus_id int,
    @cashlist_ref varchar(25),
    @company_id int,
    @currency_id smallint,
    @list_date datetime,
    @control_total numeric(19,4),
    @item_count int,
    @cashlist_drawer_id int,
    @batch_id int,
    @pmuser_id smallint,
    @confirm_pmuser_id smallint,
    @confirm2_pmuser_id smallint,
    @date_approved datetime,
    @banking_total numeric(18, 0),
    @cash_float_amount numeric(18, 0),
    @deposit_date datetime,
    @sub_branch_id int
AS
IF ISNULL(@sub_branch_id,0) = 0
BEGIN
   SELECT @sub_branch_id = sub_branch_id,
	  @company_id  = company_id
   FROM   bankaccount
   WHERE  bankaccount_id = @bankaccount_id
END

INSERT INTO CashList (
    bankaccount_id ,
    cashlisttype_id ,
    cashliststatus_id ,
    cashlist_ref ,
    company_id ,
    sub_branch_id ,
    currency_id ,
    list_date ,
    control_total ,
    item_count ,
    cashlist_drawer_id ,
    batch_id ,
    pmuser_id ,
    confirm_pmuser_id ,
    confirm2_pmuser_id ,
    date_approved ,
    banking_total ,
    cash_float_amount ,
    date_deposited )
VALUES (
    @bankaccount_id,
    @cashlisttype_id,
    @cashliststatus_id,
    @cashlist_ref,
    @company_id,
    @sub_branch_id,
    @currency_id,
    @list_date,
    @control_total,
    @item_count,
    @cashlist_drawer_id,
    @batch_id,
    @pmuser_id,
    @confirm_pmuser_id,
    @confirm2_pmuser_id,
    @date_approved,
    @banking_total,
    @cash_float_amount,
    @deposit_date )

SELECT @cashlist_id=@@IDENTITY

GO
