SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PFPremiumFinance_Import_Amendment'
GO
 CREATE  PROCEDURE spu_PFPremiumFinance_Import_Amendment    
    
    @pfprem_finance_cnt INT,    
    @pfprem_finance_version INT,    
    @action_code VARCHAR(50),    
    @PlanBankSortCode VARCHAR(8),    
    @PlanBankAccountNumber VARCHAR(31),    
    @PlanBankAccountName VARCHAR(31),    
    @PlanBankName VARCHAR(60),    
    @PlanBankAddress1 VARCHAR(31),    
    @PlanBankAddress2 VARCHAR(31),    
    @PlanBankAddress3 VARCHAR(31),    
    @PlanBankAddress4 VARCHAR(31),    
    @PlanBankPostalCode VARCHAR(8),    
    @PlanBankCountryCode VARCHAR(31),    
    @PlanCCNumber VARCHAR(30),    
    @PlanCCExpiryDate VARCHAR(10),    
    @PlanCCStartDate VARCHAR(10),    
    @PlanCCIssue VARCHAR(2),    
    @PlanCCPin VARCHAR(20),    
    @statusInd VARCHAR(10),    
    @dd_cancelled tinyint,    
    @cc_cancelled tinyint,    
    @paperdd tinyint = NULL,    
    @userID int = NULL,
    @sBusinessIdentifierCode VARCHAR(50)=NULL,
    @sInternationalBankAccountNumber VARCHAR(50)=NULL
	    
AS    
    
BEGIN    
DECLARE @Account_id INT    
DECLARE @Party_bank_id INT    
DECLARE @Country_id INT,    
    @Is_BANK INT,    
    @Count_InstancePartyBank INT ,    
    @Account_holder_name VARCHAR(50),    
    @bank_payment_type_id INT,    
    @bank_name_id INT,    
    @Bank_Town VARCHAR(50),    
    @Bank_Region VARCHAR(50),    
    @Account_Type VARCHAR(255),    
    @manual_auth_number VARCHAR(50),    
    @bank_branch VARCHAR(50),    
    @NoofInstances INT,
    @sbank_name VARCHAR(100)  
  
DECLARE @sOldBankName VARCHAR(60)  
   
    
 -- store the existing history as setup if no previous history item exists    
 EXEC spu_PFMediaTypeHistory_Add_SetupEntry @pfprem_finance_cnt,@pfprem_finance_version    
    
 DECLARE @existingpaperdd TINYINT    
 SELECT @existingpaperdd = paperdd    
 FROM pfpremiumfinance    
 WHERE pfprem_finance_cnt = @pfprem_finance_cnt    
 AND pfprem_finance_version = @pfprem_finance_version    
    
 IF @paperdd IS NULL    
  SET @paperdd = @existingpaperdd    
    
 IF EXISTS(SELECT 1 FROM pfpremiumfinance  
 INNER JOIN party_agent ON  pfpremiumfinance.agent_cnt=party_agent.party_cnt  
 WHERE pfprem_finance_cnt=@pfprem_finance_cnt  
 AND party_agent.is_single_instalment_plan=1 )  
 BEGIN  
   SELECT @Account_id = Account_ID FROM Account  
   JOIN Party ON Account_key = Party.Party_Cnt  
   JOIN PFPremiumFinance ON  
    PFPremiumFinance.Agent_cnt = PArty.PArty_Cnt  
   WHERE PFPrem_Finance_Cnt = @pfprem_finance_cnt  
 END  
 ELSE  
 BEGIN  
 
 SELECT @Account_id = Account_ID FROM Account    
 JOIN Party ON Account_key = Party.Party_Cnt    
 JOIN PFPremiumFinance ON    
  PFPremiumFinance.ClientID = PArty.PArty_Cnt    
 WHERE PFPrem_Finance_Cnt = @pfprem_finance_cnt    
END    
 --Get the Party_bank instancs of Type Instalment/Any for a party    
 SELECT @Count_InstancePartyBank = COUNT(*) FROM Party_Bank    
 JOIN bank_payment_type ON    
 bank_payment_type.bank_payment_type_id = Party_bank.bank_payment_type_id    
 WHERE Account_id= @Account_id AND    
 bank_payment_type.Code IN ( 'ANY','INS')    
    
  
