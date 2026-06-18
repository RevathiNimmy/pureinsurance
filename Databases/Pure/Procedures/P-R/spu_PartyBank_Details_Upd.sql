SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PartyBank_Details_Upd'
GO

CREATE PROCEDURE spu_PartyBank_Details_Upd  
        @party_bank_id							INT,  
        @party_cnt								INT,  
        @account_id								INT = NULL, -- PN 4154 
        @account_holder_name					VARCHAR(50),  
        @account_number							VARCHAR(50),  
        @bank_payment_type_id					INT,  
        @is_bank								TINYINT,  
        @bank_name_id							INT,  
        @bank_branch							VARCHAR(50) ,  
        @bank_branch_code						VARCHAR(50) ,  
        @Bank_Add1								VARCHAR(40),  
        @Bank_Add2								VARCHAR(40),  
        @Bank_Add3								VARCHAR(40),  
        @Bank_Town								VARCHAR(40),  
        @Bank_PCode								VARCHAR(20),  
        @Bank_Region							VARCHAR(40),  
        @Bank_Country							VARCHAR(30),  
        @CC_Num									VARCHAR(30),  
        @CC_Start_Date							VARCHAR(10),  
        @CC_Expiry_Date							VARCHAR(10),  
        @CC_Issue_Num							VARCHAR(2),  
        @CC_Pin									VARCHAR(20),  
        @Is_Registered							TINYINT,  
        @CC_Add1								VARCHAR(100),  
        @CC_Add2								VARCHAR(100),  
        @CC_Add3								VARCHAR(100),  
        @CC_Town								VARCHAR(100),  
        @CC_PCode								VARCHAR(20),  
        @CC_Country								VARCHAR(30),  
        @name_on_card							VARCHAR(100),  
        @manual_auth_number						VARCHAR(50),  
        @account_type							VARCHAR(255)=NULL,
        @UpdatePFPremiumFinance					INT = 0,
	    @IsEdited								bit =0	OUTPUT,
		@sBusinessIdentifierCode				VARCHAR(50)=NULL,
		@sInternationalBankAccountNumber	    VARCHAR(50)=NULL,
		@IsDefault								TINYINT,
		@UserId 								int = null,  
		@UniqueId 								varchar(50) = null,  
		@ScreenHierarchy 						varchar(500) = null  
  
AS 
 
SELECT 1 FROM party_bank 
WHERE
	Party_Bank_Id=@Party_Bank_Id AND 
	ISNULL(account_holder_name,'') = ISNULL(@account_holder_name,'') AND  
	ISNULL(account_number,'') = ISNULL(@account_number,'') AND  
	ISNULL(is_bank,0) = ISNULL(@is_bank,0) AND  
	ISNULL(bank_name_id,'') = ISNULL(@bank_name_id,'') AND  
	ISNULL(bank_branch,'') = ISNULL(@bank_branch,'') AND  
	ISNULL(bank_branch_code,'') = ISNULL(@bank_branch_code,'') AND  
	ISNULL(bank_add1,'') = ISNULL(@bank_add1,'') AND  
	ISNULL(bank_add2,'') = ISNULL(@bank_add2,'') AND  
	ISNULL(bank_add3,'') = ISNULL(@bank_add3,'') AND  
	ISNULL(bank_town,'') = ISNULL(@bank_town,'') AND  
	ISNULL(bank_pcode,'') = ISNULL(@bank_pcode,'') AND  
	ISNULL(bank_region,'') = ISNULL(@bank_region,'')  AND  
	ISNULL(bank_country,'') = ISNULL(@bank_country,'') AND  
	ISNULL(cc_num,'') = ISNULL(@CC_Num,'') AND  
	ISNULL(cc_start_date,'') = ISNULL(@cc_start_date,'') AND  
	ISNULL(cc_expiry_date,'') = ISNULL(@cc_expiry_date,'') AND  
	ISNULL(cc_issue_num,'') = ISNULL(@cc_issue_num,'') AND  
	ISNULL(cc_pin,'') =ISNULL(@cc_pin,'') AND  
	ISNULL(is_registered,0) = ISNULL(@is_registered,0) AND  
	ISNULL(cc_add1,'') = ISNULL(@cc_add1,'') AND  
	ISNULL(cc_add2,'') = ISNULL(@cc_add2,'') AND  
	ISNULL(cc_add3,'') = ISNULL(@cc_add3,'') AND  
	ISNULL(cc_town,'') = ISNULL(@cc_town,'') AND  
	ISNULL(cc_pcode,'') = ISNULL(@cc_pcode,'') AND  
	ISNULL(cc_country,'') = ISNULL(@cc_country,'') AND  
	ISNULL(name_on_card,'') = ISNULL(@name_on_card,'') AND  
	ISNULL(manual_auth_number,'') = ISNULL(@manual_auth_number,'') AND  
	ISNULL(account_type,'') = ISNULL(@account_type,'') AND
	ISNULL(business_identifier_code,'') = ISNULL(@sBusinessIdentifierCode,'') AND
	ISNULL(international_bank_account_number,'') = ISNULL(@sInternationalBankAccountNumber,'')


	
