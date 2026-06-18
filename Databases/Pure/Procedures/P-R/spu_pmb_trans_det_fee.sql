SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pmb_trans_det_fee'
GO

CREATE PROCEDURE spu_pmb_trans_det_fee
    @transaction_type CHAR(1),
    @transaction_export_folder_cnt INT,
    @total_fees MONEY OUTPUT
AS

DECLARE 
    @transaction_export_detail_id INT,
    @transaction_amount MONEY,
    @transaction_ledger_code CHAR(2),
    @account_type_code VARCHAR(10),
    @ceded_ref VARCHAR(10),
    @cover_share_percent FLOAT,
    @sum_insured_total MONEY,
    @charges_total MONEY,
    @taxes_total MONEY,
    @recoveries_total MONEY,
    @commission_excluded MONEY,
    @withholding_tax_excluded MONEY,
    @mapping_code VARCHAR(20),
    @transaction_account_key INT,
    @fee_account_key INT,
    @fee_shortname VARCHAR(255),
    @fee_percentage FLOAT,
    @fee_amount MONEY,
    @fee_charge TINYINT,
    @policy_fee_id INT,
    @fee_tax MONEY,
    @total_fee MONEY,
    @fee_tax_type_code VARCHAR(20),
    @fee_tax_amount MONEY,
    @fee_tax_account_mapping_code VARCHAR(20),
    @fee_tax_account_key INT,
    @spare VARCHAR(255),
    @tot_fee_percentage FLOAT,
    @this_premium MONEY,
    @tax_amount MONEY,
    @insurance_file_cnt INT,
    @return_status INT,
    @branch_id INT,
    @suspended TINYINT,
    @release_to_income TINYINT,
    @transdetail_type_code VARCHAR(20),
    @fee_tax_group_code VARCHAR(20),
    @accounting_basis INT,
	@source_id INT,
	@NZ_Config bit,
   @tax_group_id INT

IF EXISTS(SELECT NULL FROM hidden_options WHERE option_number=86 AND value='1')
	SET @NZ_Config=1
ELSE
	SET @NZ_Config=0

/*DC210606 -start -get branch for system options */
SELECT 	@source_id = branch_id 
FROM	transaction_export_folder 
WHERE	transaction_export_folder_cnt = @transaction_export_folder_cnt

SELECT	@accounting_basis = ISNULL(value , 0)
FROM	system_options
WHERE	branch_id = @source_id
AND		option_number = 4012
/*DC210606 -end */

SELECT  @insurance_file_cnt = ei.insurance_file_cnt,
    	@this_premium = ROUND(ei.this_premium, 2),
    	@tax_amount = ROUND(ei.tax_amount, 2),
    	@branch_id = ei.source_id
FROM 	transaction_export_folder tef
JOIN 	event_insurance_file ei
    ON 	ei.insurance_file_cnt = tef.insurance_file_cnt
WHERE 	tef.transaction_export_folder_cnt = @transaction_export_folder_cnt

SELECT 
    @suspended = 1,
    @release_to_income = 1,
    @transdetail_type_code = 'FEE' 

/*Get amounts from Policy Fees*/
DECLARE f_amounts CURSOR FAST_FORWARD FOR
SELECT	p.party_cnt,
        p.shortname,
        epf.fee_percentage,
        ROUND(epf.fee_amount, 2),
        ISNULL(pe.fee_charge,1),
        epf.policy_fee_id,
        epf.tax_amount,
        epf.total_fee
FROM 	event_policy_fee epf
JOIN 	party p
	ON 	p.party_cnt = epf.party_cnt
JOIN 	party_type pt
	ON 	pt.party_type_id = p.party_type_id
LEFT JOIN party_extra pe
	ON 	pe.party_cnt = p.party_cnt
WHERE 	epf.insurance_file_cnt = @insurance_file_cnt
AND 	pt.code = 'FE'

SELECT @total_fees = 0
SELECT @tot_fee_percentage = 0

OPEN f_amounts
FETCH NEXT FROM f_amounts INTO
    @fee_Account_key,
    @fee_shortname,
    @fee_percentage,
    @fee_amount,
    @fee_charge,
    @policy_fee_id,
    @fee_tax,
    @total_fee

