EXECUTE DDLDropProcedure 'spu_SIR_Calculate_Fee_Amounts'
GO
create PROCEDURE [dbo].[spu_SIR_Calculate_Fee_Amounts]
    @policy_fee_u_id INT,
	@Is_Fee_Updated TINYINT = 0
AS

    DECLARE @currency_amount MONEY
    DECLARE @fee_percentage MONEY
    DECLARE @fee_currency_id SMALLINT
    DECLARE @currency_id SMALLINT
    DECLARE @premium MONEY
    DECLARE @company_id INT
    DECLARE @base_currency_amount MONEY
    DECLARE @effective_date_local DATETIME
    DECLARE @base_amount_unrounded_local MONEY
    DECLARE @base_currency_id_local SMALLINT
    DECLARE @currency_base_date_local DATETIME
    DECLARE @return_status_local INT
    DECLARE @base_fee_amount MONEY
    DECLARE @fee_amount MONEY
    DECLARE @risk_cnt INT
    DECLARE @risk_premium MONEY
    DECLARE @premium_to_use MONEY
    DECLARE @peril_group_premium MONEY
    DECLARE @peril_group_id INT
    DECLARE @is_fee_applied_to_cr TINYINT
    DECLARE @signed_multiplier INT
    DECLARE @crProratarate NUMERIC(19,8)
    DECLARE @nIsPorated INT
    DECLARE @nTransaction_type_id SMALLINT
    DECLARE @nInsurance_file_cnt INT
    DECLARE @nParty_cnt INT
	DECLARE @insuranceFileType INT
	DECLARE @sTransactionType  VARCHAR(10)
	DECLARE @transaction_type_id INT
	DECLARE @Is_Backdated_MTA INT = 0
    -- use current date time
    SET @effective_date_local = GetDate()
    SET @premium_to_use = 0
    SET @crProratarate=0
    SET @nIsPorated =0
	--Get Suppress Decimal flag to round whole number
	DECLARE @SuppressDecimalOption AS INT=112
	DECLARE @bIsSuppressDecimal As TINYINT=(Select ISNULL(Value,0) from Hidden_options WHERE option_number=@SuppressDecimalOption)

    -- get policy fee values
     SELECT @fee_percentage = pf.fee_rate_percentage,
            @fee_amount = pf.fee_rate_amount,
            @fee_currency_id = pf.fee_rate_currency_id,
            @currency_id = pf.currency_id,
            @premium = (CASE
            WHEN ISNULL(pf.Calculation_Basis,0)=1
			THEN (i.this_premium + ISNULL(tax.value,0))
            ELSE i.this_premium
            END),
            @company_id = pf.branch_id,
            @risk_premium = r.total_this_premium,
            @risk_cnt = ISNULL(pf.risk_cnt,0),
            @peril_group_id = ISNULL(pf.peril_group_id,0),
            @peril_group_premium = pgp.peril_premium,
            @is_fee_applied_to_cr = pf.is_fee_applied_to_cr,
	        @effective_date_local = i.currency_base_date,
			@crProratarate =pf.Pro_rata_rate ,
            @nIsPorated=pf.Is_Prorated,
            @nTransaction_type_id = pf.transaction_type_id,
            @nInsurance_file_cnt=pf.insurance_file_cnt,
            @nParty_cnt=pf.party_cnt
     FROM   policy_fee_u pf
     JOIN   insurance_file i
       ON   pf.insurance_file_cnt = i.insurance_file_cnt
