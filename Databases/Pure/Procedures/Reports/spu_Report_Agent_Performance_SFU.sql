SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Agent_Performance_SFU'
GO

/**********************************************************************************************************************************
** Created by Jude Killip
** 31/08/2000
** RSA Reports - Agent_Performance.rpt
**  Created with dummy data to build the report
**********************************************************************************************************************************
** 11/11/2000   Jude Killip             real data
** 16/11/2000   Jude Killip             SQL error, remove offending "tmp" alias from updates
** 01/12/2000   Jude Killip             Add agent shortname (resolved name not always present)
**                                      Add dates to show selection criteria on report
**
** 07/12/2000   Jude Killip             bug 297 - stats_detail_type
**                                      year start calcs
**
** 12/12/2000   Jude Killip             date stuff
**
** 21/03/2001   Jude Killip             bug 334: amend dates, to stop date used twice
**                                      add claims and commission
**
** 01/05/2001   Jude Killip             base dates on This Period
**
** 12/06/2001   Jude Killip             Amend Period - use sf.posting_period_number
**
** 22/06/2001   Jude Killip             adjust the period selection to eliminate zeros
**                                      allow for very first period
**
** 04/07/2001   Jude Killip             filters for Claims/nonClaims details
**
** 21/09/2001   Jude Killip             Claims - Payments only
**
** 09/10/2001   Jude Killip             filter out failed export records
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     30/01/2002  JMK     Use new lookup parameter "Period" - user selects from list of
**                              ..current and previous period_end_dates (as a string)
**                              Add Agent parameter
***********************************************************************************************************************************
** 1.02     16/08/2002  TOMB    Fix Claim section, report on premium instead of sum insured
**
** 1.03     28/01/2003  JMK     Replace "" with ''
** 1.04     05/08/2003  AMB     Replace use of 'transaction_export_folder' with 'document' - TEF should not be used in SFU
** 1.05     15Jun2006   RC      Filter by Agent Group
***********************************************************************************************************************************/


CREATE PROCEDURE spu_Report_Agent_Performance_SFU
    @branch_id int,
    @PeriodDate varchar (20),
    @Agent varchar (20),
    @sBasis varchar(50),
	@AgentGroupCode Varchar(30)
AS
-- $Author: Jude.killip $
-- $Revision: 20 $
-- $Modtime: 28/01/03 16:18 $
-- $Workfile: sp_Report_Agent_Performance.sql $
-- $Logfile: /Sirius For Underwriting/Crystal Reports/Stored Procedures/sp_Report_Agent_Performance.sql $
-- $History: sp_Report_Agent_Performance.sql $
-- 
-- *****************  Version 20  *****************
-- User: Jude.killip  Date: 17/02/03   Time: 15:35
-- Updated in $/Sirius For Underwriting/Crystal Reports/Stored Procedures
-- Replace "" with ''
--
-- *****************  Version 19  *****************
-- User: Tom.brown    Date: 20/11/02   Time: 15:12
-- Updated in $/Sirius For Underwriting/Crystal Reports/Stored Procedures
-- Overhaul:  Standardise parameters as Branch, Period End Date, <others>
-- and Basis (Date/Period)


SET NOCOUNT ON
SET ANSI_NULLS OFF
DECLARE @DEBUG INT
SELECT @DEBUG = 0    -- 0 = OFF, 1 = ON

/*
-- for testing
DECLARE @branch_id int,
                    @PeriodDate varchar (20),
                    @Agent varchar (20),
                    @sBasis varchar(50)
SELECT  @branch_id = 0,
@PeriodDate = 'Dec 31 2002',
@Agent = 'HAMILTON FRASER',
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

SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @period_end_date = CONVERT (Datetime, @PeriodDate)

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

/*
-- calc Last Year YTD range
DECLARE @YTDRange int
SELECT @YTDRange = (@SelectedPeriodID - @YearStartPeriodID)
*/

