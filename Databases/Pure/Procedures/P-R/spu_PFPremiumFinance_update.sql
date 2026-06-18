SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_PFPremiumFinance_update'
GO

CREATE PROCEDURE spu_PFPremiumFinance_update
	@FinancePlanCnt int,    
	@FinancePlanVersion int,    
	@ClientId int,    
	@AmountToFinance numeric(19,4),    
	@StartDate datetime,    
	@ClientName char(255),    
	@ClientAddr1 char(60),    
	@ClientAddr2 char(60),    
	@ClientAddr3 char(60),    
	@ClientTown char(20),    
	@ClientAddr4 char(60),    
	@ClientPCode char(8),    
	@ClientCountry int,    
	@ClientAreaCode char(10),    
	@ClientPhone char(255),    
	@ClientExtn char(10),    
	@ClientFaxCode char(10),    
	@ClientFaxNo char(10),    
	@BankName char(50),    
	@SortCode char(50),    
	@AccountNo char(50),    
	@AccountName char(255),    
	@Branch char(50),    
	@BankAddr1 char(30),    
	@BankAddr2 char(30),    
	@BankAddr3 char(30),    
	@BankTown char(25),    
	@BankAddr4 char(30),    
	@BankPCode char(8),    
	@BankCountry int,    
	@BankAreaCode char(10),    
	@BankPhoneNo char(15),    
	@BankPhoneExtn char(6),    
	@BankFaxCode char(10),    
	@BankFaxNo char(15),    
	@StatusInd char(3),    
	@AutoGenPlanRef char(20),    
	@FinCollPlanRef char(20),    
	@ClientCode char(20),    
	@TotalCost numeric(19,4),    
	@InterestFree char(1),    
	@IsQuote tinyint,    
	@IsParentPlan tinyint,    
	@Parent_Finance_cnt int,    
	@Parent_Finance_version int,    
	@PlanTransaction_id int,    
	@PFRF_ID int,    
	@FirstInstalmentdate datetime,    
	@NextInstalmentdate  datetime,    
	@lastInstalmentdate  datetime,    
	@TaxCost numeric(19,4),    
	@CCNumber varchar(30),    
	@ExpiryDate varchar(10),  
	@CardStartDate varchar(10), 
	@Issue varchar(2),    
	@ccPin varchar(20),    
	@FinanceFee numeric(19,4),    
	@DayOfWeekOrMonth int,    
	@original_amount numeric(19,4),    
	@last_instalment numeric(19,4),    
	@claim_debt_id int,    
	@user_id smallint,    
	@agent_cnt int,    
	@agent_ref varchar(50),    
	@date_created datetime,    
	@date_modified datetime,    
	@date_confirmed datetime,    
	@date_review datetime,    
	@date_laststatement datetime,    
	@date_lastgeneration datetime,    
	@final_statement_set tinyint,    
	@no_statements tinyint,    
	@statement_pffrequency_id int,    
	@review_pmwrk_task_instance_id int,    
	@auth_code varchar(50),    
	@pfprem_business_code varchar(10),    
	@sgterms int,    
	@sgref varchar(50),    
	@sgdocurl varchar(255),    
	@sgrefundtype varchar(50),    
	@limitedcompany int,    
	@is_cardholder tinyint,    
	@cardholder_name varchar(100),    
	@cardholder_address1 varchar(100),    
	@cardholder_address2 varchar(100),    
	@cardholder_address3 varchar(100),    
	@cardholder_address4 varchar(100),    
	@cardholder_postcode varchar(100),    
	@card_type varchar(50),    
	@provider_collect_deposit tinyint,    
	@datebankdetailschanged datetime,    
	@tax_group_id int,    
	@dd_cancelled tinyint = NULL,    
	@cc_cancelled tinyint = NULL,    
	@paperdd tinyint = NULL,    
	@scheme_code VARCHAR(3),    
	@scheme_type VARCHAR(50),    
	@Party_Bank_Id INT = NULL,    
	@pfpremiumfinance_cancel_reason_id Int,    
	@is_cancel_policy_run tinyint,    
	@CreatePartyBankRecord INT = 0,    
	@deposit_cc_tracking_number varchar(255) = NULL,    
	@net_amount numeric(19,4) = NULL,    
	@first_installment numeric(19,4) = NULL,    
	@other_installments numeric(19,4) = NULL,    
	@interest_rate NUMERIC(19,4)=NULL,    
	@interest_cost NUMERIC(19,4)=NULL,    
	@AccountType VARCHAR(255)=NULL,    
	@SchemeNo INT=NULL,    
	@NoOfInstallments INT=NULL,    
	@CompanyNo INT=NULL,    
	@SchemeVersion INT=NULL,    
	@SchemeName VARCHAR(50),    
	@sBusinessIdentifierCode VARCHAR(50)=NULL,    
	@sInternationalBankAccountNumber VARCHAR(50)=NULL,    
	@Deposit  numeric(19,4) = NULL,    
	@use_trans_currency TINYINT = 0,    
	@CardHolderCountry INT,    
	@PaymentMethod VARCHAR(255),    
	@bVIAPaymentHub TINYINT=0    
    

