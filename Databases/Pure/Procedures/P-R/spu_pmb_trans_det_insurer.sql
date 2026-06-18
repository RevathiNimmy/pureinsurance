SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pmb_trans_det_insurer'
GO

CREATE PROCEDURE spu_pmb_trans_det_insurer
    @transaction_export_folder_cnt INT,
    @transaction_type CHAR(1),
    @total_insurers MONEY OUTPUT,
    @total_insurers_commission MONEY OUTPUT,
    @total_insurers_ipt MONEY OUTPUT,
    @total_insurers_fee MONEY OUTPUT
AS

DECLARE 
    @transaction_export_detail_id INT,
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
    @transaction_amount MONEY,
    @mapping_code VARCHAR(20),
    @transaction_account_key INT,
    @spare VARCHAR(255),
    @insurer_party_cnt INT,
    @insurer_party_id INT,
    @insurer_shortname VARCHAR(255),
    @this_premium MONEY,
    @tax_amount MONEY,
    @ipt_amount MONEY,
    @vat_amount MONEY,
    @branch_id INT,
    @suspended TINYINT,
    @release_to_income TINYINT,
    @fee_percentage FLOAT,
    @total_fee_amount MONEY,
    @fee_amount MONEY,
    @tax_calculation_amount MONEY,
    @domiciled_for_tax TINYINT,
    @premium_tax_account_mapping_code VARCHAR(20),
    @premium_tax_account_key INT,
    @premium_tax_type_code VARCHAR(20),
    @premium_tax_amount MONEY,
    @insurer_fee_type CHAR(1)      

/*Initialise variables*/
SELECT @total_insurers = 0
SELECT @total_insurers_commission = 0
SELECT @total_insurers_ipt = 0
SELECT @total_insurers_fee = 0
SELECT @ipt_amount = 0
SELECT @vat_amount = 0
SELECT @tax_amount = 0

SELECT @Tax_Calculation_amount = 0

/*Get insurer details*/
SELECT   
    @insurer_party_cnt = ei.lead_insurer_cnt,
    @insurer_party_id = p.party_cnt,
    @insurer_shortname = p.shortname,
    @ipt_amount =  ROUND(ei.tax_amount, 2),
    @vat_amount = 0,
    @tax_amount = ROUND(ei.tax_amount, 2),
    @this_premium = ROUND(ei.this_premium, 2),
    @total_insurers = ROUND(ei.this_premium, 2) - ROUND(ei.tax_amount, 2) - ROUND(ei.commission_amount, 2),
    @total_insurers_commission = ROUND(ei.commission_amount, 2),
    @total_insurers_ipt = ROUND(@tax_amount, 2),
    @branch_id = ei.source_id,
    @domiciled_for_Tax = p.domiciled_for_tax 
FROM event_insurance_file ei
JOIN transaction_export_folder tef
    ON tef.insurance_file_cnt = ei.insurance_file_cnt
JOIN party p
    ON p.party_cnt = ei.lead_insurer_cnt
WHERE tef.transaction_export_folder_cnt = @transaction_export_folder_cnt

/*Datasure calculate tax on insurers premium */
SELECT @tax_calculation_amount =
	(SELECT ISNULL(SUM(ROUND(ETC.value, 2)),0)
   	FROM  Event_Tax_Calculation ETC,
	 	Transaction_Export_Folder   T 
	WHERE ETC.Insurance_file_cnt = T.insurance_file_cnt
 	AND T.transaction_export_folder_cnt = @transaction_export_folder_cnt
   	AND ETC.TransType = 'TTIF')


/*Set GROSS fields*/
SELECT @suspended = 0
SELECT @release_to_income = 0 
SELECT @transaction_ledger_code = 'IN'
SELECT @account_type_code = 'INSURERLED'
SELECT @ceded_ref = NULL
SELECT @cover_share_percent = 0
SELECT @sum_insured_total = 0
SELECT @charges_total =0
SELECT @taxes_total = @tax_calculation_amount
SELECT @recoveries_total = 0
SELECT @commission_excluded = 0
SELECT @withholding_tax_excluded = 0
SELECT @mapping_code = @insurer_shortname
SELECT @transaction_account_key = @insurer_party_id
SELECT @spare = 'GROSS'

IF @domiciled_for_tax = 1
BEGIN
    SELECT @transaction_amount = @total_insurers + @total_insurers_commission + @total_insurers_ipt
END
ELSE
BEGIN
    SELECT @transaction_amount = @total_insurers + @total_insurers_commission
END

IF @this_premium > 0 AND @transaction_type = 'D'
BEGIN
    SELECT @transaction_amount = @transaction_amount * -1
END

IF @this_premium < 0 AND @transaction_amount < 0
BEGIN
    SELECT @transaction_amount = @transaction_amount * -1        
END    

