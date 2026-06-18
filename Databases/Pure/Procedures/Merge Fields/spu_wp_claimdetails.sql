SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_claimdetails'
GO


CREATE PROCEDURE spu_wp_claimdetails
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

-- PSL 23/09/2003 Issue 6439
-- PSL 09/10/2003 Issue 6439 Again
-- DC 09/10/2003 Issue 6966 comments taken from claim_comments table
-- DC 09/10/2003 modified PSL change as risk_type not used in broking
-- AR S4B Claim Enhancements R&D 2005
-- DC 060606 PN28777 added merge fields for policy number, account handler and account executive

DECLARE @Claim_Number VarChar(30),
        @Claim_Description VarChar(1000),
        @Claim_Status VarChar(22),
        @Progress_Status VarChar(50),
        @Primary_Cause VarChar(50),
        @Secondary_Cause VarChar(50),
        @Catastrophe VarChar(50),
        @Loss_From_Date DateTime,
        @Loss_To_Date DateTime,
        @Reported_Date DateTime,
        @Reported_To_Date DateTime,
        @Last_Modified_Date DateTime,
        @Handler VarChar(50),
        @Currency VarChar(255),
        @Information VarChar(3),
        @Likely_Claim VarChar(3),
        @Location VarChar(50),
        @Town VarChar(50),
        @Risk_Type VarChar(50),
        @Client_Name VarChar(60),
        @Client_Address1 VarChar(60),
        @Client_Address2 VarChar(60),
        @Client_Address3 VarChar(60),
        @Client_Address4 VarChar(60),
        @Client_Postal_Code VarChar(20),
        @Client_Tel_No VarChar(50) ,
        @Client_Fax_No VarChar(50),
        @Client_Mobile_No VarChar(50),
        @Client_Email VarChar(50),
        @Client_Claim_Number VarChar(20),
        @Insurer_Name VarChar(60),
        @Insurer_Address1 VarChar(60),
        @Insurer_Address2 VarChar(60),
        @Insurer_Address3 VarChar(60),
        @Insurer_Address4 VarChar(60),
        @Insurer_Postal_Code VarChar(20),
        @insurer_Tel_No VarChar(50),
        @insurer_Fax_No VarChar(50),
        @insurer_Email VarChar(50),
        @insurer_Claim_Number VarChar(20),
        @insurer_Contact VarChar(50),
        @VAT_Registered VarChar(3),
        @VAT_Reg_No VarChar(20) ,
        @Comments VarChar(255),
        @Claims_Status_Date DateTime,
        @Client_Short_Name Varchar(20),
        @Insurer_Short_Name Varchar(20),
        @Client_Tel_No_Off VarChar(50),
        @RiskDescription VARCHAR(255),
	@UWYearCode VARCHAR(10),
	@UWYearDesc VARCHAR(255),
        @ClaimComment Varchar(255),
        @AddressUsageId int,
	@driver_title as varchar(255),
	@driver_forename as varchar(255),
	@driver_surname as varchar(255),
	@date_passed_test as datetime,
	@employee_title as varchar(255),
	@employee_forename as varchar(255),
	@employee_surname as varchar(255),
	@employee_length_of_service as int,
	@employee_previous_claim as varchar(3),
	@employee_previous_claim_details as varchar(255),
	@ulr as varchar(3),
	@recovery_agent as varchar(255),
	@solicitor_appointed as varchar(3),
	@solicitor_name as varchar(255),
	@ulr_loss_details as varchar(255),
	@claim_at_fault as varchar(255),
	@bonus_affected as varchar(3),
	@policy_deductible as varchar(255),
	@non_standard_excess as numeric(19,4),
	@subsidiary_company_name as varchar(255),
    @policy_number as varchar(255),
    @account_handler as varchar(255),
    @account_executive as varchar(255),
    @claim_handled as varchar(4),
    @claim_TPA as varchar(255)

SELECT @AddressUsageId=address_usage_type_id
FROM address_usage_type
WHERE code='3131 XCO'

