SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pmb_trans_det_commission'
GO

CREATE PROCEDURE spu_pmb_trans_det_commission
    @transaction_export_folder_cnt INT,
    @transaction_type CHAR(1),
    @agent_amount_calc MONEY,
    @total_extras_comm_calc MONEY,
    @total_insurers_comm_calc MONEY,
    @total_coinsurers_comm_calc MONEY,
    @subagent_comm_calc MONEY
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
    @source_id SMALLINT,
    @transaction_account_key INT,
    @spare VARCHAR(255),
    @insurer_amount MONEY,
    @premium_amount MONEY,
    @tax_amount MONEY,
    @commission_amount MONEY,
    @commission_account INT,
    @insurance_file_cnt INT,
    @return_status INT,
    @CommissionByRisk INT,
    @branch_id INT,
    @suspended TINYINT,
    @release_to_income TINYINT,
    @transdetail_type_code VARCHAR(20),
    @commission_tax_total MONEY,
    @commission_tax_account_mapping_code VARCHAR(20),
    @commission_tax_account_key INT,
    @commission_tax_amount MONEY,
    @commission_tax_group_code VARCHAR(20),
	@accounting_basis INT,
    @sourceid INT,
    @agent_commission_tax MONEY,
    @commission_tax_group_id INT,
    @commission_tax_band_id INT

--===============================================================================================================

SELECT  @transaction_ledger_code = 'NO'
SELECT  @account_type_code = 'TAXOUT'
SELECT  @ceded_ref = NULL
SELECT  @cover_share_percent = 0
SELECT  @sum_insured_total = 0
SELECT  @charges_total = 0
SELECT  @taxes_total = 0
SELECT  @recoveries_total = 0
SELECT  @commission_excluded = 0
SELECT  @withholding_tax_excluded = 0
SELECT  @spare = 'TAX'
SELECT  @transdetail_type_code = 'TAX'
SELECT  @suspended = 1
SELECT  @release_to_income = 0

/*DC210606 Get the Accounting Basis system option*/

SELECT 	@sourceid
FROM 	transaction_export_folder
WHERE 	transaction_export_folder_cnt = @transaction_export_folder_cnt

SELECT 	@accounting_basis = ISNULL(value, 0)
FROM	system_options
WHERE	option_number = 4012
AND 	branch_id = @sourceid

/* First Post Any Commission Taxes */

SELECT @commission_tax_total = 0

DECLARE c_commission_taxes CURSOR FAST_FORWARD FOR
	SELECT 	SUM(ETC.value),
			MAX(TG.code),
			MAX(TG.tax_group_id),
			MAX(TB.tax_band_id)
   	FROM 	event_tax_calculation ETC
	JOIN 	transaction_export_folder T
	ON 		ETC.Insurance_file_cnt = T.insurance_file_cnt
    JOIN 	tax_band TB
	ON 		ETC.tax_band_id = TB.tax_band_id
    JOIN 	tax_group TG
	ON 		ETC.tax_group_id = TG.tax_group_id
	WHERE 	T.transaction_export_folder_cnt = @transaction_export_folder_cnt
   	AND 	ETC.transtype = 'TTIC'
   	GROUP BY ETC.tax_band_id

OPEN c_commission_taxes

FETCH NEXT FROM c_commission_taxes INTO
    @commission_tax_amount,
    @commission_tax_group_code,
	@commission_tax_group_id,
	@commission_tax_band_id

WHILE (@@FETCH_STATUS = 0)
BEGIN

/* Adjust Signs*/
	if @transaction_type = 'D'
	    if @commission_tax_amount > 0
		select @commission_tax_amount = @commission_tax_amount * -1
	if @transaction_type = 'C'
	    if @commission_tax_amount < 0
		select @commission_tax_amount = @commission_tax_amount * -1

/* Accumulate the commission taxes */
        SELECT @commission_tax_total = @commission_tax_total + @commission_tax_amount

/* Format the account code */
	SELECT 	@commission_tax_account_mapping_code = 'TAX' + rtrim(@commission_tax_group_code) + 'OUT'

	SELECT 	@commission_tax_account_key = account_key from account
	WHERE 	short_code = @commission_tax_account_mapping_code

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
    		transdetail_type_code,
			tax_group_id,
			tax_band_id
			)
	VALUES
			(
    		@transaction_export_folder_cnt,
    		@transaction_export_detail_id,
    		@commission_tax_amount,
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
    		@commission_tax_account_mapping_code,
    		@commission_tax_account_key,
    		@spare,
    		@suspended,
    		@release_to_income,
    		NULL,
    		@transdetail_type_code,
			@commission_tax_group_id,
			@commission_tax_band_id
			)

	FETCH NEXT FROM c_commission_taxes INTO
		@commission_tax_amount,
        @commission_tax_group_code,
		@commission_tax_group_id,
		@commission_tax_band_id