EXEC spu_Get_PartyBank_instances @pfprem_finance_cnt, @NoofInstances OUTPUT   
    SELECT @sOldBankName=BankName FROM pfpremiumFinance WHERE pfprem_finance_cnt=@pfprem_finance_cnt AND pfprem_finance_version=@pfprem_finance_version  
 -- update the current details to reflect the new data    
     UPDATE pfpremiumfinance    
     SET    
          pfprem_finance_cnt = @pfprem_finance_cnt,    
          pfprem_finance_version = @pfprem_finance_version,    
          BankSortCode = @PlanBankSortCode,    
          BankAccountNo = @PlanBankAccountNumber,    
          BankAccountName = @PlanBankAccountName,    
          BankName= CASE  
          WHEN @PlanBankName <>'' THEN @PlanBankName  
          ELSE @sOldBankName END,  
          BankAddr1 = @PlanBankAddress1,    
          BankAddr2 = @PlanBankAddress2,    
          BankAddr3 = @PlanBankAddress3,    
          BankRegion = @PlanBankAddress4,    
          BankPCode = @PlanBankPostalCode,    
          BankCountry = @PlanBankCountryCode,    
          CC_Number = @PlanCCNumber,    
          CC_Expiry_Date = @PlanCCExpiryDate,    
          CC_Start_Date = @PlanCCStartDate,    
          CC_Issue = @PlanCCIssue,    
          CC_Pin = @PlanCCPin,    
          statusInd = @statusInd,    
          dd_cancelled = @dd_cancelled,    
          cc_cancelled = @cc_cancelled,    
          paperdd = @paperdd,    
          date_modified = convert(char(10), getdate(), 121),    
          datebankdetailschanged = convert(char(10), getdate(), 121),
		  business_identifier_code=@sBusinessIdentifierCode,
		  international_bank_account_number=@sInternationalBankAccountNumber    
    
     WHERE pfprem_finance_cnt = @pfprem_finance_cnt    
         AND pfprem_finance_version = @pfprem_finance_version    
    
 -- store the latest changes to the history table    
EXEC spu_PFMediaTypeHistory_Add @pfprem_finance_cnt,@pfprem_finance_version, @action_code    
    
IF @NoofInstances > 1    
BEGIN    
    UPDATE PFPremiumFinance SET Party_bank_id = NULL WHERE pfprem_finance_cnt = @pfprem_finance_cnt    