AS BEGIN
    DECLARE @BankCountryText varchar(255)
    DECLARE @ClientCountryText varchar(255)
	DECLARE @CardHolderCountryText varchar(255)
    DECLARE @is_bank INT
    SELECT @ClientCountryText = description
    FROM country
    WHERE country_id = @ClientCountry
    SELECT @BankCountryText = description
    FROM country
    WHERE country_id = @BankCountry

	SELECT 
	@CompanyNo =CompanyNo,
	@SchemeNo=SchemeNo,
	@SchemeVersion=SchemeVersion
	FROM	PFRF 
	WHERE PFRF_id=@PFRF_ID

	SELECT 	@SchemeName=SchemeName FROM PFScheme 
	WHERE SchemeNo=@SchemeNo AND SchemeVersion=@SchemeVersion

	SELECT @CardHolderCountryText = description
    FROM country
    WHERE country_id = @CardHolderCountry

	--No Need to Carryforward the Token No. if the selected scheme is not Credit Card
	DECLARE @sMediaType As Varchar(10)
    SELECT @sMediaType=Mt.code FROM PFScheme PS
	INNER JOIN MediaType MT ON MT.mediatype_id=PS.mediatype_id
	WHERE ps.SchemeNo=@SchemeNo AND Ps.CompanyNo=@CompanyNo And PS.SchemeVersion=@SchemeVersion