END

CLOSE c_commission_taxes
DEALLOCATE c_commission_taxes

SELECT @agent_commission_tax = @commission_tax_total

--=======================================================================================================

If @total_insurers_comm_calc <> 0
BEGIN

	IF @transaction_type = 'D'
		SELECT @total_insurers_comm_calc = @total_insurers_comm_calc

	IF @transaction_type = 'C'
		SELECT @total_insurers_comm_calc = @total_insurers_comm_calc

END

If @total_coinsurers_comm_calc <> 0
BEGIN

	IF @transaction_type = 'D'
		SELECT @total_coinsurers_comm_calc = @total_coinsurers_comm_calc

	IF @transaction_type = 'C'
		SELECT @total_coinsurers_comm_calc = @total_coinsurers_comm_calc

END

SELECT @commission_tax_total = 0

/* Next Post Any Extras Commission Taxes */
IF @total_extras_comm_calc <> 0
BEGIN

	DECLARE c_commission_taxes CURSOR FAST_FORWARD FOR
		SELECT  SUM(ETC.value),
				MAX(TG.code),
				MAX(TG.tax_group_id),
				MAX(TB.tax_band_id)
   		FROM  	event_tax_calculation ETC
	 	JOIN	transaction_export_folder T
		ON 		ETC.Insurance_file_cnt = T.insurance_file_cnt
        JOIN	tax_band TB
		ON 		ETC.Tax_Band_ID = TB.Tax_Band_id
		JOIN 	tax_group TG
		ON		ETC.tax_group_id = TG.tax_group_id
		WHERE  	T.transaction_export_folder_cnt = @transaction_export_folder_cnt
   		AND 	ETC.transtype = 'TTFC'
   		GROUP BY ETC.tax_band_id

	OPEN c_commission_taxes

	FETCH NEXT FROM c_commission_taxes INTO
        			@commission_tax_amount,
					@commission_tax_group_code,
					@commission_tax_group_id,
					@commission_tax_band_id

	WHILE (@@FETCH_STATUS = 0)
	BEGIN

/* Adjust Signs*/
		if @transaction_type = 'D'
	    	if @commission_tax_amount > 0
				select @commission_tax_amount = @commission_tax_amount * -1

		if @transaction_type = 'C'
	    	if @commission_tax_amount < 0
				select @commission_tax_amount = @commission_tax_amount * -1

/* Accumulate the commission taxes */
        SELECT @commission_tax_total = @commission_tax_total + @commission_tax_amount

/* Format the account code */
		SELECT @commission_tax_account_mapping_code = 'TAX' + rtrim(@commission_tax_group_code) + 'OUT'

		SELECT @commission_tax_account_key=account_key from account
					    where short_code = @commission_tax_account_mapping_code

		SELECT 	@transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
		FROM    Transaction_Export_Detail
		WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt

		IF  @transaction_export_detail_id IS NULL
    		SELECT @transaction_export_detail_id = 1

		INSERT INTO Transaction_Export_Detail (
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
    			transdetail_type_code,
				tax_group_id,
				tax_band_id
				)
		VALUES
				(
    			@transaction_export_folder_cnt,
    			@transaction_export_detail_id,
    			@commission_tax_amount * -1,
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
    			@commission_tax_account_mapping_code,
    			@commission_tax_account_key,
    			@spare,
    			@suspended,
    			@release_to_income,
    			NULL,
    			@transdetail_type_code,
				@commission_tax_group_id,
				@commission_tax_band_id
				)


		FETCH NEXT FROM c_commission_taxes INTO
				@commission_tax_amount,
				@commission_tax_group_code,
				@commission_tax_group_id,
				@commission_tax_band_id

	END

	CLOSE c_commission_taxes
	DEALLOCATE c_commission_taxes

	IF @transaction_type = 'D'
		SELECT @total_extras_comm_calc = @total_extras_comm_calc + @commission_tax_total

	IF @transaction_type = 'C'
		SELECT @total_extras_comm_calc = @total_extras_comm_calc - @commission_tax_total

END

--=================================================================================================================

SELECT @commission_tax_total = 0

