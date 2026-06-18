SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Check_Posting_Criteria'
GO


CREATE PROCEDURE spu_Check_Posting_Criteria
    @Insurance_File_Cnt INT,
    @Effective_Date DATETIME
AS

DECLARE @Check VARCHAR(10),
    @InsurerCode VARCHAR(20),
    @ClientCode VARCHAR(20),
    @ActiveStatus INT,
    @InsurerLedgerId INT,
    @InsurerAccountType INT,
    @ClientLedgerId INT,
    @ClientAccountType INT,
    @ActualInsurerLedgerId INT,
    @ActualInsurerAccountType INT,
    @ActualClientLedgerId INT,
    @ActualClientAccountType INT,
    @InsurerAccountStatus INT,
    @ClientAccountStatus INT,
    @InsurerAccountId INT,
    @ClientAccountId INT,
    @InsMapInsurerLedger INT,
    @CliMapClientLedger INT,
    @InsMapCurrLiabilities INT,
    @CliMapAssets INT,
    @PolicyRisk INT,
    @CommissionAcc INT,
    @Period INT,
    @CliLedgerMapping INT,
    @CliParentNodeId INT,
    @CliGParentNodeId INT,
    @CliAssetsMapping INT,
    @InsLedgerMapping INT,
    @InsParentNodeId INT,
    @InsGParentNodeId INT,
    @InsLiabilityMapping INT,
    @useBranchId INT

SELECT @Check = ''