IF @@ROWCOUNT=0
BEGIN
	SET @IsEdited=1

	IF NOT EXISTS(SELECT NULL FROM Party_Bank 
WHERE ISNULL(is_bank,0)=0 AND ISNULL(is_default,0)=1 )
SELECT @IsDefault=1

	UPDATE party_bank  
	SET  
	     account_holder_name = @account_holder_name,  
	     account_number = LTRIM(RTRIM(@account_number)),  
	     is_bank = @is_bank,  
	     bank_name_id = @bank_name_id,  
	     bank_branch = @bank_branch,  
	     bank_branch_code = LTRIM(RTRIM(@bank_branch_code)),  
	     bank_add1 = @bank_add1,  
	     bank_add2 = @bank_add2,  
	     bank_add3 = @bank_add3,  
	     bank_town = @bank_town,  
	     bank_pcode = @bank_pcode,  
	     bank_region = @bank_region ,  
	     bank_country = @bank_country,  
	     cc_num = @CC_Num,  
	     cc_start_date = @cc_start_date,  
	     cc_expiry_date = @cc_expiry_date,  
	     cc_issue_num = @cc_issue_num,  
	     cc_pin =@cc_pin,  
	     is_registered = @is_registered,  
	     cc_add1 = @cc_add1,  
	     cc_add2 = @cc_add2,  
	     cc_add3 = @cc_add3,  
	     cc_town = @cc_town,  
	     cc_pcode = @cc_pcode,  
	     cc_country = @cc_country,  
	     name_on_card = @name_on_card,  
	     manual_auth_number = @manual_auth_number,  
	     account_type = @account_type,
		 business_identifier_code = @sBusinessIdentifierCode,
		 international_bank_account_number = @sInternationalBankAccountNumber,
		 is_default=@IsDefault,
   		 UserId = @UserId,
   		 UniqueId = @UniqueId,
   		 ScreenHierarchy = @ScreenHierarchy
	  
	WHERE Party_Bank_Id = @Party_Bank_Id  
	  
		SELECT @Party_Cnt = A.account_Key  
	FROM   Party_Bank PB  
	INNER JOIN  ACCOUNT A  
	ON   A.account_id = PB.account_id  
END

-- Call the SP to update Bank Details for live installments plans held against this party on the
  
IF Exists (Select 1 from Party_Bank
           Where party_bank_id = @party_bank_id) 
		   AND @IsEdited=1

	EXEC Spu_pfPremiumFinance_bankDet_upd @party_bank_id = @party_bank_id,
	                                      @sAccount_type=@account_type ,
										  @nBank_payment_type_id=@bank_payment_type_id

IF ISNULL(@party_cnt,0)<>0 
BEGIN

DECLARE @nAccountID AS INT

SELECT @nAccountID =account_id FROM ACCOUNT WHERE account_key=@party_cnt

IF NOT EXISTS(SELECT NULL FROM Party_Bank 
				WHERE ISNULL(is_bank,0)=0 AND ISNULL(is_default,0)=1 AND account_id = @nAccountID)
		UPDATE party_bank  
	    SET   is_default=1 WHERE Party_Bank_Id = @Party_Bank_Id  

ELSE
BEGIN
UPDATE party_bank  
SET   is_default=0 WHERE Party_Bank_Id <> @Party_Bank_Id  AND  account_id = @nAccountID

UPDATE party_bank  
	    SET   is_default=1 WHERE Party_Bank_Id = @Party_Bank_Id 		
END
END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
