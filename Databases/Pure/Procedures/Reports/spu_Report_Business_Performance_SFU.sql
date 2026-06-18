
EXECUTE DDLDropProcedure 'spu_Report_Business_Performance_SFU'
GO
/**********************************************************************************************************************************
** Created by Jude Killip
** 23/08/2000
** RSA Reports - Business_Performance.rpt
**  Created with dummy data to build the report
**********************************************************************************************************************************
** 10/11/2000   Thinh Nguyen            real data
**              Jude Killip             add transaction_type_id and description
**                                              remove sum and group by, split LY/TY using subqueries instead
**
** 16/11/2000   Jude Killip             SQL error, remove offending "tmp" alias from updates
**
** 24/11/2000   Jude Killip             SQL error, DB change Stats_Detail.stats_detail_type change from char (1) to (3)
**
** 08/12/2000   Jude Killip             Add date variables.
**
** 12/12/2000   Jude Killip             Amend dodgy dates
**
** 01/05/2001   Jude Killip             Base dates on Current Period; Split Premium and Commission ** Claims to be done
**
** 12/06/2001   Jude Killip             Amend Period - use sf.posting_period_number
**
** 22/06/2001   Jude Killip             adjust the period selection to eliminate zeros
**                                      allow for very first period
**                                      set 'NET','TTY','TAF','TAC','TAN','TAT' record values * -1
**
** 04/07/2001   Jude Killip             filters for Claims/nonClaims details
**
** 17/09/2001   Jude Killip             Where were the claims? Include them
**                                      'FAC' * -1
**
** 18/09/2001   Jude Killip             Limit to ('GRS', 'COI','NET','TTY', 'FAC') records
**                                      rename 'NET' = 'RET'
** 28/05/2003   JOn Kemp		added branch parameter to report
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     31/01/2002  JMK     Use new lookup parameter "Period" - user selects from list of
**                              ..current and previous period_end_dates (as a string)
***********************************************************************************************************************************/
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_Report_Business_Performance_SFU
                    @PeriodDate varchar (20),
		    @branch_id int
AS
SET NOCOUNT ON
declare @ibranchid int
IF @branch_id IS NULL
		SELECT @iBranchID = 0
	ELSE
		SELECT @iBranchID = @branch_id
/*
-- for testing
DECLARE  @PeriodDate varchar (20)
SELECT @PeriodDate = 'Dec 31 2001'
*/
-- which period do we want to base this report on?
DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime

SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @dtSelectedPeriodEnd = CONVERT (Datetime, @PeriodDate)

SELECT @SelectedPeriodID = period_id
FROM Period
WHERE period_end_date = @dtSelectedPeriodEnd

-- get the year start period ID
DECLARE @YearStartPeriodID int
EXECUTE spu_Report_GetYearStartID_SFU @SelectedPeriodID, @YearStartPeriodID OUTPUT

-- get the 12 period ID
DECLARE @12PeriodsAgoID int
SELECT @12PeriodsAgoID = @SelectedPeriodID - 12
IF isnull(@12PeriodsAgoID,0) <=0 SELECT @12PeriodsAgoID = 0

-- get the 24 period ID
DECLARE @24PeriodsAgoID int
SELECT @24PeriodsAgoID = @SelectedPeriodID - 24
IF isnull(@24PeriodsAgoID ,0) <=0 SELECT @24PeriodsAgoID = 0

-- calc Last Year YTD range
DECLARE @YTDRange int
SELECT @YTDRange = (@SelectedPeriodID - @YearStartPeriodID)

