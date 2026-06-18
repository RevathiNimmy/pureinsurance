SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO


DDLDropProcedure 'spu_Report_Agent_Analysis'
GO


---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/**********************************************************************************************************************************
** Created by Jude Killip
** 24/08/2000
** RSA Reports - Agent_Analysis.rpt
**  Created with dummy data to build the report
**********************************************************************************************************************************
** 09/11/2000   Thinh Nguyen    real data
**              Jude Killip     use temp table and add dummy records necessary for subreport
**
** 07/12/2000   Jude Killip     bug 297 - stats_detail_type
**
** 22/03/2001   Jude Killip     rewrite
**
** 30/04/2001   Jude Killip     use Period for dates
**
** 12/06/2001   Jude Killip     Amend Period - use sf.posting_period_number
**
** 04/07/2001   Jude Killip     filters for Claims/nonClaims details
**                              allow for very first period
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     30/01/2002  JMK     Use new lookup parameter "Period" - user selects from list of
**                              ..current and previous period_end_dates (as a string)
**                              Add Agent parameter
**                              Add date filters to WHERE
***********************************************************************************************************************************
** 2.00     12/08/2002  TOMB    Link to claims table to calculate incurred claims using the forumla
**                              Inc Claim = Initial Reserve + sum(reserve changes) + sum(claim payments) - sum(claim recoveries)
**                              formula supplied by Bernie Bradley
**                              Also claim data converted from local to base currency
** 2.01     16/08/2002  TOMB    Change incurred claims to John Whites formula = sum(initial reserves) + sum(reserve changes)
**
** 2.01     30/01/2003  JMK     Replace double quotes
** 3		12/05/2004	DD		Revised for Multi-Currency - all rates are multipliers
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_Agent_Analysis
                    @branch_id int,
                    @PeriodDate varchar (20),
                    @Agent varchar (20),
                    @sBasis varchar(50)
AS
-- $Author: Jude.killip $
-- $Revision: 14 $
-- $Modtime: 30/01/03 14:45 $
-- $Workfile: sp_Report_Agent_Analysis.sql $
-- $Logfile: /Sirius For Underwriting/Crystal Reports/Stored Procedures/sp_Report_Agent_Analysis.sql $
-- $History: sp_Report_Agent_Analysis.sql $
-- 
-- *****************  Version 14  *****************
-- User: Jude.killip  Date: 30/01/03   Time: 14:53
-- Updated in $/Sirius For Underwriting/Crystal Reports/Stored Procedures
-- Replace double quotes with single quotes
--
-- *****************  Version 13  *****************
-- User: Tom.brown    Date: 20/11/02   Time: 15:12
-- Updated in $/Sirius For Underwriting/Crystal Reports/Stored Procedures
-- Overhaul:  Standardise parameters as Branch, Period End Date, <others>
-- and Basis (Date/Period)
--
-- *****************  Version 1  *****************
-- User: Tom.brown    Date: 19/11/02   Time: 16:42
-- Created in $/Work/SWIssues/Reports/ParamChanges
-- Interim:  Work to end 19 Nov 2002

SET NOCOUNT ON
SET ANSI_NULLS OFF
DECLARE @DEBUG INT
SELECT @DEBUG = 0    -- 0 = OFF, 1 = ON

/*
-- For testing
DECLARE @branch_id int,
        @PeriodDate varchar (20),
        @Agent varchar (20),
        @sBasis varchar(50)
SELECT  @branch_id = 0,
        @PeriodDate = 'Dec 31 2002',
        @Agent = 'HFRASER',
        @sBasis = 'Transaction Date'
*/

-- Report Basis:  By Date or by Period ID
DECLARE @iBasis INT
-- Branch (Company No)
DECLARE @iBranchID INT
-- Branch to select from Period Table
DECLARE @iBranchPeriod INT
-- Always use Branch 1 period table
-- if anyone sets up different periods for different branches this will
-- need to be revisited and worked out for the branch
SELECT @iBranchPeriod = 1


-- Report Data selection Dates and Period ID
DECLARE @SelectedPeriodID int
DECLARE @iPeriod INT
DECLARE @dtSelectedPeriodEnd datetime
DECLARE @period_end_date    datetime
DECLARE @prev_period_end_date datetime

DECLARE @YearStartPeriodID int
DECLARE @dtYearStart datetime
DECLARE @YearBeforeStartID int
DECLARE @dtYearBeforeStart datetime

-- e.g. if 12 periods ago is March 2001, 12Agoprev is Feb 2001
DECLARE @12PeriodsAgoID int
DECLARE @dt12PeriodsAgo datetime
DECLARE @dt12PeriodsAgoPrev datetime

DECLARE @24PeriodsAgoID int
DECLARE @dt24PeriodsAgo datetime
DECLARE @dt24PeriodsAgoPrev datetime

--SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @period_end_date = CONVERT (Datetime, @PeriodDate)
SELECT @period_end_date = DATEADD (d, 1, @period_end_date)
SELECT @period_end_date = DATEADD (s, -1, @period_end_date)

SELECT @dtSelectedPeriodEnd = @period_end_date
if @branch_id is null
    SELECT @iBranchID = 0
else
    SELECT @iBranchID = @branch_id

IF @sBasis = 'Transaction Date'
BEGIN
    SELECT @iBasis = 1    -- Transaction Date
END
ELSE
BEGIN
    SELECT @iBasis = 0    -- Transaction Period
END
    SELECT @prev_period_end_date = ( SELECT max(period_end_date)
                                       FROM Period
                                      WHERE period_end_date < @period_end_date
                                        AND company_id = @iBranchPeriod )

    SELECT @iPeriod = ( SELECT period_id
                        FROM period
                        WHERE period_end_Date = @period_end_date
                         AND company_id = @iBranchPeriod )

-- Selected Period
SELECT @SelectedPeriodID = @iPeriod
-- get the year start period ID
SELECT @YearStartPeriodID = ( SELECT  min(period_id)
                                FROM period
                                WHERE year_name = ( SELECT year_name
                                                      FROM period
                                                     WHERE period_id = @SelectedPeriodID
                                                       AND company_id = @iBranchPeriod)
                                  AND company_id = @iBranchPeriod )
SELECT @dtYearStart = ( SELECT max(period_end_date)
                          FROM Period
                         WHERE period_id < @YearStartPeriodID
                           AND company_id = @iBranchPeriod )

-- Year before
SELECT @dtYearBeforeStart = DATEADD(year, -1, @dtYearStart)
SELECT @YearBeforeStartID =  (SELECT period_id
                            FROM period
                           WHERE period_end_Date = (
                                 SELECT max(period_end_date)
                                   FROM period
                                  WHERE period_End_date <= @dtYearBeforeStart
                                    AND company_id = @iBranchPeriod )
                             AND company_id = @iBranchPeriod )
IF ISNULL(@YearBeforeStartID, 0) <= 0 SELECT @YearBeforeStartID = 0

-- get the 12 period ID
SELECT @dt12PeriodsAgo = DATEADD(year, -1, @period_end_date)
SELECT @dt12PeriodsAgoPrev = DATEADD(year, -1, @prev_period_end_date)
SELECT @12PeriodsAgoID = (SELECT period_id
                            FROM period
                           WHERE period_end_Date = (
                                 SELECT max(period_end_date)
                                   FROM period
                                  WHERE period_End_date <= @dt12PeriodsAgo
                                    AND company_id = @iBranchPeriod )
                             AND company_id = @iBranchPeriod )
IF ISNULL(@12PeriodsAgoID, 0) <= 0 SELECT @12PeriodsAgoID = 0

-- get the 24 period ID
SELECT @dt24PeriodsAgo = DATEADD(year, -2, @period_end_date)
SELECT @dt24PeriodsAgoPrev = DATEADD(year, -2, @prev_period_end_date)
SELECT @24PeriodsAgoID = (SELECT period_id
                            FROM period
                           WHERE period_end_Date = (
                                 SELECT max(period_end_date)
                                   FROM period
                                  WHERE period_End_date <= @dt24PeriodsAgo
                                    AND company_id = @iBranchPeriod )
                             AND company_id = @iBranchPeriod )

IF ISNULL(@24PeriodsAgoID ,0) <= 0 SELECT @24PeriodsAgoID = 0


IF @DEBUG = 1
BEGIN
    SELECT ' @SelectedPeriodID     = ',  @SelectedPeriodID
    SELECT ' @iPeriod              = ',  @iPeriod
    SELECT ' @dtSelectedPeriodEnd  = ',  @dtSelectedPeriodEnd
    SELECT ' @period_end_date      = ',  @period_end_date
    SELECT ' @prev_period_end_date = ',  @prev_period_end_date
    SELECT ' @YearStartPeriodID    = ',  @YearStartPeriodID
    SELECT ' @dtYearStart          = ',  @dtYearStart
    SELECT ' @YearBeforeStartID    = ',  @YearBeforeStartID
    SELECT ' @dtYearBeforeStart    = ',  @dtYearBeforeStart
    SELECT ' @12PeriodsAgoID       = ',  @12PeriodsAgoID
    SELECT ' @dt12PeriodsAgo       = ',  @dt12PeriodsAgo
    SELECT ' @dt12PeriodsAgoPrev   = ',  @dt12PeriodsAgoPrev
    SELECT ' @24PeriodsAgoID       = ',  @24PeriodsAgoID
    SELECT ' @dt24PeriodsAgo       = ',  @dt24PeriodsAgo
    SELECT ' @dt24PeriodsAgoPrev   = ',  @dt24PeriodsAgoPrev
