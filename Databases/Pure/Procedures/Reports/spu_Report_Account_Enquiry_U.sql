SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO


DDLDropProcedure 'spu_Report_Account_Enquiry_U'
GO


--*********************************************************************************************************************************
-- $Workfile:  spu_PMU_Report_Account_Enquiry.sql  $
-- $Modtime:  2002/07/19  $

-- VER      DATE        WHO     WHAT
-- 1.01     27 Sep 02   JMK     Reduce temp table to just the fields we need
--                              Rename from:spu_PMU_Report_Account_Enquiry to:spu_Report_Account_Enquiry_U
--                              Populate Insured and matched_amount fields
--                              Add Cash and Debit/Credit totals
--                              Calculate Settled taking over allocation into account
--                              Do not allow 'ALL' Accounts
--                              Report does not require date criteria - should always be up to present datetime
--*********************************************************************************************************************************
CREATE PROCEDURE spu_Report_Account_Enquiry_U
                 @short_code varchar(30)

AS


set nocount on

/*
-- Remove this bit
declare    @short_code varchar(30)
declare    @start_date datetime
declare    @end_date   datetime
select @Short_Code = 'foster'
--select @Short_Code = 'all'
select @start_date = '2001-01-01'
select @end_date = '2002-09-26'
 -- end remove
*/


DECLARE @account_id int,
        @bf_balance numeric(19,4),
        @cf_balance numeric(19,4),
        @transdetail_id int,
        @match_total    numeric(19,4),
        @cf_Cashbalance numeric(19,4),
        @cf_DCbalance numeric(19,4),
        @CashSettled_total numeric(19,4),
        @DCSettled_total numeric(19,4),
        @IsCash int,
        @GrossAmount numeric(19,4),
        @Settled numeric(19,4),
        @OA_total numeric(19,4)

-- Get account_id
IF UPPER(@short_code) = 'ALL'
BEGIN
    SELECT  @account_id = 0
END
ELSE
BEGIN
    SELECT  @account_id = account_id
    FROM    Account
    WHERE   short_code = @short_code
END

-- Use a temp table instead of the Report_Transaction table
CREATE TABLE #Report_Transaction
(
    account_code            char (30) NULL,
    account_name            varchar (60) NULL,
    document_date           datetime NULL,
    document_ref            varchar (25) NULL,
    IsCash                  int,
    transdetail_id          int NULL,
    InsuranceRef            varchar (30) NULL,
    amount                  numeric(19, 4) NULL,
    media_ref               varchar (100) NULL,
    Insured                 varchar (100) NULL,
    balanceBF               numeric(19, 4) NULL,
    balanceBFCash           numeric(19, 4) NULL,    -- Cash
    balanceBFDebitCredit    numeric(19, 4) NULL,    -- Debit/Credit
    balanceCF               numeric(19, 4) NULL,
    balanceCFCash           numeric(19, 4) NULL,    -- Cash
    balanceCFDebitCredit    numeric(19, 4) NULL,    -- Debit/Credit
    matched_amount          numeric(19, 4) NULL,
    CashSettled_amount      numeric(19, 4) NULL,    -- Cash
    DCSettled_amount        numeric(19, 4) NULL,    -- Debit/Credit
    Settled                 numeric(19, 4) NULL,    -- matched_amount - OATotal
    OATotal                 numeric(19, 4) NULL     -- OverAllocated Amount
)

-- Create Individual Records
INSERT INTO #Report_Transaction
    SELECT A.short_code,            --account_code
        A.account_name,             --account_name
        D.document_date,            --document_date
        D.document_ref,             --document_ref
        CASE D.documenttype_id
            WHEN 22 THEN 1
            WHEN 23 THEN 1
            ELSE 0
        END,                        --IsCash
        T.transdetail_id,           --transdetail_id
        T.insurance_ref,            --InsuranceRef
        T.amount,                   --amount
        I.media_ref,                --media_ref
        '',
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0
    FROM Account A
    JOIN Transdetail T                ON A.account_id = T.account_id
    JOIN Document D                   ON  D.document_id = T.document_id
    LEFT OUTER JOIN CashlistItem I    ON T.transdetail_id = I.transdetail_id
    WHERE --( ( ISNULL(@account_id, 0) = 0 )
          --   OR ( (ISNULL(@account_id, 0) <> 0 )
          --   AND A.account_id = @account_id )
          --  )
         A.account_id = @account_id
    --AND ( D.document_date >= @start_date
    --     AND D.document_date <= @end_date )

