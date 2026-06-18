SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_ACT_Import_Payment_CreateCashList'
GO


CREATE PROCEDURE spu_ACT_Import_Payment_CreateCashList
    @batch_id int,
    @bankaccount_code char(10),
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
        @base_currency_id int

    -- Get info from bank account
    Select  @bankaccount_id = bankaccount_id,
            @company_id  = company_id,
            @sub_branch_id = sub_branch_id
    From    bankaccount
    Where   code = @bankaccount_code

    -- Get currency
    Select  @currency_id = currency_id
    From    currency
    Where   code = @currency_code

    -- Get user id
    Select  @pmuser_id = user_id
    From    pmuser
    Where   username = @username


    -- Check to see if we already have a cashlist open for this batch
    Select  @cashlist_id = cashlist_id
    From    cashlist
    Where   batch_id = @batch_id
    And     bankaccount_id = @bankaccount_id
    And     currency_id = @currency_id
    And     pmuser_id = @pmuser_id


    -- If we don't yet have a cashlist we should create one...
    If @cashlist_id Is Null Begin
        -- Get cash list reference
        Select  @cashlist_ref = batch_ref
        From    batch
        Where   batch_id = @batch_id

        -- Get base currency id
        Select  @base_currency_id = base_currency_id
        From    source
        Where   source_id = @company_id

        -- Get new id
        --Select  @cashlist_id = IsNull(Max(cashlist_id), 0) + 1
        --From    cashlist

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
        Values (@bankaccount_id,
                1, -- payments
                2, -- opened
                @cashlist_ref,
                @company_id,
                @sub_branch_id,
                @currency_id,
                GetDate(), 
                0, 
                0, 
                @batch_id,
                @pmuser_id,
                @base_currency_id)
	--Set new CashID
	SELECT @cashlist_id = @@IDENTITY

    End

Go