END


/* this is shite we need last year start ID and prev year end date
-- calc Last Year YTD range
-- No of periods between now and year start
DECLARE @YTDRange int
SELECT @YTDRange = (@SelectedPeriodID - @YearStartPeriodID)
*/

-- Get the Incurred Claims first
CREATE TABLE #ClaimReserve
 (
    ClaimID int,
    InitRsv money NULL,
    Paid    money NULL,
    RevRsv  money NULL
 )

IF @iBasis = 0  -- Transaction Period
BEGIN
    INSERT #ClaimReserve
    SELECT cp.claim_id, sum(rv.Initial_reserve), sum(rv.Paid_to_date), sum(rv.Revised_reserve)
     FROM reserve rv
    INNER JOIN claim_peril cp
       ON rv.claim_peril_id = cp.claim_peril_id
    WHERE claim_id in (SELECT sf.loss_id
                     FROM stats_folder sf
                    INNER JOIN stats_detail sd
                       ON sd.stats_folder_cnt = sf.stats_folder_cnt
                    WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')               -- just claims
                        AND ( sf.posting_period_number = @SelectedPeriodID OR
                              sf.posting_period_number = @12PeriodsAgoID OR
                             (sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID ) OR
                             (sf.posting_period_number BETWEEN @YearBeforeStartID AND @12PeriodsAgoID) OR
                             (sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID) OR
                             (sf.posting_period_number BETWEEN @24PeriodsAgoID + 1  AND @12PeriodsAgoID)
                            )
                    )
    GROUP BY cp.claim_id
END    -- iBasis = 0 Transaction Period
ELSE
BEGIN    -- iBasis <> 0 Transaction Date
    INSERT #ClaimReserve
    SELECT cp.claim_id, sum(rv.Initial_reserve), sum(rv.Paid_to_date), sum(rv.Revised_reserve)
     FROM reserve rv
    INNER JOIN claim_peril cp
       ON rv.claim_peril_id = cp.claim_peril_id
    WHERE claim_id in (SELECT sf.loss_id
                     FROM stats_folder sf
                    INNER JOIN stats_detail sd
                       ON sd.stats_folder_cnt = sf.stats_folder_cnt
                    WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code LIKE ('C_%')               -- just claims
                        AND ( ( sf.document_Date > @prev_period_end_date AND sf.document_Date <= @period_end_date    ) OR
                              ( sf.document_Date > @dt12PeriodsAgoPrev   AND sf.document_date <= @dt12PeriodsAgo     ) OR
                              ( sf.document_date > @dtYEarStart          AND sf.document_Date <= @period_end_date    ) OR
                              ( sf.document_date > @dtYearBeforeStart    AND sf.document_date <= @dt12PeriodsAgoPrev ) OR
                              ( sf.document_date > @dt12PeriodsAgo       AND sf.document_date <= @period_end_date    ) OR
                              ( sf.document_date > @dt24PeriodsAgo       AND sf.document_date <= @dt12PeriodsAgoPrev )
                            )
                    )
    GROUP BY cp.claim_id
END    -- iBasis <> 0 Transaction Date


CREATE TABLE #ClaimRecovery
 (
    ClaimID int,
    InitRsv money NULL,
    Received money NULL,
    RevRsv  money NULL
 )

/*
IF @iBasis = 0  -- Transaction Period
BEGIN
END    -- iBasis = 0 Transaction Period
ELSE
BEGIN    -- iBasis <> 0 Transaction Date
END    -- iBasis <> 0 Transaction Date
*/

IF @iBasis = 0  -- Transaction Period
BEGIN
    INSERT #ClaimRecovery
    SELECT cp.claim_id, sum(rc.Initial_reserve), sum(rc.received_to_date), sum(rc.Revised_reserve)
     FROM recovery rc
    INNER JOIN claim_peril cp
       ON rc.claim_peril_id = cp.claim_peril_id
    WHERE claim_id in (SELECT sf.loss_id
                         FROM stats_folder sf
                        INNER JOIN stats_detail sd
                           ON sd.stats_folder_cnt = sf.stats_folder_cnt
                        WHERE sd.stats_detail_type = 'GRS'
                          AND sf.transaction_type_code LIKE ('C_%')               -- just claims
                          AND ( sf.posting_period_number = @SelectedPeriodID OR
                                sf.posting_period_number = @12PeriodsAgoID OR
                               (sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID ) OR
                               (sf.posting_period_number BETWEEN @YearBeforeStartID AND @12PeriodsAgoID) OR
                               (sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID) OR
                               (sf.posting_period_number BETWEEN @24PeriodsAgoID + 1  AND @12PeriodsAgoID)
                              )
                       )
    GROUP BY cp.claim_id
