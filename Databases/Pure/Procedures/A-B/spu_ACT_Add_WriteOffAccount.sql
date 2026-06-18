SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Add_WriteOffAccount'
GO

CREATE PROCEDURE spu_ACT_Add_WriteOffAccount
	@short_code CHAR(20),
	@Account_ID INT OUTPUT
AS

DECLARE  
    @purgefrequency_id smallint,  
    @accountstatus_id smallint,
    @company_id smallint,      
    @sub_branch_id int, 
    @currency_id smallint,  
    @accounttype_id smallint,  
    @paymenttype_id smallint,  
    @ledger_id smallint,  
    @address_country smallint, 
    @bank_country smallint,
    @payment_expiry_date datetime,
    @ElementID int,@node_id int, 
    @AccID_tmp int , @Parent_node_id int,@element_id int 

DECLARE @value varchar(10)	

IF Exists(Select Account_id from account where short_code=@short_code)
BEGIN 
	Select @Account_ID=Account_id from account where short_code=@short_code 
	RETURN
END
	
Set @payment_expiry_date=getdate()
SELECT @value=value from system_options where option_number=152

SELECT @purgefrequency_id=purgefrequency_id ,  
    @accountstatus_id =accountstatus_id,
    @company_id=company_id,      
    @sub_branch_id =sub_branch_id, 
    @currency_id =currency_id,  
    @accounttype_id =accounttype_id,  
    @paymenttype_id =paymenttype_id,  
    @ledger_id =ledger_id,  
    @address_country=address_country, 
    @bank_country =bank_country, @AccID_tmp=Account_ID
    FROM Account where short_code=@Value
    AND company_id=1


EXEC spu_ACT_add_Account @Account_id = @Account_ID output, 
@company_id = @company_id, @purgefrequency_id = @purgefrequency_id, @accounttype_id =@accounttype_id , 
@paymenttype_id = @paymenttype_id, @currency_id = @currency_id, @ledger_id = @ledger_id, @account_name = @short_code, 
@short_code = @short_code, @restrict_enquiry = 0, @restrict_update = 0, 
@delete_at_purge = 0, @contact_name = '', @address1 = '', @address2 = '', @address3 = '', @address4 = '', 
@postal_code = '', @address_country = @address_country, @phone_area_code = '', @phone_number = '', 
@phone_extension = '', @fax_area_code = '', @fax_number = '', @fax_extension = '', @payment_name = '',
@payment_account_code = '', @payment_branch_code = '', 
@payment_expiry_date = @payment_expiry_date,
@payment_reference1 = '', @payment_reference2 = '', @prooflist_report_id = NULL, 
@bordereau_report_id = NULL, @credit_limit = 0, 
@discount_percentage = 0.0, @settlement_period = 0, 
@bank_name = '', @bank_address1 = '', @bank_address2 = '', @bank_address3 = '', @bank_address4 = '',
@bank_postal_code = '', @bank_country = @bank_country, @bank_phone_area_code = ' ', @bank_phone_number = ' ', 
@bank_phone_extension = '', @bank_fax_area_code = '', @bank_fax_number = '', @bank_fax_extension = '',
@comments = '',@account_key = 0, @nominal_account_id = 0, @accountstatus_id = 1, 
@sub_branch_id = 1, @allow_electronic_payment = 0, @client_money_calc_account_type = 0, 
@client_bank_account_type = NULL


EXEC spu_ACT_Add_Element @element_id=@Element_ID OUTPUT, @element_name=@short_code

Select @Parent_node_id=Parent_node_Id from structuretree where account_id=@AccID_tmp

EXEC spu_ACT_add_StructureTree @node_id=@node_id  OUTPUT,  
    @company_id =@company_id,  
    @mapping_id  = NULL,  
    @account_id  = @Account_ID,  
    @element_id  = @element_id,  
    @parent_node_id  = @Parent_node_id  

exec spe_ElementExtras_add @element_id = @element_id, @totalling_id = 0, @description = '', @report_map_id = 0, @account_map_id = 0, @Is_deletable = 1