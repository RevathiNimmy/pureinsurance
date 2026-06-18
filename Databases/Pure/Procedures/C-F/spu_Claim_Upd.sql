SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_Upd'
GO

CREATE PROCEDURE spu_Claim_Upd    
    @Claim_id int,    
    @Policy_id int,    
    @Policy_Number varchar(30),    
    @Description varchar(1000),    
    @Claim_Status_id int,    
    @Progress_Status_id int,    
    @Primary_Cause_id int,    
    @Secondary_Cause_id int,    
    @Catastrophe_code_id int,    
    @Loss_from_date datetime,    
    @Loss_to_date datetime,    
    @Reported_date datetime,    
    @Reported_to_date datetime,    
    @Last_modified_date datetime,    
    @Handler_id int,    
    @Currency_id int,    
    @Info_only bit,    
    @Likely_claim bit,    
    @Location varchar(50),    
    @Town int,    
    @Risk_type_id int,    
    @Client_name varchar(255),    
    @Client_address int,    
    @Client_tel_no varchar(255),    
    @Client_fax_no varchar(255),    
    @Client_mobile_no varchar(255),    
    @Client_email varchar(255),    
    @Client_claim_number varchar(20),    
    @Insurer_name varchar(255),    
    @insurer_address int,    
    @insurer_tel_no varchar(255),    
    @insurer_fax_no varchar(255),    
    @insurer_email varchar(255),    
    @insurer_claim_number varchar(20),    
    @insurer_contact varchar(50),    
    @VAT_registered int,    
    @VAT_reg_no varchar(20),    
    @Comments text,    
    @Claims_Status_Date datetime,    
    @Client_Short_name char(20),    
    @Insurer_Short_name char(20),    
    @Client_tel_no_off varchar(255),    
    @UserDefFldA int,    
    @UserDefFldB int,    
    @UserDefFldC int,    
    @UserDefFldD int,    
    @UserDefFldE int,    
    /*RWH(14/11/2000) added in Claim_Number*/    
    @Claim_Number varchar(30),    
    @Branch_Id int,    
    @underwriting_year_id int,    
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
    @claim_handled int,
    @tpa int,    
	@claim_version_description varchar(1000)= null
AS    
    
DECLARE @AgentUnderwriter varchar(1)  
DECLARE @LenClaimNumber int  
    
SELECT  @AgentUnderwriter = value    
FROM    hidden_options    
WHERE   branch_id = 1 and option_number = 1    
    
IF @AgentUnderwriter is null    
    SELECT @AgentUnderwriter = 'A'    
    
IF @AgentUnderwriter = ''    
    SELECT @AgentUnderwriter = 'A'    

    IF @tpa=0
    SELECT @tpa=NULL


IF @Claim_Number <> NULL AND @Description <> NULL
BEGIN     
	/* DC250405 PN20447 update documaster with the new description */    
	IF EXISTS ( SELECT NULL FROM system_options WHERE option_number = '10' AND value = '1' ) AND @AgentUnderwriter = 'A'    
	BEGIN    
	    
	 SELECT @LenClaimNumber = len(RTrim(@Claim_Number))
	
	 UPDATE doc_folder    
	 SET folder_name = RTRIM(@Claim_Number) + '   ' + RTRIM(@Description)    
	 WHERE SUBSTRING(folder_name, 1, @LenClaimNumber) = @Claim_Number
	 AND folder_level = 2  
	    
	END    