END    -- iBasis = 0 Transaction Period
ELSE
BEGIN    -- iBasis <> 0 Transaction Date
    INSERT #ClaimRecovery
    SELECT cp.claim_id, sum(rc.Initial_reserve), sum(rc.received_to_date), sum(rc.Revised_reserve)
     FROM recovery rc
    INNER JOIN claim_peril cp
       ON rc.claim_peril_id = cp.claim_peril_id
    WHERE claim_id in (SELECT sf.loss_id
                         FROM stats_folder sf
                        INNER JOIN stats_detail sd
                           ON sd.stats_folder_cnt = sf.stats_folder_cnt
                        WHERE sd.stats_detail_type = 'GRS'
                          AND sf.transaction_type_code LIKE ('C_%')               -- just claims
                        AND ( ( sf.document_Date > @prev_period_end_date AND sf.document_Date <= @period_end_date    ) OR
                              ( sf.document_Date > @dt12PeriodsAgoPrev   AND sf.document_date <= @dt12PeriodsAgo     ) OR
                              ( sf.document_date > @dtYEarStart          AND sf.document_Date <= @period_end_date    ) OR
                              ( sf.document_date > @dtYearBeforeStart    AND sf.document_date <= @dt12PeriodsAgoPrev ) OR
                              ( sf.document_date > @dt12PeriodsAgo       AND sf.document_date <= @period_end_date    ) OR
                              ( sf.document_date > @dt24PeriodsAgo       AND sf.document_date <= @dt12PeriodsAgoPrev )
                            )
                    )
    GROUP BY cp.claim_id
END    -- iBasis <> 0 Transaction Date


CREATE TABLE #IncurredClaims
(
    ClaimID int,
    IncurredClaims money NULL
)



--SELECT * FROM #ClaimReserve
--SELECT * FROM #ClaimRecovery
/* View contents of claims incurred tables
SELECT rv.claimID, isnull(rv.InitRsv,0), isnull(rv.Paid,0), isnull(rv.RevRsv,0),
                   isnull(rc.InitRsv,0), isnull(rc.Received,0),  isnull(rc.RevRsv,0),
                   'Incurred'= isnull(rv.InitRsv,0) + isnull(rv.RevRsv,0) - isnull(rv.Paid,0) + isnull(rc.Received,0)
  FROM #ClaimReserve rv
  LEFT OUTER JOIN #ClaimRecovery rc
    ON rv.claimid = rc.claimid
 ORDER BY incurred
*/

INSERT #IncurredClaims
-- TB 16/08/02 - leave the old formula in, in case I need to change it back
--SELECT rv.claimID, 'Incurred'= isnull(rv.InitRsv,0) + isnull(rv.RevRsv,0) - isnull(rv.Paid,0) + isnull(rc.Received,0)
SELECT rv.claimID, 'Incurred'= isnull(rv.InitRsv,0) + isnull(rv.RevRsv,0)
  FROM #ClaimReserve rv
  LEFT OUTER JOIN #ClaimRecovery rc
    ON rv.claimid = rc.claimid
-- ORDER BY incurred

--SELECT * FROM #IncurredClaims

CREATE TABLE #tempRSAAgentAnal
(
            AgentCnt int NULL,
            Agent varchar (20) NULL,
            Period varchar (40),
            dtSelectedPeriodEnd datetime,
            TransTypeID int,
            TransType varchar (10) NULL,
            RiskTypeCode varchar (10) NULL,
            RiskTypeDescription varchar (255) NULL,
            ProductCode varchar (10) NULL,
            Product varchar (255) NULL,
            NBPRemium money NULL,
            RenPremium money NULL,
            AllPremium money NULL,
            AllPremiumCompare money NULL,
            Commission money NULL,
            Claim money NULL,
            ClaimCompare money NULL,
            Claim_ID int NULL,
            ClaimDate datetime NULL,
            ClaimCurrency_ID int NULL,
            CurrencyRate money NULL,
            TempID int IDENTITY

)

--set IDENTITY_INSERT #tempRSAAgentAnal ON
--go