WHILE @@FETCH_STATUS = 0
BEGIN

    /*Write individual fee records */

    SELECT @transaction_ledger_code = 'FE'
    SELECT @account_type_code = 'FEELEDGR'
    SELECT @ceded_ref = NULL
    SELECT @cover_share_percent = 0
    SELECT @sum_insured_total = 0
    SELECT @charges_total = 0
    SELECT @taxes_total = 0
    SELECT @recoveries_total = 0
    SELECT @commission_excluded = 0
    SELECT @withholding_tax_excluded = 0
    SELECT @mapping_code = @fee_shortname
    SELECT @transaction_account_key = @fee_account_key
    SELECT @transaction_amount = @fee_amount

    IF @transaction_type = 'C' AND @fee_charge = 0
    BEGIN
        IF @transaction_amount < 0
        BEGIN
            SELECT @transaction_amount = @transaction_amount * -1
            SELECT @total_fee = @total_fee * -1    
        END
    END
    ELSE
    BEGIN
        IF @transaction_amount > 0
        BEGIN
            SELECT @transaction_amount = @transaction_amount * -1
            SELECT @total_fee = @total_fee * -1
        END
    END

    SELECT  @total_fees = @total_fees + @total_fee * -1

    SELECT 
        @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
    FROM transaction_export_detail
    WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
    
    IF @transaction_export_detail_id IS NULL
    BEGIN
        SELECT @transaction_export_detail_id = 1
    END

	SELECT @taxes_total = 	( 
							SELECT 	SUM(ROUND(ETC.value, 2))
							FROM  	event_tax_calculation ETC
							JOIN    transaction_export_folder T
							ON		ETC.insurance_file_cnt = T.insurance_file_cnt
							WHERE 	T.transaction_export_folder_cnt = @transaction_export_folder_cnt
							AND 	ETC.transtype = 'TTF'
							AND 	policy_fee_id = @policy_fee_id
							)
        SELECT @tax_group_id = 
				(SELECT	TG.tax_group_id		
				FROM  	event_tax_calculation ETC
				JOIN	event_policy_fee EPF
					ON EPF.policy_fee_id=ETC.policy_fee_id
				JOIN	party_extra PE
					ON PE.party_cnt=EPF.party_cnt
				JOIN    transaction_export_folder T
					ON	ETC.insurance_file_cnt = T.insurance_file_cnt
				JOIN	tax_group TG
					ON	ETC.tax_group_id = TG.tax_group_id
				WHERE 	T.transaction_export_folder_cnt = @transaction_export_folder_cnt
				AND 	ETC.transtype = 'TTF'
			AND 	etc.policy_fee_id = @policy_fee_id)
			
    INSERT INTO transaction_export_detail 
    (
        transaction_export_folder_cnt,
        transaction_export_detail_id,
        transaction_amount,
        transaction_ledger_code,
        account_type_code,
        ceded_ref,
        cover_share_percent,
        sum_insured_total,
        charges_total,
        taxes_total,
        recoveries_total,
        commission_excluded,
        withholding_tax_excluded,
        mapping_code,
        transaction_account_key,
        suspended,
        release_to_income,
        release_account_code,
        transdetail_type_code,
	tax_group_id
    )        
    VALUES 
    (
        @transaction_export_folder_cnt,
        @transaction_export_detail_id,
        @transaction_amount,
        @transaction_ledger_code,
        @account_type_code,
        @ceded_ref,
        @cover_share_percent,
        @sum_insured_total,
        @charges_total,
        @taxes_total,
        @recoveries_total,
        @commission_excluded,
        @withholding_tax_excluded,
        @mapping_code,
        @transaction_account_key,
        @suspended,
        @release_to_income,
        NULL,
        @transdetail_type_code,
	@tax_group_id
    )

    /* Fetch Next */
    FETCH NEXT FROM f_amounts INTO
        @fee_account_key,
        @fee_shortname,
        @fee_percentage ,
        @fee_amount,
        @fee_charge,
        @policy_fee_id,
        @fee_tax,
        @total_fee

END

/*Close and Deallocate Cursor*/
CLOSE f_amounts
DEALLOCATE f_amounts