CREATE TABLE #tmpAgentPerformance
(
        AgentCnt int,
        AgentShort varchar (30) NULL,
        Agent varchar (255) NULL,
        RecordType varchar (15),
        TransTypeID int NULL,
        RiskTypeID int NULL,
        RiskTypeCode varchar(10) NULL,
        StatsDetailType varchar(3) NULL,
        LY12 money NULL,
        TY12 money NULL,
        LYYTD money NULL,
        TYYTD money NULL,
        LYM money NULL,
        TYM money NULL,
        dtSelectedPeriodEnd datetime
)



IF @iBasis = 0  -- Transaction Period
BEGIN
    -- Premium Records by period
    INSERT #tmpAgentPerformance
    SELECT  sf.agent_cnt,
            NULL,
            NULL,
            'Premium',
            sf.transaction_type_id,
            sd.risk_type_id,
            sd.risk_type_code,
            sd.stats_detail_type,
            -- get last year 12 months To Date
            (Select sd.this_premium_home
            WHERE sf.posting_period_number BETWEEN @24PeriodsAgoID + 1  AND @12PeriodsAgoID
            ) LY12,
            -- get this year 12 months To Date
            (Select sd.this_premium_home
            WHERE sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID
            ) TY12,
            -- last year Year To Date
            (Select sd.this_premium_home
            WHERE sf.posting_period_number BETWEEN @YearBeforeStartID AND @12PeriodsAgoID
            ) LYYTD,
            -- this year Year To Date
            (Select sd.this_premium_home
            WHERE sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID
            ) TYYTD,
            -- Current Period last year
            (Select sd.this_premium_home
            WHERE sf.posting_period_number = @12PeriodsAgoID
            ) LYM,
            -- Current Period
            (Select sd.this_premium_home
            WHERE sf.posting_period_number = @SelectedPeriodID
            ) TYM,
            @dtSelectedPeriodEnd
            FROM  stats_detail sd
            JOIN stats_folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
            -- AMB 04/08/03 - replaced transaction_export_folder link with join to 'Document'
            INNER JOIN Document AS doc 
                ON doc.document_ref = sf.document_ref
            WHERE stats_detail_type = ('GRS')
            AND sf.transaction_type_code NOT LIKE ('C_%')           -- Not claims
            AND isnull(sf.agent_cnt,'') <> ''
            AND isnull(sd.this_premium_home,0) <> 0
            AND sf.posting_period_number BETWEEN @24PeriodsAgoID + 1 AND @SelectedPeriodID
            AND (
                @Agent = 'ALL'
                OR
                sf.agent_shortname = @Agent
                )
            AND ( @iBranchID = 0
                  or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
                )

    -- Commission Records by period
    INSERT #tmpAgentPerformance
    SELECT  sf.agent_cnt,
            NULL,
            NULL,
            'Commission',
            sf.transaction_type_id,
            sd.risk_type_id,
            sd.risk_type_code,
            sd.stats_detail_type,
            -- last year 12 months To Date
            (Select isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)
            WHERE sf.posting_period_number BETWEEN @24PeriodsAgoID + 1  AND @12PeriodsAgoID
            ) LY12,
            -- this year 12 months To Date
            (Select isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)
            WHERE sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID
            ) TY12,
            -- last year Year To Date
            (Select isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)
            WHERE sf.posting_period_number BETWEEN @YearBeforeStartID AND @12PeriodsAgoID
            ) LYYTD,
            -- this year Year To Date
            (Select isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)
            WHERE sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID
            ) TYYTD,
            -- Current Period last year
            (Select isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)
            WHERE sf.posting_period_number = @12PeriodsAgoID
            ) LYM,
            -- Current Period
            (Select isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)
            WHERE sf.posting_period_number = @SelectedPeriodID
            ) TYM,
            @dtSelectedPeriodEnd
            FROM  stats_detail sd
            JOIN stats_folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
            -- AMB 04/08/03 - replaced transaction_export_folder link with join to 'Document'
            INNER JOIN Document AS doc 
                ON doc.document_ref = sf.document_ref
            WHERE stats_detail_type = ('GRS')
            AND sf.transaction_type_code NOT LIKE ('C_%')           -- Not claims
            AND isnull(sf.agent_cnt,'') <> ''
            AND (isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)) <> 0
            AND sf.posting_period_number BETWEEN @24PeriodsAgoID + 1 AND @SelectedPeriodID
            AND (
                @Agent = 'ALL'
                OR
                sf.agent_shortname = @Agent
                )
            AND ( @iBranchID = 0
                  or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
                )

    -- Claims Records by period
    INSERT #tmpAgentPerformance
    SELECT  sf.agent_cnt,
            NULL,
            NULL,
            'Claims',
            sf.transaction_type_id,
            sd.risk_type_id,
            sd.risk_type_code,
            sd.stats_detail_type,
            -- get last year 12 months To Date
            (Select sd.this_premium_home
            WHERE sf.posting_period_number BETWEEN @24PeriodsAgoID + 1  AND @12PeriodsAgoID
            ) LY12,
            -- get this year 12 months To Date
            (Select  sd.this_premium_home
            WHERE sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID
            ) TY12,
            -- last year Year To Date
            (Select sd.this_premium_home
            WHERE sf.posting_period_number BETWEEN @YearBeforeStartID AND @12PeriodsAgoID
            ) LYYTD,
            -- this year Year To Date
            (Select sd.this_premium_home
            WHERE sf.posting_period_number BETWEEN @YearStartPeriodID AND @SelectedPeriodID
            ) TYYTD,
            -- Current Period last year
            (Select sd.this_premium_home
            WHERE sf.posting_period_number = @12PeriodsAgoID
            ) LYM,
            -- Current Period
            (Select sd.this_premium_home
            WHERE sf.posting_period_number = @SelectedPeriodID
            ) TYM,
            @dtSelectedPeriodEnd
            FROM  stats_detail sd
            JOIN stats_folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
            -- AMB 04/08/03 - replaced transaction_export_folder link with join to 'Document'
            INNER JOIN Document AS doc 
                ON doc.document_ref = sf.document_ref
            WHERE stats_detail_type = ('GRS')
            AND sf.transaction_type_code = ('C_CP')           -- claims payments
            AND isnull(sf.agent_cnt,'') <> ''
            AND isnull(sd.this_premium_home,0) <> 0
            AND sf.posting_period_number BETWEEN @24PeriodsAgoID + 1 AND @SelectedPeriodID
            AND (
                @Agent = 'ALL'
                OR
                sf.agent_shortname = @Agent
                )
            AND ( @iBranchID = 0
                  or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
                )

