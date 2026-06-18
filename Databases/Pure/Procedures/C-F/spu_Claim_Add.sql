SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_Add'
GO

CREATE PROCEDURE spu_Claim_Add  
    @Claim_id INT OUTPUT,  
    @Policy_id INT,  
    @Policy_Number VARCHAR(30),  
    @Description VARCHAR(1000),  
    @Claim_Status_id INT,  
    @Progress_Status_id INT,  
    @Primary_Cause_id INT,  
    @Secondary_Cause_id INT,  
    @Catastrophe_code_id INT,  
    @Loss_from_date DATETIME,  
    @Loss_to_date DATETIME,  
    @Reported_date DATETIME,  
    @Reported_to_date DATETIME,  
    @Last_modified_date DATETIME,  
    @Handler_id INT,  
    @Currency_id INT,  
    @Info_only BIT,  
    @Likely_claim BIT,  
    @Location VARCHAR(50),  
    @Town INT,  
    @Risk_type_id INT,  
    @Client_name VARCHAR(255),  
    @Client_address INT,  
    @Client_tel_no VARCHAR(255),  
    @Client_fax_no VARCHAR(255),  
    @Client_mobile_no VARCHAR(255),  
    @Client_email VARCHAR(255),  
    @Client_claim_number VARCHAR(20),  
    @Insurer_name VARCHAR(255),  
    @insurer_address INT,
    @insurer_tel_no VARCHAR(255) = NULL,
    @insurer_fax_no VARCHAR(255)=NULL,
    @insurer_email VARCHAR(255)=NULL,
    @insurer_claim_number VARCHAR(20)=NULL,
    @insurer_contact VARCHAR(255)=NULL, 
    @VAT_registered INT,  
    @VAT_reg_no VARCHAR(20),  
    @Comments text,  
    @Claims_Status_Date DATETIME,  
    @Client_Short_name CHAR(20),  
    @Insurer_Short_name CHAR(20),  
    @Client_tel_no_off VARCHAR(255),  
    @UserDefFldA INT,  
    @UserDefFldB INT,  
    @UserDefFldC INT,  
    @UserDefFldD INT,  
    @UserDefFldE INT,  
    @Claim_Number VARCHAR(30),  
    @Branch_Id INT,  
    @underwriting_year_id INT,  
    @driver_title as varchar(255),  
    @driver_forename as varchar(255),  
    @driver_surname as varchar(255),  
    @date_passed_test as datetime,  
    @employee_title as varchar(255),  
    @employee_forename as varchar(255),  
    @employee_surname as varchar(255),  
    @employee_length_of_service as int,  
    @employee_previous_claim as tinyint,  
    @employee_previous_claim_details as varchar(255),  
    @ulr as tinyint,  
    @recovery_agent as varchar(255),  
    @solicitor_appointed as tinyint,  
    @solicitor_name as varchar(255),  
    @ulr_loss_details as varchar(255),  
    @claim_at_fault_id as int,  
    @bonus_affected as tinyint,  
    @policy_deductible_id as int,  
    @non_standard_excess as numeric(19,4),  
    @subsidiary_company_name as varchar(255), 
    @created_by_id  int,
    @claim_handled int,
    @base_case_id int=NULL,
    @tpa int =NULL
  
AS  
  
DECLARE @suppress_reserves int  
DECLARE @suppress_payments int  
DECLARE @suppress_recoveries int  
  
DECLARE @BrkUnderwrite CHAR  
DECLARE @Clientid INT  
DECLARE @cnt INT  
DECLARE @Clm_No VARCHAR(40)  
  
DECLARE @Policyid INT  
  
DECLARE @Check INT  
  
DECLARE @Year CHAR(4)  
DECLARE @ClaimCount VARCHAR(5)  
DECLARE @ClientCount VARCHAR(5)  
  
DECLARE @Exclude VARCHAR(1)  
DECLARE @ClientCountLength INT  
DECLARE @UnderWritingYearID INT   
/*Is this a broking or underwriting system*/  
SELECT  
    @BrkUnderwrite = value  
FROM hidden_options  
WHERE branch_id = 1  
AND option_number = 1  

IF @tpa=0
  SELECT @tpa=NULL


  
