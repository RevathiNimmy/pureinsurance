SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Unearned_Premium'
GO


CREATE PROCEDURE spu_Report_Unearned_Premium
    @source_id int
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 11/06/2001
** RSA Reports -  Unearned_Premium.rpt
**
** Reports calculates unearned values
**********************************************************************************************************************************
**
** 16/06/2001   Jude Killip     12months correction if we're in the very first period
**
** 19/06/2001   Jude Killip     Just get every transaction with future expiry date
**                              (i.e. expiry date  > current period end date)
**
** 02/07/2001   Jude Killip     add calculation rounding checks
**                              document_ref
**                              set 'NET','TTY','TAF','TAC','TAN','TAT' record values * -1
**
** 04/07/2001   Jude Killip     filters for Claims/nonClaims details
**
** 13/09/2001   Jude Killip     set 'FAC' record values * -1
**                              comment out NET records - not needed??
**
** 13/09/2001   Jude Killip     increase decimal places for daily rate to lessen rounding problems
**
** 18/09/2001   Jude Killip     Add day to days of cover for midnight renewal products
**                              Get rid of commented out 'NET' stuff
**                              remove rounding checks
**
** 28/09/2001   Jude Killip     Filter out failed Export records
***********************************************************************************************************************************/
SET NOCOUNT ON

IF ISNULL(@source_id, 0) = 0
    SELECT @source_id = 1

-- get sub_branch_id
DECLARE @sub_branch_id int
EXECUTE spu_sub_branch_default @source_id, @sub_branch_id OUTPUT

-- get current period values
DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod @sub_branch_id, @CurrentPeriodID OUTPUT, @dtCurrentPeriodEnd OUTPUT

-- get current 12 month period values
DECLARE @12MonthPeriodID int, @dt12MonthPeriodEnd datetime
EXECUTE spu_Report_GetCurrent12MonthPeriod @sub_branch_id, @12MonthPeriodID OUTPUT, @dt12MonthPeriodEnd OUTPUT

-- bad form, not used, commented
/*
-- in case we're in the first period ever...
DECLARE @12MonthPeriodIDPlusOne int
IF @12MonthPeriodID = @CurrentPeriodID
    BEGIN
        SELECT @12MonthPeriodIDPlusOne = @12MonthPeriodID
    END
ELSE
    BEGIN
        SELECT @12MonthPeriodIDPlusOne = @12MonthPeriodID + 1
    END
*/

CREATE TABLE #tempRSAUnEarndPrem
(
    StatsFolderCnt int,
    ProductCode varchar (10) NULL,
    ProductDesc varchar (255) NULL,
    CommissionOrPremium int NULL, -- 0=premium, 1=commission
    RiskTypeCode varchar (10) NULL,
    RiskTypeDescription varchar (255) NULL,
    Gross decimal (19, 8) NULL, -- GRS Daily Rate = premium/days of cover
    GrossTotal decimal (19, 4) NULL,
    Coinsurance decimal (19, 8) NULL, -- COI "
    CoinsTotal decimal (19, 4) NULL,
    Treaty decimal (19, 8) NULL, -- TTY "
    TreatyTotal decimal (19, 4) NULL,
    Facultative decimal (19, 8) NULL, -- FAC "
    FacTotal decimal (19, 4) NULL,
    DocumentRef varchar (25) NULL,
    FromDate datetime NULL,
    ToDate datetime NULL,
    dtCurrentPeriodEnd datetime,
    PostingPeriodID int,
    CurrentPeriodID int,
    DaysOfCoverTotal int,
    IsMidnightRenewal int
)