/* Now post any agent commission taxes */
IF @agent_amount_calc <> 0 OR @subagent_comm_calc <> 0
BEGIN

	SELECT  @account_type_code = 'TAXIN'
	SELECT  @transdetail_type_code = 'AGENTTAX'

	/* Ensure that tax is only posted for domiciled agents */
	DECLARE c_commission_taxes CURSOR FAST_FORWARD FOR
		SELECT 	SUM(ETC.value),
				MAX(TG.code),
				MAX(TG.tax_group_id),
				MAX(TB.tax_band_id)
   		FROM  	event_tax_calculation ETC
	 	JOIN	transaction_export_folder T
		ON		ETC.Insurance_file_cnt = T.insurance_file_cnt
        JOIN	tax_band TB
		ON		ETC.Tax_Band_ID = TB.Tax_Band_id
		JOIN	event_policy_agents EP
		ON		ETC.policy_agents_id = EP.policy_agents_id
		JOIN	party P
		ON		EP.agent_cnt = P.party_cnt
		JOIN	tax_group TG
		ON		ETC.tax_group_id = TG.tax_group_id
		WHERE 	T.transaction_export_folder_cnt = @transaction_export_folder_cnt
   		AND 	ETC.transtype = 'TTAC'
        AND 	P.domiciled_for_tax = 1
   		GROUP BY ETC.tax_band_id

	OPEN c_commission_taxes

	FETCH NEXT FROM c_commission_taxes INTO
       	@commission_tax_amount,
		@commission_tax_group_code,
		@commission_tax_group_id,
		@commission_tax_band_id

	WHILE (@@FETCH_STATUS = 0)
	BEGIN

/* Adjust Signs for credits - tax is always stored as positive */
 		IF @transaction_type = 'C'
 			SELECT @commission_tax_amount = @commission_tax_amount * -1

/* Accumulate the commission taxes */
    		SELECT @commission_tax_total = @commission_tax_total + @commission_tax_amount

/* Format the account code */
		SELECT 	@commission_tax_account_mapping_code = 'TAX' + rtrim(@commission_tax_group_code) + 'IN'

		SELECT 	@commission_tax_account_key=account_key from account
		WHERE 	short_code = @commission_tax_account_mapping_code

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
    		transdetail_type_code,
			tax_group_id,
			tax_band_id
			)
		VALUES
			(
  	 	 	@transaction_export_folder_cnt,
 	 	  	@transaction_export_detail_id,
	    	@commission_tax_amount,
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
    		@commission_tax_account_mapping_code,
    		@commission_tax_account_key,
    		@spare,
    		@suspended,
    		@release_to_income,
    		NULL,
    		@transdetail_type_code,
			@commission_tax_group_id,
			@commission_tax_band_id
			)


		FETCH NEXT FROM c_commission_taxes INTO
			@commission_tax_amount,
			@commission_tax_group_code,
			@commission_tax_group_id,
			@commission_tax_band_id

	END

	CLOSE c_commission_taxes
	DEALLOCATE c_commission_taxes

--
--Test SELECT @agent_amount_calc = @agent_amount_calc + @commission_tax_total

END
/*End of Agent Tax Postings */

--Datasure End

SELECT  @premium_amount = ROUND(ei.this_premium, 2),
    	@tax_amount = ROUND(ei.tax_amount, 2),
    	@commission_amount = ROUND(ei.commission_amount, 2),
    	@source_id = ei.source_id,
    	@insurance_file_cnt = ei.insurance_file_cnt,
    	@branch_id = ei.source_id
FROM 	transaction_export_folder tef
JOIN 	event_insurance_file ei
ON 		ei.insurance_file_cnt = tef.insurance_file_cnt
WHERE 	tef.transaction_export_folder_cnt = @transaction_export_folder_cnt

SELECT	@transdetail_type_code = 'BROK',
    	@suspended = 1,
    	@release_to_income = 1

SELECT  @CommissionByRisk = Value
FROM 	hidden_options
WHERE 	branch_id = 1
AND 	option_number = 40

IF @CommissionByRisk = 1
BEGIN

    SELECT
        @transaction_account_key = p.party_cnt,
        @mapping_code = p.shortname
    FROM event_insurance_file i
    JOIN party_consultant pc
        ON i.account_executive_cnt = pc.party_cnt
    JOIN party p
        ON p.party_cnt = pc.commission_cnt
    WHERE i.insurance_file_cnt = @insurance_file_cnt