LEFT JOIN  (SELECT  SUM(this_premium) peril_premium,
                    rst.peril_group_id,
                    risk_cnt
             FROM   rating_section rs
        LEFT JOIN   rating_section_type rst
               ON   rst.rating_section_type_id = rs.rating_section_type_id
         GROUP BY   peril_group_id, risk_cnt) AS pgp
		       ON   pgp.peril_group_id = pf.peril_group_id
              AND   pgp.risk_cnt = pf.risk_cnt
        LEFT JOIN   risk r ON r.risk_cnt = pf.risk_cnt
        LEFT JOIN   (SELECT   Insurance_File_cnt,SUM(ISNULL(value,0)) value
                       FROM   Tax_Calculation tc
                      WHERE   tc.transtype IN('TTR','TTIF')
					    AND   (tc.risk_cnt is null or tc.risk_cnt in
					          (SELECT risk_cnt
						         FROM risk
							    WHERE ISNULL(is_risk_selected,0)<>0))
                             GROUP BY insurance_file_cnt) AS tax
				        ON     TAX.insurance_file_cnt =I.insurance_file_cnt
    WHERE   policy_fee_u_id = @policy_fee_u_id

    -- if this fee is associated with a peril_group use the peril_group premium
    If @peril_group_id <> 0
        SET @premium_to_use = @peril_group_premium
    -- if this fee is associated with a risk_type_group use the risk premium
    ELSE IF @risk_cnt <> 0
        SET @premium_to_use = @risk_premium
    -- otherwise default to using the policy_level premium
    ELSE
        SET @premium_to_use = @premium

    -- if the fee is based on a percentage of the premium then
    If @fee_percentage <> 0
    BEGIN
        --- calculate the fee amount
        SET @currency_amount = ((@premium_to_use * @fee_percentage) /100)
	SET @currency_base_date_local=GETDATE()

        -- get the base fee amount
        EXEC spu_ACT_Do_Currency_Conversion
            @company_id = @company_id,
            @currency_id = @currency_id,
            @currency_amount_unrounded = @currency_amount,
            @currency_base_date = @currency_base_date_local,
            @mode = '1',
            @base_amount_unrounded = @base_fee_amount OUTPUT,
            @base_currency_id = @base_currency_id_local OUTPUT,
            @return_status = @return_status_local
    END
    ELSE -- the fee is based on a value in a particular currency
    BEGIN
        -- convert fee value into actual fee currency
        -- and get base fee amount whilst were at it
		IF @crProratarate<>0 AND @nIsPorated=1
			SELECT @fee_amount=@fee_amount * @crProratarate

        EXEC spu_ACT_Do_Currency_To_Currency_Conversion
        @currency_id_from = @fee_currency_id,
        @currency_amount_from = @fee_amount,
        @company_id = @company_id,
        @currency_id_to = @currency_id,
	    @effective_date = @effective_date_local,
        @currency_amount_to = @currency_amount OUTPUT,
        @base_amount_unrounded = @base_fee_amount OUTPUT
    END

	IF EXISTS(select NULL from mta_insurance_file_link mifl
			  inner join Insurance_File inf on mifl.insurance_file_cnt=inf.Base_Insurance_File_Cnt
			  where inf.insurance_file_cnt=@nInsurance_file_cnt and inf.insurance_file_cnt<>mifl.new_linked_insurance_file_cnt)
	SELECT @Is_Backdated_MTA = 1

	SELECT	@insuranceFileType = insurance_file_type_id FROM insurance_file WHERE insurance_file_cnt = @nInsurance_file_cnt

    SELECT @transaction_type_id =
    CASE WHEN @insuranceFileType IN (1,2) THEN 4
              WHEN @insuranceFileType IN (4,5,6,7) THEN 9
              WHEN @insuranceFileType IN (8,11,12) THEN 7
              WHEN @insuranceFileType IN (9,10) THEN 20
              WHEN @insuranceFileType IN (3)    THEN 10
     END

    SELECT @sTransactionType = Code FROM Transaction_Type WHERE transaction_type_id = @transaction_type_id

	IF @sTransactionType = 'MTC' AND @Is_Backdated_MTA = 1
		SET @premium_to_use = 0

    SET @signed_multiplier = 1

    IF @fee_percentage <> 0
    BEGIN
        -- if the fee is applied on a credit
        IF @premium_to_use < 0 AND @is_fee_applied_to_cr = 1
            SET @signed_multiplier = -1
    END
    ELSE -- amount
    BEGIN
        -- if the fee is applied on a credit
        IF @premium_to_use < 0 AND @is_fee_applied_to_cr = 0
            SET @signed_multiplier = -1
    END

     IF @premium_to_use < 0 AND @is_fee_applied_to_cr = 1
		BEGIN
			IF @nTransaction_type_id = 0
			   SET @signed_multiplier = @signed_multiplier * -1
		END
 IF @sTransactionType = 'MTR' AND @Is_Backdated_MTA <> 1 AND @is_fee_applied_to_cr = 1
		SET @signed_multiplier = -1
 IF @Is_Backdated_MTA <> 1
 BEGIN
    IF ISNULL(@premium_to_use,0)=0
		BEGIN
			DECLARE @nOriginalInsuranceFileCnt INT,
					@nOriginal_currency_amount MONEY

			SELECT @nOriginalInsuranceFileCnt=original_linked_insurance_file_cnt
			  FROM mta_insurance_file_link
			 WHERE new_linked_insurance_file_cnt=@nInsurance_file_cnt

			IF ISNULL(@nOriginalInsuranceFileCnt,0)<>0
			   BEGIN
					SELECT  @nOriginal_currency_amount=currency_amount
					  FROM  policy_fee_u
					 WHERE  insurance_file_cnt=@nOriginalInsuranceFileCnt
					   AND  party_cnt=@nParty_cnt

					IF @nOriginal_currency_amount>=0
						SET @signed_multiplier = 1
					ELSE
						SET @signed_multiplier = -1
			END
	 END
END
	 IF @Is_Fee_Updated = 1
	  SET @signed_multiplier = 1
   -- update the policy fee amounts...
    UPDATE  policy_fee_u
        SET  fee_rate_amount = ISNULL(fee_rate_amount,0) * @signed_multiplier,
			 currency_amount = ROUND(@currency_amount * @signed_multiplier,2),
             base_fee_amount = ROUND(@base_fee_Amount * @signed_multiplier,2),
            fee_premium = @premium_to_use
      WHERE  policy_fee_u_id = @policy_fee_u_id

    
 
GO

