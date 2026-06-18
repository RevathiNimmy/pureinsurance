SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_AUA_Premium_Bordereau'
GO


CREATE PROCEDURE spu_Report_AUA_Premium_Bordereau
    @Reinsurer varchar(255),
    @PeriodDate varchar(255),
    @source_id int
AS
/**********************************************************************************************************************************
** Created by Kerry Butler
** 03/09/2001
** AUA Bespoke Reports - AUA_Premium_Bordereau.rpt
**
**    v1.1 KB 2/1/2 Added variables for IPT and VAT. Added selection criteria for IPT and VAT
**     1.2 KB 10/01/02  Dont include tax records when calculating the premium amount.
**          Update tax records with treaty so they get anaysed properly.
**     1.3 PW110402 - use period instead of start/end date
**     1.4 SJP 13/06/02 - Underwriting Type (UWType) is selected by option number and branch (ensuring unique record retrieved)
**********************************************************************************************************************************
**
***********************************************************************************************************************************/
SET NOCOUNT ON

DECLARE @sub_branch_id int
EXEC spu_sub_branch_default @source_id, @sub_branch_id OUTPUT

-- PW110402 - find which period we want to base this report on
DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime

SELECT @PeriodDate = @PeriodDate + " 23:59:59.000"
SELECT @dtSelectedPeriodEnd = CONVERT (Datetime, @PeriodDate)

SELECT @SelectedPeriodID = period_id
FROM Period
WHERE period_end_date = @dtSelectedPeriodEnd
AND sub_branch_id = @sub_branch_id


CREATE TABLE #tempAUAPremiumBordereau
(
        PolNum varchar (30) NULL,           -- 2
        Client varchar (60) NULL,           -- 4
        TransTypeID int NULL,               -- 5
        TransTypeCode varchar (10) NULL,    -- 6
        ProductCode varchar (10) NULL,      -- 7
        Product varchar (255) NULL,         -- 9
        RiskTypeID int NULL,                -- 11
        ReinsurerCnt int NULL,              -- 12
        ReinsurerShort varchar (20) NULL,   -- 13
        Reinsurer varchar (100) NULL,       -- 14
        AmountPremium decimal (19,4) NULL,  -- 15
--        AmountCommission decimal (19,4) NULL,  -- 16
--        PercCommission decimal (19,4) NULL,    -- 17
    IPT decimal (19,4)  NULL,
    VAT decimal (19,4)  NULL,
        RiskType varchar (255) NULL,           -- 18
        FromDate    datetime NULL,
        ToDate datetime NULL,
        dtCurrentPeriodEnd datetime,
    Stats_detail_type varchar(3) NULL ,       -- 19
    UWType char(1) NULL)

-- get current period values
DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT

-- Decide if underwriting or Agency
DECLARE @iLedgerID int, @UWType char(1)

--SJP (13/06/2002) UW_Type selected by branch_id  = 1 and option_number = 1 to ensure unique record
SELECT @UWtype = UW_type FROM hidden_options where branch_id = 1 and option_number = 1

IF isnull(@Reinsurer,'') = ''
    SELECT @Reinsurer = 'ALL'

IF @Reinsurer = 'ALL'
BEGIN
    INSERT INTO #tempAUAPremiumBordereau
        SELECT
            sf.insurance_ref,          -- 2
            sf.insurance_holder_name,  -- 4
            sf.transaction_type_id,    -- 5
            sf.transaction_type_code,  -- 6
            sf.product_code,           -- 7
            p.description,             -- 9
            sd.risk_type_id,           -- 11
            sd.ri_party_cnt,           -- 12
            sd.ri_shortname,           -- 13
            NULL,                  -- 14
            (SELECT sd.this_premium_home WHERE sd.stats_detail_type in ('TTY', 'COI')),      -- 15