END    
    
 IF @NoofInstances = 1    
 BEGIN    
  --SELECT @Party_bank_id = Party_bank_id FROM Party_bank WHERE Account_id = @account_id    
  	SELECT @Party_bank_id = Party_bank_id FROM PFPremiumFinance WHERE PFPrem_Finance_cnt = @pfprem_finance_cnt
    
    IF (RTRIM(@PlanCCNumber) ='' OR @PlanCCNumber IS NULL) AND  ( @PlanBankAccountNumber <> '' OR  @PlanBankAccountNumber IS  NOT NULL)    
        SET @is_bank = 1    
    ELSE    
        SET @is_bank =  0    
    
  SELECT @Country_id = Country_id FROM Country WHERE Code = RTRIM(@PlanBankCountryCode)    
    
    IF @Country_id IS NULL OR  @Country_id = 0    
        IF @is_bank = 1    
            SELECT @Country_id = bank_country FROM Party_bank WHERE party_bank_id = @party_bank_id    
        IF @is_bank = 0    
            SELECT @Country_id = cc_country FROM Party_bank WHERE party_bank_id = @party_bank_id    
    
  SELECT @Account_holder_name = Account_holder_name,    
     @bank_payment_type_id = bank_payment_type_id,    
     @bank_name_id = bank_name_id,    
     @Bank_Region = Bank_Region,    
     @Account_Type = Account_Type,    
     @manual_auth_number = manual_auth_number,    
     @bank_branch = Bank_branch    
     FROM party_bank WHERE Party_bank_id = @Party_bank_id    
    
  IF @is_bank = 1    
  BEGIN    
  SELECT @sbank_name= CASE  
  WHEN @PlanBankName <>'' THEN @PlanBankName  
  ELSE @sOldBankName END  

  UPDATE party_bank    
  SET    
        account_number = @PlanBankAccountNumber,    
        bank_branch_code = @PlanBankSortCode,    
        is_bank = 1,    
        Bank_Name= @sbank_name,  
       -- bank_country = @Country_id,    
        bank_add1 = @PlanBankAddress1,    
        bank_add2 = @PlanBankAddress2,    
        bank_add3 = @PlanBankAddress3,
        bank_town = @PlanBankAddress4,  
	    bank_pcode= @PlanBankPostalCode,  
	    bank_country=ISNULL(@PlanBankCountryCode,0),  
	    bank_region= CASE  
     	WHEN ISNULL(@PlanBankPostalCode,'')='' THEN ''  
        ELSE  
   		@Bank_Region  
        END,  
        Account_holder_name = @PlanBankAccountName,
		business_identifier_code = @sBusinessIdentifierCode,
		international_bank_account_number = @sInternationalBankAccountNumber     
        WHERE Party_Bank_Id = @Party_Bank_Id
    
    
    EXEC Spu_partyBank_History_Add    
        @Party_bank_id = @Party_bank_id,    
        @bank_branch_code = @PlanBankSortCode,    
        @action_Code = @Action_Code,    
        @party_cnt  = NULL,    
        @account_id = @Account_id,    
        @account_holder_name = @PlanBankAccountName,        
        @account_number = @PlanBankAccountNumber,    
        @bank_payment_type_id = @bank_payment_type_id,    
        @bank_name_id = @bank_name_id ,    
        @Bank_Add1  = @PlanBankAddress1,    
        @Bank_Add2  = @PlanBankAddress2,    
        @Bank_Add3  = @PlanBankAddress3,    
        @Bank_Town  =   @Bank_Town,    
        @Bank_PCode =   @PlanBankPostalCode,    
        @Bank_Region =  @Bank_Region,    
        @user_id =      @userid,    
        @account_type = @account_type,    
        @Bank_Branch = @Bank_Branch,
        @sBusinessIdentifierCode = @sBusinessIdentifierCode,
        @sInternationalBankAccountNumber = @sInternationalBankAccountNumber   
  END    
  IF @is_bank = 0    
  BEGIN    
    UPDATE party_bank    
    SET    
        is_bank = 0,    
        cc_country = @Country_id,    
        cc_num = @PlanCCNumber,    
        cc_start_date = @PlanCCStartDate,    
        cc_expiry_date = @PlanCCExpiryDate,    
        cc_issue_num = @PlanCCIssue,    
        cc_pin =@PlanCCPin,    
        cc_add1 = @PlanBankAddress1,    
        cc_add2 = @PlanBankAddress2,    
        cc_add3 = @PlanBankAddress3,    
        cc_pcode =@PlanBankPostalCode,  
        Account_holder_name = @PlanBankAccountName  
     WHERE Party_Bank_Id = @Party_Bank_Id    
    
    EXEC Spu_partyBank_History_Add    
        @Party_bank_id = @Party_bank_id,    
        @bank_branch_code = @PlanBankSortCode,    
        @action_Code = @Action_Code,    
        @party_cnt  = NULL,    
        @account_id = @Account_id ,    
        @account_holder_name = @PlanBankAccountName,  
	@account_number = @PlanBankAccountNumber,     
        @bank_payment_type_id =@bank_payment_type_id,    
        @CC_Num   = @PlanCCNumber,    
        @CC_Start_Date = @PlanCCStartDate,    
        @cc_expiry_date = @PlanCCExpiryDate,    
        @cc_issue_num = @PlanCCIssue,    
        @CC_Add1   = @PlanBankAddress1,    
        @CC_Add2   = @PlanBankAddress2,    
        @CC_Add3   = @PlanBankAddress3,    
        @CC_Town  =   @Bank_Town,    
        @CC_PCode = @PlanBankPostalCode,    
        @account_type = @Account_type,    
        @manual_auth_number = @manual_auth_number,    
        @user_id = @userid,
        @sBusinessIdentifierCode = @sBusinessIdentifierCode,
        @sInternationalBankAccountNumber = @sInternationalBankAccountNumber    
    
     END    
 END    
END    
  