END    -- iBasis = 0 Transaction Period
ELSE
BEGIN    -- iBasis <> 0 Transaction Date
    -- Premium Records by date
    INSERT #tmpAgentPerformance
    SELECT  sf.agent_cnt,
            NULL,
            NULL,
            'Premium',
            sf.transaction_type_id,
            sd.risk_type_id,
            sd.risk_type_code,
            sd.stats_detail_type,
            -- get last year 12 months To Date
            (Select sd.this_premium_home
            WHERE ( sf.document_date > @dt24PeriodsAgo       AND sf.document_date <= @dt12PeriodsAgoPrev    )
            ) LY12,
            -- get this year 12 months To Date
            (Select sd.this_premium_home
            WHERE ( sf.document_date > @dt12PeriodsAgo       AND sf.document_date <= @period_end_date    )
            ) TY12,
            -- last year Year To Date
            (Select sd.this_premium_home
            WHERE ( sf.document_date > @dtYearBeforeStart    AND sf.document_date <= @dt12PeriodsAgoPrev )
            ) LYYTD,
            -- this year Year To Date
            (Select sd.this_premium_home
            WHERE ( sf.document_date > @dtYearStart          AND sf.document_Date <= @period_end_date    )
            ) TYYTD,
            -- Current Period last year
            (Select sd.this_premium_home
            WHERE ( sf.document_Date > @dt12PeriodsAgoPrev   AND sf.document_date <= @dt12PeriodsAgo     )
            ) LYM,
            -- Current Period
            (Select sd.this_premium_home
            WHERE ( sf.document_Date > @prev_period_end_date AND sf.document_Date <= @period_end_date    )
            ) TYM,
            @dtSelectedPeriodEnd
            FROM  stats_detail sd
            JOIN stats_folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
            -- AMB 04/08/03 - replaced transaction_export_folder link with join to 'Document'
            INNER JOIN Document AS doc 
                ON doc.document_ref = sf.document_ref
            WHERE stats_detail_type = ('GRS')
            AND sf.transaction_type_code NOT LIKE ('C_%')           -- Not claims
            AND isnull(sf.agent_cnt,'') <> ''
            AND isnull(sd.this_premium_home,0) <> 0
            AND ( sf.document_date > @dt24PeriodsAgo       AND sf.document_date <= @period_end_date )
            AND (
                @Agent = 'ALL'
                OR
                sf.agent_shortname = @Agent
                )

             AND ( @iBranchID = 0
                  or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
                )
   -- Commission Records by date
    INSERT #tmpAgentPerformance
    SELECT  sf.agent_cnt,
            NULL,
            NULL,
            'Commission',
            sf.transaction_type_id,
            sd.risk_type_id,
            sd.risk_type_code,
            sd.stats_detail_type,
            -- last year 12 months To Date
            (Select isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)
            WHERE ( sf.document_date > @dt24PeriodsAgo       AND sf.document_date <= @dt12PeriodsAgoPrev    )
            ) LY12,
            -- this year 12 months To Date
            (Select isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)
            WHERE ( sf.document_date > @dt12PeriodsAgo       AND sf.document_date <= @period_end_date    )
            ) TY12,
            -- last year Year To Date
            (Select isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)
            WHERE ( sf.document_date > @dtYearBeforeStart    AND sf.document_date <= @dt12PeriodsAgoPrev )
            ) LYYTD,
            -- this year Year To Date
            (Select isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)
            WHERE ( sf.document_date > @dtYearStart          AND sf.document_Date <= @period_end_date    )
            ) TYYTD,
            -- Current Period last year
            (Select isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)
            WHERE ( sf.document_Date > @dt12PeriodsAgoPrev   AND sf.document_date <= @dt12PeriodsAgo     )
            ) LYM,
            -- Current Period
            (Select isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)
            WHERE ( sf.document_Date > @prev_period_end_date AND sf.document_Date <= @period_end_date    )
            ) TYM,
            @dtSelectedPeriodEnd
            FROM  stats_detail sd
            JOIN stats_folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
            -- AMB 04/08/03 - replaced transaction_export_folder link with join to 'Document'
            INNER JOIN Document AS doc 
                ON doc.document_ref = sf.document_ref
            WHERE stats_detail_type = ('GRS')
            AND sf.transaction_type_code NOT LIKE ('C_%')           -- Not claims
            AND isnull(sf.agent_cnt,'') <> ''
            AND (isnull(sd.lead_commission_value_home,0) +
                            isnull(sd.sub_commission_value_home,0)) <> 0
            AND ( sf.document_date > @dt24PeriodsAgo       AND sf.document_date <= @period_end_date )
            AND (
                @Agent = 'ALL'
                OR
                sf.agent_shortname = @Agent
                )

            AND ( @iBranchID = 0
                  or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
                )
    -- Claims Records by date
    INSERT #tmpAgentPerformance
    SELECT  sf.agent_cnt,
            NULL,
            NULL,
            'Claims',
            sf.transaction_type_id,
            sd.risk_type_id,
            sd.risk_type_code,
            sd.stats_detail_type,
            -- get last year 12 months To Date
            (Select sd.this_premium_home
            WHERE ( sf.document_date > @dt24PeriodsAgo       AND sf.document_date <= @dt12PeriodsAgoPrev    )
            ) LY12,
            -- get this year 12 months To Date
            (Select  sd.this_premium_home
            WHERE ( sf.document_date > @dt12PeriodsAgo       AND sf.document_date <= @period_end_date    )
            ) TY12,
            -- last year Year To Date
            (Select sd.this_premium_home
            WHERE ( sf.document_date > @dtYearBeforeStart    AND sf.document_date <= @dt12PeriodsAgoPrev )
            ) LYYTD,
            -- this year Year To Date
            (Select sd.this_premium_home
            WHERE ( sf.document_date > @dtYearStart          AND sf.document_Date <= @period_end_date    )
            ) TYYTD,
            -- Current Period last year
            (Select sd.this_premium_home
            WHERE ( sf.document_Date > @dt12PeriodsAgoPrev   AND sf.document_date <= @dt12PeriodsAgo     )
            ) LYM,
            -- Current Period
            (Select sd.this_premium_home
            WHERE ( sf.document_Date > @prev_period_end_date AND sf.document_Date <= @period_end_date    )
            ) TYM,
            @dtSelectedPeriodEnd
            FROM  stats_detail sd
            JOIN stats_folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
            -- AMB 04/08/03 - replaced transaction_export_folder link with join to 'Document'
            INNER JOIN Document AS doc 
                ON doc.document_ref = sf.document_ref
            WHERE stats_detail_type = ('GRS')
            AND sf.transaction_type_code = ('C_CP')           -- claims payments
            AND isnull(sf.agent_cnt,'') <> ''
            AND isnull(sd.this_premium_home,0) <> 0
            AND ( sf.document_date > @dt24PeriodsAgo       AND sf.document_date <= @period_end_date )
            AND (
                @Agent = 'ALL'
                OR
                sf.agent_shortname = @Agent
                )

            AND ( @iBranchID = 0
                  or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
                )