IF @NZ_Config=1
BEGIN

	/*Datasure Get Fee Tax Values and post to Tax accounts */
	SELECT  @transaction_ledger_code = 'NO'

	/*DC210606 changed from TAX to TAXOUT */
	SELECT  @account_type_code = 'TAXOUT'
	SELECT  @ceded_ref = NULL
	SELECT  @cover_share_percent = 0
	SELECT  @sum_insured_total = 0
	SELECT  @charges_total = 0
	SELECT  @taxes_total = 0
	SELECT  @recoveries_total = 0
	SELECT  @commission_excluded = 0
	SELECT  @withholding_tax_excluded = 0
	SELECT  @spare = 'FEE TAX'
	/*DC210606 changed from TAX to TAXOUT */
	SELECT  @transdetail_type_code = 'FEETAX'
	SELECT  @suspended = 1
	SELECT  @release_to_income = 0

	/*DC210606 added tax group code */
	DECLARE c_fee_taxes CURSOR FAST_FORWARD FOR
	SELECT	ROUND(ETC.value, 2),
		TG.code,
		ISNULL(PE.fee_charge,1)
	FROM  	event_tax_calculation ETC
	JOIN	event_policy_fee EPF
		ON EPF.policy_fee_id=ETC.policy_fee_id
	JOIN	party_extra PE
		ON PE.party_cnt=EPF.party_cnt
	JOIN    transaction_export_folder T
		ON	ETC.insurance_file_cnt = T.insurance_file_cnt
	JOIN	tax_group TG
		ON	ETC.tax_group_id = TG.tax_group_id
	WHERE 	T.transaction_export_folder_cnt = @transaction_export_folder_cnt
	AND 	ETC.transtype = 'TTF'

	OPEN c_fee_taxes

	/* DC210606 added tax group code */
	FETCH NEXT FROM c_fee_taxes INTO
			@fee_tax_amount,
		@fee_tax_group_code,
		@fee_charge

	WHILE (@@FETCH_STATUS = 0)
	BEGIN

		/* Adjust Signs*/
		IF @transaction_type = 'C' AND @fee_charge = 0
		BEGIN
			if @fee_tax_amount < 0
				select @fee_tax_amount = @fee_tax_amount * -1
		END
		ELSE
		BEGIN
		if @fee_tax_amount > 0
				select @fee_tax_amount = @fee_tax_amount * -1
		END

		/* Format the account code */

		/* DC190606 change the mapping code */
		SELECT @fee_tax_account_mapping_code = 'TAX' + rtrim(@fee_tax_group_code) + 'OUT'

		SELECT 	@fee_tax_account_key = account_key from account
		WHERE 	short_code = @fee_tax_account_mapping_code

		SELECT  @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
		FROM    Transaction_Export_Detail
		WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt

		IF  @transaction_export_detail_id IS NULL
			SELECT  @transaction_export_detail_id = 1

		INSERT INTO Transaction_Export_Detail 
			(
			transaction_export_folder_cnt,
			transaction_export_detail_id,
			transaction_amount,
			transaction_ledger_code,
			account_type_code,
			ceded_ref,
			cover_share_percent,
			sum_insured_total,
			charges_total,
			taxes_total,
			recoveries_total,
			commission_excluded,
			withholding_tax_excluded,
			mapping_code,
			transaction_account_key,
			spare,
			suspended,
			release_to_income,
			release_account_code,
			transdetail_type_code
			)
		VALUES 
			(
			@transaction_export_folder_cnt,
			@transaction_export_detail_id,
			@fee_tax_amount,
			@transaction_ledger_code,
			@account_type_code,
			@ceded_ref,
			@cover_share_percent,
			@sum_insured_total,
			@charges_total,
			@taxes_total,
			@recoveries_total,
			@commission_excluded,
			@withholding_tax_excluded,
			@fee_tax_account_mapping_code,
			@fee_tax_account_key,
			@spare,
			@suspended,
			@release_to_income,
			NULL,
			@transdetail_type_code)

		/* DC210606 added tax group code */
		FETCH NEXT FROM c_fee_taxes INTO
			@fee_tax_amount,
		@fee_tax_group_code,
		@fee_charge

	END

	CLOSE c_fee_taxes
	DEALLOCATE c_fee_taxes

END

/*Datasure end */

GO

