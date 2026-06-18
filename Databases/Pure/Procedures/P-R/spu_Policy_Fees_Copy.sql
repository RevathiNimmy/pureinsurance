SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Policy_Fees_Copy' 
GO

CREATE PROCEDURE spu_Policy_Fees_Copy
 @insurance_File_Cnt    INT,
 @source_insurance_file_cnt  INT
AS
BEGIN

 -- DELETE Current Risk Fees if there are any
 DELETE Tax_Calculation
 FROM   Tax_Calculation
 INNER  JOIN Policy_Fee_U ON Policy_Fee_U.policy_fee_u_id = Tax_Calculation.policy_fee_u_id
 WHERE  Policy_Fee_U.insurance_file_cnt = @Insurance_File_Cnt AND Policy_Fee_U.risk_cnt IS NULL AND transtype='TTF'

 DELETE Policy_Fee_U
 WHERE  Policy_Fee_U.insurance_file_cnt = @Insurance_File_Cnt AND Policy_Fee_U.risk_cnt IS NULL

 DECLARE @policy_fee_u_id INT
 DECLARE @Use_when_deleted INT
 DECLARE @policy_fee_u_id_new INT

 DECLARE @insuranceFileType INT
 DECLARE @product_id INT
 DECLARE @sMakeliveoptions_id INT
 DECLARE @sPaymentDebitOrCash_id INT
 DECLARE @sMakeliveoptions VARCHAR(20)= NULL
 DECLARE @sPaymentDebitOrCash VARCHAR(20)= NULL
 DECLARE @effective_fee_date DATETIME
 DECLARE @Transaction_type_id INT 
 DECLARE @transaction_sub_type_id INT
 DECLARE @BaseInsuranceFileCnt INT=0
 DECLARE 	@premium NUMERIC(19, 4) 

   SELECT  @premium =i.this_premium,
	  		@insuranceFileType = insurance_file_type_id ,
                     @product_id = product_id ,
                     @sMakeliveoptions=
                      CASE WHEN UPPER(payment_method)='INVOICE'  THEN 'INVOICE'
						  WHEN UPPER(payment_method)='INSTALMENTS'  THEN 'INST'
						  WHEN UPPER(payment_method)='PREMIUMFINANCE'  THEN 'INST'
						  WHEN UPPER(payment_method)='BANK GUARANTEE'  THEN 'BG'
						  WHEN UPPER(payment_method)='PAYNOW'  THEN 'PAYNOW'
						  WHEN UPPER(payment_method)='CASH DEPOSIT'  THEN 'CD'
						  WHEN UPPER(payment_method)='MARK FOR COLLECTION'  THEN 'MARKED'
					 END,
                     @sPaymentDebitOrCash_id = CASE WHEN ISNULL(@sPaymentDebitOrCash,'')<>'' THEN @sPaymentDebitOrCash_id
                                                 ELSE ISNULL(DOPaymentTerms_id,0)
                                             END,
		@effective_fee_date=i.cover_start_date,
		@BaseInsuranceFileCnt =i.base_insurance_file_cnt
    FROM    insurance_file i WITH (NOLOCK)
    WHERE   I.insurance_file_cnt = @insurance_File_Cnt

	SELECT @transaction_type_id =    CASE WHEN @insuranceFileType IN (1,2) THEN 4
            WHEN @insuranceFileType IN (4,5,6,7) THEN 9
            WHEN @insuranceFileType IN (8,11,12) THEN 7
            WHEN @insuranceFileType IN (9,10) THEN 20
            WHEN @insuranceFileType IN (3)    THEN 10 END

  DECLARE fee_cursor CURSOR FAST_FORWARD
  FOR

 SELECT DISTINCT policy_fee_u_id
		FROM    Fee_Amounts fa WITH (NOLOCK)
		LEFT JOIN policy_fee_u pfu WITH (NOLOCK) ON pfu.party_cnt = fa.party_cnt
		WHERE   ((ISNULL(fa.Use_when_deleted,0) = 1 AND  fa.is_deleted = 1 AND ISNULL(@BaseInsuranceFileCnt,0)<>0) OR  fa.is_deleted = 0)
		AND    (fa.transaction_type_id = @transaction_type_id    OR ISNULL(fa.transaction_type_id,0)=0)
		AND     (@transaction_sub_type_id = 0  OR fa.transaction_sub_type = @transaction_sub_type_id OR ISNULL(fa.transaction_sub_type,0) = 0)
		AND    (fa.product_id = @product_id OR ISNULL(fa.product_id,0) = 0)
		AND     (ISNULL(fa.MakeLiveOptions_id, 0) = 0 OR fa.MakeLiveOptions_id = @sMakeliveoptions_id)
		AND     (ISNULL(fa.DoPaymentTerms_id, 0) = 0 OR fa.DoPaymentTerms_id = @sPaymentDebitOrCash_id)
		AND    (
							isnull(@premium,0) >= 0 OR
							(
							isnull(@premium,0) < 0 AND
								(ISNULL(fa.is_fee_applied_to_cr, 0) = 1 AND ISNULL(fa.transaction_type_id, 0) = 0)
								OR
								(ISNULL(fa.transaction_type_id, 0) > 0)
							)
					)
		AND     fa.effective_date = (SELECT  MAX(effective_date)
									FROM    fee_amounts fa2 WITH (NOLOCK)
									WHERE	(fa2.transaction_type_id = @transaction_type_id    OR ISNULL(fa2.transaction_type_id,0)=0)
									AND		( @transaction_sub_type_id = 0 OR fa2.transaction_sub_type = @transaction_sub_type_id OR ISNULL(fa2.transaction_sub_type,0) = 0)
									AND		(fa2.product_id = @product_id OR ISNULL(fa2.product_id,0) = 0)
									AND		((ISNULL(fa.Use_when_deleted,0) = 1 AND  fa.is_deleted = 1 AND ISNULL(@BaseInsuranceFileCnt,0)<>0) or  fa2.is_deleted = 0)
									AND     (ISNULL(fa2.MakeLiveOptions_id, 0) = 0 OR fa2.MakeLiveOptions_id = @sMakeliveoptions_id)
									AND     (ISNULL(fa2.DoPaymentTerms_id, 0) = 0 OR fa2.DoPaymentTerms_id = @sPaymentDebitOrCash_id)
									AND     fa2.effective_date <= @effective_fee_date
									AND     fa2.party_cnt = fa.party_cnt
									AND		fa2.product_id IS NOT NULL)

          AND pfu.insurance_file_cnt = @source_insurance_file_cnt AND pfu.risk_cnt IS NULL

 OPEN fee_cursor
 FETCH NEXT FROM fee_cursor
  INTO @policy_fee_u_id

 WHILE @@FETCH_STATUS = 0
  BEGIN
   SELECT @policy_fee_u_id_new = NULL
   INSERT INTO policy_fee_u
    ([insurance_file_cnt]
    ,[party_cnt]
    ,[fee_rate_percentage]
    ,[fee_rate_amount]
    ,[currency_id]
    ,[transaction_type_id]
    ,[product_id]
    ,[branch_id]
    ,[risk_cnt]
    ,[base_currency_id]
    ,[base_fee_amount]
    ,[base_tax_amount]
    ,[currency_amount]
    ,[currency_tax_amount]
    ,[risk_type_group_id]
    ,[peril_group_id]
    ,[tax_group_id]
    ,[transaction_sub_type]
    ,[is_fee_applied_to_cr]
    ,[fee_rate_currency_id]
    ,[Fee_Premium]
    ,[include_fee_in_instalments]
    ,[spread_fee_across_instalments]
    ,[MakeLiveOptions_id]
    ,[DoPaymentTerms_id]
    ,[Calculation_Basis]
    ,[Is_Prorated]
    ,[Pro_rata_rate]
    ,[Is_Override]
    ,[FeeTypePercent]
	,[fee_amount_id])
   SELECT
    @insurance_File_Cnt
    ,[party_cnt]
    ,[fee_rate_percentage]
    ,[fee_rate_amount]
    ,[currency_id]
    ,[transaction_type_id]
    ,[product_id]
    ,[branch_id]
    ,[risk_cnt]
    ,[base_currency_id]
    ,[base_fee_amount]
    ,[base_tax_amount]
    ,[currency_amount]
    ,[currency_tax_amount]
    ,[risk_type_group_id]
    ,[peril_group_id]
    ,[tax_group_id]
    ,[transaction_sub_type]
    ,[is_fee_applied_to_cr]
    ,[fee_rate_currency_id]
    ,[Fee_Premium]
    ,[include_fee_in_instalments]
    ,[spread_fee_across_instalments]
    ,[MakeLiveOptions_id]
    ,[DoPaymentTerms_id]
    ,[Calculation_Basis]
    ,[Is_Prorated]
    ,[Pro_rata_rate]
    ,[Is_Override]
    ,[FeeTypePercent]
	,[fee_amount_id]
   FROM policy_fee_u
   WHERE policy_fee_u.policy_fee_u_id = @policy_fee_u_id

   --Get the New Identity
   SELECT @policy_fee_u_id_new = SCOPE_IDENTITY()

   -- Copy the taxes between two versions
   INSERT INTO Tax_Calculation
      (risk_cnt,
      tax_band_id,
      premium,
      percentage,
      value,
      is_value,
      is_manually_changed,
      Calc_Basis,
      Basis_Value,
      Sum_Insured,
      Sum_Insured_Rounded,
      currency_id,
      allow_tax_credit,
      original_sum_insured,
      country_id,
      state_id,
      class_of_business_id,
      tax_group_id,
      sequence,
      insurance_file_cnt,
      transtype,
      policy_fee_u_id,
      agent_commission_cnt,
      ri_party_cnt,
      ri_arrangement_line_id,
      insurance_section_id,
      policy_fee_id,
      policy_agents_id,
      insurer_party_cnt,
      claim_peril_id,
      claim_payment_id,
      claim_receipt_id,
      claim_payment_item_id,
      claim_receipt_item_id,
      is_not_applied_to_client,
      include_tax_in_instalments,
      spread_tax_across_instalments,
      base_tax_calculation_cnt,
      version_id,
      pfprem_finance_cnt,
      pfprem_finance_version,
      policy_coinsurers_section_id,
      is_commission_tax,
      apply_tax_by,
      tax_band_rate_id,
      is_suspended)
   SELECT
      risk_cnt,
      tax_band_id,
      premium,
      percentage,
      value,
      is_value,
      is_manually_changed,
      Calc_Basis,
      Basis_Value,
      Sum_Insured,
      Sum_Insured_Rounded,
      currency_id,
      allow_tax_credit,
      original_sum_insured,
      country_id,
      state_id,
      class_of_business_id,
      tax_group_id,
      sequence,
      @insurance_File_Cnt,
      transtype,
      @policy_fee_u_id_new,
      agent_commission_cnt,
      ri_party_cnt,
      ri_arrangement_line_id,
      insurance_section_id,
      policy_fee_id,
      policy_agents_id,
      insurer_party_cnt,
      claim_peril_id,
      claim_payment_id,
      claim_receipt_id,
      claim_payment_item_id,
      claim_receipt_item_id,
      is_not_applied_to_client,
      include_tax_in_instalments,
      spread_tax_across_instalments,
      base_tax_calculation_cnt,
      version_id,
      pfprem_finance_cnt,
      pfprem_finance_version,
      policy_coinsurers_section_id,
      is_commission_tax,
      apply_tax_by,
      tax_band_rate_id,
      is_suspended
     FROM Tax_Calculation
     WHERE  policy_fee_u_id = @policy_fee_u_id
     AND   @policy_fee_u_id IS NOT NULL
   FETCH NEXT FROM fee_cursor INTO @policy_fee_u_id

  END
 CLOSE fee_cursor
 DEALLOCATE fee_cursor

END
