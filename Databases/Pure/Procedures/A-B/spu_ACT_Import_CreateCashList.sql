Execute DDLDropProcedure 'spu_ACT_Import_CreateCashList'
GO

CREATE PROCEDURE spu_ACT_Import_CreateCashList
    @batch_id  INT,  
    @bankaccount_code VARCHAR(60),  
    @currency_code char(10),  
    @username VARCHAR(255), 
    @cashlisttype TINYINT, 
    @cashlist_id  INT output  
AS  
  
    DECLARE  
        @nBankaccount_id  INT,  
        @sCashlist_ref VARCHAR(25),  
        @nCompany_id  INT,  
        @nSub_branch_id  INT,  
        @nCurrency_id  INT,  
        @nPmuser_id  INT,  
        @nBase_currency_id  INT
  
    -- Get info from bank account  
    SELECT  @nBankaccount_id = bankaccount_id,  
            @nCompany_id  = company_id,  
            @nSub_branch_id = sub_branch_id  
    FROM    bankaccount  
    WHERE   bank_account_name = @bankaccount_code  
  
    -- Get currency  
    SELECT  @nCurrency_id = currency_id  
    FROM    currency  
    WHERE   code = @currency_code  
  
    -- Get user id  
    SELECT  @nPmuser_id = user_id  
    FROM    pmuser  
    WHERE   username = @username  
  
    -- Get cash list reference  
    SELECT  @sCashlist_ref = batch_ref  
    FROM    batch  
    WHERE   batch_id = @batch_id  

    -- Get base currency id  
    SELECT  @nBase_currency_id = base_currency_id  
    FROM    source  
    WHERE   source_id = @nCompany_id  

    INSERT INTO cashlist (  
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
    VALUES (
            @nBankaccount_id,  
            @cashlisttype, -- receipts or payments 2 or 1 respectively  
            2, -- opened  
            '',  
            @nCompany_id,  
            @nSub_branch_id,  
            @nCurrency_id,  
            GetDate(),  
            0,  
            0,  
            @batch_id,  
            @nPmuser_id,  
            @nBase_currency_id)  

    SELECT @cashlist_id = @@IDENTITY 

    UPDATE CashList SET cashlist_ref = @sCashlist_ref WHERE cashlist_id = @@IDENTITY
GO