IF ( SELECT value FROM hidden_options WHERE branch_id =1 AND option_number = 1 ) = "A"
BEGIN

    IF ( SELECT value from hidden_options WHERE branch_id = 1 AND option_number = 16 ) = "1"
    BEGIN

        SELECT @UseBranchId = ( SELECT source_id FROM event_insurance_file WHERE insurance_file_cnt = @Insurance_File_Cnt )

    END
    ELSE
    BEGIN

        SELECT @UseBranchId = 1

    END

    --CHECK A - SYSTEM OPTION FOR Authorise Accounts Transactions is unchecked.

    IF  (   
        SELECT value 
        FROM system_options 
        WHERE option_number = 81 
        AND branch_id = @UseBranchId
        ) = 1
    BEGIN
        SELECT @Check = @Check + 'A'
    END

    
    
    IF NOT EXISTS
        (
            SELECT NULL
            FROM hidden_options
            WHERE option_number = 40
            AND value = '1'
        )
    BEGIN
        --CHECK B - Commission Account mapped to risk_group
    
        SELECT  @CommissionAcc = rbs.commission_cnt
        FROM    event_insurance_file ifi
        JOIN    risk_code rc
        ON  ifi.risk_code_id = rc.risk_code_id
        JOIN    risk_by_source rbs
        ON  rc.risk_group_id = rbs.risk_group_id
        WHERE   ifi.insurance_file_cnt = @Insurance_File_Cnt
        AND rbs.source_id = ( SELECT source_id FROM event_insurance_file WHERE insurance_file_cnt = @Insurance_File_Cnt )

        IF @CommissionAcc = 0 OR @CommissionAcc IS NULL
        BEGIN
            --no commission account against actual branch, no check for all branches
            SELECT  @CommissionAcc = rbs.commission_cnt
            FROM    event_insurance_file ifi
            JOIN    risk_code rc
            ON  ifi.risk_code_id = rc.risk_code_id
            JOIN    risk_by_source rbs
            ON  rc.risk_group_id = rbs.risk_group_id
            WHERE   ifi.insurance_file_cnt = @Insurance_File_Cnt
            AND rbs.source_id = 0

            --no commission account against all branches, so need to report
            IF @CommissionAcc = 0 OR @CommissionAcc IS NULL 
            BEGIN
                SELECT @Check = @Check + 'B'
            END
        END
    END
    ELSE
    BEGIN
        --CHECK F - Commission Account mapped to account executive

        IF NOT EXISTS
            (
                SELECT NULL
                FROM event_insurance_file i
                JOIN party p
                    ON p.party_cnt = i.account_executive_cnt
                JOIN party_handler ph
                    ON ph.party_cnt = p.party_cnt 
                WHERE i.insurance_file_cnt = @Insurance_File_Cnt
                AND ph.commission_cnt IS NOT NULL
            )
        BEGIN
            SELECT @Check = @Check + 'F'
        END

    END

    --CHECK C - Account Period set up for effective date of transaction

    SELECT  @Period = MIN(p.period_id)
    FROM    period p
    WHERE   p.period_end_date > @effective_date
    AND p.company_id = @UseBranchId

    --no period for effective date of transaction
    IF  @Period = 0 OR @Period IS NULL
    BEGIN
        SELECT @Check = @Check + 'C'
    END

    --check client and insurer are set up correctly

    SELECT  @ActiveStatus = accountstatus_id 
    FROM    accountstatus
    WHERE   code = 'ACTIVE'

    --CHECK D
    --now check if the insurer used has been set up correctly
    --i.e. is set as insurer ledger, is active, is account type Liablity and 
    --is set in Account Explorer correctly as Current Liabilities/Insurer Ledger

    IF (@UseBranchId = 1 OR 
        ( @UseBranchid <> 1 AND 
        NOT EXISTS ( SELECT mapping_id FROM mapping WHERE [description] = 'Current Liabilities' AND company_id = @UseBranchId )))
    BEGIN

        SELECT  @InsMapCurrLiabilities = mapping_id 
        FROM    mapping 
        WHERE   [description] = 'Current Liabilities'
        AND     company_id = 1

    END
    ELSE
    BEGIN

        SELECT  @InsMapCurrLiabilities = mapping_id 
        FROM    mapping 
        WHERE   [description] = 'Current Liabilities'
        AND     company_id = @UseBranchId

    END

    IF (@UseBranchId = 1 OR 
        ( @UseBranchid <> 1 AND 
        NOT EXISTS ( SELECT mapping_id FROM mapping WHERE [description] = 'Insurer Ledger' AND company_id = @UseBranchId )))
    BEGIN

        SELECT  @InsMapInsurerLedger = mapping_id 
        FROM    mapping 
        WHERE   [description] = 'Insurer Ledger'
        AND     company_id = 1

    END
    ELSE
    BEGIN

        SELECT  @InsMapInsurerLedger = mapping_id 
        FROM    mapping 
        WHERE   [description] = 'Insurer Ledger'
        AND     company_id = @UseBranchId

    END

    SELECT  @InsurerAccountType = accounttype_id
    FROM    accounttype 
    WHERE   [description] = 'Liability'

    SELECT  @InsurerCode = pins.shortname 
    FROM    event_insurance_file ifi 
    JOIN    party pins 
    ON  ifi.lead_insurer_cnt = pins.party_cnt
    WHERE   ifi.insurance_file_cnt = @insurance_file_cnt

    /*DC171104 PN16838 */
    SELECT  @InsurerLedgerId = ledger_id 
    FROM    ledger
    WHERE   ledger_short_name = 'IN'
    AND company_id = @UseBranchId

    IF (@UseBranchId = 1 OR 
        ( @UseBranchid <> 1 AND 
        NOT EXISTS ( SELECT account_id FROM account WHERE short_code = @InsurerCode AND company_id = @UseBranchId )))
    BEGIN

        SELECT  @InsurerAccountId = account_id,
                @ActualInsurerLedgerId = ledger_id,
                @InsurerAccountStatus = accountstatus_id,
                @ActualInsurerAccountType = accounttype_id
        FROM    account 
        WHERE   short_code = @InsurerCode
        AND     company_id = 1

    END
    ELSE
    BEGIN

        SELECT  @InsurerAccountId = account_id,
                @ActualInsurerLedgerId = ledger_id,
                @InsurerAccountStatus = accountstatus_id,
                @ActualInsurerAccountType = accounttype_id
        FROM    account 
        WHERE   short_code = @InsurerCode
        AND     company_id = @UseBranchId

    END

    SELECT @InsParentNodeId = parent_node_id FROM structuretree WHERE account_id = @InsurerAccountId
    SELECT @InsLedgerMapping = mapping_id FROM structuretree WHERE node_id = @InsParentNodeId
    SELECT @InsGParentNodeId = parent_node_id FROM structuretree WHERE node_id = @InsParentNodeId
    SELECT @InsLiabilityMapping = mapping_id FROM structuretree WHERE node_id = @InsGParentNodeId

    IF  ((@InsurerLedgerId <> @ActualInsurerLedgerId OR @InsurerAccountStatus <> @ActiveStatus) OR
        (@InsurerAccountType <> @ActualInsurerAccountType OR @InsMapInsurerLedger <> @InsLedgerMapping)) OR
        (@InsMapCurrLiabilities <> @InsLiabilityMapping)
    BEGIN
        SELECT @Check = @Check + 'D'
    END 

    --CHECK E
    --now check if the client used has been set up correctly
    --i.e. is set as client ledger, is active and is an Asset and 
    --is set in Account Explorer correctly as Current Assets/Client Ledger
    IF (@UseBranchId = 1 OR 
        ( @UseBranchid <> 1 AND 
        NOT EXISTS ( SELECT mapping_id FROM mapping WHERE [description] = 'Current Assets' AND company_id = @UseBranchId )))
    BEGIN

        SELECT  @CliMapAssets = mapping_id 
        FROM    mapping 
        WHERE   [description] = 'Current Assets'
        AND     company_id = 1

    END
    ELSE
    BEGIN

        SELECT  @CliMapAssets = mapping_id 
        FROM    mapping 
        WHERE   [description] = 'Current Assets'
        AND     company_id = @UseBranchId

    END

    IF (@UseBranchId = 1 OR 
        ( @UseBranchid <> 1 AND 
        NOT EXISTS ( SELECT mapping_id FROM mapping WHERE [description] = 'Client Ledger' AND company_id = @UseBranchId )))
    BEGIN

        SELECT  @CliMapClientledger = mapping_id 
        FROM    mapping 
        WHERE   [description] = 'Client Ledger'
        AND     company_id = 1

    END
    ELSE
    BEGIN

        SELECT  @CliMapClientledger = mapping_id 
        FROM    mapping 
        WHERE   [description] = 'Client Ledger'
        AND     company_id = @UseBranchId

    END

    SELECT  @ClientAccountType = accounttype_id
    FROM    accounttype 
    WHERE   [description] = 'Asset'

    SELECT  @ClientCode = pcli.shortname 
    FROM    event_insurance_file ifi 
    JOIN    party pcli 
    ON  ifi.insured_cnt = pcli.party_cnt
    WHERE   ifi.insurance_file_cnt = @insurance_file_cnt

    /*DC171104 PN16838 */
    SELECT  @ClientLedgerId =  ledger_id 
    FROM    ledger
    WHERE   ledger_short_name = 'SA'
    AND company_id = @UseBranchId

    IF (@UseBranchId = 1 OR 
        ( @UseBranchid <> 1 AND 
        NOT EXISTS ( SELECT account_id FROM account WHERE short_code = @ClientCode AND company_id = @UseBranchId )))
    BEGIN

        SELECT  @ClientAccountId = account_id,
            @ActualClientLedgerId = ledger_id,
            @ClientAccountStatus = accountstatus_id,
            @ActualClientAccountType = accounttype_id
        FROM    account 
        WHERE   short_code = @ClientCode
        AND     company_id = 1

    END
    ELSE
    BEGIN

        SELECT  @ClientAccountId = account_id,
            @ActualClientLedgerId = ledger_id,
            @ClientAccountStatus = accountstatus_id,
            @ActualClientAccountType = accounttype_id
        FROM    account 
        WHERE   short_code = @ClientCode
        AND     company_id = @UseBranchId

    END

    SELECT @CliParentNodeId = parent_node_id FROM structuretree WHERE account_id = @ClientAccountId
    SELECT @CliLedgerMapping = mapping_id FROM structuretree WHERE node_id = @CliParentNodeId
    SELECT @CliGParentNodeId = parent_node_id FROM structuretree WHERE node_id = @CliParentNodeId
    SELECT @CliAssetsMapping = mapping_id FROM structuretree WHERE node_id = @CliGParentNodeId

    IF  ((@ClientLedgerId <> @ActualClientledgerId OR @ClientAccountStatus <> @ActiveStatus) OR
        (@ClientAccountType <> @ActualClientAccountType OR @CliMapClientLedger <> @CliLedgerMapping)) OR
        (@CliMapAssets <> @CliAssetsMapping)
    BEGIN
        SELECT @Check = @Check + 'E'
    END 

    --CHECK G
    --now check if any fees attached to policy are not mapped to an income account

    IF EXISTS   ( 
        SELECT NULL
            FROM   event_insurance_file eif
            JOIN   event_policy_fee epf
            ON     eif.insurance_file_cnt = epf.insurance_file_cnt
            JOIN   party p
            ON     epf.party_cnt = p.party_cnt
            JOIN   party_type pt
            ON     p.party_type_id = pt.party_type_id
            JOIN   account a
            ON     p.party_cnt = a.account_key
            JOIN   structuretree st
            ON     a.account_id = st.account_id
            JOIN   elementextras ee
            ON     st.element_id = ee.element_id    
            WHERE  eif.insurance_file_cnt = @Insurance_File_Cnt
            AND    pt.code = 'FE'
            AND    ( ee.account_map_id IS NULL OR ee.account_map_id = 0 )
            )
    BEGIN

         SELECT @Check = @Check + 'G'

    END
     
    --Check H
    --Check that the net premium for the insurer(s) is not less than the commission for that insurer.
    IF EXISTS
        (
            SELECT
                NULL
            FROM event_policy_coinsurers
            WHERE insurance_file_cnt = @Insurance_File_Cnt
        )
    BEGIN
        /*Coinsurer policy, check each insurers premium against it's commission*/
        IF EXISTS
            (
                SELECT
                    NULL
                FROM event_policy_coinsurers
                WHERE insurance_file_cnt = @Insurance_File_Cnt
                AND coinsurer_value < coinsurer_commission_amount
            )
        BEGIN        
        
            SELECT @Check = @Check + 'H'
            
        END        
    END
    ELSE
    BEGIN
        /*Not coinsurer policy, just check main premium against commission*/
        IF EXISTS
            (
                SELECT
                    NULL
                FROM event_insurance_file
                WHERE insurance_file_cnt = @Insurance_File_Cnt
                AND this_premium < commission_amount
            )
        BEGIN        
        
            SELECT @Check = @Check + 'H'
            
        END
    END
    
     
    --Check I
    --Check that the net premium for the extra(s) is not less than the commission for that extra.
    IF EXISTS
        (
            SELECT
                NULL
            FROM event_policy_fee
            WHERE insurance_file_cnt = @Insurance_File_Cnt
            AND fee_amount < commission_amount
            AND commission_amount <> 0
        )
    BEGIN        

        SELECT @Check = @Check + 'I'

    END

END

SELECT @Check

GO