--            isnull(sd.lead_commission_value_home,0) +
  --                 isnull(sd.sub_commission_value_home,0),   -- 16
    --        sd.commission_percent,                           -- 17
        (SELECT sd.tax_value WHERE sd.stats_detail_type = 'TAT' and sd.tax_type_code = 'IPT'),
        (SELECT sd.tax_value WHERE sd.stats_detail_type = 'TAT' and sd.tax_type_code = 'VAT'),
            rt.description,                                  -- 18
            sf.cover_start_date,
            sf.expiry_date,
            @dtCurrentPeriodEnd,                              -- 19
        sd.stats_detail_type,
        @UWType


        FROM Stats_Folder sf
        JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
                AND (isnull(sd.this_premium_home,0) <> 0
                OR isnull(sd.lead_commission_value_home,0) +
                    isnull(sd.sub_commission_value_home,0) <> 0)
        LEFT OUTER JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
        LEFT OUTER JOIN Product p ON sf.product_id = p.product_id
        WHERE
        sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
    AND (stats_detail_type = 'TTY' OR stats_detail_type = 'COI' or stats_detail_type = 'TAT')
-- PW110402 - use period instead of start/end date
    AND (sf.posting_period_number = @SelectedPeriodID)


END
ELSE
BEGIN

    INSERT INTO #tempAUAPremiumBordereau
        SELECT
            sf.insurance_ref,         -- 2
            sf.insurance_holder_name, -- 4
            sf.transaction_type_id,   -- 5
            sf.transaction_type_code, -- 6
            sf.product_code,          -- 7
            p.description,            -- 9
            sd.risk_type_id,          -- 11
            sd.ri_party_cnt,          -- 12
            sd.ri_shortname,          -- 13
            NULL,                 -- 14
            (SELECT sd.this_premium_home WHERE sd.stats_detail_type in ('TTY', 'COI')),      -- 15
            --isnull(sd.lead_commission_value_home,0) +
              --    isnull(sd.sub_commission_value_home,0),  --16
            --sd.commission_percent,  -- 17
        (SELECT sd.tax_value WHERE sd.stats_detail_type = 'TAT' and sd.tax_type_code = 'IPT'),
        (SELECT sd.tax_value WHERE sd.stats_detail_type = 'TAT' and sd.tax_type_code = 'VAT'),
            rt.description,         -- 18
            sf.cover_start_date,
            sf.expiry_date,
            @dtCurrentPeriodEnd,     -- 19
        sd.stats_detail_type,
        @UWType

        FROM Stats_Folder sf
        JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
             AND sd.ri_party_cnt IN
                (SELECT party_cnt
                FROM Party
                WHERE resolved_name LIKE (@Reinsurer)                 -- allow for wildcard searches
            AND (isnull(sd.this_premium_home,0) <> 0)
                OR isnull(sd.lead_commission_value_home,0) +
                    isnull(sd.sub_commission_value_home,0) <> 0)
        LEFT OUTER JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id

        LEFT OUTER JOIN Product p ON sf.product_id = p.product_id
        WHERE sf.posting_period_number = @CurrentPeriodID
        AND
        sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
    AND (stats_detail_type = 'TTY' OR stats_detail_type = 'COI' or stats_detail_type = 'TAT')
-- PW110402 - use period instead of start/end date
    AND (sf.posting_period_number = @SelectedPeriodID)


END
-- Update with appropraite details for treaty/coinsurance
UPDATE #tempAUAPremiumBordereau
SET Reinsurer = t.description
FROM treaty t
WHERE  ReinsurerShort = t.code  and stats_detail_type IN ('TTY','TAT')

UPDATE #tempAUAPremiumBordereau
SET Reinsurer = py.name
FROM party py
WHERE  ReinsurerShort = py.shortname  and stats_detail_type = 'COI'

SET NOCOUNT OFF

SELECT * FROM #tempAUAPremiumBordereau

DROP TABLE  #tempAUAPremiumBordereau

GO