-- Add Match totals
DECLARE c_cursor CURSOR FAST_FORWARD FOR
    SELECT DISTINCT transdetail_id, IsCash
    FROM #Report_Transaction

    OPEN c_cursor

    FETCH NEXT FROM c_cursor INTO @transdetail_id, @IsCash

    WHILE @@FETCH_STATUS = 0
    BEGIN

        -- Get Match Total
        SELECT @match_total = SUM(M.base_match_amount)
        FROM TransMatch  M
        --JOIN MatchGroup  G ON G.match_id = M.match_id
        WHERE M.transdetail_id = @transdetail_id
        --AND G.match_date <= @end_date
        AND M.allocationdetail_id IS NOT NULL

        -- Get gross value (for finding out whether it is over allocation)
        SELECT @GrossAmount = amount
        FROM transdetail
        WHERE transdetail_id = @transdetail_id
        AND spare = 'gross'

        -- Get Outstanding Total
        SELECT @OA_total = SUM(ad.os_base_amount)
        FROM TransMatch  M
        --JOIN MatchGroup  G ON G.match_id = M.match_id
        JOIN Allocationdetail ad ON ad.transdetail_id = m.transdetail_id
        WHERE M.transdetail_id = @transdetail_id
        --AND G.match_date <= @end_date
        AND M.allocationdetail_id is not null


        IF  @OA_total <> 0
        BEGIN
            IF (@OA_total > 0 AND @GrossAmount > 0) OR (@OA_total < 0 AND @GrossAmount < 0)
            BEGIN
                --print 'No need to include OS amount: ' + convert(varchar(50),@OA_total)
                SELECT @OA_total = 0
            END
        /*
            ELSE
            BEGIN
                --print 'OverAlloc - include OS: ' + convert(varchar(50),@OA_total)
            END
        */
        END


        SELECT @Settled = isnull(@match_total,0) - isnull(@OA_total,0)

        -- Get Settled Cash and Debit/Credit amounts
        IF @IsCash = 1
        BEGIN
            -- Settled CASH
            SELECT @CashSettled_total = @Settled
            SELECT  @DCSettled_total = 0
        END
        ELSE
        BEGIN
            -- Settled Debit and Credit
            SELECT  @DCSettled_total = @Settled
            SELECT @CashSettled_total = 0
        END

        -- Update Table
        UPDATE #Report_Transaction
            SET matched_amount = @match_total,
                Settled = @settled,
                OATotal = @OA_Total,
                CashSettled_amount = @CashSettled_total,
                DCSettled_amount = @DCSettled_total
            WHERE transdetail_id = @transdetail_id
        --select @match_total '@match_total,', @settled '@settled,', @OA_Total '@OA_Total,', @CashSettled_total '@CashSettled_total,', @DCSettled_total '@DCSettled_total'

        FETCH NEXT FROM c_cursor INTO @transdetail_id, @IsCash

    END

     CLOSE c_cursor
DEALLOCATE c_cursor

-- Add B/F C/F balances
-- NB - cannot get CF Settled and Outstanding amounts in this way because there is currently no direct way of dealing with Over Allocation
--      so it is calculated on the report based on values within the selected dates.

/* redundant now we don't use date params
-- Get B/F Balance
   SELECT @bf_balance = SUM(amount)
   FROM TransDetail T
   JOIN Document D     ON D.document_id = T.document_id
   WHERE ( ISNULL(@account_id,0) = 0
            OR ( ISNULL(@account_id,0) <> 0
            AND T.account_id = @account_id )
          )
   AND D.document_date < @start_date
*/
-- Get C/F Balance
    SELECT @cf_balance = SUM(amount)
    FROM TransDetail T
    JOIN Document D      ON D.document_id = T.document_id
    WHERE ( ( ISNULL(@account_id, 0) = 0 )
          OR (  (ISNULL(@account_id, 0) <> 0 )
          AND T.account_id = @account_id )
          )
    --AND D.document_date <= @end_date

-- Get C/F Balance - CASH
    SELECT @cf_Cashbalance = SUM(amount)
    FROM TransDetail T
    JOIN Document D      ON D.document_id = T.document_id
    WHERE ( ( ISNULL(@account_id, 0) = 0 )
          OR (  (ISNULL(@account_id, 0) <> 0 )
          AND T.account_id = @account_id )
          )
    --AND D.document_date <= @end_date
    AND D.documenttype_id IN (22,23)   -- SRP, SPY

-- Get C/F Balance - Debit/Credit
    SELECT @cf_DCbalance = SUM(amount)
    FROM TransDetail T
    JOIN Document D      ON D.document_id = T.document_id
    WHERE ( ( ISNULL(@account_id, 0) = 0 )
          OR (  (ISNULL(@account_id, 0) <> 0 )
          AND T.account_id = @account_id )
          )
    --AND D.document_date <= @end_date
    AND D.documenttype_id NOT IN (22,23)

-- Update Table
UPDATE #Report_Transaction
    SET --balanceBF = @bf_balance,
        balanceCF = @cf_balance,
        balanceCFCash = @cf_Cashbalance,
        balanceCFDebitCredit = @cf_DCbalance

-- Add Insured name
UPDATE #Report_Transaction
    SET Insured =  pClient.resolved_name
    FROM party pClient
    WHERE pClient.party_cnt =
        (SELECT max(insured_cnt)
        FROM Insurance_File WHERE insurance_ref = InsuranceRef
        AND isnull(InsuranceRef, '') <> ''
        )

-- Return Results
SELECT  account_code,
        account_name,
        document_date,
        document_ref,
        InsuranceRef,
        Insured,
        media_ref,
        amount,
        balanceBF,
        balanceBFCash,
        balanceBFDebitCredit,
        balanceCF,
        balanceCFCash,
        balanceCFDebitCredit,
        matched_amount,
        CashSettled_amount,
        DCSettled_amount,
        Settled
FROM #Report_Transaction

-- remove the temporary table
DROP TABLE #Report_Transaction


GO


