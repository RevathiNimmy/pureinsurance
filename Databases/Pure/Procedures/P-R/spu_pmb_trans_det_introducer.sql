SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pmb_trans_det_introducer'
GO

CREATE PROCEDURE spu_pmb_trans_det_introducer
    @transaction_export_folder_cnt INT,
    @transaction_type CHAR(1),
    @orig_doc_ref VARCHAR(30),
    @introducer_cnt INT
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
    @insurance_holder_shortname VARCHAR(255),
    @insurance_holder_account_key INT,
    @spare VARCHAR(255),
    @source_id SMALLINT,
    @agent_cnt INT,
    @agent_account_key INT,
    @agent_shortname VARCHAR(255),
    @transaction_account_key INT,
    @premium_amount MONEY,
    @agent_value MONEY,
    @insurance_file_cnt INT,
    @return_status INT,
    @branch_id INT,
    @expense_account_key INT,
    @expense_shortname VARCHAR(255)

DECLARE c_agents CURSOR FAST_FORWARD FOR
    SELECT  
        epa.agent_cnt,
        p.shortname,
        ROUND(epa.agent_commission_value, 2),
        p.party_cnt,
        eif.source_id,
        tef.insurance_file_cnt,
        eif.source_id,
        a.account_key,
        a.short_code
    FROM transaction_export_folder tef
    JOIN event_insurance_file eif
        ON tef.insurance_file_cnt = eif.insurance_file_cnt
    JOIN event_policy_agents epa
        ON epa.insurance_file_cnt = eif.insurance_file_cnt
    JOIN party p
        ON p.party_cnt = epa.agent_cnt
    JOIN party_agent pa
        ON p.party_cnt = pa.party_cnt
    JOIN party_agent_type pat
        ON pat.party_agent_type_id = pa.party_agent_type_id
    JOIN account a
        ON a.account_id = pa.expense_account_id
    WHERE tef.transaction_export_folder_cnt = @transaction_export_folder_cnt
    AND pat.description = 'INTRODUCER'
    AND epa.agent_cnt = @introducer_cnt

/* Open the Agents Cursor */
OPEN c_agents

FETCH NEXT FROM c_agents
INTO    
    @agent_cnt,
    @agent_shortname,
    @agent_value,
    @agent_account_key,
    @source_id,
    @insurance_file_cnt,
    @branch_id,
    @expense_account_key,
    @expense_shortname

WHILE @@FETCH_STATUS = 0
BEGIN

    /* Insert the Trans Export Details */
    /* Credit Agent Ledger  Account */
    IF @agent_value <> 0
    BEGIN
        SELECT @transaction_ledger_code = 'GL'
        SELECT @account_type_code = 'EXPENSE'
        SELECT @ceded_ref = NULL
        SELECT @cover_share_percent = 0
        SELECT @sum_insured_total = 0
        SELECT @charges_total = 0
        SELECT @taxes_total = 0
        SELECT @recoveries_total = 0
        SELECT @commission_excluded = 0
        SELECT @withholding_tax_excluded = 0
        SELECT @mapping_code = 'AGENT NET'
        SELECT @transaction_account_key = NULL
        SELECT @premium_amount =  @agent_value

        IF @premium_amount IS NULL
        BEGIN
            SELECT premium_amount = 0
        END
        
        SELECT @transaction_ledger_code = 'TR'
        SELECT @account_type_code = 'AGENTLEDGR'
        SELECT @ceded_ref = NULL
        SELECT @cover_share_percent = 0
        SELECT @sum_insured_total = 0
        SELECT @charges_total = 0
        SELECT @taxes_total = 0
        SELECT @recoveries_total = 0
        SELECT @commission_excluded = 0
        SELECT @withholding_tax_excluded = 0
        SELECT @mapping_code = @agent_shortname
        SELECT @transaction_account_key = @agent_account_key
        SELECT @spare = 'INT COMM ' + @orig_doc_ref

        /* Set transaction amount */
        SELECT @transaction_amount = @premium_amount
        IF @transaction_amount > 0 AND @transaction_type = 'D'
        BEGIN
            SELECT @transaction_amount = @transaction_amount * -1
        END
        IF @transaction_amount < 0 AND @transaction_type = 'C'
        BEGIN
            SELECT @transaction_amount = @transaction_amount * -1
        END
        
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
            0,
            0,
            NULL,
            'AGENT'
        )

        SELECT @transaction_ledger_code = 'NO'
        SELECT @account_type_code = 'EXPENSE'
        SELECT @transaction_account_key = @expense_account_key
        SELECT @mapping_code = @expense_shortname

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
            suspended,
            release_to_income,
            release_account_code,
            transdetail_type_code
        )
        VALUES 
        (
            @transaction_export_folder_cnt,
            @transaction_export_detail_id,
            @transaction_amount * -1,
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
            0,
            0,
            NULL,
            'AGENT'
        )
    END

    FETCH NEXT FROM c_agents INTO    
        @agent_cnt,
        @agent_shortname,
        @agent_value,
        @agent_account_key,
        @source_id,
        @insurance_file_cnt,
        @branch_id,
        @expense_account_key,
        @expense_shortname
END

CLOSE c_agents
DEALLOCATE c_agents

GO