-- MOVING 12 MONTHS
-- Add Premium Records
INSERT INTO #tempRSAUnEarndPrem
    SELECT sd.stats_folder_cnt,
        sf.product_code,
        p.description,
        0, -- Premium
        sd.risk_type_code,
        rt.description,
        NULL,
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS'),
        NULL,
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI'),
        NULL,
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'TTY')*-1,
        NULL,
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'FAC')*-1,
        sf.document_ref,
        sf.cover_start_date,
        sf.expiry_date,
        @dtCurrentPeriodEnd,
        sf.posting_period_number,
        @CurrentPeriodID,
        datediff(day, sf.cover_start_date, sf.expiry_date),
        isnull(p.is_midnight_renewal, 0)
    FROM Stats_Folder sf
    JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND isnull(sd.this_premium_home, 0) <> 0
        --AND sd.stats_detail_type in ('GRS', 'COI', 'TTY', 'FAC', 'NET')
        AND sd.stats_detail_type in ('GRS', 'COI', 'TTY', 'FAC')
    JOIN Product p ON sf.product_id = p.product_id
    JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
    WHERE datediff(day, sf.cover_start_date, sf.expiry_date)<> 0 -- in case of div/zero
    AND sf.expiry_date > @dtCurrentPeriodEnd
    AND sf.transaction_type_code NOT LIKE ('C_%') -- all but claims
    --sj 31/07/2002 - Start
    --AND(
    --    SELECT isnull(max(tef.accounts_export_status), 'x')
    --    FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
    --) = 'c'
    --sj 31/07/2002 - End

-- Now add Commission Records
INSERT INTO #tempRSAUnEarndPrem
    SELECT sd.stats_folder_cnt,
        sf.product_code,
        p.description,
        1, -- Commission
        sd.risk_type_code,
        rt.description,
        NULL,
        (SELECT (isnull(sd.lead_commission_value_home, 0) +
                    isnull(sd.sub_commission_value_home, 0)) WHERE sd.stats_detail_type = 'GRS'),
        NULL,
        (SELECT (isnull(sd.lead_commission_value_home, 0) +
                    isnull(sd.sub_commission_value_home, 0)) WHERE sd.stats_detail_type = 'COI'),
        NULL,
        (SELECT (isnull(sd.lead_commission_value_home, 0) +
                    isnull(sd.sub_commission_value_home, 0)) WHERE sd.stats_detail_type = 'TTY')*-1,
        NULL,
        (SELECT (isnull(sd.lead_commission_value_home, 0) +
                    isnull(sd.sub_commission_value_home, 0)) WHERE sd.stats_detail_type = 'FAC')*-1,
        sf.document_ref,
        sf.cover_start_date,
        sf.expiry_date,
        @dtCurrentPeriodEnd,
        sf.posting_period_number,
        @CurrentPeriodID,
        datediff(day, sf.cover_start_date, sf.expiry_date),
        isnull(p.is_midnight_renewal, 0)
    FROM Stats_Folder sf
    JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND isnull(sd.lead_commission_value_home, 0) +
            isnull(sd.sub_commission_value_home, 0) <> 0
        AND sd.stats_detail_type in ('GRS', 'COI', 'TTY', 'FAC')
    JOIN Product p ON sf.product_id = p.product_id
    JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
    WHERE datediff(day, sf.cover_start_date, sf.expiry_date) <> 0 -- in case of div/zero
    AND sf.expiry_date > @dtCurrentPeriodEnd
    AND sf.transaction_type_code NOT LIKE ('C_%') -- all but claims
     --sj 31/07/2002 - Start
    --AND(
    --    SELECT isnull(max(tef.accounts_export_status), 'x')
    --    FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
    --) = 'c'
    --sj 31/07/2002 - End


-- update with daily rates
UPDATE #tempRSAUnEarndPrem
    SET Gross = GrossTotal/(DaysOfCoverTotal+IsMidnightRenewal),
        Coinsurance = CoinsTotal/(DaysOfCoverTotal+IsMidnightRenewal),
        Treaty = TreatyTotal/(DaysOfCoverTotal+IsMidnightRenewal),
        Facultative = FacTotal/(DaysOfCoverTotal+IsMidnightRenewal)

SET NOCOUNT OFF
SELECT * FROM #tempRSAUnEarndPrem
DROP TABLE #tempRSAUnEarndPrem
GO