IF @Policy_ID > 0 
 BEGIN
	IF @Policy_Number=''  
		BEGIN
			SELECT @Policy_Number = insurance_ref ,@UnderWritingYearID=underwriting_year_id FROM insurance_file  
			WHERE insurance_file_cnt = @Policy_ID  
	    END
	 ELSE
		BEGIN
			SELECT @UnderWritingYearID=underwriting_year_id FROM insurance_file  
			WHERE insurance_file_cnt = @Policy_ID  
	 END
 END
  
IF @BrkUnderwrite='A'  
BEGIN  
    /*Broking*/  
  
    SELECT  
        @Clientid = insured_cnt  
    FROM insurance_file  
    WHERE insurance_file_cnt = @Policy_id  
  
    /*Get year used as first part of auto numbering*/  
    SELECT  
        @Year = value  
    FROM system_options  
    WHERE option_number =2008  
    AND branch_id = @Branch_Id  
  
    /*Get total claim count for second part of auto numbering*/  
    IF NOT EXISTS  
        (  
            SELECT  
                NULL  
            FROM claim  
            WHERE SUBSTRING(claim_number,1,5) = @Year + '/'  
        )  
	BEGIN  
	        /*Set count to one as no others exist*/  
		SELECT @cnt=1  
	  
	        /*Add preceding zeros and then add the new count*/  
	        SELECT @ClaimCount = REPLICATE('0',5 - DATALENGTH(CAST(@Cnt AS VARCHAR(5))))  
	        SELECT @ClaimCount = @ClaimCount + CAST(@Cnt AS VARCHAR(5))  
	END  
    ELSE  
	BEGIN            /*Get claim number of latest claim*/  
	        SELECT  
	            @clm_no = MAX(claim_number)  
	        FROM claim  
	        WHERE SUBSTRING(claim_number,1,5) = @Year + '/'  
	  
	        /*Extract the number and increment*/  
	        SELECT @Cnt = CAST(SUBSTRING(@clm_no,6,5) AS INT)  
	        SELECT @Cnt = @Cnt + 1  
	  
	        /*Add preceding zeros and then add the new count*/  
	        SELECT @ClaimCount = REPLICATE('0',5 - DATALENGTH(CAST(@Cnt AS VARCHAR(5))))  
	        SELECT @ClaimCount = @ClaimCount + CAST(@Cnt AS VARCHAR(5))  
	END  
  

    /*Get system option : "Claims: Exclude Auto Numbering On Clients" */  
    SELECT  
        @Exclude = value  
    FROM system_options  
    WHERE option_number=2014  
    AND branch_id = @Branch_Id  
  
    IF ISNULL(@Exclude,0) = 0  
    BEGIN  
  
        /*Get total claim count for second part of auto numbering*/  
        IF NOT EXISTS  
            (  
                SELECT  
                    NULL  
                FROM claim  
                WHERE SUBSTRING(claim_number,1,5) = @Year + '/'  
                AND client_id = @Clientid  
            )  
        BEGIN  
            /*Set count to one as no others exist*/  
            SELECT @cnt=1  
  
            /*Add preceding zeros and then add the new count*/  
            SELECT @ClientCount = REPLICATE('0',2 - DATALENGTH(CAST(@Cnt AS VARCHAR(5))))  
            SELECT @ClientCount = @ClientCount + CAST(@Cnt AS VARCHAR(5))  
        END  
        ELSE  
        BEGIN  
  
            /*Get client number of latest claim and increment it*/  
            SELECT  
                @Cnt = MAX(CAST(SUBSTRING(claim_number,12,5) AS INT))  
            FROM claim  
            WHERE SUBSTRING(claim_number,1,5) = @Year + '/'  
            AND client_id = @Clientid  
  
            SELECT @Cnt = @Cnt + 1  
  
            /*Get the number of characters in the count, with a minimum of 2 characters*/  
            SELECT @ClientCountLength = LEN(CAST(@Cnt AS VARCHAR(5)))  
            IF @ClientCountLength < 2  
            BEGIN  
                SELECT @ClientCountLength = 2  
            END  
  
            /*Add preceding zeros and then add the new count*/  
            SELECT @ClientCount = REPLICATE('0',@ClientCountLength - DATALENGTH(CAST(@Cnt AS VARCHAR(5))))  
            SELECT @ClientCount = @ClientCount + CAST(@Cnt AS VARCHAR(5))  
        END  
  
        /*Claim number is made from year, claim count and client count, e.g. "2004/00157/02"*/  
        SELECT @clm_no = @Year + '/' + @ClaimCount + '/' + @ClientCount  
    END  
    ELSE  
    BEGIN  
        /*Claim number is made from year and claim count, e.g. "2004/00157"*/  
        SELECT @clm_no = @Year + '/' + @ClaimCount  
    END  
  