END
ELSE
BEGIN

    SELECT	@transaction_account_key = p.party_cnt,
        	@mapping_code = p.shortname
    FROM 	event_insurance_file ei
    JOIN 	risk_code rc
    ON 		rc.risk_code_id = ei.risk_code_id
    JOIN 	risk_group rg
        ON rg.risk_group_id = rc.risk_group_id
    JOIN 	risk_by_source rbs
    ON 		rbs.risk_group_id = rg.risk_group_id
    		AND rbs.source_id = ei.source_id
    JOIN 	party p
    ON 		p.party_cnt = rbs.commission_cnt
    WHERE 	ei.insurance_file_cnt = @insurance_file_cnt

    IF @transaction_account_key IS NULL
    BEGIN

        SELECT	@transaction_account_key = p.party_cnt,
            	@mapping_code = p.shortname
        FROM 	event_insurance_file ei
        JOIN 	risk_code rc
        ON 		rc.risk_code_id = ei.risk_code_id
        JOIN 	risk_group rg
        ON 		rg.risk_group_id = rc.risk_group_id
        JOIN 	risk_by_source rbs
        ON 		rbs.risk_group_id = rg.risk_group_id
        AND 	rbs.source_id = 0
        JOIN 	party p
        ON 		p.party_cnt = rbs.commission_cnt
        WHERE 	ei.insurance_file_cnt = @insurance_file_cnt

    END

END

--========================================================================================================================

SELECT @transaction_ledger_code = 'CO'
SELECT @account_type_code = 'COMMLEDGR'
SELECT @ceded_ref = NULL
SELECT @cover_share_percent = 0
SELECT @sum_insured_total = 0
SELECT @charges_total = 0
SELECT @taxes_total = 0
SELECT @recoveries_total = 0
SELECT @commission_excluded = 0
SELECT @withholding_tax_excluded = 0
SELECT @spare = 'BROK'

IF @transaction_type='D' AND @total_coinsurers_comm_calc > 0
BEGIN
    SELECT @total_coinsurers_comm_calc = @total_coinsurers_comm_calc * -1
END

IF @transaction_type='D' AND @total_insurers_comm_calc > 0
BEGIN
    SELECT @total_insurers_comm_calc = @total_insurers_comm_calc * -1
END

IF @transaction_type='D' AND @total_extras_comm_calc > 0
BEGIN
    SELECT @total_extras_comm_calc = @total_extras_comm_calc * -1
END

IF @transaction_type='D' AND @subagent_comm_calc < 0
BEGIN
    SELECT @subagent_comm_calc = @subagent_comm_calc * -1
END

IF @transaction_type='C' AND @total_insurers_comm_calc < 0
BEGIN
    SELECT @total_insurers_comm_calc = @total_insurers_comm_calc * -1
END

IF @transaction_type='C' AND @total_coinsurers_comm_calc < 0
BEGIN
    SELECT @total_coinsurers_comm_calc = @total_coinsurers_comm_calc * -1
END

IF @transaction_type='C' AND @total_extras_comm_calc < 0
BEGIN
    SELECT @total_extras_comm_calc = @total_extras_comm_calc * -1
END

IF @transaction_type='C' AND @subagent_comm_calc > 0
BEGIN
    SELECT @subagent_comm_calc = @subagent_comm_calc * -1
END

SELECT 	@transaction_amount = @total_insurers_comm_calc + @total_coinsurers_comm_calc +
       		@total_extras_comm_calc + @subagent_comm_calc - @agent_amount_calc - @commission_tax_total

SELECT 	@transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
FROM 	transaction_export_detail
WHERE 	transaction_export_folder_cnt = @transaction_export_folder_cnt

IF @transaction_export_detail_id IS NULL
BEGIN
    SELECT  @transaction_export_detail_id = 1
END

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
    @spare,
    @suspended,
    @release_to_income,
    NULL,
    @transdetail_type_code
	)

--===================================================================================================================

SELECT @commission_tax_total = @agent_commission_tax 

If @commission_tax_total <> 0
BEGIN

	SELECT @transaction_ledger_code = 'CO'
	SELECT @account_type_code = 'BROK TAX'
	SELECT @transdetail_type_code='BROK TAX'
	SELECT @ceded_ref = NULL
	SELECT @cover_share_percent = 0
	SELECT @sum_insured_total = 0
	SELECT @charges_total = 0
	SELECT @taxes_total = 0
	SELECT @recoveries_total = 0
	SELECT @commission_excluded = 0
	SELECT @withholding_tax_excluded = 0
	SELECT @spare = 'BROK TAX'

	SELECT 	@transaction_amount = @commission_tax_total * -1

	SELECT 	@transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
	FROM 	transaction_export_detail
	WHERE 	transaction_export_folder_cnt = @transaction_export_folder_cnt

	IF @transaction_export_detail_id IS NULL
	BEGIN
   	 	SELECT  @transaction_export_detail_id = 1
	END

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
    @spare,
    @suspended,
    @release_to_income,
    NULL,
    @transdetail_type_code
	)
END

GO
