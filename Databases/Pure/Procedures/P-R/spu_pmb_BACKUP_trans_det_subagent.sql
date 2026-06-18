SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pmb_BACKUP_trans_det_subagent'
GO

CREATE PROCEDURE spu_pmb_BACKUP_trans_det_subagent
    @transaction_export_folder_cnt int,
    @transaction_type char(1),
    @total_extras_gross numeric(19, 4),
    @total_fees_calc numeric (19,4),
    @total_discount_calc numeric (19,4),
    @subagent_commission numeric(19, 4) OUTPUT
AS
BEGIN
    DECLARE @transaction_export_detail_id int,
    @transaction_ledger_code char(2),
    @account_type_code varchar(10),
    @ceded_ref varchar(10),
    @cover_share_percent numeric(12,8),
    @sum_insured_total numeric(19,4),
    @charges_total numeric(19,4),
    @taxes_total numeric(19,4),
    @recoveries_total numeric(19,4),
    @commission_excluded numeric(19,4),
    @withholding_tax_excluded numeric(19,4),
    @transaction_amount numeric(19,4),
    @mapping_code varchar(20),
    @transaction_account_key int,
    @insurance_file_cnt int,
    @cover_start_date datetime,
    @risk_code_id int,
    @tax_amount numeric(19,4),
    @policyshares_count int,
    @party_cnt int,
    @party_id int,
    @shortname varchar(255),
    @split_percentage numeric(7,4),
    @split_value numeric(19,4),
    @split_ipt numeric(19,4),
    @split_extras numeric(19,4),
    @split_fees numeric(19,4),
    @split_discount numeric(19,4),
    @this_premium numeric(19,4),
    @commission_premium numeric(19,4),
    @commission_amount numeric(19,4),
    @subagent_cnt int,
    @subagent_id int,
    @subagent_shortname varchar(255),
    @subagent_amount numeric(19,4),
    @paid_direct int,
    @source_id int

    /* Determine whether paid direct - if so contra out fees extras and discounts only from client
    postings as they will already have been */

    SELECT @subagent_commission = 0
    SELECT @paid_direct = 0

    SELECT
        @subagent_cnt = E.agent_cnt,
        @subagent_shortname = P.shortname,
        @subagent_commission = E.agent_commission_value,
        @subagent_id = P.party_cnt,
        @source_id = P.source_id,
        @insurance_File_cnt = E.insurance_File_Cnt
        FROM Event_Policy_Agents E,
        Transaction_Export_Folder T,
        Party P,
        Party_Agent A,
        Party_Agent_type Y
        WHERE T.transaction_Export_folder_cnt = @transaction_export_folder_cnt
        AND E.insurance_file_Cnt = T.insurance_File_cnt
        AND P.party_cnt = E.agent_cnt
        AND A.party_cnt = E.agent_cnt
        AND A.party_agent_type_id = Y.party_agent_type_id
        AND Y.description = "SUB AGENT"

    SELECT @paid_direct = 0

    /*SELECT @paid_direct = 1
        FROM Event_Insurance_File I
        WHERE I.Insurance_file_cnt = @insurance_file_cnt
        AND I.Payment_method = "Direct Debit"*/

      SELECT @paid_direct = 1
        FROM Event_Insurance_File I
	INNER JOIN payment_method p ON p.description=I.payment_method 
        WHERE I.Insurance_file_cnt = @insurance_file_cnt
        AND P.direct_to_insurer=1
	AND p.roll_up_tax_postings=1
	
    --For each Sub Agent calculate and post
    -- See if we need to reverse shares or single client

    SELECT @policyshares_count = COUNT(H.party_cnt)
        FROM Event_policy_shared_premiums H
        WHERE H.Insurance_file_cnt = @insurance_file_cnt

    --Post Cancelling Out Transaction(s) to the Client
    -- SINGLE CLIENT
    --eck120401 if paid direct don't bother with the contra at all
    IF @policyshares_count = 0 and @paid_direct = 0 BEGIN
        SELECT @party_cnt = T.insurance_holder_cnt,
            @party_id = P.party_cnt,
            @shortname = P.shortname,
            @this_premium = I.this_premium
            FROM Event_insurance_file AS I
            INNER JOIN Transaction_Export_Folder AS T
                ON I.insurance_file_cnt = T.insurance_file_cnt
            INNER JOIN Party AS P
                ON T.insurance_holder_cnt = P.party_cnt
            WHERE T.transaction_export_folder_cnt = @transaction_export_folder_cnt

        -- Credit Net Premium to the Client Account
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
        -- Set transaction amount
        -- amount contrad will depend upon whether paid direct

        SELECT @transaction_amount = @this_premium + @total_extras_gross + @total_fees_calc - @total_discount_calc
        IF @transaction_amount < 0 AND @transaction_type = "D"
            SELECT @transaction_amount = @transaction_amount * -1
        IF @transaction_amount > 0 AND @transaction_type = "C"
            SELECT @transaction_amount = @transaction_amount * -1
        -- reverse it whatever
        SELECT @transaction_amount = @transaction_amount * -1
        SELECT @subagent_amount = @transaction_amount

        SELECT @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
            FROM Transaction_Export_Detail
            WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

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
                transaction_account_key)
            VALUES (
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
                @transaction_account_key)
    END

    --eck120401 if paid direct don't bother with the contra at all
    IF @policyshares_count > 0 and @paid_direct = 0 BEGIN
        SELECT @insurance_file_cnt = I.insurance_file_cnt,
            @tax_amount = I.tax_amount + I.vat_amount
            FROM Event_Insurance_File AS I
            INNER JOIN Transaction_Export_Folder AS T
                ON I.insurance_file_cnt = T.insurance_file_cnt
            WHERE T.transaction_export_folder_cnt = @transaction_export_folder_cnt

        DECLARE s_shares CURSOR FAST_FORWARD FOR
            SELECT party_cnt = S.party_cnt,
            party_id = P.party_cnt,
            shortname = P.shortname,
            split_percentage = S.split_percentage,
            split_value = S.split_value
            FROM Event_policy_shared_premiums AS S
            INNER JOIN Party AS P ON S.party_cnt = P.party_cnt
            WHERE S.insurance_file_cnt = @insurance_file_cnt

        -- Open the Shares Cursor
        OPEN s_shares
        FETCH NEXT FROM s_shares INTO
            @party_cnt,
            @party_id,
            @shortname,
            @split_percentage,
            @split_value

        WHILE @@FETCH_STATUS = 0 BEGIN
            -- Post Transactions to the Client
            SELECT @split_ipt = 0
            SELECT @split_extras = 0
            SELECT @split_fees = 0
            SELECT @split_ipt = (@tax_amount * @split_percentage / 100)
            SELECT @split_extras = (@total_extras_gross * @split_percentage / 100)
            SELECT @split_fees = (@total_fees_calc * @split_percentage / 100)
            SELECT @split_discount = (@total_discount_calc * @split_percentage / 100)
            -- Credit Net Premium to the Client Account
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
            -- Set transaction amount
            -- Will depend upon paid direct
            SELECT @transaction_amount = @split_value + + @split_ipt + @split_extras + @split_fees - @split_discount
            IF @transaction_amount < 0 AND @transaction_type = "D"
                SELECT @transaction_amount = @transaction_amount * -1
            IF @transaction_amount > 0 AND @transaction_type = "C"
                SELECT @transaction_amount = @transaction_amount * -1
            -- reverse it whatever
            SELECT @transaction_amount = @transaction_amount * -1
            SELECT @subagent_amount = @subagent_amount + @transaction_amount

            SELECT @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
                FROM Transaction_Export_Detail
                WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

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
                transaction_account_key)
            VALUES (
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
                @transaction_account_key)

            -- Fetch Next
            FETCH NEXT FROM s_shares INTO
                @party_cnt,
                @party_id,
                @shortname,
                @split_percentage,
                @split_value
        END

        -- Close and Deallocate Cursor
        CLOSE s_shares
        DEALLOCATE s_shares
    END

    -- Debit the Sub Agent
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
    /* Set transaction amount which must be opposite of the contra*/
    SELECT @subagent_amount = @subagent_amount * -1

    /*eck120401 if paid direct only do the commission*/
    IF @transaction_type = "D" BEGIN
        IF @paid_direct = 0
            SELECT @transaction_amount = @subagent_amount - @subagent_commission
        IF @paid_direct = 1
            SELECT @transaction_amount = - @subagent_commission
    END

    /*eck120401 if paid direct only do the commission*/
    IF @transaction_type = "C" BEGIN
        IF @paid_direct = 0
            SELECT @transaction_amount = @subagent_amount + @subagent_commission
        IF @paid_direct = 1
            SELECT @transaction_amount = + @subagent_commission
    END

    SELECT @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
        FROM Transaction_Export_Detail
        WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

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
        transaction_account_key)
    VALUES (
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
        @transaction_account_key)

    RETURN

Err_Add_Trans_Details:
    /* Delete all transactions for this folder */
    DELETE FROM Transaction_Export_Detail
        WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

    /* Delete the transactions folder record */
    DELETE FROM Transaction_Export_Folder
        WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

END
GO

