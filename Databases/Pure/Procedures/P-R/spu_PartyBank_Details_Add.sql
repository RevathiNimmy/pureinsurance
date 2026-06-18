SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PartyBank_Details_Add'
GO

CREATE PROCEDURE spu_PartyBank_Details_Add
	   @party_bank_id 							INT	OUTPUT,
	    @party_cnt 								INT,
        @account_id             				INT = NULL,
	    @account_holder_name					VARCHAR(50),
	    @account_number 						VARCHAR(50),
	    @bank_payment_type_id 					INT,
	    @is_bank 								TINYINT,
	    @bank_name_id 							INT,
	    @bank_branch 							VARCHAR(50) ,
	    @bank_branch_code 						VARCHAR(50) ,
	    @Bank_Add1 								VARCHAR(40),
	    @Bank_Add2 								VARCHAR(40),
	    @Bank_Add3 								VARCHAR(40),
	    @Bank_Town 								VARCHAR(40),
	    @Bank_PCode 							VARCHAR(20),
	    @Bank_Region 							VARCHAR(40),
	    @Bank_Country							VARCHAR(30),
	    @CC_Num 								VARCHAR(30),
	    @CC_Start_Date 							VARCHAR(10),
	    @CC_Expiry_Date 						VARCHAR(10),
	    @CC_Issue_Num 							VARCHAR(2),
	    @CC_Pin 								VARCHAR(20),
	    @Is_Registered 							TINYINT,
	    @CC_Add1 								VARCHAR(100),
	    @CC_Add2 								VARCHAR(100),
	    @CC_Add3 								VARCHAR(100),
    	@CC_Town								VARCHAR(100),
	    @CC_PCode								VARCHAR(20),
	    @CC_Country								VARCHAR(30),
	    @name_on_card							VARCHAR(100),
	    @manual_auth_number						VARCHAR(50),
	    @account_type							VARCHAR(255)=NULL,
		@cc_tracking_number						VARCHAR(255)=NULL,
		@sBusinessIdentifierCode				VARCHAR(50)=NULL,
		@sInternationalBankAccountNumber	    VARCHAR(50)=NULL,
		@IsDefault								TINYINT=0,  
  		@UserId int = null,  
  		@UniqueId VARCHAR(50) = null,  
  		@ScreenHierarchy VARCHAR(500) = null

AS
BEGIN

IF ISNULL(@account_id,0) = 0
	SELECT @account_id = account_id FROM Account
	WHERE account_key = @Party_Cnt
if @CC_Num <>'' AND @bank_name_id=0
	BEGIN
	SET @bank_name_id=NULL
	END
IF NOT EXISTS(SELECT NULL FROM Party_Bank 
WHERE ISNULL(is_bank,0)=0 AND ISNULL(is_default,0)=1 AND account_id=@account_id)
SELECT @IsDefault=1

INSERT INTO party_bank(
		account_id,
		bank_payment_type_id,
		account_holder_name,
		account_number,
		is_bank,
		bank_name_id,
		bank_branch,
		bank_branch_code,
		bank_add1,
		bank_add2,
		bank_add3,
		bank_town,
		bank_pcode,
		bank_region,
		bank_country,
		cc_num,
		cc_start_date,
		cc_expiry_date,
		cc_issue_num,
		cc_pin,
		is_registered,
		cc_add1,
		cc_add2,
		cc_add3,
		cc_town,
		cc_pcode,
		cc_country,
		is_deleted,
		name_on_card,
		manual_auth_number,
		account_type,
		cc_tracking_number,
		business_identifier_code,
		international_bank_account_number,
		is_default,
		UserId,  
  		UniqueId,  
  		ScreenHierarchy
	)
VALUES (    @account_id,
	    	@bank_payment_type_id,
	    	@account_holder_name,
	    	LTRIM(RTRIM(@account_number)),
	    	@is_bank,
	    	@bank_name_id,
	    	@bank_branch,
	    	LTRIM(RTRIM(@bank_branch_code)),
	    	@Bank_Add1,
	    	@Bank_Add2,
	    	@Bank_Add3,
	    	@Bank_Town,
	    	@Bank_PCode,
	    	@Bank_Region,
	    	@Bank_Country,
	    	@CC_Num,
	    	@CC_Start_Date,
	    	@CC_Expiry_Date,
	    	@CC_Issue_Num,
	    	@CC_Pin,
	    	@Is_Registered,
	    	@CC_Add1,
	    	@CC_Add2,
	    	@CC_Add3,
    	    @CC_Town,
	    	@CC_PCode,
	    	@CC_Country,
	    	0,
			@name_on_card,
			@manual_auth_number,
	    	@account_type,
			@cc_tracking_number,
			@sBusinessIdentifierCode,
			@sInternationalBankAccountNumber,
			@IsDefault, 
   			@UserId,  
   			@UniqueId,  
   			@ScreenHierarchy
	)
SELECT @Party_Bank_id = SCOPE_IDENTITY()
END

