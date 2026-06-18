SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_sir_policy_fee_rev'
GO

CREATE PROCEDURE spu_sir_policy_fee_rev
    @OldInsuranceFileCnt int,
    @NewInsuranceFileCnt int
AS

DECLARE @policy_fee_u_id INT,
		@New_policy_fee_u_id INT,
		@premium NUMERIC(19, 4),
		@tax NUMERIC(19, 4),
		@fee_value NUMERIC(19, 4),
		@base_fee_value	NUMERIC(19, 4),
		@tax_currency_amount NUMERIC(19, 4),
		@tax_group_id INT,
		@fee_party INT,
		@insuranceFolderCnt INT,
		@Use_When_Deleted INT

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
	DELETE Tax_Calculation
	FROM   Tax_Calculation tc
	INNER JOIN policy_fee_u pfu On pfu.policy_fee_u_id = tc.policy_fee_u_id
	WHERE pfu.insurance_file_cnt = @NewInsuranceFileCnt AND tc.transtype = 'TTF'
        AND   pfu.risk_cnt IS NULL

	DELETE policy_fee_u
        WHERE insurance_file_cnt = @NewInsuranceFileCnt AND risk_cnt IS NULL

	SELECT @insuranceFolderCnt = insurance_folder_cnt
        FROM   insurance_file WITH (NOLOCK)
        WHERE insurance_file_cnt = @OldInsuranceFileCnt

    -- select new premium and tax
    SELECT  @premium =i.this_premium,
	    @tax =tax.value,
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
    LEFT JOIN (SELECT Insurance_File_cnt, SUM(ISNULL(value,0)) value
               FROM   Tax_Calculation tc WITH (NOLOCK)
               WHERE  tc.transtype IN('TTR','TTIF')
               GROUP BY insurance_file_cnt ) AS tax ON TAX.insurance_file_cnt =I.insurance_file_cnt
    WHERE   I.insurance_file_cnt = @NewInsuranceFileCnt

	SELECT @BaseInsuranceFileCnt

	SELECT @transaction_type_id =    CASE WHEN @insuranceFileType IN (1,2) THEN 4
            WHEN @insuranceFileType IN (4,5,6,7) THEN 9
            WHEN @insuranceFileType IN (8,11,12) THEN 7
            WHEN @insuranceFileType IN (9,10) THEN 20
            WHEN @insuranceFileType IN (3)    THEN 10 END

    DECLARE Fee_Cursor CURSOR FAST_FORWARD FOR
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

          AND pfu.insurance_file_cnt = @OldInsuranceFileCnt AND pfu.risk_cnt IS NULL

   -- Process for each Fee
   OPEN Fee_Cursor
   FETCH NEXT FROM Fee_Cursor INTO @policy_fee_u_id

   WHILE @@Fetch_Status = 0 
  	BEGIN
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
		FeeTypePercent,
		fee_amount_id)
    SELECT  @NewInsuranceFileCnt,
			party_cnt,
			fee_rate_percentage,
			fee_rate_amount * -1,
			currency_id,
			transaction_type_id,
			product_id,
			branch_id,
			risk_cnt,
			base_currency_id,
			Case ISNull(fee_rate_percentage, 0)
			When 0 Then base_fee_amount * -1 -- pro rata and currency rate will be taken care here as we not looking for new rates
				Else
					Case Calculation_Basis
					When 1 Then (((@premium + @tax) * fee_rate_percentage) / 100)
						Else (@premium * fee_rate_percentage / 100) End End,
			0,
			Case ISNull(fee_rate_percentage, 0)
			When 0 Then Round(currency_amount * -1,2) -- pro rata will be taken care here
				Else
					Case Calculation_Basis
					When 1 Then Round(((@premium + @tax) * fee_rate_percentage) / 100,2)
						Else Round(@premium * fee_rate_percentage / 100,2) End End,
			0,
			risk_type_group_id,
			peril_group_id,
			tax_group_id,
			transaction_sub_type,
			is_fee_applied_to_cr,
			fee_rate_currency_id,
			Case Calculation_Basis
				When 1 Then @premium + @tax
				Else @premium End,
			include_fee_in_instalments,
			spread_fee_across_instalments,
			MakeLiveOptions_id,
			DoPaymentTerms_id,
			Calculation_Basis,
			Is_Prorated,
			Pro_rata_rate,
			Is_Override,
			FeeTypePercent,
			fee_amount_id
    FROM    policy_fee_u WITH (NOLOCK)
    WHERE   policy_fee_u_id = @policy_fee_u_id

	SELECT @New_policy_fee_u_id = @@IDENTITY

	SELECT  @fee_value = currency_amount,
			@base_fee_value = base_fee_amount,
			@tax_group_id = tax_group_id,
			@fee_party = party_cnt
		FROM policy_fee_u WITH (NOLOCK)
		WHERE   policy_fee_u_id = @New_policy_fee_u_id

	IF ISNull(@tax_group_id, 0) > 0 BEGIN
		IF NOT EXISTS (SELECT NULL FROM tax_calculation WHERE policy_fee_u_id = @policy_fee_u_id)
		BEGIN
			-- Identify previous iFile where fee tax was charged for fee party
			Select @policy_fee_u_id = pfu.policy_fee_u_id
				From policy_fee_u pfu WITH (NOLOCK)
					Inner Join insurance_file ifi WITH (NOLOCK) On ifi.insurance_file_cnt = pfu.insurance_file_cnt and pfu.risk_cnt IS NULL
					Inner Join Tax_Calculation tc WITH (NOLOCK) On tc.policy_fee_u_id = pfu.policy_fee_u_id
				Where ifi.insurance_folder_cnt = @insuranceFolderCnt
						And ifi.insurance_file_cnt < @OldInsuranceFileCnt
						AND pfu.party_cnt = @fee_party
		END
	END

	INSERT INTO tax_calculation (
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
	   Select tax_band_id,
		@fee_value,
		percentage,
		Case is_value
			When 1 Then value * -1
			Else (@fee_value * percentage) / 100.00 End,
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
		@NewInsuranceFileCnt,
		transtype,
		@New_policy_fee_u_id,
		is_not_applied_to_client,
		include_tax_in_instalments,
		spread_tax_across_instalments,
		tax_band_rate_id,
		is_suspended
    FROM    tax_calculation WITH (NOLOCK)
    WHERE   policy_fee_u_id = @policy_fee_u_id

  SELECT @tax_currency_amount = 0
  SELECT @tax_currency_amount = value
    FROM Tax_Calculation WITH (NOLOCK) WHERE policy_fee_u_id = @New_policy_fee_u_id

	/* Revise Tax amounts */
	IF ISNULL(@tax_currency_amount, 0) <> 0 AND ISNULL(@fee_value, 0) <> 0
		UPDATE  policy_fee_u
		SET     base_tax_amount = @tax_currency_amount * (@base_fee_value/@fee_value),
				currency_tax_amount = @tax_currency_amount
		WHERE   policy_fee_u_id=@New_policy_fee_u_id
  
    Fetch Next From Fee_Cursor Into @policy_fee_u_id
  END
    -- end of cursor
	-- Close and Deallocate
	Close Fee_Cursor
	Deallocate Fee_Cursor

GO
