SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Get_Claim_details'
GO

CREATE PROCEDURE spu_SAM_Get_Claim_details

	@claim_id integer,
	@source_id integer,
	@baseclaim_id integer=0

AS

	DECLARE @user_defined_field_A_Code varchar(20)
	DECLARE @user_defined_field_B_Code varchar(20)
	DECLARE @user_defined_field_C_Code varchar(20)
	DECLARE @user_defined_field_D_Code varchar(20)
	DECLARE @user_defined_field_E_Code varchar(20)


	-- get the associated user defined codes for claims user definied fields
	EXEC spu_SAM_Get_Claim_User_Defined_Codes
			@claim_id,
			@source_id,
			@user_defined_field_A_Code OUTPUT,
			@user_defined_field_B_Code OUTPUT,
			@user_defined_field_C_Code OUTPUT,
			@user_defined_field_D_Code OUTPUT,
			@user_defined_field_E_Code OUTPUT

IF @baseclaim_id=0
BEGIN
	SELECT
		c.Claim_id,
		c.Policy_id,
		ifile.insurance_folder_cnt,
		c.Policy_Number,
		c.Claim_Number,
		c.Description,
		c.Claim_Status_id,
		cs.Code as claim_status_code,
		c.Progress_Status_id,
		ps.Code as progress_status_code,
		c.Primary_Cause_id,
		pc.Code as primary_cause_code,
		c.Secondary_Cause_id,
		sc.code as secondary_cause_code,
		c.Catastrophe_code_id,
		cc.code as catastrophe_code,
		c.Coinsurance_treatment_id,
		ct.code as coinsurance_treatment_code,
		c.Loss_from_date,
		c.Loss_to_date,
		c.Reported_date,
		c.Reported_to_date,
		c.Last_modified_date,
		c.Handler_id,
		h.Code as handler_code,
		c.Currency_id,
		cur.code as currency_code,
		c.Info_only,
		c.Likely_claim,
		c.Location,
		c.Town,
		t.code as town_code,
		c.Risk_type_id,
		rt.code as risk_type_code,
		c.Client_name,
		c.Client_address,
		c.Client_tel_no,
		c.Client_fax_no,
		c.Client_mobile_no,
		c.Client_email,
		c.Client_claim_number,
		c.Insurer_name,
		c.insurer_address,
		c.insurer_tel_no,
		c.insurer_fax_no,
		c.insurer_email,
		c.insurer_claim_number,
		c.Insurer_Contact,
		c.VAT_registered,
		c.VAT_reg_no,
		c.Comments,
		c.Claims_status_date,
		c.Client_short_name,
		c.Insurer_short_name,
		c.Client_tel_no_off,
		c.user_defined_field_A,
		@user_defined_field_A_Code as user_defined_field_A_code,
		c.user_defined_field_B,
		@user_defined_field_B_Code as user_defined_field_B_code,
		c.user_defined_field_C,
		@user_defined_field_C_Code as user_defined_field_C_code,
		c.user_defined_field_D,
		@user_defined_field_D_Code as user_defined_field_D_code,
		c.user_defined_field_E,
		@user_defined_field_E_Code as user_defined_field_E_code,
		ifile.insured_cnt as Client_id, --c.Client_id,
		c.create_date,
		c.created_by_id,
		c.Modified_by_id,
		c.underwriting_year_id,
		u.code as underwriting_year_code,
		c.gis_screen_id,
		gs.code as gis_screen_code,
		c.Suppress_Reserves,
		c.Suppress_Payments,
		c.Suppress_Recoveries,
		c.is_dirty,
		c.transaction_type_id,
		c.claim_folder_id,
		c.base_claim_id,
		c.version_id,
		c.claim_version_description,
		c.batch_id,
                c.claim_handled,
                c.other_party_id,
                --Start (Vijayakumar Ramasamy)- (Generate Documents Claim)-(15-Sep-2008)
                cs.Description as ClaimStatusDescription,
                --End (Vijayakumar Ramasamy)- (Generate Documents Claim)-(15-Sep-2008)
		-- claim fields not currently required
		--c.Claim_version_number,
		--c.claim_version_status_id,
		--c.exchange_rate_override_reason_id,
		--c.currency_base_xrate,
		--c.currency_base_date,
		--c.account_base_xrate,
		--c.account_base_date,
		--c.system_base_xrate,
		--c.system_base_date,
		--c.driver_title,
		--c.driver_forename,
		--c.driver_surname,
		--c.date_passed_test,
		--c.employee_title,
		--c.employee_forename,
		--c.employee_surname,
		--c.employee_length_of_service,
		--c.employee_previous_claim,
		--c.employee_previous_claim_details,
		--c.ulr,
		--c.recovery_agent,
		--c.solicitor_appointed,
		--c.solicitor_name,
		--c.ulr_loss_details,
		--c.claim_at_fault_id,
		--c.bonus_affected,
		--c.policy_deductible_id,
		--c.non_standard_excess,
		--c.subsidiary_company_name,
		c.base_case_id,
		ifile.insurance_folder_cnt
	FROM Claim c

		LEFT OUTER JOIN Claim_Status cs ON
			cs.Claim_Status_id = c.claim_status_id

		LEFT OUTER JOIN Progress_Status ps ON
			ps.Progress_Status_id = c.Progress_Status_id

		LEFT OUTER JOIN Primary_Cause pc ON
			pc.Primary_Cause_id = c.Primary_Cause_id

		LEFT OUTER JOIN Secondary_Cause sc ON
			sc.Secondary_Cause_id = c.Secondary_Cause_id

		LEFT OUTER JOIN CAtastrophe_Code cc ON
			cc.Catastrophe_code_id = c.Catastrophe_code_id

		LEFT OUTER JOIN Coinsurance_treatment ct ON
			ct.Coinsurance_treatment_id = c.Coinsurance_treatment_id

		LEFT OUTER JOIN Handler h ON
			h.Handler_id = c.Handler_id

		LEFT OUTER JOIN Currency cur ON
			cur.Currency_id = c.Currency_id

		LEFT OUTER JOIN Town t ON
			t.Town_id = c.Town

		LEFT OUTER JOIN Risk_Type rt ON
			rt.Risk_Type_id = c.Risk_Type_Id

		LEFT OUTER JOIN Underwriting_Year u ON
			u.Underwriting_Year_id = c.Underwriting_Year_id

		LEFT OUTER JOIN Gis_Screen gs ON
			gs.Gis_Screen_id = c.Gis_Screen_id

		INNER JOIN Insurance_File ifile ON
			ifile.insurance_file_cnt = c.policy_id

	WHERE claim_id = @claim_id
	
END

ELSE
	BEGIN
       SELECT is_dirty,transaction_type_id FROM Claim WHERE claim_id = (SELECT Max(claim_id) FROM claim WHERE base_claim_id=@baseclaim_id)
    END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
