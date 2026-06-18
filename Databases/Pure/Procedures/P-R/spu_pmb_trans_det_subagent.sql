SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pmb_trans_det_subagent'
GO

CREATE PROCEDURE spu_pmb_trans_det_subagent
    @transaction_export_folder_cnt INT,
    @transaction_type CHAR(1),
    @total_extras_gross MONEY,
    @total_fees_calc MONEY,
    @total_discount_calc MONEY,
    @total_insurer_fee_calc MONEY,
    @subagent_commission MONEY OUTPUT
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
    @insurance_file_cnt INT,
    @policy_agents_id INT,
    @cover_start_date DATETIME,
    @risk_code_id INT,
    @tax_amount MONEY,
    @policyshares_count INT,
    @party_id INT,
    @shortname VARCHAR(255),
    @split_percentage FLOAT,
    @split_value MONEY,
    @split_ipt MONEY,
    @split_extras MONEY,
    @split_fees MONEY,
    @split_discount MONEY,
    @split_insurer_fees MONEY,
    @this_premium MONEY,
    @commission_premium MONEY,
    @commission_amount MONEY,
    @subagent_cnt INT,
    @subagent_id INT,
    @subagent_shortname VARCHAR(255),
    @subagent_amount MONEY,
    @paid_direct INT,
    @source_id INT,
    @domiciled_for_tax tinyint,
    @subagent_tax MONEY 
/* Determine whether paid direct - if so   contra out fees extras and discounts only from client
postings as they will already have been */

SELECT @subagent_commission = 0
SELECT @paid_direct = 0

SELECT
    @subagent_cnt = e.agent_cnt,
    @subagent_shortname = p.shortname,
    @subagent_commission = ROUND(e.agent_commission_value, 2) + ROUND(e.tax_amount, 2),
    @subagent_id = p.party_cnt,
    @source_id = p.source_id,
    @insurance_file_cnt = e.insurance_File_Cnt,
    @policy_agents_id = e.policy_agents_id,
    @domiciled_for_Tax = p.domiciled_for_tax,
    @subagent_tax = e.tax_amount 
FROM transaction_export_folder t
JOIN event_policy_agents e
    ON e.insurance_file_cnt = t.insurance_file_cnt
JOIN party p
    ON p.party_cnt = e.agent_cnt
JOIN party_agent a
    ON a.party_cnt = p.party_cnt
JOIN party_agent_type y
    ON y.party_agent_type_id = a.party_agent_type_id
WHERE t.transaction_export_folder_cnt = @transaction_export_folder_cnt
AND y.description = 'SUB AGENT'

SELECT @paid_direct = 0

/*SELECT 
    @paid_direct = 1
FROM event_insurance_file ei
WHERE ei.insurance_file_cnt = @insurance_file_cnt
AND ei.payment_method = 'Direct Debit'*/

SELECT 
    @paid_direct = 1
FROM event_insurance_file ei
INNER JOIN payment_method P ON P.description=ei.payment_method
WHERE ei.insurance_file_cnt = @insurance_file_cnt
AND P.direct_to_insurer=1
AND p.roll_up_tax_postings=1

/*For Gemini/Schemes policies this value will be negative for credit transactions*/
IF @subagent_commission < 0 AND @transaction_type = 'C'
BEGIN
    SELECT @subagent_commission = @subagent_commission * -1
END
            
/*See if we need to reverse shares or single client*/
SELECT 
    @policyshares_count = COUNT(H.party_cnt)
FROM Event_Policy_Shared_Premiums    H
WHERE H.Insurance_file_cnt = @insurance_file_cnt