-- add 'Selected Period' records
IF @iBasis = 0  -- Transaction Period
BEGIN
    INSERT  #tempRSAAgentAnal
        SELECT  DISTINCT  sf.agent_cnt,
                sf.agent_shortname,
                'Selected Period',
                @dtSelectedPeriodEnd,
                sf.transaction_type_id,
                sf.transaction_type_code,
                sd.risk_type_code,
                rt.description,
                p.code,
                p.description,
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'NB'                     -- new business
                        AND sf.posting_period_number = @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'REN'                    -- renewals
                        AND sf.posting_period_number = @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number = @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number = @12PeriodsAgoID),
                (SELECT isnull(sd.lead_commission_value_home,0) +
                                isnull(sd.sub_commission_value_home,0)
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number = @SelectedPeriodID),
               (SELECT 'Incurred Claims' = ic.IncurredClaims
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = ('C_CO')               -- just claims opened
                        AND sf.posting_period_number = @SelectedPeriodID),
                (SELECT 'Claim Compare' = ic.IncurredClaims
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = ('C_CO')               -- just claims opened
                        AND sf.posting_period_number = @12PeriodsAgoID),
                  sf.loss_id,
                  c.loss_from_date,
                  c.currency_id
        FROM Stats_Folder sf
        JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
        JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
        JOIN Product p ON sf.product_id = p.product_id
        LEFT OUTER JOIN claim c ON sf.loss_id = c.claim_id
        LEFT OUTER JOIN #IncurredClaims ic ON c.claim_id = ic.claimID
        WHERE isnull(sf.agent_cnt,'') <> ''
        AND sd.stats_detail_type = 'GRS'
        AND (isnull(sd.this_premium_home,0) <> 0
            OR (isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0)) <> 0
            OR isnull(sd.sum_insured_total,0) <> 0)
        AND sf.posting_period_number IN (@SelectedPeriodID,@12PeriodsAgoID)
        AND (
            @Agent = 'ALL'
            OR
            sf.agent_shortname = @Agent
            )
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )
    GROUP BY sf.agent_cnt, sf.agent_shortname, sf.transaction_type_id,
             sf.transaction_type_code, sd.risk_type_code, rt.description,
             p.code, p.description, sd.this_premium_home, sd.stats_detail_type,
             sd.lead_commission_value_home, sd.sub_commission_value_home, sd.sum_insured_total,
             sf.posting_period_number, sf.loss_id, c.loss_from_date, c.currency_id, ic.IncurredClaims


    -- add 'this year' records (year to selected period)
    INSERT  #tempRSAAgentAnal
        SELECT  DISTINCT sf.agent_cnt,
                sf.agent_shortname,
                'Year To Selected Period',
                @dtSelectedPeriodEnd,
                sf.transaction_type_id,
                sf.transaction_type_code,
                sd.risk_type_code,
                rt.description,
                p.code,
                p.description,
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'NB'                     -- new business
                        AND sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'REN'                    -- renewals
                        AND sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number BETWEEN @YearBeforeStartID AND @12PeriodsAgoID),
                (SELECT isnull(sd.lead_commission_value_home,0) +
                                isnull(sd.sub_commission_value_home,0)
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID),
                (SELECT 'Incurred Claims' = ic.IncurredClaims
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = ('C_CO')               -- just claims
                        AND sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID),
                (SELECT 'Claim Compare' = ic.IncurredClaims
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = ('C_CO')               -- just claims
                        AND sf.posting_period_number BETWEEN @YearBeforeStartID AND @12PeriodsAgoID),
                  sf.loss_id,
                  c.loss_from_date,
                  c.currency_id
        FROM Stats_Folder sf
        JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
        JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
        JOIN Product p ON sf.product_id = p.product_id
        LEFT OUTER JOIN claim c on sf.loss_id = c.claim_id
        LEFT OUTER JOIN #IncurredClaims ic ON c.claim_id = ic.claimID
        WHERE isnull(sf.agent_cnt,'') <> ''
        AND sd.stats_detail_type = 'GRS'
        AND (isnull(sd.this_premium_home,0) <> 0
            OR (isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0)) <> 0
            OR isnull(sd.sum_insured_total,0) <> 0)
        AND (
            sf.posting_period_number BETWEEN @YearBeforeStartID AND @12PeriodsAgoID
            OR
            sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID
            )
        AND (
            @Agent = 'ALL'
            OR
            sf.agent_shortname = @Agent
            )
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )
    GROUP BY sf.agent_cnt, sf.agent_shortname, sf.transaction_type_id,
             sf.transaction_type_code, sd.risk_type_code, rt.description,
             p.code, p.description, sd.this_premium_home, sd.stats_detail_type,
             sd.lead_commission_value_home, sd.sub_commission_value_home, sd.sum_insured_total,
             sf.posting_period_number, sf.loss_id, c.loss_from_date, c.currency_id, ic.IncurredClaims


    -- add '12 period' records (12 periods including selected period)
    INSERT  #tempRSAAgentAnal
        SELECT  DISTINCT sf.agent_cnt,
                sf.agent_shortname,
                '12 Periods',
                @dtSelectedPeriodEnd,
                sf.transaction_type_id,
                sf.transaction_type_code,
                sd.risk_type_code,
                rt.description,
                p.code,
                p.description,
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'NB'                     -- new business
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID + 1  AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'REN'                    -- renewals
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number BETWEEN @24PeriodsAgoID + 1  AND @12PeriodsAgoID),
                (SELECT isnull(sd.lead_commission_value_home,0) +
                                isnull(sd.sub_commission_value_home,0)
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID),
                (SELECT 'Incurred Claims' = ic.IncurredClaims
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = ('C_CO')               -- just claims
                        AND sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID),
                (SELECT 'Claim Compare' = ic.IncurredClaims
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = ('C_CO')               -- just claims
                        AND sf.posting_period_number BETWEEN @24PeriodsAgoID + 1  AND @12PeriodsAgoID),
                  sf.loss_id,
                  c.loss_from_date,
                  c.currency_id
        FROM Stats_Folder sf
        JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
        JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
        JOIN Product p ON sf.product_id = p.product_id
        LEFT OUTER JOIN claim c on sf.loss_id = c.claim_id
        LEFT OUTER JOIN #IncurredClaims ic ON c.claim_id = ic.claimID
        WHERE isnull(sf.agent_cnt,'') <> ''
        AND sd.stats_detail_type = 'GRS'
        AND (isnull(sd.this_premium_home,0) <> 0
            OR (isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0)) <> 0
            OR isnull(sd.sum_insured_total,0) <> 0)
        AND sf.posting_period_number BETWEEN @24PeriodsAgoID + 1  AND @SelectedPeriodID
        AND (
            @Agent = 'ALL'
            OR
            sf.agent_shortname = @Agent
            )
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )
    GROUP BY sf.agent_cnt, sf.agent_shortname, sf.transaction_type_id,
             sf.transaction_type_code, sd.risk_type_code, rt.description,
             p.code, p.description, sd.this_premium_home, sd.stats_detail_type,
             sd.lead_commission_value_home, sd.sub_commission_value_home, sd.sum_insured_total,
             sf.posting_period_number, sf.loss_id, c.loss_from_date, c.currency_id, ic.IncurredClaims