END    -- iBasis <> 0 Transaction Date


-- add Agent Resolved Name
UPDATE  #tmpAgentPerformance
SET     Agent = p.resolved_name, AgentShort = p.shortname
FROM    Party p
WHERE   AgentCnt = p.party_cnt

-- get data back from temporary table

IF LOWER(@AgentGroupCode) = 'all'
BEGIN
 PRINT 'ENTER1.1'

	SELECT * FROM #tmpAgentPerformance
	WHERE (isnull(LY12 ,0) <> 0
	        OR isnull(TY12 ,0) <> 0
	        OR isnull(LYYTD ,0) <> 0
	        OR isnull(TYYTD ,0) <> 0
	        OR isnull(LYM ,0) <> 0
	        OR isnull(TYM ,0) <> 0
	        )
END

IF LOWER(@AgentGroupCode) <> 'all'
BEGIN
 PRINT 'ENTER2.1'
	SELECT * FROM #tmpAgentPerformance
	WHERE (isnull(LY12 ,0) <> 0
	        OR isnull(TY12 ,0) <> 0
	        OR isnull(LYYTD ,0) <> 0
	        OR isnull(TYYTD ,0) <> 0
	        OR isnull(LYM ,0) <> 0
	        OR isnull(TYM ,0) <> 0
	        )
	--RC-- 15 Jun 2006
	AND Agent IN(
	select trading_name from party_agent where linked_account_group = (
	select  party_cnt from party where shortname = @AgentGroupCode) )
	--RC-- 15 Jun 2006
	--AgentShort
END


-- delete temporary table
DROP TABLE #tmpAgentPerformance

GO

-- End of $Workfile: sp_Report_Agent_Performance.sql $
