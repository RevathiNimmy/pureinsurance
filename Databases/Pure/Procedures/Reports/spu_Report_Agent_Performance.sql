SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Agent_Performance'
GO


CREATE PROCEDURE spu_Report_Agent_Performance
    @PeriodDate varchar(255),
    @Agent varchar(255),
    @source_id int
AS

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
**
** 06/08/2002   Peter Finney            Remove horrendous Period_ID usage, sub-queries and temporary tables
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     30/01/2002  JMK     Use new lookup parameter "Period" - user selects from list of
**                              ..current and previous period_end_dates (as a string)
**                              Add Agent parameter
***********************************************************************************************************************************/

    DECLARE 
        @sub_branch_id int,
        @period_id int,
        @working_period_id int,
        @dtSelectedPeriodEnd datetime,
        @dtYearPeriodStart datetime,
        @dt11MonthPeriodEnd datetime,
        @dt12MonthPeriodEnd datetime,
        @dt23MonthPeriodEnd datetime,
        @dt24MonthPeriodEnd datetime,
        @dtYTDPeriodEnd datetime,
        @dtLYYTDPeriodEnd datetime
    
    -- get default sub-branch for supplied source_id
    EXEC spu_sub_branch_default 
        @source_id, 
        @sub_branch_id OUTPUT
    
    -- which period do we want to base this report on?
    SELECT @dtSelectedPeriodEnd = CONVERT(datetime, @PeriodDate + ' 23:59:59.000')
   
    SELECT @period_id = period_id
    FROM   period
    WHERE  period_end_date = @dtSelectedPeriodEnd
    AND    sub_branch_id = @sub_branch_id
    
    -- get the year start date
    SELECT @working_period_id= NULL 
    EXECUTE spu_Report_GetYearStartID 
        @sub_branch_id, 
        @period_id, 
        @working_period_id OUTPUT
    
    SELECT @dtYearPeriodStart = period_end_date
    FROM   period
    WHERE  period_id = @working_period_id
    
    -- get the first date after the 12 period date
    SELECT @working_period_id= NULL 
    EXECUTE spu_Report_GetCurrent12MonthPeriod 
        @sub_branch_id, 
        @working_period_id OUTPUT, 
        @dt12MonthPeriodEnd OUTPUT

    IF ISNULL(@dt12MonthPeriodEnd, 0) = 0 
    BEGIN
        SELECT @dt11MonthPeriodEnd = CONVERT(datetime, -1)
        SELECT @dt12MonthPeriodEnd = CONVERT(datetime, -1)
    END
    ELSE
        SELECT @dt11MonthPeriodEnd = MIN(period_end_date)
        FROM   period
        WHERE  period_end_date > @dt12MonthPeriodEnd
        AND    sub_branch_id = @sub_branch_id
    
    -- get the first date after the 24 period date
    SELECT @working_period_id= NULL 
    EXECUTE spu_Report_GetCurrent24MonthPeriod 
        @sub_branch_id, 
        @working_period_id OUTPUT, 
        @dt24MonthPeriodEnd OUTPUT

    IF ISNULL(@dt24MonthPeriodEnd, 0) = 0 
    BEGIN
        SELECT @dt23MonthPeriodEnd = CONVERT(datetime, -1)
        SELECT @dt24MonthPeriodEnd = CONVERT(datetime, -1)
    END
    ELSE
        SELECT @dt23MonthPeriodEnd = MIN(period_end_date)
        FROM   period
        WHERE  period_end_date > @dt24MonthPeriodEnd
        AND    sub_branch_id = @sub_branch_id

    -- calc Last Year YTD range
    SELECT @dtYTDPeriodEnd = (@dtSelectedPeriodEnd - @dtYearPeriodStart)
    SELECT @dtLYYTDPeriodEnd = (@dt12MonthPeriodEnd - @dtYTDPeriodEnd)

 
    -- The big query (in one)
    SELECT  AgentCnt        = sf.agent_cnt,
            AgentShort      = pt.shortname,
            Agent           = pt.resolved_name, 
            RecordType      = 'Premium',
            TransTypeID     = sf.transaction_type_id,
            RiskTypeID      = sd.risk_type_id,
            RiskTypeCode    = sd.risk_type_code,
            StatsDetailType = sd.stats_detail_type,
            -- get last year 12 months To Date
            LY12  = CASE WHEN p.period_end_date BETWEEN @dt23MonthPeriodEnd AND @dt12MonthPeriodEnd
                         THEN sd.this_premium_home 
                         ELSE NULL END,
            -- get this year 12 months To Date
            TY12  = CASE WHEN p.period_end_date BETWEEN @dt11MonthPeriodEnd AND @dtSelectedPeriodEnd
                         THEN sd.this_premium_home 
                         ELSE NULL END,
            -- last year Year To Date
            LYYTD = CASE WHEN p.period_end_date BETWEEN @dtLYYTDPeriodEnd AND @dt12MonthPeriodEnd
                         THEN sd.this_premium_home 
                         ELSE NULL END,
            -- this year Year To Date
            TYYTD = CASE WHEN p.period_end_date BETWEEN @dtYearPeriodStart AND @dtSelectedPeriodEnd
                         THEN sd.this_premium_home 
                         ELSE NULL END,
            -- Current Period last year
            LYM   = CASE WHEN p.period_end_date = @dt12MonthPeriodEnd
                         THEN sd.this_premium_home 
                         ELSE NULL END,
            -- Current Period
            TYM   = CASE WHEN p.period_end_date = @dtSelectedPeriodEnd
                         THEN sd.this_premium_home 
                         ELSE NULL END,
            dtSelectedPeriodEnd = @dtSelectedPeriodEnd,
            stats_detail_id = sd.stats_detail_id
    FROM    stats_detail sd
    JOIN    stats_folder sf ON sf.stats_folder_cnt = sd.stats_folder_cnt 
    JOIN    period p ON p.period_id = sf.posting_period_number
    JOIN    party pt ON pt.party_cnt = sf.agent_cnt
    WHERE   isnull(sd.this_premium_home, 0) <> 0                -- Check premium
    AND     sd.stats_detail_type = 'GRS'                        -- Check type
    AND    (sf.agent_shortname = @Agent OR @Agent = 'ALL')      -- Check agent name
    AND     sf.transaction_type_code NOT LIKE 'C_%'             -- Not claims
    AND     p.period_end_date BETWEEN @dt23MonthPeriodEnd AND @dtSelectedPeriodEnd
    AND     p.sub_branch_id = @sub_branch_id

    UNION

    SELECT  AgentCnt        = sf.agent_cnt,
            AgentShort      = pt.shortname,
            Agent           = pt.resolved_name, 
            RecordType      = 'Commission',
            TransTypeID     = sf.transaction_type_id,
            RiskTypeID      = sd.risk_type_id,
            RiskTypeCode    = sd.risk_type_code,
            StatsDetailType = sd.stats_detail_type,
            -- get last year 12 months To Date
            LY12  = CASE WHEN p.period_end_date BETWEEN @dt23MonthPeriodEnd AND @dt12MonthPeriodEnd
                         THEN ISNULL(sd.lead_commission_value_home, 0) + ISNULL(sd.sub_commission_value_home, 0)
                         ELSE NULL END,
            -- get this year 12 months To Date
            TY12  = CASE WHEN p.period_end_date BETWEEN @dt11MonthPeriodEnd AND @dtSelectedPeriodEnd
                         THEN ISNULL(sd.lead_commission_value_home, 0) + ISNULL(sd.sub_commission_value_home, 0)
                         ELSE NULL END,
            -- last year Year To Date
            LYYTD = CASE WHEN p.period_end_date BETWEEN @dtLYYTDPeriodEnd AND @dt12MonthPeriodEnd
                         THEN ISNULL(sd.lead_commission_value_home, 0) + ISNULL(sd.sub_commission_value_home, 0)
                         ELSE NULL END,
            -- this year Year To Date
            TYYTD = CASE WHEN p.period_end_date BETWEEN @dtYearPeriodStart AND @dtSelectedPeriodEnd
                         THEN ISNULL(sd.lead_commission_value_home, 0) + ISNULL(sd.sub_commission_value_home, 0)
                         ELSE NULL END,
            -- Current Period last year
            LYM   = CASE WHEN p.period_end_date = @dt12MonthPeriodEnd
                         THEN ISNULL(sd.lead_commission_value_home, 0) + ISNULL(sd.sub_commission_value_home, 0)
                         ELSE NULL END,
            -- Current Period
            TYM   = CASE WHEN p.period_end_date = @dtSelectedPeriodEnd
                         THEN ISNULL(sd.lead_commission_value_home, 0) + ISNULL(sd.sub_commission_value_home, 0)
                         ELSE NULL END,
            dtSelectedPeriodEnd = @dtSelectedPeriodEnd,
            stats_detail_id = sd.stats_detail_id
    FROM    stats_detail sd
    JOIN    stats_folder sf ON sf.stats_folder_cnt = sd.stats_folder_cnt 
    JOIN    period p ON p.period_id = sf.posting_period_number
    JOIN    party pt ON pt.party_cnt = sf.agent_cnt
    WHERE   ISNULL(sd.lead_commission_value_home, 0) + ISNULL(sd.sub_commission_value_home, 0) <> 0 -- Check premium
    AND     sd.stats_detail_type = 'GRS'                        -- Check type
    AND    (sf.agent_shortname = @Agent OR @Agent = 'ALL')      -- Check agent name
    AND     sf.transaction_type_code NOT LIKE 'C_%'             -- Not claims
    AND     p.period_end_date BETWEEN @dt23MonthPeriodEnd AND @dtSelectedPeriodEnd
    AND     p.sub_branch_id = @sub_branch_id

    UNION

    SELECT  AgentCnt        = sf.agent_cnt,
            AgentShort      = pt.shortname,
            Agent           = pt.resolved_name, 
            RecordType      = 'Claims',
            TransTypeID     = sf.transaction_type_id,
            RiskTypeID      = sd.risk_type_id,
            RiskTypeCode    = sd.risk_type_code,
            StatsDetailType = sd.stats_detail_type,
            -- get last year 12 months To Date
            LY12  = CASE WHEN p.period_end_date BETWEEN @dt23MonthPeriodEnd AND @dt12MonthPeriodEnd
                         THEN sd.this_premium_home 
                         ELSE NULL END,
            -- get this year 12 months To Date
            TY12  = CASE WHEN p.period_end_date BETWEEN @dt11MonthPeriodEnd AND @dtSelectedPeriodEnd
                         THEN sd.this_premium_home 
                         ELSE NULL END,
            -- last year Year To Date
            LYYTD = CASE WHEN p.period_end_date BETWEEN @dtLYYTDPeriodEnd AND @dt12MonthPeriodEnd
                         THEN sd.this_premium_home 
                         ELSE NULL END,
            -- this year Year To Date
            TYYTD = CASE WHEN p.period_end_date BETWEEN @dtYearPeriodStart AND @dtSelectedPeriodEnd
                         THEN sd.this_premium_home 
                         ELSE NULL END,
            -- Current Period last year
            LYM   = CASE WHEN p.period_end_date = @dt12MonthPeriodEnd
                         THEN sd.this_premium_home 
                         ELSE NULL END,
            -- Current Period
            TYM   = CASE WHEN p.period_end_date = @dtSelectedPeriodEnd
                         THEN sd.this_premium_home 
                         ELSE NULL END,
            dtSelectedPeriodEnd = @dtSelectedPeriodEnd,
            stats_detail_id = sd.stats_detail_id
    FROM    stats_detail sd
    JOIN    stats_folder sf ON sf.stats_folder_cnt = sd.stats_folder_cnt 
    JOIN    period p ON p.period_id = sf.posting_period_number
    JOIN    party pt ON pt.party_cnt = sf.agent_cnt
    WHERE   isnull(sd.this_premium_home, 0) <> 0                -- Check premium
    AND     sd.stats_detail_type = 'GRS'                        -- Check type
    AND    (sf.agent_shortname = @Agent OR @Agent = 'ALL')      -- Check agent name
    AND     sf.transaction_type_code = 'C_CP'                   -- Not claims
    AND     p.period_end_date BETWEEN @dt23MonthPeriodEnd AND @dtSelectedPeriodEnd
    AND     p.sub_branch_id = @sub_branch_id



GO
    
