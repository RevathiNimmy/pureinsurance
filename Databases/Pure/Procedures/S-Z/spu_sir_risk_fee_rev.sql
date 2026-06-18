SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_sir_risk_fee_rev'
GO


CREATE PROCEDURE spu_sir_risk_fee_rev 
   @nOldRiskCnt INT,
   @nNewRiskCnt INT
AS

BEGIN
	DELETE TC 
 FROM Tax_Calculation TC 
 INNER JOIN policy_fee_u PF ON TC.policy_fee_u_ID=PF.policy_fee_u_id
 WHERE PF.risk_cnt=@nNewRiskCnt

	DELETE FROM policy_fee_u WHERE risk_cnt=@nNewRiskCnt

	DECLARE @nNewInsurance_File_Cnt INT

	SELECT @nNewInsurance_File_Cnt=Insurance_File_Cnt FROM insurance_file_risk_link WHERE risk_cnt=@nNewRiskCnt

	INSERT INTO policy_fee_u (
		insurance_file_cnt,
		party_cnt,
		fee_rate_percentage,
		fee_rate_amount,
		currency_id,
		transaction_type_id,
		product_id,
		branch_id,
		risk_cnt,
		base_currency_id,
		base_fee_amount,
		base_tax_amount,
		currency_amount,
		currency_tax_amount,
		risk_type_group_id,
		peril_group_id,
		tax_group_id,
		transaction_sub_type,
		is_fee_applied_to_cr,
		fee_rate_currency_id,
		Fee_Premium,
		include_fee_in_instalments,
		spread_fee_across_instalments,
		MakeLiveOptions_id,
		DoPaymentTerms_id,
		Calculation_Basis,
		Is_Prorated,
		Pro_rata_rate,
		Is_Override,
		FeeTypePercent)
    SELECT  @nNewInsurance_File_Cnt,
			party_cnt,
			fee_rate_percentage,
			fee_rate_amount * -1,
			currency_id,
			transaction_type_id,
			product_id,
			branch_id,
			risk_cnt,
			base_currency_id,
			base_fee_amount * -1,
			base_tax_amount * -1,
			currency_amount * -1,
			currency_tax_amount * -1,
			risk_type_group_id,
			peril_group_id,
			tax_group_id,
			transaction_sub_type,
			is_fee_applied_to_cr,
			fee_rate_currency_id,
			Fee_Premium * -1,
			include_fee_in_instalments,
			spread_fee_across_instalments,
			MakeLiveOptions_id,
			DoPaymentTerms_id,
			Calculation_Basis,
			Is_Prorated,
			Pro_rata_rate,
			Is_Override,
			FeeTypePercent
    FROM    policy_fee_u WITH (NOLOCK)
    WHERE   risk_cnt = @nOldRiskCnt	
END
GO