/*Post cancelling out transaction to the client*/
IF @policyshares_count = 0 AND @paid_direct= 0
BEGIN
    SELECT 
        @party_id = p.party_cnt,
        @shortname = p.shortname,
        @this_premium = ROUND(i.this_premium, 2)
    FROM transaction_export_folder t 
    JOIN event_insurance_file i
        ON i.insurance_file_cnt = t.insurance_file_cnt
    JOIN party p
        ON p.party_cnt = t.insurance_holder_cnt
    WHERE t.transaction_export_folder_cnt = @transaction_export_folder_cnt

    /*Credit Net Premium to the Client Account*/
    SELECT @transaction_ledger_code = 'SL'
    SELECT @account_type_code = 'SALESLEDGR'
    SELECT @ceded_ref = NULL
    SELECT @cover_share_percent = 0
    SELECT @sum_insured_total = 0
    SELECT @charges_total = 0
    SELECT @taxes_total = 0
    SELECT @recoveries_total = 0
    SELECT @commission_excluded = 0
    SELECT @withholding_tax_excluded = 0
    SELECT @mapping_code = @shortname
    SELECT @transaction_account_key = @party_id
    
    IF @transaction_type = 'C' 
    BEGIN
        SELECT @total_insurer_fee_calc = (@total_insurer_fee_calc * -1)
    END
    
    /*Set transaction amount*/ 
    SELECT @transaction_amount = @this_premium + @total_extras_gross - @total_discount_calc + @total_insurer_fee_calc
    
    IF @transaction_amount < 0 AND @transaction_type = 'D'
    BEGIN
        SELECT @transaction_amount = @transaction_amount * -1
    END
    
    IF @transaction_amount > 0 AND @transaction_type = 'C'
    BEGIN
        SELECT @transaction_amount = @transaction_amount * -1
    END
        
    SELECT @transaction_amount = @transaction_amount + @total_fees_calc
        
    SELECT @transaction_amount = @transaction_amount * -1
    
    SELECT @subagent_amount = @transaction_amount

    /*Get next export record*/
    SELECT 
        @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
    FROM transaction_export_detail
    WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
    
    /*Insert contra client record*/
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
        'GROSS'
    )
END

/*Post cancelling out transactions to the policy shares*/
IF @policyshares_count > 0 AND @paid_direct = 0
BEGIN
    
    SELECT @subagent_amount = 0

    DECLARE s_shares CURSOR FAST_FORWARD FOR
        SELECT 
            transaction_account_key,
            mapping_code,
            ROUND(SUM(transaction_amount) * -1, 2)
        FROM transaction_export_detail 
        WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
        AND transaction_ledger_code = 'SL'
        GROUP BY
            transaction_account_key,
            mapping_code

    OPEN s_shares
    
    FETCH NEXT FROM s_shares INTO
        @party_id,
        @shortname,
        @transaction_amount

    WHILE @@FETCH_STATUS = 0
    BEGIN
    
        /*Credit Net Premium to the Client Account*/
        SELECT @transaction_ledger_code = 'SL'
        SELECT @account_type_code = 'SALESLEDGR'
        SELECT @ceded_ref = NULL
        SELECT @cover_share_percent = 0
        SELECT @sum_insured_total = 0
        SELECT @charges_total = 0
        SELECT @taxes_total = 0
        SELECT @recoveries_total = 0
        SELECT @commission_excluded = 0
        SELECT @withholding_tax_excluded = 0
        SELECT @mapping_code = @shortname
        SELECT @transaction_account_key = @party_id

        /*Update the subagent amount*/
        SELECT @subagent_amount = @subagent_amount + @transaction_amount

        /*Get next export record*/
        SELECT 
            @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
        FROM transaction_export_detail
        WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
        
        /*Insert contra policy share records*/
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
            'GROSS'
        )

        FETCH NEXT FROM s_shares INTO
            @party_id,
            @shortname,
            @transaction_amount
    END

    CLOSE s_shares
    DEALLOCATE s_shares

END

/*Debit the Sub Agent*/
SELECT @transaction_ledger_code = 'SB'
SELECT @account_type_code = 'SUBAGENTLG'
SELECT @ceded_ref = NULL
SELECT @cover_share_percent = 0
SELECT @sum_insured_total = 0
SELECT @charges_total = 0
SELECT @taxes_total = 0
SELECT @recoveries_total = 0
SELECT @commission_excluded = 0
SELECT @withholding_tax_excluded = 0
SELECT @mapping_code = @subagent_shortname
SELECT @transaction_account_key = @subagent_id