END    
IF @AgentUnderwriter = 'A'    
    
    UPDATE CLAIM SET    
    Policy_id = @Policy_id,    
    Policy_Number = @Policy_Number,    
    /*RWH(14/11/2000) added in Claim_Number*/    
    Claim_Number = @Claim_Number,    
    /*Claim_id = @Claim_id ,*/    
    Description = @Description,    
    Claim_Status_id = @Claim_Status_id,    
    Progress_Status_id = @Progress_Status_id,    
    Primary_Cause_id = @Primary_Cause_id,    
    Secondary_Cause_id = @Secondary_Cause_id,    
    Catastrophe_code_id = @Catastrophe_code_id,    
    Loss_from_date = @Loss_from_date,    
    Loss_to_date = @Loss_to_date,    
    Reported_date = @Reported_date,    
    Reported_to_date = @Reported_to_date,    
    Last_modified_date = @Last_modified_date,    
    Handler_id = @Handler_id,    
    Currency_id = @Currency_id,    
    Info_only = @Info_only,    
    Likely_claim = @Likely_claim,    
    Location = @Location,    
    Town = @Town,    
    Risk_type_id = @Risk_type_id,    
    Client_name = @Client_name,    
    Client_address = @Client_address,    
    Client_tel_no = @Client_tel_no,    
    Client_fax_no = @Client_fax_no,    
    Client_mobile_no = @Client_mobile_no,    
    Client_email = @Client_email,    
    Client_claim_number = @Client_claim_number,    
    Insurer_name = @Insurer_name,    
    insurer_address = @insurer_address,    
    insurer_tel_no = @insurer_tel_no,    
    insurer_fax_no = @insurer_fax_no,    
    insurer_email = @insurer_email,    
    insurer_claim_number = @insurer_claim_number,    
    insurer_contact = @insurer_contact,    
    VAT_registered = @VAT_registered,    
    VAT_reg_no = @VAT_reg_no ,    
    Comments = @Comments,    
    Claims_Status_Date=@Claims_Status_Date,    
    Client_Short_name =@Client_Short_name,    
    Insurer_Short_name=@Insurer_Short_name,    
    Client_tel_no_off=@Client_tel_no_off,    
    user_defined_field_A=@UserDefFldA,    
    user_defined_field_B=@UserDefFldB,    
    user_defined_field_C=@UserDefFldC,    
    user_defined_field_D=@UserDefFldD,    
    user_defined_field_E=@UserDefFldE,    
    driver_title = @driver_title,    
    driver_forename = @driver_forename,    
    driver_surname = @driver_surname,    
    date_passed_test = @date_passed_test,    
    employee_title = @employee_title,    
    employee_forename = @employee_forename,    
    employee_surname = @employee_surname,    
    employee_length_of_service = @employee_length_of_service,    
    employee_previous_claim = @employee_previous_claim,    
    employee_previous_claim_details = @employee_previous_claim_details,    
    ulr = @ulr,    
    recovery_agent = @recovery_agent,    
    solicitor_appointed = @solicitor_appointed,    
    solicitor_name = @solicitor_name,    
    ulr_loss_details = @ulr_loss_details,    
    claim_at_fault_id = @claim_at_fault_id,    
    bonus_affected = @bonus_affected,    
    policy_deductible_id = @policy_deductible_id,    
    non_standard_excess = @non_standard_excess,    
    subsidiary_company_name = @subsidiary_company_name,
    claim_handled = @claim_handled,
    other_party_id=@tpa,
	claim_version_description   = @claim_version_description
    WHERE Claim_id = @Claim_id    
ELSE   
    UPDATE claim SET    
    Policy_id = @Policy_id,    
    Policy_Number = @Policy_Number,    
    /*RWH(14/11/2000) added in Claim_Number*/    
    Claim_Number = @Claim_Number,    
    /*Claim_id = @Claim_id ,*/    
    Description = @Description,    
    Claim_Status_id = @Claim_Status_id,    
    Progress_Status_id = @Progress_Status_id,    
    Primary_Cause_id = @Primary_Cause_id,    
    Secondary_Cause_id = @Secondary_Cause_id,    
    Catastrophe_code_id = @Catastrophe_code_id,    
    Loss_from_date = @Loss_from_date,    
    Loss_to_date = @Loss_to_date,    
    Reported_date = @Reported_date,    
    Reported_to_date = @Reported_to_date,    
    Last_modified_date = @Last_modified_date,    
    Handler_id = @Handler_id,    
    Currency_id = @Currency_id,    
    Info_only = @Info_only,    
    Likely_claim = @Likely_claim,    
    Location = @Location,    
    Town = @Town,    
    Risk_type_id = @Risk_type_id,    
    Client_name = @Client_name,    
    Client_address = @Client_address,    
    Client_tel_no = @Client_tel_no,    
    Client_fax_no = @Client_fax_no,    
    Client_mobile_no = @Client_mobile_no,    
    Client_email = @Client_email,    
    Client_claim_number = @Client_claim_number,    
    Insurer_name = @Insurer_name,    
    insurer_address = @insurer_address,    
    insurer_tel_no = @insurer_tel_no,    
    insurer_fax_no = @insurer_fax_no,    
    insurer_email = @insurer_email,    
    insurer_claim_number = @insurer_claim_number,    
    insurer_contact = @insurer_contact,    
    VAT_registered = @VAT_registered,    
    VAT_reg_no = @VAT_reg_no ,    
    Comments = @Comments,    
    Claims_Status_Date=@Claims_Status_Date,    
    Client_Short_name =@Client_Short_name,    
    Insurer_Short_name=@Insurer_Short_name,    
    Client_tel_no_off=@Client_tel_no_off,    
    user_defined_field_A=@UserDefFldA,    
    user_defined_field_B=@UserDefFldB,    
    user_defined_field_C=@UserDefFldC,    
    user_defined_field_D=@UserDefFldD,    
    user_defined_field_E=@UserDefFldE,    
    underwriting_year_id=@underwriting_year_id,
    other_party_id=@tpa,
	claim_version_description   = @claim_version_description  
  
    WHERE Claim_id = @Claim_id    
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