CREATE TABLE #tmpBusinessPerformance
(
        RecordType varchar (10) NULL,
        TransTypeID int NULL,
        TransTypeDescription varchar (255) NULL,
        RiskTypeID int NULL,
        RiskTypeDesc varchar(255) NULL,
        StatsDetailType varchar(3) NULL,
        LY12 money NULL,
        TY12 money NULL,
        LYYTD money NULL,
        TYYTD money NULL,
        LYM money NULL,
        TYM money NULL,
        dtSelectedPeriodEnd datetime
)
--print 'Premium'
INSERT #tmpBusinessPerformance
SELECT  'Premium',
        sf.transaction_type_id,
        NULL,
        sd.risk_type_id,
        Null,
        sd.stats_detail_type,
        -- get last year 12 months To Date
        (Select sd.this_premium_home
        WHERE sf.posting_period_number BETWEEN @24PeriodsAgoID + 1 AND @12PeriodsAgoID
        ) LY12,
        -- get this year 12 months To Date
        (Select sd.this_premium_home
        WHERE sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID
        ) TY12,
        -- last year Year To Date
        (Select sd.this_premium_home
        WHERE sf.posting_period_number BETWEEN @12PeriodsAgoID - @YTDRange AND @12PeriodsAgoID
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
        FROM    stats_detail sd
        JOIN stats_folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
        WHERE isnull(sd.this_premium_home,0) <> 0
        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
        AND sf.posting_period_number BETWEEN @24PeriodsAgoID + 1 AND @SelectedPeriodID
        AND sd.stats_detail_type IN ('GRS', 'COI','NET','TTY', 'FAC')
        AND sf.posting_period_number > @24PeriodsAgoID
AND ( @iBranchID= 0
              or    (   @iBranchID <> 0 and sf.branch_id = @iBranchID ))

--print 'Commission'
INSERT #tmpBusinessPerformance
SELECT  'Commission',
        sf.transaction_type_id,
        NULL,
        sd.risk_type_id,
        Null,
        sd.stats_detail_type,
        -- last year 12 months To Date
        (Select isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)
        WHERE sf.posting_period_number BETWEEN @24PeriodsAgoID + 1 AND @12PeriodsAgoID
        ) LY12,
        -- this year 12 months To Date
        (Select isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)
        WHERE sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID
        ) TY12,
        -- last year Year To Date
        (Select isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)
        WHERE sf.posting_period_number BETWEEN @12PeriodsAgoID - @YTDRange AND @12PeriodsAgoID
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
        FROM    stats_detail sd
        JOIN stats_folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
        WHERE (isnull(sd.lead_commission_value_home,0) +
                        isnull(sd.sub_commission_value_home,0)) <> 0
        AND sf.transaction_type_code NOT LIKE ('C_%')           -- all but claims
        AND sf.posting_period_number BETWEEN @24PeriodsAgoID + 1 AND @SelectedPeriodID
        AND sd.stats_detail_type IN ('GRS', 'COI','NET','TTY', 'FAC')
        AND sf.posting_period_number > @24PeriodsAgoID
AND ( @iBranchID= 0
              or    (   @iBranchID <> 0 and sf.branch_id = @iBranchID ))
--print 'Claims'

INSERT #tmpBusinessPerformance
SELECT  'Claims',
        t.transaction_type_id,
        NULL,
        sd.risk_type_id,
        Null,
        sd.stats_detail_type,
        -- get last year 12 months To Date
        (Select sd.this_premium_home
        WHERE sf.posting_period_number BETWEEN @24PeriodsAgoID + 1 AND @12PeriodsAgoID
        ) LY12,
        -- get this year 12 months To Date
        (Select sd.this_premium_home
        WHERE sf.posting_period_number BETWEEN @12PeriodsAgoID + 1 AND @SelectedPeriodID
        ) TY12,
        -- last year Year To Date
        (Select sd.this_premium_home
        WHERE sf.posting_period_number BETWEEN @12PeriodsAgoID - @YTDRange AND @12PeriodsAgoID
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
        FROM    stats_detail sd
        JOIN stats_folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
		JOIN Transaction_Type t ON t.code=sf.transaction_type_code
        WHERE isnull(sd.this_premium_home,0) <> 0
        AND sf.transaction_type_code LIKE ('C_%')           -- Claims
        AND sf.posting_period_number BETWEEN @24PeriodsAgoID + 1 AND @SelectedPeriodID
        AND sd.stats_detail_type IN ('GRS', 'COI','NET','TTY', 'FAC')
        AND sf.posting_period_number > @24PeriodsAgoID
AND ( @iBranchID= 0
              or    (   @iBranchID <> 0 and sf.branch_id = @iBranchID ))
--print 'delete nulls from temp table'
DELETE FROM #tmpBusinessPerformance
WHERE (isnull(LY12, 0) <> 0
        AND isnull(TY12, 0) <> 0
        AND isnull(LYYTD, 0) <> 0
        AND isnull(TYYTD, 0) <> 0
        AND isnull(LYM, 0) <> 0
        AND isnull(TYM, 0) <> 0
        )

--print 'change signs'
UPDATE #tmpBusinessPerformance
    SET LY12 = LY12 * -1,
        TY12 = TY12 * -1,
        LYYTD = LYYTD * -1,
        TYYTD = TYYTD * -1,
        LYM = LYM * -1,
        TYM = TYM * -1
    WHERE StatsDetailType IN ('NET','TTY', 'FAC')
    AND RecordType <> 'Claims'

--print 'rename NET
UPDATE #tmpBusinessPerformance
    SET StatsDetailType = 'RET'
    WHERE StatsDetailType = 'NET'

--print 'get transaction type description'
UPDATE  #tmpBusinessPerformance
SET     TransTypeDescription = tt.description
FROM    Transaction_Type tt
WHERE   TransTypeID = tt.transaction_type_id

--print 'get risk type description'
UPDATE  #tmpBusinessPerformance
SET     RiskTypeDesc = rt.description
FROM    Risk_Type rt
WHERE   RiskTypeID = rt.risk_type_id

-- get data back from temporary table
SELECT * FROM #tmpBusinessPerformance

SET NOCOUNT OFF
-- delete temporary table
DROP TABLE #tmpBusinessPerformance
GO