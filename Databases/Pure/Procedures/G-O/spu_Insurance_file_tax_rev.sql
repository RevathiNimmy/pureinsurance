EXECUTE DDLDropProcedure 'spu_Insurance_file_tax_rev'
GO

CREATE PROCEDURE spu_Insurance_file_tax_rev
(
@nOldInsuranceFileCnt INT,
@nNewInsuranceFileCnt INT
)
AS
BEGIN

DELETE FROM Tax_Calculation WHERE insurance_file_cnt=@nNewInsuranceFileCnt AND risk_cnt IS NULL AND transtype IN('TTR','TTIF','TTF','TTAC')

		
INSERT INTO tax_calculation (
		risk_cnt,
		tax_band_id,
		premium,
		percentage,
		value,
		is_value,
		is_manually_changed,
		Calc_Basis,
		Basis_Value,
		currency_id,
		allow_tax_credit,
		country_id,
		state_id,
		class_of_business_id,
		tax_group_id,
		sequence,
		insurance_file_cnt,
		transtype,
		policy_fee_u_id,
		is_not_applied_to_client,
		include_tax_in_instalments,
		spread_tax_across_instalments,
		tax_band_rate_id,
		is_suspended)
	   SELECT 
	   risk_cnt,
	   tax_band_id,
	   premium * -1,
		percentage,
		value * -1,
		is_value,
		is_manually_changed,
		Calc_Basis,
		Basis_Value,
		currency_id,
		allow_tax_credit,
		country_id,
		state_id,
		class_of_business_id,
		tax_group_id,
		sequence,
		@nNewInsuranceFileCnt,
		transtype,
		policy_fee_u_id,
		is_not_applied_to_client,
		include_tax_in_instalments,
		spread_tax_across_instalments,
		tax_band_rate_id,
		is_suspended
    FROM    tax_calculation WITH (NOLOCK)
    WHERE   insurance_file_cnt=@nOldInsuranceFileCnt AND risk_cnt IS NULL AND transtype IN('TTR','TTIF','TTF','TTAC')

END

GO