IF @sMediaType<>'CC'
     BEGIN
         SET @auth_code=''
     END

    UPDATE
        PFPremiumFinance
    SET
        ClientId = @ClientId,
        AmountToFinance = @AmountToFinance,
        StartDate = @StartDate,
        ClientName = @ClientName,
        ClientAddr1 = @ClientAddr1,
        ClientAddr2 = @ClientAddr2,
        ClientAddr3 = @ClientAddr3,
        ClientTown = @ClientTown,
        ClientAddr4 = @ClientAddr4,
        ClientPCode = @ClientPCode,
        ClientCountry = @ClientCountryText,
        ClientAreaCode = @ClientAreaCode,
        ClientPhoneNo = @ClientPhone,
        ClientExtension = @ClientExtn,
        ClientFaxAreaCode = @ClientFaxCode,
        ClientFaxNo = @ClientFaxNo,
        Party_bank_id = @party_Bank_Id,
        BankName = @BankName,
        BankSortCode = @SortCode,
        BankAccountNo = @AccountNo,
        BankAccountName = @AccountName,
        BankBranch = @Branch,
        BankAddr1 = @BankAddr1,
        BankAddr2 = @BankAddr2,
        BankAddr3 = @BankAddr3,
        BankTown = @BankTown,
        BankRegion = @BankAddr4,
        BankPCode = @BankPCode,
        BankCountry = @BankCountryText,
        BankAreaCode = @BankAreaCode,
        BankPhoneNo = @BankPhoneNo,
        BankExtension = @BankPhoneExtn,
        BankFaxAreaCode = @BankFaxCode,
        BankFaxNo = @BankFaxNo,
        StatusInd = @StatusInd,
        AutoGeneratedPlanRef = @AutoGenPlanRef,
        FinanceCollatedPlanRef = @FinCollPlanRef,
        ClientCode = @ClientCode,
        TotalCost = @TotalCost,
        InterestFree = @InterestFree,
        IsQuote = @IsQuote,
        IsParentPlan = @IsParentPlan,
        Parent_Finance_cnt = @Parent_Finance_cnt,
        Parent_Finance_version = @Parent_Finance_version,
        PlanTransaction_id = @PlanTransaction_id,
        PFRF_ID = @PFRF_ID,
        first_instalment_date = @FirstInstalmentdate,
        next_instalment_date = @NextInstalmentdate,
        last_instalment_date = @LastInstalmentdate,
        tax_cost = @TaxCost,
        cc_number = @CCNumber,
        cc_expiry_date = @ExpiryDate,
        cc_start_date = @CardStartDate,
        cc_issue = @Issue,
        cc_pin = @ccPin,
        FinanceFee = @FinanceFee,
        DayOfWeekOrMonth = @DayOfWeekOrMonth,
        original_amount = @original_amount ,
        last_instalment = @last_instalment,
        claim_debt_id  = @claim_debt_id,
        user_id = @user_id,
        agent_cnt = @agent_cnt,
        agent_ref = @agent_ref,
        date_created = @date_created,
        date_modified = @date_modified,
        date_confirmed = @date_confirmed,
        date_review = @date_review,
        date_laststatement = @date_laststatement,
        date_lastgeneration = @date_lastgeneration,
        final_statement_set = @final_statement_set,
        no_statements = @no_statements,
        statement_pffrequency_id  = @statement_pffrequency_id,
        review_pmwrk_task_instance_id =@review_pmwrk_task_instance_id,
        auth_code = @auth_code,
        pfprem_business_code = @pfprem_business_code,
        terms = @sgterms,
        sgref = @sgref,
        sgdocurl = @sgdocurl,
        sgrefundtype = @sgrefundtype,
        limitedcompany = @limitedcompany,
        is_cardholder = @is_cardholder,
        cardholder_name = @cardholder_name,
        cardholder_address1 = @cardholder_address1,
        cardholder_address2 = @cardholder_address2,
        cardholder_address3 = @cardholder_address3,
        cardholder_address4 = @cardholder_address4,
        cardholder_postcode = @cardholder_postcode,
        card_type = @card_type,
        provider_collect_deposit = @provider_collect_deposit,
        datebankdetailschanged = @datebankdetailschanged,
        tax_group_id = @tax_group_id,
        dd_cancelled =     @dd_cancelled,
        cc_cancelled =     @cc_cancelled,
        paperdd = @paperdd,
        scheme_code = @scheme_code,
        scheme_type = @scheme_type,
        pfpremiumfinance_cancel_reason_id=@pfpremiumfinance_cancel_reason_id,
        is_cancel_policy_run=@is_cancel_policy_run,
		deposit_cc_tracking_number = @deposit_cc_tracking_number,
		netamount = ISNULL(@net_amount, netamount),
		firstinstallment = ISNULL(@first_installment, firstinstallment),  
		othinstallments = ISNULL(@other_installments, othinstallments),
		InterestRate=ISNULL(@interest_rate,InterestRate),
		InterestCost=ISNULL(@interest_cost,InterestCost),
		SchemeNo = @SchemeNo,  
		NoOfInstallments = @NoOfInstallments,  
		CompanyNo = @CompanyNo,  
		SchemeVersion = @SchemeVersion,  
		SchemeName = ISNULL(@SchemeName, SchemeName),
		Deposit = ISNULL(@Deposit, Deposit),    
		use_trans_currency=@use_trans_currency,
		CardHolderCountry = @CardHolderCountryText ,
		business_identifier_code = @sBusinessIdentifierCode,
		international_bank_account_number = @sInternationalBankAccountNumber,
		VIAPaymentHub=@bVIAPaymentHub
	WHERE
        pfprem_finance_cnt = @FinancePlanCnt
    AND
        pfprem_finance_version = @FinancePlanVersion
	--IF  ISNULL(@party_Bank_Id,0)<>0 
	--BEGIN
 --	    UPDATE PFPremiumFinance SET  
 --           Party_bank_id = @party_bank_id WHERE  
 --           pfprem_finance_cnt = @FinancePlanCnt  
 --           AND pfprem_finance_version = @FinancePlanVersion  
	--END  
        ---------------------------------------------------

    IF EXISTS (SELECT * FROM Hidden_Options WHERE option_number = 1 and value = 'U')
    BEGIN
        IF @StatusInd = '040'
        BEGIN
  IF @CCNumber is null or @CCNumber=''
             UPDATE Insurance_File
             SET
             Insurance_File.payment_method = 'Direct Debit'
             FROM Insurance_File
             INNER JOIN PFPremiumFinance ON Insurance_File.insurance_file_cnt = PFPremiumFinance.Insurance_File_Cnt
             WHERE PFPremiumFinance.pfprem_finance_cnt = @FinancePlanCnt
             AND pfprem_finance_version = @FinancePlanVersion
			 --  AND Insurance_File.payment_method <> 'Invoice' 
  ELSE
      UPDATE Insurance_File
             SET Insurance_File.payment_method = 'Credit Card'
             FROM Insurance_File
             INNER JOIN PFPremiumFinance 
			 ON Insurance_File.insurance_file_cnt = PFPremiumFinance.Insurance_File_Cnt
             WHERE PFPremiumFinance.pfprem_finance_cnt = @FinancePlanCnt
             AND pfprem_finance_version = @FinancePlanVersion
        END
