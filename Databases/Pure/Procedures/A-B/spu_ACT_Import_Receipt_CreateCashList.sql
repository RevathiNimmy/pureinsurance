SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_ACT_Import_Receipt_CreateCashList'
GO


CREATE PROCEDURE spu_ACT_Import_Receipt_CreateCashList  
    @batch_id int,  
    @bankaccount_code varchar(60),  
    @currency_code char(10),  
    @username varchar(255),  
    @cashlist_id int output  
AS  
  
    Declare  
        @bankaccount_id int,  
        @cashlist_ref varchar(25),  
        @company_id int,  
        @sub_branch_id int,  
        @currency_id int,  
        @pmuser_id int,  
        @base_currency_id int,
	@ID INT  
  
    -- Get info from bank account  
    Select  @bankaccount_id = bankaccount_id,  
            @company_id  = company_id,  
            @sub_branch_id = sub_branch_id  
    From    bankaccount  
    Where   bank_account_name = @bankaccount_code  
  
    -- Get currency  
    Select  @currency_id = currency_id  
    From    currency  
    Where   code = @currency_code  
  
    -- Get user id  
    Select  @pmuser_id = user_id  
    From    pmuser  
    Where   username = @username  
  
    -- Get cash list reference  
    Select  @cashlist_ref = batch_ref  
    From    batch  
    Where   batch_id = @batch_id  

    -- Get base currency id  
    Select  @base_currency_id = base_currency_id  
    From    source  
    Where   source_id = @company_id  
  

    Insert Into cashlist (  
            bankaccount_id,  
            cashlisttype_id,  
            cashliststatus_id,  
            cashlist_ref,  
            company_id,  
            sub_branch_id,  
            currency_id,  
            list_date,  
            control_total,  
            item_count,  
            batch_id,  
            pmuser_id,  
            base_currency_id)  
    Values (
            @bankaccount_id,  
            2, -- receipts  
            2, -- opened  
            '',  
            @company_id,  
            @sub_branch_id,  
            @currency_id,  
            GetDate(),  
            0,  
            0,  
            @batch_id,  
            @pmuser_id,  
            @base_currency_id)  


    SELECT @ID = @@IDENTITY 

    SET @cashlist_id = @ID

    UPDATE CashList SET cashlist_ref = @cashlist_ref WHERE cashlist_id = @@IDENTITY
GO
