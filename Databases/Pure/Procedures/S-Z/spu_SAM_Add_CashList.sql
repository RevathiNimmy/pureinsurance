SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Add_CashList'
GO

CREATE  PROCEDURE spu_SAM_Add_CashList
    @username varchar(255),
    @cashlist_ref varchar(25),
    @bankaccount_code varchar(25),      
    @cashlisttype_id int,
    @currency_id int,
    @cashliststatus_id int,
    @list_date datetime,
	@bankAccount_name VArchar(25)= Null,  
    @source_id INT =0,
    @cashlist_id int output,
    @nbankAccount_id int = null,
	@subbranch_id int = 0  
AS
BEGIN

    DECLARE
        @bankaccount_id int,
        @company_id int,
        @sub_branch_id int,
        @pmuser_id int,
        @base_currency_id int

    -- Get info from bank account
 if (IsNull(@bankAccount_name,'') <> '')  
 Begin  
     SELECT  @bankaccount_id = bankAccount_id,      
             @company_id  = company_id,      
             @sub_branch_id = sub_branch_id      
     FROM    bankaccount      
     WHERE   bank_account_name = @bankAccount_name      
 End  
 Else  
 Begin  
    SELECT  @bankAccount_id = bankAccount_id,
            @company_id  = company_id,
            @sub_branch_id = sub_branch_id
    FROM    bankaccount
    WHERE   code = @bankaccount_code
 End  

If ISNULL(@nbankAccount_id,0) = 0
BEGIN
	SELECT @nbankAccount_id = @bankAccount_id
END   
   
 IF @source_id<>0
 BEGIN
    SELECT @company_id =@source_id
    SELECT @sub_branch_id=@source_id
 END
 
  IF @subbranch_id <> 0   
 BEGIN   
 SET @sub_branch_id=@subbranch_id  
 END 

    -- Get user id
    SELECT  @pmuser_id = user_id
    FROM    pmuser
    WHERE   username = @username

   -- Get base currency id
    SELECT  @base_currency_id = base_currency_id
    FROM    source
    WHERE   source_id = @company_id

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
                           pmuser_id,
                           base_currency_id)
    VALUES (
            @nbankAccount_id,
            @cashlisttype_id,
            @cashliststatus_id,
            @cashlist_ref,
            @company_id,
            @sub_branch_id,
            @currency_id,
            @list_date,
            0,
            0,
            @pmuser_id,
            @base_currency_id)

SET @cashlist_id=@@IDENTITY
END

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
  