END    -- iBasis = 0 Transaction Period
ELSE
BEGIN    -- iBasis <> 0 Transaction Date

    INSERT  #tempRSAAgentAnal
        SELECT  DISTINCT  sf.agent_cnt,
                sf.agent_shortname,
                'Selected Period',
                @dtSelectedPeriodEnd,
                sf.transaction_type_id,
                sf.transaction_type_code,
                sd.risk_type_code,
                rt.description,
                p.code,
                p.description,
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'NB'                     -- new business
                        AND ( sf.document_Date > @prev_period_end_date AND sf.document_Date <= @period_end_date    ) ),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'REN'                    -- renewals
                        AND ( sf.document_Date > @prev_period_end_date AND sf.document_Date <= @period_end_date    ) ),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND ( sf.document_Date > @prev_period_end_date AND sf.document_Date <= @period_end_date    ) ),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND ( sf.document_Date > @dt12PeriodsAgoPrev   AND sf.document_date <= @dt12PeriodsAgo     ) ),
                (SELECT isnull(sd.lead_commission_value_home,0) +
                                isnull(sd.sub_commission_value_home,0)
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND ( sf.document_Date > @prev_period_end_date AND sf.document_Date <= @period_end_date    )),
               (SELECT 'Incurred Claims' = ic.IncurredClaims
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = ('C_CO')               -- just claims opened
                        AND ( sf.document_Date > @prev_period_end_date AND sf.document_Date <= @period_end_date    )),
                (SELECT 'Claim Compare' = ic.IncurredClaims
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = ('C_CO')               -- just claims opened
                        AND ( sf.document_Date > @dt12PeriodsAgoPrev   AND sf.document_date <= @dt12PeriodsAgo     ) ),
                  sf.loss_id,
                  c.loss_from_date,
                  c.currency_id
        FROM Stats_Folder sf
        JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
        JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
        JOIN Product p ON sf.product_id = p.product_id
        LEFT OUTER JOIN claim c ON sf.loss_id = c.claim_id
        LEFT OUTER JOIN #IncurredClaims ic ON c.claim_id = ic.claimID
        WHERE isnull(sf.agent_cnt,'') <> ''
        AND sd.stats_detail_type = 'GRS'
        AND (isnull(sd.this_premium_home,0) <> 0
            OR (isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0)) <> 0
            OR isnull(sd.sum_insured_total,0) <> 0)
        AND ( ( sf.document_Date > @prev_period_end_date AND sf.document_Date <= @period_end_date    ) OR
              ( sf.document_Date > @dt12PeriodsAgoPrev   AND sf.document_date <= @dt12PeriodsAgo     )  )
        AND (
            @Agent = 'ALL'
            OR
            sf.agent_shortname = @Agent
            )
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )
    GROUP BY sf.agent_cnt, sf.agent_shortname, sf.transaction_type_id,
             sf.transaction_type_code, sd.risk_type_code, rt.description,
             p.code, p.description, sd.this_premium_home, sd.stats_detail_type,
             sd.lead_commission_value_home, sd.sub_commission_value_home, sd.sum_insured_total,
             sf.document_Date, sf.loss_id, c.loss_from_date, c.currency_id, ic.IncurredClaims


    -- add 'this year' records (year to selected period)
    INSERT  #tempRSAAgentAnal
        SELECT  DISTINCT sf.agent_cnt,
                sf.agent_shortname,
                'Year To Selected Period',
                @dtSelectedPeriodEnd,
                sf.transaction_type_id,
                sf.transaction_type_code,
                sd.risk_type_code,
                rt.description,
                p.code,
                p.description,
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'NB'                     -- new business
                        AND ( sf.document_date > @dtYEarStart          AND sf.document_Date <= @period_end_date    ) ),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'REN'                    -- renewals
                        AND ( sf.document_date > @dtYEarStart          AND sf.document_Date <= @period_end_date    ) ),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND ( sf.document_date > @dtYEarStart          AND sf.document_Date <= @period_end_date    ) ),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND ( sf.document_date > @dtYearBeforeStart    AND sf.document_date <= @dt12PeriodsAgoPrev ) ),
                (SELECT isnull(sd.lead_commission_value_home,0) +
                                isnull(sd.sub_commission_value_home,0)
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND ( sf.document_date > @dtYearBeforeStart    AND sf.document_date <= @dt12PeriodsAgoPrev ) ),
                (SELECT 'Incurred Claims' = ic.IncurredClaims
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = ('C_CO')               -- just claims
                        AND ( sf.document_date > @dtYEarStart          AND sf.document_Date <= @period_end_date    ) ),
                (SELECT 'Claim Compare' = ic.IncurredClaims
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = ('C_CO')               -- just claims
                        AND ( sf.document_date > @dtYearBeforeStart    AND sf.document_date <= @dt12PeriodsAgoPrev ) ),
                  sf.loss_id,
                  c.loss_from_date,
                  c.currency_id
        FROM Stats_Folder sf
        JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
        JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
        JOIN Product p ON sf.product_id = p.product_id
        LEFT OUTER JOIN claim c on sf.loss_id = c.claim_id
        LEFT OUTER JOIN #IncurredClaims ic ON c.claim_id = ic.claimID
        WHERE isnull(sf.agent_cnt,'') <> ''
        AND sd.stats_detail_type = 'GRS'
        AND (isnull(sd.this_premium_home,0) <> 0
            OR (isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0)) <> 0
            OR isnull(sd.sum_insured_total,0) <> 0)
        AND (
            ( sf.document_date > @dtYEarStart          AND sf.document_Date <= @period_end_date    )
            OR
            ( sf.document_date > @dtYearBeforeStart    AND sf.document_date <= @dt12PeriodsAgoPrev )
            )
        AND (
            @Agent = 'ALL'
            OR
            sf.agent_shortname = @Agent
            )
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )
    GROUP BY sf.agent_cnt, sf.agent_shortname, sf.transaction_type_id,
             sf.transaction_type_code, sd.risk_type_code, rt.description,
             p.code, p.description, sd.this_premium_home, sd.stats_detail_type,
             sd.lead_commission_value_home, sd.sub_commission_value_home, sd.sum_insured_total,
             sf.document_Date, sf.loss_id, c.loss_from_date, c.currency_id, ic.IncurredClaims


    -- add '12 period' records (12 periods including selected period)
    INSERT  #tempRSAAgentAnal
        SELECT  DISTINCT sf.agent_cnt,
                sf.agent_shortname,
                '12 Periods',
                @dtSelectedPeriodEnd,
                sf.transaction_type_id,
                sf.transaction_type_code,
                sd.risk_type_code,
                rt.description,
                p.code,
                p.description,
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'NB'                     -- new business
                        AND ( sf.document_date > @dt12PeriodsAgo       AND sf.document_date <= @period_end_date    ) ),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = 'REN'                    -- renewals
                        AND( sf.document_date > @dt12PeriodsAgo       AND sf.document_date <= @period_end_date    ) ),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND ( sf.document_date > @dt12PeriodsAgo       AND sf.document_date <= @period_end_date    ) ),
                (SELECT sd.this_premium_home
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND ( sf.document_date > @dt24PeriodsAgo       AND sf.document_date <= @dt12PeriodsAgoPrev )),
                (SELECT isnull(sd.lead_commission_value_home,0) +
                                isnull(sd.sub_commission_value_home,0)
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
                        AND ( sf.document_date > @dt12PeriodsAgo       AND sf.document_date <= @period_end_date    ) ),
                (SELECT 'Incurred Claims' = ic.IncurredClaims
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = ('C_CO')               -- just claims
                        AND ( sf.document_date > @dt12PeriodsAgo       AND sf.document_date <= @period_end_date    ) ),
                (SELECT 'Claim Compare' = ic.IncurredClaims
                        WHERE sd.stats_detail_type = 'GRS'
                        AND sf.transaction_type_code = ('C_CO')               -- just claims
                        AND ( sf.document_date > @dt24PeriodsAgo       AND sf.document_date <= @dt12PeriodsAgoPrev )),
                  sf.loss_id,
                  c.loss_from_date,
                  c.currency_id
        FROM Stats_Folder sf
        JOIN Stats_Detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
        JOIN Risk_Type rt ON sd.risk_type_id = rt.risk_type_id
        JOIN Product p ON sf.product_id = p.product_id
        LEFT OUTER JOIN claim c on sf.loss_id = c.claim_id
        LEFT OUTER JOIN #IncurredClaims ic ON c.claim_id = ic.claimID
        WHERE isnull(sf.agent_cnt,'') <> ''
        AND sd.stats_detail_type = 'GRS'
        AND (isnull(sd.this_premium_home,0) <> 0
            OR (isnull(sd.lead_commission_value_home,0) +
                isnull(sd.sub_commission_value_home,0)) <> 0
            OR isnull(sd.sum_insured_total,0) <> 0)
        AND ( ( sf.document_date > @dt24PeriodsAgo       AND sf.document_date <= @dt12PeriodsAgoPrev )
              OR ( sf.document_date > @dt12PeriodsAgo       AND sf.document_date <= @period_end_date    ) )
        AND (
            @Agent = 'ALL'
            OR
            sf.agent_shortname = @Agent
            )
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )
    GROUP BY sf.agent_cnt, sf.agent_shortname, sf.transaction_type_id,
             sf.transaction_type_code, sd.risk_type_code, rt.description,
             p.code, p.description, sd.this_premium_home, sd.stats_detail_type,
             sd.lead_commission_value_home, sd.sub_commission_value_home, sd.sum_insured_total,
             sf.document_Date, sf.loss_id, c.loss_from_date, c.currency_id, ic.IncurredClaims