IF @transaction_amount <> 0 
BEGIN

    /*Get next export record*/
    SELECT
        @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
    FROM transaction_export_detail
    WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

    IF @transaction_export_detail_id IS NULL
    BEGIN
        SELECT @transaction_export_detail_id = 1
    END

    /*Insert GROSS record*/
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
        'GROSS'
    )

    /*Set COMM fields*/
    /*Datasure calculate tax on insurers commission */
    SELECT @tax_calculation_amount =
        (SELECT ISNULL(SUM(ROUND(ETC.value, 2)),0)
        FROM  Event_Tax_Calculation ETC,
            Transaction_Export_Folder   T 
        WHERE ETC.Insurance_file_cnt = T.insurance_file_cnt
        AND T.transaction_export_folder_cnt = @transaction_export_folder_cnt
        AND ETC.TransType = 'TTIC') 


    SELECT @suspended = 0
    SELECT @release_to_income = 0 
    SELECT @transaction_ledger_code = 'IN'
    SELECT @account_type_code = 'INSURERLED'
    SELECT @ceded_ref = NULL
    SELECT @cover_share_percent = 0
    SELECT @sum_insured_total = 0
    SELECT @charges_total = 0
    SELECT @taxes_total = @tax_calculation_amount
    SELECT @recoveries_total = 0
    SELECT @commission_excluded = 0
    SELECT @withholding_tax_excluded = 0
    SELECT @mapping_code = @insurer_shortname
    SELECT @transaction_account_key = @insurer_party_id
    SELECT @spare = 'COMM'
    SELECT @transaction_amount = @total_insurers_commission

    /*If credit then always credit the commission back to the insurer*/
    IF @transaction_type = 'C' AND @Transaction_amount > 0 
    BEGIN
        SELECT @transaction_amount = @transaction_amount * -1
        SELECT @taxes_total = @taxes_total * -1
    END

    /*If debit then always debit the insurer our commission*/
    IF @transaction_type = 'D' AND @Transaction_amount < 0 
    BEGIN
        SELECT @transaction_amount = @transaction_amount * -1
        SELECT @taxes_total = @taxes_total * -1
    END

    /*Get next export record*/
    SELECT @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
    FROM transaction_export_detail
    WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

    /*Insert COMM record*/
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
        'COMM'
    )
END

IF EXISTS
    (
        SELECT   
            NULL
        FROM transaction_export_folder t 
        JOIN event_policy_fee pf
            ON pf.insurance_file_cnt = t.insurance_file_cnt
        WHERE t.transaction_export_folder_cnt = @transaction_export_folder_cnt
        AND pf.party_cnt = @insurer_party_cnt
    )
BEGIN
    /*Get insurer fee details*/
    SET @total_fee_amount=0

    DECLARE IFEE_CURSOR CURSOR FAST_FORWARD FOR
    SELECT   
        epf.fee_percentage,
        ROUND(epf.total_fee, 2),
        ISNULL(epf.insurer_fee_type,'')    
    FROM transaction_export_folder tef 
    JOIN event_policy_fee epf
        ON epf.insurance_file_cnt = tef.insurance_file_cnt
    WHERE tef.transaction_export_folder_cnt = @transaction_export_folder_cnt
    AND epf.party_cnt = @insurer_party_cnt

    OPEN IFEE_CURSOR
    FETCH NEXT FROM IFEE_CURSOR INTO @fee_percentage,@fee_amount,@insurer_fee_type

    WHILE @@FETCH_STATUS=0

    BEGIN

        IF @fee_amount = 0
            SET @fee_amount = ROUND(@fee_percentage * @this_premium / 100, 2)

	IF @insurer_fee_type='C'
	    SET @fee_amount=-@fee_amount

	SET @total_fee_amount=@total_fee_amount + @fee_amount

        FETCH NEXT FROM IFEE_CURSOR INTO @fee_percentage,@fee_amount,@insurer_fee_type

    END

    CLOSE IFEE_CURSOR
    DEALLOCATE IFEE_CURSOR

    /*Set IFEE fields*/
    SELECT @suspended = 0
    SELECT @release_to_income = 0 
    SELECT @transaction_ledger_code = 'IN'
    SELECT @account_type_code = 'INSURERLED'
    SELECT @ceded_ref = NULL
    SELECT @cover_share_percent = 0
    SELECT @sum_insured_total = 0
    SELECT @charges_total = 0
    SELECT @taxes_total = 0
    SELECT @recoveries_total = 0
    SELECT @commission_excluded = 0
    SELECT @withholding_tax_excluded = 0
    SELECT @mapping_code = @insurer_shortname
    SELECT @transaction_account_key = @insurer_party_id
    SELECT @spare = 'IFEE'
    SELECT @transaction_amount = @total_fee_amount

    /*Set total fee amount to return*/
    SELECT @total_insurers_fee = @transaction_amount
    
    IF @transaction_type = 'D' 
        SELECT @transaction_amount = @transaction_amount * -1
    
    /*Get next export record*/
    SELECT 
        @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
    FROM transaction_export_detail
    WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

    IF @transaction_export_detail_id IS NULL
    BEGIN
        SELECT @transaction_export_detail_id = 1
    END

    /*Insert IFEE record*/
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
        'IFEE'
    )
END

GO
