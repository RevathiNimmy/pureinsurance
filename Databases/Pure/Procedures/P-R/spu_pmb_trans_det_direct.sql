SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pmb_trans_det_direct'
GO

CREATE PROCEDURE spu_pmb_trans_det_direct
    @transaction_export_folder_cnt INT,
    @transaction_type CHAR(1),
    @total_extras_gross MONEY,
    @total_fees_calc MONEY,
    @total_discount_calc MONEY,
    @total_insurer_fee_calc MONEY,
    @manual_aj MONEY,
    @docref VARCHAR(25)
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
    @insurance_file_cnt INT,
    @tax_amount MONEY,
    @policyshares_count INT,
    @party_cnt INT,
    @party_id INT,
    @shortname VARCHAR(255),
    @split_percentage FLOAT,
    @split_value MONEY,
    @split_ipt MONEY,
    @split_extras MONEY,
    @split_fees MONEY,
    @split_discount MONEY,
    @this_premium MONEY,
    @insurer_cnt INT,
    @insurer_id INT,
    @insurer_shortname VARCHAR(255),
    @insurer_amount MONEY,
    @sub_agent_cnt INT


SELECT @insurer_amount = 0
SELECT @sub_agent_cnt = 0


/*Get insurer details*/
SELECT 
    @insurer_cnt = i.lead_insurer_cnt,
    @insurer_id = p.party_cnt,
    @insurer_shortname = p.shortname,
    @insurance_file_cnt = i.insurance_file_cnt 
FROM event_insurance_file i
JOIN transaction_export_folder t
    ON t.insurance_file_cnt = i.insurance_file_cnt
JOIN party p
    ON p.party_cnt = i.lead_insurer_cnt 
WHERE t.transaction_export_folder_cnt = @transaction_export_folder_cnt

IF @manual_aj <> 0
BEGIN

    IF @insurer_shortname LIKE 'MULTI%'
    BEGIN
        SELECT 
            @insurer_cnt = p.party_cnt,
            @insurer_id = p.party_cnt,
            @insurer_shortname = p.shortname,
            @insurance_file_cnt = tef.insurance_file_cnt 
        FROM transaction_export_folder tef
        JOIN event_policy_coinsurers pc
            ON pc.insurance_file_cnt = tef.insurance_file_cnt
        JOIN party p
            ON p.party_cnt = pc.party_cnt
        WHERE tef.transaction_export_folder_cnt = @transaction_export_folder_cnt
    END

    /*Get Sub Agent*/
    SELECT @sub_agent_cnt =
        (
            SELECT  
                ISNULL(MAX(P.party_cnt), 0)
            FROM event_policy_agents EA
            JOIN transaction_export_folder T
                ON T.insurance_file_cnt = EA.insurance_File_cnt
            JOIN party P
                ON P.party_cnt = EA.agent_Cnt
            JOIN party_agent PA
                ON PA.party_Cnt = P.party_cnt
            JOIN party_agent_type PAT
                ON PAT.party_agent_type_id = PA.party_agent_type_id
                AND PAT.description = 'SUB AGENT'
            WHERE T.transaction_export_folder_cnt = @transaction_export_folder_cnt
        )
END

/*See if we need to reverse shares or single client */
SELECT 
    @policyshares_count = COUNT(party_cnt)
FROM event_policy_shared_premiums 
WHERE insurance_file_cnt = @insurance_file_cnt

/* Post Cancelling Out Transaction(s) to the Client */

/*SUB-AGENT*/
IF @sub_agent_cnt <> 0
BEGIN
    SELECT  
        @party_cnt = party_cnt,
        @party_id = party_cnt,
        @shortname = shortname
    FROM party
    WHERE party_cnt = @sub_agent_Cnt

    SELECT  
        @this_premium = ROUND(i.this_premium, 2)
    FROM event_insurance_file  i
    JOIN transaction_export_folder t
        ON t.insurance_file_cnt = i.insurance_file_cnt
    WHERE t.transaction_export_folder_cnt = @transaction_export_folder_cnt


    /* Credit Net Premium to the Sub Agent Account */

    SELECT @transaction_ledger_code = 'SB'
    SELECT @account_type_code = 'SUBAGENTLD'
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
    SELECT @spare = 'DIRECTDEBIT'
    
    /*Set transaction amount*/
    SELECT @transaction_amount = @manual_aj

    IF @transaction_amount < 0 AND @transaction_type = 'D'
    BEGIN
        SELECT @transaction_amount = @transaction_amount * -1
    END
    
    IF @transaction_amount > 0 AND @transaction_type = 'C'
    BEGIN
        SELECT @transaction_amount = @transaction_amount * -1
    END

    SELECT @insurer_amount = @transaction_amount



    /* reverse it whatever */
    SELECT @transaction_amount = @transaction_amount * -1
    
    SELECT  
        @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
    FROM transaction_export_detail
    WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
    
    IF @transaction_export_detail_id IS NULL
    BEGIN
        SELECT @transaction_export_detail_id = 1
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
        spare
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
        @spare
    )

END