/*Set transaction amount which must be opposite of the contra*/
SELECT @subagent_amount = @subagent_amount * -1

IF @transaction_type = 'D'
BEGIN
    IF @paid_direct = 0
    BEGIN
        SELECT @transaction_amount = @subagent_amount - @subagent_commission
    END
    IF @paid_direct = 1
    BEGIN
        SELECT @transaction_amount = @subagent_commission * -1
    END
END

IF @transaction_type = 'C'
BEGIN
    IF @paid_direct = 0
    BEGIN
        SELECT @transaction_amount = @subagent_amount + @subagent_commission
    END
    IF @paid_direct = 1
    BEGIN
        SELECT @transaction_amount = @subagent_commission
    END
END
IF @domiciled_for_tax = 0 
BEGIN
    SELECT @subagent_commission = @subagent_commission - @subagent_tax
END
/*Datasure*/
SELECT @taxes_total =
    (SELECT ISNULL(SUM(ROUND(ETC.value, 2)),0)
    FROM  Event_Tax_Calculation ETC,
        Transaction_Export_Folder   T 
    WHERE ETC.Insurance_file_cnt = T.insurance_file_cnt
    AND T.transaction_export_folder_cnt = @transaction_export_folder_cnt
    AND ETC.TransType = 'TTAC'
    AND ETC.policy_agents_id = @policy_agents_id)        
/*Get next export record*/

IF @Transaction_amount < 0  
BEGIN
    SELECT @taxes_total = @taxes_total * -1
END

SELECT 
    @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
FROM transaction_export_detail
WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

/*Insert sub agent records*/
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
    'NET'
)

/*If Paid direct and extras/fees/discount/insurer fee, then post add ons from client to subagent account*/        
SELECT @transaction_amount = @total_extras_gross

IF @transaction_amount < 0 AND @transaction_type = 'D'
BEGIN
    SELECT @transaction_amount = @transaction_amount * -1
END

IF @transaction_amount > 0 AND @transaction_type = 'C'
BEGIN
    SELECT @transaction_amount = @transaction_amount * -1
END

IF @transaction_type = 'D'
BEGIN
    SELECT @transaction_amount = @transaction_amount - @total_discount_calc
END

IF @transaction_type = 'C'
BEGIN
    SELECT @transaction_amount = @transaction_amount + @total_discount_calc
END

SELECT @transaction_amount = @transaction_amount + @total_fees_calc

IF @paid_direct = 1 AND @transaction_amount <> 0 AND @policyshares_count = 0
BEGIN            
                    
    /*Get next export record*/
    SELECT 
        @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
    FROM transaction_export_detail
    WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
    
    /*Insert sub agent extra records*/
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
        transaction_account_key
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
        @transaction_account_key
    )

    SELECT 
        @party_id = P.party_cnt,
        @shortname = P.shortname
    FROM Transaction_Export_Folder T
    JOIN Event_Insurance_File I
        ON I.insurance_file_cnt = T.insurance_file_cnt
    JOIN Party P
        ON P.party_cnt = T.insurance_holder_cnt
    WHERE T.transaction_export_folder_cnt = @transaction_export_folder_cnt

    SELECT @transaction_ledger_code = 'SL'
    SELECT @account_type_code = 'SALESLEDGR'
    SELECT @mapping_code = @shortname
    SELECT @transaction_account_key = @party_id
    SELECT @taxes_total = 0
    SELECT @transaction_amount = @transaction_amount * -1
    
    /*Get next export record*/
    SELECT 
        @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
    FROM transaction_export_detail
    WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
        
    /*Insert client extra records*/
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
        transaction_account_key
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
        @transaction_account_key
    )
END

GO