SELECT  @Claim_Number = c.Claim_Number,
        @Claim_Description = c.Description,
        @Claim_Status =
            CASE c.Claim_Status_id
                WHEN 1 THEN "Provisional Open Claim"
                WHEN 2 THEN "Live Open Claim"
                WHEN 3 THEN "Closed"
                WHEN 4 THEN "ReOpened"
                WHEN 5 THEN "ReClosed"
            END,
        @Progress_Status =
            CASE c.progress_status_id
                WHEN NULL THEN ''
                ELSE ps.description
            END,
        @Primary_Cause =
            CASE c.primary_cause_id
                WHEN NULL THEN ''
                ELSE pc.description
            END,
        @Secondary_Cause =
            CASE c.secondary_cause_id
                WHEN NULL THEN ''
                ELSE sc.description
            END,
        @Catastrophe =
            CASE c.catastrophe_code_id
                WHEN NULL THEN ''
                ELSE cc.description
            END,
        @Loss_From_Date = c.Loss_from_date,
        @Loss_To_Date = c.Loss_to_date,
        @Reported_Date = c.Reported_date,
        @Reported_To_Date = c.Reported_to_date,
        @Last_Modified_Date = c.Last_modified_date,
        @Handler =
            CASE c.handler_id
                WHEN NULL THEN ''
                ELSE h.description
            END,
        @Currency =
            CASE c.currency_id
                WHEN NULL THEN ''
                ELSE cu.description
            END,
        @Information =
            CASE c.Info_only
                WHEN 0 THEN "No"
                WHEN 1 THEN "Yes"
            END,
        @Likely_Claim =
            CASE c.Likely_claim
                WHEN 0 THEN "No"
                WHEN 1 THEN "Yes"
            END,
        @Location = c.Location,
        @Town =
            CASE c.town
                WHEN NULL THEN ''
                ELSE t.description
            END,
        @Risk_Type =
            CASE c.risk_type_id
                WHEN NULL THEN ''
                ELSE r.description
            END,
        @Client_Name = c.Client_name,
        @Client_Address1 = cac.address1,
        @Client_Address2 = cac.address2,
        @Client_Address3 = cac.address3,
        @Client_Address4 = cac.address4,
        @Client_Postal_Code = cac.postal_code,
        @Client_Tel_No = c.Client_tel_no ,
        @Client_Fax_No = c.Client_fax_no,
        @Client_Mobile_No = c.Client_mobile_no,
        @Client_Email = c.Client_email,
        @Client_Claim_Number = c.Client_claim_number ,
        @Insurer_Name = c.Insurer_name,
        @Insurer_Address1 = cai.address1,
        @Insurer_Address2 = cai.address2,
        @Insurer_Address3 = cai.address3,
        @Insurer_Address4 = cai.address4,
        @Insurer_Postal_Code = cai.postal_code ,
        @Insurer_Tel_No = c.insurer_tel_no,
        @Insurer_Fax_No = c.insurer_fax_no,
        @Insurer_Email = c.insurer_email,
        @Insurer_Claim_Number = c.insurer_claim_number,
        @Insurer_Contact = c.insurer_contact,
        @VAT_registered =
            CASE c.VAT_registered
                WHEN 0 THEN "No"
                WHEN 1 THEN "Yes"
            END,
        @Vat_Reg_No = c.VAT_reg_no ,
        @Comments = ccom.comment_line,
        @Claims_Status_Date = c.Claims_status_date ,
        @Client_Short_Name = c.client_short_name,
        @Insurer_Short_Name = c.Insurer_short_Name,
        @Client_Tel_No_Off = c.Client_Tel_No_off,
        @RiskDescription = RTRIM(r.Description),
	@UWYearCode = UW.code,
	@UWYearDesc = UW.description,
        @ClaimComment = c.comments,
        @driver_title = c.driver_title,
        @driver_forename = c.driver_forename,
        @driver_surname = c.driver_surname,
        @date_passed_test = c.date_passed_test,
        @employee_title = c.employee_title,
        @employee_forename = c.employee_forename,
        @employee_surname = c.employee_surname,
        @employee_length_of_service = c.employee_length_of_service,
        @employee_previous_claim = CASE c.employee_previous_claim WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END,
        @employee_previous_claim_details = c.employee_previous_claim_details,
        @ulr = CASE c.ulr WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END,
        @recovery_agent = c.recovery_agent,
        @solicitor_appointed = CASE c.solicitor_appointed WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END,
        @solicitor_name = c.solicitor_name,
        @ulr_loss_details = c.ulr_loss_details,
        @claim_at_fault = CAF.description,
        @bonus_affected = CASE c.bonus_affected WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END,
        @policy_deductible = PD.description,
        @non_standard_excess = c.non_standard_excess,
        @subsidiary_company_name = c.subsidiary_company_name,
        @policy_number = c.policy_number,
        @account_handler = ahp.resolved_name,
        @account_executive = aep.resolved_name,
        @claim_handled =CASE c.claim_handled
                            WHEN 0 THEN 'No'
                            WHEN 1 THEN 'No'
                            WHEN 2 THEN 'Yes'
                            WHEN NULL THEN ''
                        END,
        @claim_TPA= ptpa.resolved_name

FROM    Claim c left join progress_status ps on ps.progress_status_id = c.progress_status_id
        left join primary_cause pc on pc.primary_cause_id = c.primary_cause_id
        left join secondary_cause sc on sc.secondary_cause_id = c.secondary_cause_id
        left join catastrophe_code cc on cc.catastrophe_code_id = c.catastrophe_code_id
        left join handler h on h.handler_id = c.handler_id
        left join currency cu on cu.currency_id = c.currency_id
        left join town t on t.town_id = c.town
        left join risk r on r.risk_cnt = c.risk_type_id
        left outer join risk_code rc on rc.risk_code_id = c.risk_type_id
        left join claim_address cac on cac.address_cnt = c.client_address
        left join claim_address cai on cai.address_cnt = c.insurer_address
	    left join claim_comments ccom on ccom.claim_id = c.claim_id
   	    and ccom.entity_id = 0 and ccom.comment_type = 0 and ccom.claim_comment_id = 1
	    LEFT JOIN underwriting_year UW ON UW.underwriting_year_id = C.underwriting_year_id
	    LEFT JOIN claim_at_fault CAF ON CAF.claim_at_fault_id = C.claim_at_fault_id
	    LEFT JOIN policy_deductible PD ON PD.policy_deductible_id = C.policy_deductible_id
        left join insurance_file ifi on c.policy_id = ifi.insurance_file_cnt
        left join party ahp on ifi.account_handler_cnt = ahp.party_cnt
        left join party p on c.client_id = p.party_cnt
        left join party aep on p.consultant_cnt = aep.party_cnt
        left join party ptpa on c.other_party_id = ptpa.party_cnt