/*SINGLE CLIENT*/
IF @policyshares_count = 0 AND @sub_agent_cnt = 0
BEGIN

    SELECT  
        @party_cnt = party_cnt,
        @party_id = party_cnt,
        @shortname = shortname,
        @this_premium = ROUND(i.this_premium, 2)
    FROM event_insurance_file  i
    JOIN transaction_export_folder t
        ON t.insurance_file_cnt = i.insurance_file_cnt
    JOIN party p
        ON p.party_cnt = t.insurance_holder_cnt
    WHERE t.transaction_export_folder_cnt = @transaction_export_folder_cnt
    

    /* Credit Net Premium to the Client Account */

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
    SELECT @spare = 'DIRECTDEBIT'
    
    IF @transaction_type = 'C' 
    BEGIN
	SELECT @total_insurer_fee_calc = (@total_insurer_fee_calc * -1)
    END
    /* Set transaction amount */
    SELECT @transaction_amount = @this_premium + @total_insurer_fee_calc

    /* override if manual aj */
    If @manual_aj <> 0
    BEGIN
        SELECT @transaction_amount = @manual_aj
    END

    IF @transaction_amount < 0 AND @transaction_type = 'D'
    BEGIN
        SELECT @transaction_amount = @transaction_amount * -1
    END
    
    IF @transaction_amount > 0 AND @transaction_type = 'C'
    BEGIN
        SELECT @transaction_amount = @transaction_amount * -1
    END

    SELECT @insurer_amount = @transaction_amount

    SELECT @transaction_amount = @transaction_amount * -1
    
    SELECT 
        @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
    FROM transaction_export_detail
    WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
    
    IF @transaction_export_detail_id IS NULL
    BEGIN
        SELECT @transaction_export_detail_id = 1
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
        'DIRECTDEBIT'
    )
END

/*Multi-Clients*/
IF @policyshares_count > 0 AND @sub_agent_cnt = 0
BEGIN
    SELECT 
        @insurance_file_cnt = i.insurance_file_cnt,
        @tax_amount = ROUND(i.tax_amount, 2) + ROUND(i.vat_amount, 2)
    FROM event_insurance_file i
    JOIN transaction_export_folder t
        ON t.insurance_file_cnt = i.insurance_file_cnt 
    WHERE t.transaction_export_folder_cnt = @transaction_export_folder_cnt

    DECLARE s_shares CURSOR FAST_FORWARD FOR
        SELECT 
            s.party_cnt,
            p.party_cnt,
            p.shortname,
            s.split_percentage,
            ROUND(s.split_value, 2)
        FROM event_policy_shared_premiums s
        JOIN party p
            ON p.party_cnt = s.party_cnt
        WHERE s.insurance_file_cnt = @insurance_file_cnt

    /* Open the Shares Cursor */
    OPEN s_shares
    FETCH NEXT FROM s_shares INTO
        @party_cnt,
        @party_id,
        @shortname,
        @split_percentage,
        @split_value

    WHILE @@FETCH_STATUS = 0
    BEGIN

        /* Post Transactions to the Client */
        SELECT @split_ipt = 0
        SELECT @split_extras = 0
        SELECT @split_fees = 0
        SELECT @split_ipt = ROUND(@tax_amount * @split_percentage / 100, 2)
        SELECT @split_extras = ROUND(@total_extras_gross * @split_percentage / 100, 2)
        SELECT @split_fees = ROUND(@total_fees_calc * @split_percentage / 100, 2)
        SELECT @split_discount = ROUND(@total_discount_calc * @split_percentage / 100, 2)
        
        /* Credit Net Premium to the Client Account */
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
        SELECT @spare = 'DIRECTDEBIT'
        
        /* Set transaction amount */
        SELECT @transaction_amount = @split_value + @split_ipt

        IF @transaction_amount < 0 AND @transaction_type = 'D'
        BEGIN
            SELECT @transaction_amount = @transaction_amount * -1
        END
        
        IF @transaction_amount > 0 AND @transaction_type = 'C'
        BEGIN
            SELECT @transaction_amount = @transaction_amount * -1
        END
        
        SELECT @insurer_amount = @insurer_amount + @transaction_amount
        
        /* reverse it whatever */
        SELECT @transaction_amount = @transaction_amount * -1
        
        SELECT 
            @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
        FROM transaction_export_detail
        WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
        
        IF @transaction_export_detail_id IS NULL
        BEGIN
            SELECT @transaction_export_detail_id = 1
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
            'DIRECTDEBIT'
        )

        /* Fetch Next */
        FETCH NEXT FROM s_shares INTO
                @party_cnt,
                @party_id,
                @shortname,
                @split_percentage,
                @split_value

    END

    /* Close and Deallocate Cursor */

    CLOSE s_shares
    DEALLOCATE s_shares

END

/* Debit the Insurer */

/* Credit Net Premium to the Client Account */
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
SELECT @transaction_account_key = @insurer_id

SELECT  @spare = 'DDREV ' + @docref

/* Set transaction amount */
SELECT @transaction_amount = @insurer_amount

SELECT 
    @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
FROM transaction_export_detail
WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

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
    'DDREV'
)


GO


