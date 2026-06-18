SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'Spu_partyBank_History_Add'
GO

CREATE  PROCEDURE Spu_partyBank_History_Add
        @Party_bank_id   INT,
        @action_Code    VARCHAR(10),
        @party_cnt    INT,
        @account_id    INT = NULL,
        @account_holder_name  VARCHAR(255),
        @account_number    VARCHAR(50),
        @bank_payment_type_id   INT,
        @bank_name_id    INT = NULL ,
        @bank_branch    VARCHAR(50) = NULL ,
        @bank_branch_code    VARCHAR(50)= NULL ,
        @Bank_Add1     VARCHAR(40)= NULL,
        @Bank_Add2     VARCHAR(40)= NULL ,
        @Bank_Add3     VARCHAR(40)= NULL ,
        @Bank_Town     VARCHAR(40)= NULL ,
        @Bank_PCode    VARCHAR(20)= NULL ,
        @Bank_Region    VARCHAR(40)= NULL ,
        @Bank_Country   VARCHAR(30)=NULL,
        @CC_Num     VARCHAR(30)= NULL ,
        @CC_Start_Date   VARCHAR(30) = NULL ,
        @CC_Expiry_Date   VARCHAR(30) = NULL,
        @CC_Issue_Num    VARCHAR(2)= NULL ,
        @CC_Pin     VARCHAR(20)= NULL ,
        @Is_Registered    TINYINT = NULL ,
        @CC_Add1     VARCHAR(100)= NULL,
        @CC_Add2     VARCHAR(100) = NULL,
        @CC_Add3     VARCHAR(100) = NULL,
        @CC_Town    VARCHAR(100) = NULL,
        @CC_PCode    VARCHAR(20)= NULL,
        @CC_Country    VARCHAR(30)= NULL ,
        @user_id    INT,
        @name_on_card   VARCHAR(100)= NULL ,
        @manual_auth_number   VARCHAR(50)= NULL ,
        @account_type   VARCHAR(255)=NULL,
		@cc_tracking_number VARCHAR(255)=NULL,
        @sBusinessIdentifierCode VARCHAR(50)=NULL,
        @sInternationalBankAccountNumber VARCHAR(50)=NULL ,
		@IsDefault    TINYINT = NULL 

AS
BEGIN


IF ISNULL(@account_id,0) = 0
 SELECT @account_id = account_id from Account
 Where account_key = @Party_Cnt

INSERT INTO party_bank_History(
    party_bank_id,
    action_code,
    account_id,
    bank_payment_type_id,
    account_holder_name,
    account_number,
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
    user_id,
    date_modified,
    name_on_card,
    manual_auth_number,
    account_type,
	cc_tracking_number,
	business_identifier_code,
	international_bank_account_number,
	Is_Default
 )
    VALUES
(
    @party_bank_id,
    @action_code,
    @account_id,
    @bank_payment_type_id,
    @account_holder_name,
    @account_number,
    @bank_name_id,
    @bank_branch,
    @bank_branch_code,
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
    @user_id,
    getdate(),
    @name_on_card,
    @manual_auth_number,
    @account_type,
	@cc_tracking_number,
	@sBusinessIdentifierCode,
	@sInternationalBankAccountNumber,
	@IsDefault
 )
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

