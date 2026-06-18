SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Product_Details_For_Claim'
GO


CREATE Procedure spu_Get_Product_Details_For_Claim
    @claim_Id int 
AS
Select  
	multiple_claims_payments,
	max_unauthorised_claim_value,
	max_unauthorised_no_claim_payments,
	run_authorisation_scripts_claim_payments,
	Claim_value_for_large_loss_advice,
	Inclusion_of_CoInsurers_On_Claims,
	Allow_Negative_Reserve,
	Ext_Clm_Handler_Acknowledged_Task_Allowed_Time,
	Ext_Clm_Handler_Supply_Pre_Report_Task_Allowed_Time,
	valid_policy_version_at_loss_date,
	is_Gross_Claim_Payment_Amount,
	Claim_Task_Group,
	Claim_User_Group,
	is_Duplicate_Claim_check_Enabled,
	is_advanced_Tax_Script_Enabled,
	is_Payment_Ref_Check_Enabled,
	is_Recommend_Claim_Payments,
	p.Payment_Cannot_Exceed_Reserve,
	p.check_mediatype_status_at_claim_payment,
	p.suppress_reserves,
	p.Authorisation_Threshold
	
From Claim C 
    Join Insurance_file IFL
ON C.Policy_ID = IFL.Insurance_file_cnt
    Join Product P 
ON IFL.Product_id = P.Product_id
Where C.claim_id=@claim_Id