END

    IF @CreatePartyBankRecord = 1
    BEGIN
       DECLARE @bank_payment_type_id INT
	   	--Currently, BindQuote method from SAM is using this sp to store party bank information also. As per 
	   	--Progressive's requirement, if the system option 'CreditCardProcessingMethod' which has an option number 5076
	   	--is set, we have to make sure the bank payment type is 'Any' and cc_tracking_number and account_type are stored
	   	--Party_Bank table. Also Is_Registered field should be set to 1.
		--Account_type field should contain masked credit card number which has to be provided by the calling function.		
	   
		DECLARE @IsExternalCreditCardProcessing AS TINYINT
		SELECT 
			@IsExternalCreditCardProcessing=ISNULL(value,0) 
		FROM 
			system_options 
		WHERE 
			option_number=5069
		
		IF @IsExternalCreditCardProcessing=1 
			SELECT 
				@bank_payment_type_id = bank_payment_type_id  
			FROM 
				Bank_payment_type 
			WHERE 
				CODE= 'ANY'  
		ELSE
			SELECT 
				@bank_payment_type_id = bank_payment_type_id  
			FROM 
				Bank_payment_type 
			WHERE 
				CODE= 'INS'  

        IF @SortCode = '' or @SortCode is NULL
            Set @is_bank = 0
        ELSE
            Set @is_bank = 1

        EXEC spu_PartyBank_Details_Add
            @party_bank_id = @party_bank_id OUTPUT,
            @party_cnt = @ClientId,
            @account_id = 0,
            @account_holder_name = @ClientName,
            @account_number = @AccountNo,
            @bank_payment_type_id =@bank_payment_type_id,
            @is_bank = @is_Bank,
            @bank_name_id = NULL,
            @bank_branch = @Branch,
            @bank_branch_code  = @SortCode,
            @Bank_Add1 = @BankAddr1,
            @Bank_Add2 = @BankAddr2,
            @Bank_Add3 = @BankAddr3,
            @Bank_Town = @BankTown,
            @Bank_PCode = @BankPCode,
            @Bank_Region = '',
            @Bank_Country = @BankCountry,
            @CC_Num = @CCNumber,
            @CC_Start_Date = @CardStartDate  ,
            @CC_Expiry_Date = @ExpiryDate ,
            @CC_Issue_Num = @Issue,
            @CC_Pin  =@ccPin,  
            @Is_Registered =  @IsExternalCreditCardProcessing,  
            @CC_Add1 = @cardholder_address1,
            @CC_Add2 = @cardholder_address2,
            @CC_Add3 = @cardholder_address3,
            @CC_Town = NULL,
            @CC_PCode = @cardholder_postcode  ,
            @CC_Country = @BankCountry,
            @name_on_card = @cardholder_name,
            @manual_auth_number = @auth_code  ,
            @account_type  = @AccountType,
			@cc_tracking_number=@deposit_cc_tracking_number,
			@sBusinessIdentifierCode = @sBusinessIdentifierCode,
			@sInternationalBankAccountNumber = @sInternationalBankAccountNumber			

            UPDATE PFPremiumFinance SET
            Party_bank_id = @party_bank_id WHERE
            pfprem_finance_cnt = @FinancePlanCnt
            AND pfprem_finance_version = @FinancePlanVersion