WHERE   c.Claim_id = @ClaimCnt

--AR20050421 - PN20411 If no claim address, default in the party's correspondence address
IF (@Client_Address1 IS NULL) AND (@Client_Address2 IS NULL) AND (@Client_Address3 IS NULL) AND (@Client_Address4 IS NULL) AND (@Client_Postal_Code IS NULL)
	SELECT
		@Client_Address1=a.address1,
		@Client_Address2=a.address2,
		@Client_Address3=a.address3,
		@Client_Address4=a.address4,
		@Client_Postal_Code=a.postal_code
	FROM
		address a INNER JOIN party_address_usage pau ON pau.address_cnt=a.address_cnt
	WHERE pau.party_cnt=@PartyCnt AND pau.address_usage_type_id=@AddressUsageId

SELECT  'claim_number' = @Claim_Number,
        'claim_description' = @Claim_Description,
        'claim_status' = @Claim_Status,
        'claim_handled'=@claim_handled,
        'progress_status' = @Progress_Status,
        'primary_cause' = @Primary_Cause,
        'secondary_cause' = @Secondary_Cause,
        'catastrophe' = @Catastrophe,
        'loss_from_date' = @Loss_From_Date,
        'loss_to_date' = @Loss_To_Date,
        'reported_date' = @Reported_Date,
        'reported_to_date' = @Reported_To_Date,
        'last_modified_date' = @Last_Modified_Date,
        'handler' = @Handler,
        'currency' = @Currency,
        'information' = @Information,
        'likely_claim' = @Likely_Claim,
        'location' = @Location,
        'town' = @Town,
        'risk_type' = @Risk_Type,
        'client_name' = @Client_Name,
        'client_address1' = @Client_Address1,
        'client_address2' = @Client_Address2,
        'client_address3' = @Client_Address3,
        'client_address4' = @Client_Address4,
        'client_postal_code' = @Client_Postal_Code,
        'client_tel_no' = @Client_Tel_No,
        'client_fax_no' = @Client_Fax_No,
        'client_mobile_no' = @Client_Mobile_No,
        'client_email' = @Client_Email,
        'client_claim_number' = @Client_Claim_Number,
        'insurer_name' = @Insurer_Name,
        'insurer_address1' = @Insurer_Address1,
        'insurer_address2' = @Insurer_Address2,
        'insurer_address3' = @Insurer_Address3,
        'insurer_address4' = @Insurer_Address4,
        'insurer_postal_code' = @Insurer_Postal_Code,
        'insurer_tel_no' = @insurer_Tel_No,
        'insurer_fax_no' = @insurer_Fax_No,
        'insurer_email' = @insurer_Email,
        'insurer_claim_number' = @insurer_Claim_Number,
        'insurer_contact' = @insurer_Contact,
        'vat_registered' = @VAT_Registered,
        'vat_reg_no' = @VAT_Reg_No,
        'comments' = @Comments,
        'claims_status_date' = @Claims_Status_Date,
        'client_short_name' = @Client_Short_Name,
        'insurer_short_name' = @Insurer_Short_Name,
        'client_tel_no_off' = @Client_Tel_No_Off,
        'RiskDescription' = @RiskDescription,
	'UWYearCode' = @UWYearCode,
	'UWYearDesc' = @UWYearDesc,
        'ClaimComment' = @ClaimComment,
        'driver_title' = @driver_title,
	'driver_forename' = @driver_forename,
	'driver_surname' = @driver_surname,
	'date_passed_test' = @date_passed_test,
	'employee_title' = @employee_title,
	'employee_forename' = @employee_forename,
	'employee_surname' = @employee_surname,
	'employee_length_of_service' = @employee_length_of_service,
	'employee_previous_claim' = @employee_previous_claim,
	'employee_previous_claim_details' = @employee_previous_claim_details,
	'ulr' = @ulr,
	'recovery_agent' = @recovery_agent,
	'solicitor_appointed' = @solicitor_appointed,
	'solicitor_name' = @solicitor_name,
	'ulr_loss_details' = @ulr_loss_details,
	'claim_at_fault' = @claim_at_fault,
	'bonus_affected' = @bonus_affected,
	'policy_deductible' = @policy_deductible,
	'non_standard_excess' = @non_standard_excess,
	'subsidiary_company_name' = @subsidiary_company_name,
    'policy_number' = @policy_number,
    'account_handler' = @account_handler,
    'account_executive' = @account_executive,
     'claim_TPA'=@claim_TPA
GO