END  
ELSE  
BEGIN  
    /*Underwriting*/  
  
    -- get initial values for claim transaction suppression indicators from the associated product  
    SELECT 
		@suppress_reserves = suppress_reserves,  
		@suppress_payments = suppress_payments,  
		@suppress_recoveries  = suppress_recoveries  
    FROM product  
    WHERE product_id IN (SELECT product_id  
    FROM insurance_file  
    WHERE insurance_file_cnt = @policy_id)  
  
    --DC090904 PN13111 broking should use insurance_file not event_insurance_file  
    SELECT @Clientid = insured_cnt FROM insurance_file WHERE insurance_file_cnt=@Policy_id  
END  

BEGIN TRAN

BEGIN TRY

IF @BrkUnderwrite='A'  
BEGIN  
 /*Broking*/  
 DECLARE @claim_folder_id int  
  
 INSERT INTO Claim_Folder (insurance_file_cnt, insurance_ref, risk_cnt)  
 VALUES (@policy_id, @Policy_Number, @risk_type_id)  
  
 SELECT @claim_folder_id = @@IDENTITY  
  
 INSERT INTO Claim  
 (  
 Policy_id,  
 Policy_Number,  
 Claim_Number,  
 Description,  
 Claim_Status_id,  
 Progress_Status_id,  
 Primary_Cause_id,  
 Secondary_Cause_id,  
 Catastrophe_code_id,  
 Loss_from_date,  
 Loss_to_date,  
 Reported_date,  
 Reported_to_date,  
 Last_modified_date,  
 Handler_id,  
 Currency_id,  
 Info_only,  
 Likely_claim,  
 Location,  
 Town,  
 Risk_type_id,  
 Client_name,  
 Client_address,  
 Client_tel_no,  
 Client_fax_no,  
 Client_mobile_no,  
 Client_email,  
 Client_claim_number,  
 Insurer_name,  
 insurer_address,  
 insurer_tel_no,  
 insurer_fax_no,  
 insurer_email,  
 insurer_claim_number,  
 insurer_contact,  
 VAT_registered,  
 VAT_reg_no ,  
 Comments,  
 Claims_Status_Date ,  
 Client_Short_name ,  
 Insurer_Short_name,  
 Client_tel_no_off,  
 user_defined_field_A,  
 user_defined_field_B,  
 user_defined_field_C,  
 user_defined_field_D,  
 user_defined_field_E,  
 client_id,  
 driver_title,  
 driver_forename,  
 driver_surname,  
 date_passed_test,  
 employee_title,  
 employee_forename,  
 employee_surname,  
 employee_length_of_service,  
 employee_previous_claim,  
 employee_previous_claim_details,  
 ulr,  
 recovery_agent,  
 solicitor_appointed,  
 solicitor_name,  
 ulr_loss_details,  
 claim_at_fault_id,  
 bonus_affected,  
 policy_deductible_id,  
 non_standard_excess,  
 subsidiary_company_name,  
 is_dirty,  
 version_id,  
 claim_folder_id, 
 created_by_id, 
 create_date,
 claim_handled,
 other_party_id
 )  
 VALUES  
 (  
 @Policy_id,  
 @Policy_Number,  
 @Clm_No ,  
 @Description,  
 @Claim_Status_id,  
 @Progress_Status_id,  
 @Primary_Cause_id,  
 @Secondary_Cause_id,  
 @Catastrophe_code_id,  
 @Loss_from_date,  
 @Loss_to_Date,  
 @Reported_date,  
 @Reported_to_Date,  
 @Last_modified_date,  
 @Handler_id,  
 @Currency_id,  
 @Info_only,  
 @Likely_claim,  
 @Location,  
 @Town,  
 @Risk_type_id,  
 @Client_name,  
 @Client_address,  
 @Client_tel_no,  
 @Client_fax_no,  
 @Client_mobile_no,  
 @Client_email,  
 @Client_claim_number,  
 @Insurer_name,  
 @insurer_address,  
 @insurer_tel_no,  
 @insurer_fax_no,  
 @insurer_email,  
 @insurer_claim_number,  
 @insurer_contact,  
 @VAT_registered,  
 @VAT_reg_no ,  
 @Comments,  
 @Claims_Status_Date ,  
 @Client_Short_name ,  
 @Insurer_Short_name ,  
 @Client_tel_no_off ,  
 @UserDefFldA,  
 @UserDefFldB,  
 @UserDefFldC,  
 @UserDefFldD,  
 @UserDefFldE,  
 @Clientid,  
 @driver_title,  
 @driver_forename,  
 @driver_surname,  
 @date_passed_test,  
 @employee_title,  
 @employee_forename,  
 @employee_surname,  
 @employee_length_of_service,  
 @employee_previous_claim,  
 @employee_previous_claim_details,  
 @ulr,  
 @recovery_agent,  
 @solicitor_appointed,  
 @solicitor_name,  
 @ulr_loss_details,  
 @claim_at_fault_id,  
 @bonus_affected,  
 @policy_deductible_id,  
 @non_standard_excess,  
 @subsidiary_company_name,  
 0,  
 1,  
 @claim_folder_id, 
 @created_by_id, 
 getdate(),
 @claim_handled,
 @tpa
 )  