EXEC Spu_partyBank_History_Add   @Party_bank_id = @Party_bank_id,
    @action_Code = 'Setup',
    @party_cnt = @ClientID,
    @account_id = 0,
    @account_holder_name = @ClientName, -- PN4375
    @account_number = @AccountNo,
    @bank_payment_type_id = @bank_payment_type_id,
    @bank_name_id = NULL,
    @bank_branch = @Branch,
    @bank_branch_code = @SortCode,
    @Bank_Add1 = @BankAddr1,
    @Bank_Add2 = @BankAddr2,
    @Bank_Add3 = @BankAddr3,
    @Bank_Town = @BankTown,
    @Bank_PCode = @BankPCode,
    @Bank_Region = '',
    @Bank_Country = @BankCountry,
    @CC_Num = @CCNumber,
    @CC_Start_Date = @CardStartDate  ,
    @CC_Expiry_Date = @ExpiryDate ,
    @CC_Issue_Num = @Issue,
    @CC_Add1 = @cardholder_address1,
    @CC_Add2 = @cardholder_address2,
    @CC_Add3 = @cardholder_address3,
    @CC_PCode = @cardholder_postcode  ,
    @CC_Country = @BankCountry,
    @user_id = 1,
    @name_on_card = @cardholder_name,
    @manual_auth_number = @auth_code,
	@CC_Pin  =@ccPin,
	@Is_Registered =  @IsExternalCreditCardProcessing ,  
	@account_type  = @AccountType,
	@cc_tracking_number=@deposit_cc_tracking_number,
	@sBusinessIdentifierCode = @sBusinessIdentifierCode,
	@sInternationalBankAccountNumber = @sInternationalBankAccountNumber

    END

	DECLARE @sProductClass AS VARCHAR(3) = ''

	SELECT @sProductClass= ProductClass FROM PFPremiumFinance WHERE pfprem_finance_cnt = @FinancePlanCnt AND pfprem_finance_version = @FinancePlanVersion

	IF @interest_rate = 0 AND @interest_cost > 0 AND @sProductClass='MTA' AND @FinancePlanVersion > 0
		BEGIN
			DECLARE @nOldInterestRate AS FLOAT = 0
			SELECT @nOldInterestRate= ISNULL(InterestRate,0)  FROM PFPremiumFinance WHERE pfprem_finance_cnt = @FinancePlanCnt AND pfprem_finance_version = @FinancePlanVersion - 1
			UPDATE PFPremiumFinance SET InterestRate=@nOldInterestRate FROM PFPremiumFinance WHERE pfprem_finance_cnt=@FinancePlanCnt AND pfprem_finance_version = @FinancePlanVersion
    END
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
