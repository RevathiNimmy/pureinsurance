SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Add_Receipt_CreateCashList'
GO

CREATE PROCEDURE spu_SAM_Add_Receipt_CreateCashList  
--As told by Gaurav, changing @bank_account_name size from 10 to 60 because the back_account_name column in bankaccount  
--table is of type varchar(60)- Prakash  
    @bank_account_name varchar(60),  
    @currency_code varchar(10),  
    @username varchar(255),  
    @cashlist_ref varchar(25),  
    @cashlist_id int output,
    @company_id int=0,
	@subbranch_id int = 0 
AS  

    Declare  
        @bankaccount_id int,  
        @sub_branch_id int,  
        @currency_id int,  
        @pmuser_id int,  
        @base_currency_id int

    -- Get info from bank account  
    Select  @bankaccount_id = bankaccount_id,  
            @company_id  = CASE @company_id WHEN 0 then company_id else @company_id end  
    From    bankaccount  
    Where   bank_account_name = @bank_account_name  

     If @subbranch_id <> 0   
		  BEGIN  
			SET @sub_branch_id = @subbranch_id  
		  END   
	 ELSE  
		  BEGIN  
			Select @sub_branch_id=sub_branch_id from sub_branch where source_id = @company_id and is_deleted <> 1 
		  END 

    -- Get currency  
    Select  @currency_id = currency_id  
    From    currency  
    Where   code = @currency_code  
  
    -- Get user id  
    Select  @pmuser_id = user_id  
    From    pmuser  
    Where   username = @username  
  
    -- Get cash list reference  
    IF @cashlist_ref IS NULL  
 Select  @cashlist_ref = 'AgentReceiptViaSAM'  
  
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
            pmuser_id,  
            base_currency_id)  
    Values (  
            @bankaccount_id,  
            2, -- receipts  
            2, -- opened  
            @cashlist_ref,  
            @company_id,  
            @sub_branch_id,  
            @currency_id,  
            GetDate(),  
            0,  
            0,  
            @pmuser_id,  
            @base_currency_id)  
  
SET @cashlist_id=@@IDENTITY  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