END  
ELSE  
BEGIN  
  
    /*Underwriting*/  
  
 INSERT INTO Claim_Folder (insurance_file_cnt, insurance_ref, risk_cnt)  
 VALUES (@policy_id, @Policy_Number, @risk_type_id)  
  
 SELECT @claim_folder_id = @@IDENTITY  
  
 DECLARE @transaction_type_id int  
  
 SELECT @transaction_type_id = transaction_type_id  
 FROM transaction_type  
 WHERE code = 'C_CO'  
  
 INSERT INTO Claim  
 (  
 Policy_id,  
 Policy_Number,  
 Claim_Number,  
 Description,  
 Claim_Status_id,  
 Progress_Status_id,  
 Primary_Cause_id,  
 Secondary_Cause_id,  
 Catastrophe_code_id,  
 Loss_from_date,  
 Loss_to_date,  
 Reported_date,  
 Reported_to_date,  
 Last_modified_date,  
 Handler_id,  
 Currency_id,  
 Info_only,  
 Likely_claim,  
 Location,  
 Town,  
 Risk_type_id,  
 Client_name,  
 Client_address,  
 Client_tel_no,  
 Client_fax_no,  
 Client_mobile_no,  
 Client_email,  
 Client_claim_number,  
 Insurer_name,  
 insurer_address,  
 insurer_tel_no,  
 insurer_fax_no,  
 insurer_email,  
 insurer_claim_number,  
 insurer_contact,  
 VAT_registered,  
 VAT_reg_no ,  
 Comments,  
 Claims_Status_Date ,  
 Client_Short_name ,  
 Insurer_Short_name,  
 Client_tel_no_off,  
 user_defined_field_A,  
 user_defined_field_B,  
 user_defined_field_C,  
 user_defined_field_D,  
 user_defined_field_E,  
 client_id,  
 underwriting_year_id,  
 suppress_reserves,  
 suppress_payments,  
 suppress_recoveries,  
 is_dirty,  
 version_id,  
 claim_folder_id,  
 transaction_type_id, 
 created_by_id, 
 create_date,
 base_case_id,
 other_party_id
 )  
 VALUES  
 (  
 @Policy_id,  
 @Policy_Number,  
 @Claim_Number,  
 @Description,  
 @Claim_Status_id,  
 @Progress_Status_id,  
 @Primary_Cause_id,  
 @Secondary_Cause_id,  
 @Catastrophe_code_id,  
 @Loss_from_date,  
 @Loss_to_Date,  
 @Reported_date,  
 @Reported_to_Date,  
 @Last_modified_date,  
 @Handler_id,  
 @Currency_id,  
 @Info_only,  
 @Likely_claim,  
 @Location,  
 @Town,  
 @Risk_type_id,  
 @Client_name,  
 @Client_address,  
 @Client_tel_no,  
 @Client_fax_no,  
 @Client_mobile_no,  
 @Client_email,  
 @Client_claim_number,  
 @Insurer_name,  
 @insurer_address,  
 @insurer_tel_no,  
 @insurer_fax_no,  
 @insurer_email,  
 @insurer_claim_number,  
 @insurer_contact,  
 @VAT_registered,  
 @VAT_reg_no ,  
 @Comments,  
 @Claims_Status_Date ,  
 @Client_Short_name ,  
 @Insurer_Short_name ,  
 @Client_tel_no_off ,  
 @UserDefFldA,  
 @UserDefFldB,  
 @UserDefFldC,  
 @UserDefFldD,  
 @UserDefFldE,  
 @Clientid,  
 CASE 
 WHEN ISNULL(@underwriting_year_id,0)=0 THEN @underwritingyearid 
 ELSE @underwriting_year_id
 END  , 
 @suppress_reserves,  
 @suppress_payments,  
 @suppress_recoveries,  
 1,  
 1,  
 @claim_folder_id,  
 @transaction_type_id, 
 @created_by_id, 
 getDate(),
 @base_case_id,
 @tpa)  
  
