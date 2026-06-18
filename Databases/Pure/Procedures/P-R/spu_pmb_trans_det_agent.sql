SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmb_trans_det_agent'
GO


CREATE PROCEDURE spu_pmb_trans_det_agent
    @transaction_export_folder_cnt INT,
    @transaction_type CHAR(1),
    @agent_amount MONEY OUTPUT
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
    @policy_agents_id INT,
    @domiciled_for_tax TINYINT,
    @premium_tax_account_mapping_code VARCHAR(20),
    @premium_tax_account_key INT, 
    @premium_tax_type_code VARCHAR(20),
    @premium_tax_amount MONEY    

SELECT @agent_amount = 0

DECLARE c_agents CURSOR FAST_FORWARD FOR
    SELECT
        epa.agent_cnt,
        p.shortname,
        ROUND(epa.agent_commission_value, 2) + ROUND(epa.tax_amount, 2),
        p.party_cnt,
        p.source_id,
        epa.insurance_file_cnt,
        ei.source_id,
        epa.policy_agents_id,
        p.domiciled_for_tax 
    FROM transaction_export_folder tef
    JOIN event_policy_agents epa
        ON epa.insurance_file_cnt = tef.insurance_file_cnt
    JOIN party p
        ON p.party_cnt = epa.agent_cnt
    JOIN party_agent pa
        ON pa.party_cnt = epa.agent_cnt
    JOIN party_agent_type pat
        ON pat.party_agent_type_id = pa.party_agent_type_id
        AND pat.description = 'AGENT'
    JOIN event_insurance_file ei
        ON ei.insurance_file_cnt = tef.insurance_file_cnt
    WHERE tef.transaction_export_folder_cnt = @transaction_export_folder_cnt
    

/* Open the Agents Cursor */
OPEN c_agents

FETCH NEXT FROM c_agents INTO
    @agent_cnt,
    @agent_shortname,
    @agent_value,
    @agent_account_key,
    @source_id,
    @insurance_file_cnt,
    @branch_id,
    @policy_agents_id,
    @domiciled_for_tax

WHILE @@FETCH_STATUS = 0
BEGIN
    /* Insert the Trans Export Details */
    /* Credit Agent Ledger  Account */
    IF @agent_value <> 0
    BEGIN
        SELECT  @transaction_ledger_code = 'GL'
        SELECT  @account_type_code = 'EXPENSE'
        SELECT  @ceded_ref = NULL
        SELECT  @cover_share_percent = 0
        SELECT  @sum_insured_total = 0
        SELECT  @charges_total = 0
        SELECT  @taxes_total = 0
        SELECT  @recoveries_total = 0
        SELECT  @commission_excluded = 0
        SELECT  @withholding_tax_excluded = 0
        SELECT  @mapping_code = 'AGENT NET'
        SELECT  @transaction_account_key = NULL
        SELECT  @premium_amount =  @agent_value

        IF @premium_amount IS NULL
        BEGIN
            SELECT premium_amount = 0
        END
        
        SELECT @transaction_ledger_code = 'AG'
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
        SELECT @spare = 'AGENT'

        /*Datasure calculate tax on agent amount */
        SELECT @taxes_total =
        (SELECT ISNULL(SUM(ETC.value),0)
        FROM  Event_Tax_Calculation ETC,
        Transaction_Export_Folder   T 
        WHERE ETC.Insurance_file_cnt = T.insurance_file_cnt
        AND T.transaction_export_folder_cnt = @transaction_export_folder_cnt
        AND ETC.TransType = 'TTAC'
        AND ETC.policy_agents_id = @policy_agents_id)
        /*Set transaction amount */
        SELECT @transaction_amount = @premium_amount

        IF @transaction_amount > 0 AND @transaction_type = 'D'
        BEGIN
            SELECT @transaction_amount = @transaction_amount * -1
            SELECT @taxes_total = @taxes_total * -1
        END
        
        IF @transaction_amount < 0 AND @transaction_type = 'C'
        BEGIN
            SELECT @transaction_amount = @transaction_amount * -1
            SELECT @taxes_total = @taxes_total * -1
        END
        
        /* If Agent is not tax registered  deduct taxes from output which will be passed to the commission procedure */
        IF @domiciled_for_tax = 1
        BEGIN
            SELECT @agent_amount = @agent_amount + @transaction_amount
        END
        ELSE
        BEGIN
            SELECT @agent_amount = @agent_amount + @transaction_amount - @taxes_total
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
    END
    
    FETCH NEXT FROM c_agents INTO    
        @agent_cnt,
        @agent_shortname,
        @agent_value,
        @agent_account_key,
        @source_id,
        @insurance_file_cnt,
        @branch_id,
        @policy_agents_id,
        @domiciled_for_tax
END

/* Close and Deallocate Cursor */
CLOSE c_agents
DEALLOCATE c_agents

GO


