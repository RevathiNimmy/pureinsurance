EXECUTE DDLDropProcedure 'spu_SIR_Recalculate_Peril_Fees'
GO

CREATE PROCEDURE [dbo].[spu_SIR_Recalculate_Peril_Fees]
 @transaction_type_id int,
 @risk_cnt int,
 @insurance_file_cnt int,
 @use_existing_fee_details INT = 0,
 @sTransactiontype VARCHAR(10)= NULL

AS
       DECLARE @base_currency_id SMALLINT
    DECLARE @currency_id SMALLINT
    DECLARE @currency_base_xrate FLOAT
    DECLARE @source_id INT
    DECLARE @TypeOfRates TINYINT
    DECLARE @effective_date DATETIME
    DECLARE @effective_fee_date DATETIME
    DECLARE @premium MONEY
    DECLARE @currency_desc VARCHAR(255)
    DECLARE @currency_isocode varchar(10)
    DECLARE @transaction_sub_type_id int
    DECLARE @peril_group_id INT
    DECLARE @risk_type_group_id INT
    DECLARE @rating_section_type_id INT
    DECLARE @proratarate NUMERIC(19,8)

IF @use_existing_fee_details <> 1
    BEGIN

    --Get policy details
    SELECT  @effective_date = i.currency_base_date,
            @effective_Fee_date = i.cover_start_date,
            @base_currency_id = s.base_currency_id,
            @currency_id = i.currency_id,
            @currency_base_xrate = i.currency_base_xrate,
            @source_id = i.source_id,
            @currency_desc = c.description,
            @currency_isocode = c.iso_code
    FROM    insurance_file i
    JOIN    source s
            ON s.source_id = i.source_id
    JOIN    currency c
            ON c.currency_id = i.currency_id
    WHERE   i.insurance_file_cnt = @insurance_file_cnt

    -- if a valid effective date cannot be found then use todays date
    Select @effective_date = ISNULL(@effective_date, GetDAte())

    --Get source_id where rates are stored
    EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT
    IF @TypeOfRates = 1
        SELECT @source_id = 1

   EXEC spu_get_policy_pro_rata_rate @nInsurancefilecnt=@insurance_file_cnt,@sTransactionType=@sTransactiontype, @crProratarate =  @proratarate OUTPUT
    IF ISNULL(@proratarate,0) =0
        SELECT @proratarate = 1

        -- Cursor for Peril Fees
   DECLARE PerilFeesCursor Cursor Fast_Forward For
   SELECT  Distinct rs.rating_section_type_id, peril_group_id
                FROM    rating_section_type rst
                JOIN    rating_section rs
                        ON rs.rating_section_type_id = rst.rating_section_type_id
                WHERE   rs.risk_cnt = @risk_cnt

    -- Open the Cursor
	Open PerilFeesCursor
	FETCH NEXT FROM PerilFeesCursor
	INTO
	@rating_section_type_id,
	@peril_group_id

	WHILE(@@FETCH_STATUS = 0 )
	BEGIN
		--PerilFeesCursor

			--Not ideal situation,Recalculate Risk Fees also inserts Peril Fees
			--Which need to be cleared before new row is inserted.
			--Only removing Peril related fees inserted by Recalculate Risk Fees sp.
			-- This sp gets called from spu_SIR_Recalculate_Risk_Fees

			-- clear down any entries that have already been calculated
			DELETE  policy_fee_u
			WHERE   transaction_type_id = @Transaction_Type_id
			AND     insurance_file_cnt = @insurance_file_cnt
			AND     risk_cnt = @risk_cnt
			AND		peril_group_id = @peril_group_id

		
		Select @premium =  Sum(this_premium)  from Rating_Section where risk_cnt = @risk_cnt and rating_section_type_id = @rating_section_type_id

		-- set transaction sub_type so that by default it is ignored by the main select
		SET @transaction_sub_type_id = 0

		-- if this is an MTA with a positive premium this is transaction_sub_type : Additional MTA
		IF @premium >= 0 AND @transaction_type_id = 9
        SET @transaction_sub_type_id = 1

		-- if this is an MTA with a negative premium this is transaction_sub_type : Return MTA
		IF @premium < 0 AND @transaction_type_id = 9
			SET @transaction_sub_type_id = 2

    INSERT INTO Policy_Fee_U (
            insurance_file_cnt,
			fee_rate_percentage,
            fee_rate_amount,
            fee_rate_currency_id,
			currency_id,
            branch_id,
			base_currency_id,
            risk_type_group_id,
            peril_group_id,
            transaction_sub_type,
            tax_group_id,
            risk_cnt,
            is_fee_applied_to_cr,
            party_cnt,
            product_id,
            transaction_type_id,
            include_fee_in_instalments,
			spread_fee_across_instalments,
			FeeTypePercent,
			MakeLiveOptions_id ,
			DoPaymentTerms_id,
			Calculation_Basis,
			Is_Prorated,
			Pro_rata_rate,
			is_override,
			fee_amount_id)
    SELECT  @insurance_file_cnt,
            fa.fee_percentage,
            CASE  WHEN FA.Is_Prorated=1 THEN @proratarate*(fa.fee_amount)ELSE fa.fee_amount END ,
            CASE WHEN fa.currency_id IS NULL THEN @currency_id ELSE fa.currency_id END,
            @currency_id,
            @source_id,
            @base_currency_id,
            fa.risk_type_group_id,
            fa.peril_group_id,
            fa.transaction_sub_type,
            fa.tax_group_id,
            @risk_cnt,
            fa.is_fee_applied_to_cr,
            fa.party_cnt,
            fa.product_id,
            fa.transaction_type_id,
            fa.include_fee_in_instalments,
			fa.spread_fee_across_instalments,
			Case when ISNULL(fa.fee_amount,0) = 0 and fa.currency_id IS NULL Then 1
			 ELSE 0
			 END ,
			NULL,
			NULL,
			NULL ,
			FA.Is_Prorated,
			CASE
			WHEN  fa.fee_percentage <> 0 THEN  1
			WHEN  (ISNULL(fa.is_Prorated,0) <> 0) THEN @proratarate
			ELSE  1
			END,
			fa.Is_Override,
			fa.fee_amount_id
    FROM    Fee_Amounts fa
    WHERE   fa.transaction_type_id = @transaction_type_id
    AND    (@transaction_sub_type_id = 0 OR fa.transaction_sub_type = @transaction_sub_type_id)
    AND     fa.product_id IS NULL
    AND     fa.is_deleted = 0
    AND		(fa.peril_group_id = @peril_group_id)
    AND     fa.effective_date =
								(SELECT  MAX(effective_date)
                                 FROM    fee_amounts fa2
                                 WHERE   fa2.transaction_type_id = @Transaction_Type_id
                                 AND    (@transaction_sub_type_id = 0 OR fa2.transaction_sub_type = @transaction_sub_type_id)
                                 AND     fa2.product_id IS NULL
                                 AND     fa2.is_deleted = 0
                                 AND     fa2.peril_group_id = @peril_group_id
                                 AND     fa2.effective_date <= @effective_fee_date
                                 AND     fa2.party_cnt = fa.party_cnt)

	FETCH NEXT FROM PerilFeesCursor
	INTO
	@rating_section_type_id,
	@peril_group_id

    END
END
    Close PerilFeesCursor
    Deallocate PerilFeesCursor

GO