END  
  
SELECT @Claim_id = @@IDENTITY  
  
UPDATE Claim SET base_claim_id = @claim_id WHERE claim_id = @claim_id  

UPDATE Claim SET system_base_date = GETDATE(), system_base_xrate =(SELECT cr.rate_against_base FROM CurrencyRate cr  
JOIN PMSystem pms ON pms.currency_id =  cr.currency_id AND pms.system_id = 1  
AND CR.effective_from IN  
                (  
                        SELECT MAX(effective_from)  
                        FROM CurrencyRate  
                        WHERE effective_from <= GETDATE()  
                        AND   currency_id = CR.currency_id  
                )  
JOIN Insurance_File ifi ON
	ifi.insurance_file_cnt = claim.Policy_id
	AND cr.company_id = ifi.source_id
) WHERE Claim_id = @Claim_id  
  
UPDATE Claim SET currency_base_date = GETDATE(), currency_base_xrate =(SELECT cr.rate_against_base FROM CurrencyRate cr  
JOIN Claim c ON cr.currency_id = c.Currency_id  
JOIN Insurance_File ifi ON
	ifi.insurance_file_cnt = claim.Policy_id 
	AND cr.company_id = ifi.source_id  
WHERE Claim_id = @Claim_id AND CR.effective_from IN  
                (  
                        SELECT MAX(effective_from)  
                        FROM CurrencyRate  
                        WHERE effective_from <= GETDATE()  
                        AND   currency_id = CR.currency_id  
                )             
) WHERE Claim_id = @Claim_id 

IF @BrkUnderwrite='A'
BEGIN
	IF EXISTS ( SELECT * FROM policy_coinsurers WHERE insurance_file_cnt = @Policy_id )
	BEGIN
		INSERT INTO claim_coinsurers
		SELECT  @claim_id, 
        		pc.party_cnt,
				pc.coinsurer_count,
				pc.coinsurer_percentage,
				pc.coinsurer_value,
				pc.coinsurer_commission_rate,
				pc.coinsurer_commission_amount,
				pc.coinsurer_ipt_amount,
				pc.coinsurer_policy_number,
				pc.base_currency_id,
				pc.base_coinsurer_commission_amount,
				pc.base_coinsurer_value,
				pc.insurance_section_id,
				pc.coinsurer_net_commission,
				pc.coinsurer_commission_tax,
				pc.coinsurer_cover_percentage
		FROM 	policy_coinsurers pc
		WHERE	pc.insurance_file_cnt = @Policy_id
	END
END

   COMMIT TRAN

END TRY
BEGIN CATCH

  ROLLBACK TRAN

END CATCH

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
