SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_Sel'
GO

CREATE PROCEDURE spu_Claim_Sel  
    @Claim_id int  
AS  
  
DECLARE @AgentUnderwriter varchar(1)

SELECT  @AgentUnderwriter = value
FROM    hidden_options
WHERE   branch_id = 1 and option_number = 1

IF @AgentUnderwriter is null
    SELECT @AgentUnderwriter = 'A'

IF @AgentUnderwriter = ''
    SELECT @AgentUnderwriter = 'A'

IF @AgentUnderwriter = 'A'
    select Claim_Number,policy_id ,Policy_Number ,
        Description ,Claim_Status_id ,Progress_Status_id ,
        Primary_Cause_id ,Secondary_Cause_id ,Catastrophe_code_id ,
        Loss_from_date ,Loss_to_date ,Reported_date ,Reported_to_date ,
        Last_modified_date ,Handler_id ,c.Currency_id ,Info_only ,Likely_claim,
        Location ,Town ,Risk_type_id ,Client_name ,Client_address ,Client_tel_no ,
        Client_fax_no ,Client_mobile_no ,Client_email ,Client_claim_number ,
        Insurer_name ,insurer_address ,insurer_tel_no ,insurer_fax_no ,insurer_email ,
        insurer_claim_number ,insurer_contact ,VAT_registered ,VAT_reg_no ,Comments,
        Claims_status_date,client_short_name,Insurer_short_Name,
        Client_Tel_No_off,user_defined_field_A, user_defined_field_B,
        user_defined_field_C,user_defined_field_D,user_defined_field_E,
 	underwriting_year_id,
	driver_title,driver_forename,driver_surname,date_passed_test,
	employee_title,employee_forename,employee_surname,employee_length_of_service,
	employee_previous_claim,employee_previous_claim_details,ulr,recovery_agent,
	solicitor_appointed,solicitor_name,ulr_loss_details,claim_at_fault_id,
	bonus_affected,policy_deductible_id,non_standard_excess,subsidiary_company_name, version_id, claim_handled,other_party_id,p.name 'other_party_name'
        from Claim(NOLOCK) c LEFT JOIN party p
	on p.party_cnt=c.other_party_id
	where Claim_id = @Claim_id
ELSE
    select Claim_Number,policy_id ,Policy_Number ,
        Description ,Claim_Status_id ,Progress_Status_id ,
        Primary_Cause_id ,Secondary_Cause_id ,Catastrophe_code_id ,
        Loss_from_date ,Loss_to_date ,Reported_date ,Reported_to_date ,
        Last_modified_date ,Handler_id ,c.Currency_id ,Info_only ,Likely_claim,
        Location ,Town ,Risk_type_id ,Client_name ,Client_address ,Client_tel_no ,
        Client_fax_no ,Client_mobile_no ,Client_email ,Client_claim_number ,
        Insurer_name ,insurer_address ,insurer_tel_no ,insurer_fax_no ,insurer_email ,
        insurer_claim_number ,insurer_contact ,VAT_registered ,VAT_reg_no ,Comments,
        Claims_status_date,client_short_name,Insurer_short_Name,
        Client_Tel_No_off,user_defined_field_A, user_defined_field_B,
        user_defined_field_C,user_defined_field_D,user_defined_field_E,other_party_id,
        underwriting_year_id, version_id,(SELECT case_number FROM [case] WHERE case_id= c.base_case_id) 'case_number',
        base_case_id,other_party_id,p.name 'other_party_name'
        from Claim(NOLOCK) c LEFT JOIN party p
	on p.party_cnt=c.other_party_id
	where Claim_id = @Claim_id




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