END -- @iBasis <> 0 -- Transaction Date

--set IDENTITY_INSERT #tempRSAAgentAnal OFF
--go
--------------------------------------------------------------------------------
--- Claim values are in local currency, so we have to convert to home currency
---------------------------------------------------------------------------------


-- Currency Rate Cursor variables
DECLARE @CR_Effective_from datetime,
        @CR_Rate_against_base money,
        @CR_Currency_Id int

---------- Cursor Efficiency Notes -----------------------------
-- Original Cursor went through #TempRSAAgentAnal and looked up
-- currency rate for each entry.  That took Ages:
-- New version has cursor go through currency rate and update
-- all #TempRSAAgenAnal records affected by that rate
-- Can now use FAST_FORWARD as no updating of the cursor
----------------------------------------------------------------

DECLARE curCurrencyRate CURSOR FAST_FORWARD FOR
    SELECT effective_from,
           rate_against_base,
           currency_id
    FROM CurrencyRate
    ORDER BY effective_From, Currency_ID

OPEN curCurrencyRate
FETCH NEXT FROM curCurrencyRate
    INTO @CR_effective_from,
         @CR_rate_against_base,
         @CR_currency_id

WHILE @@FETCH_STATUS = 0
BEGIN

    -- Update all #TempRSAAgentAnal Rows affected by this Rate
    -- This may mean some rows are updated more than once
    -- e.g. rate exists from date1 and date2, claim is after date2
    -- cursor is sequenced by rate date, so claim is first updated with
    -- currency data for date1, then later for date2.

    UPDATE #TempRSAAgentAnal
        SET CurrencyRate = @CR_Rate_Against_Base
        WHERE ClaimCurrency_ID = @CR_Currency_ID
          AND ClaimDate >= @CR_Effective_From


    FETCH NEXT FROM curCurrencyRate
        INTO @CR_effective_from,
             @CR_rate_against_base,
             @CR_currency_id

END
CLOSE curCurrencyRate
DEALLOCATE curCurrencyRate


/* select for the report */
-- Records to select for the report
-- Including conversion of currencyRate
SELECT
            AgentCnt,
            Agent,
            Period,
            dtSelectedPeriodEnd,
            TransTypeID,
            TransType,
            RiskTypeCode,
            RiskTypeDescription,
            ProductCode,
            Product,
            NBPRemium,
            RenPremium,
            AllPremium,
            AllPremiumCompare,
            Commission,
        	Claim*CurrencyRate Claim,
            ClaimCompare*CurrencyRate ClaimCompare
FROM #TempRSAAgentAnal


-- select * from #TempRSAAgentAnal


DROP TABLE #tempRSAAgentAnal

DROP TABLE #ClaimReserve
DROP TABLE #ClaimRecovery
DROP TABLE #IncurredClaims

GO

-- End of $Workfile: sp_Report_Agent_Analysis.sql $

