SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Renewals_And_Lapses_by_Agent'
GO


CREATE PROCEDURE spu_Report_Renewals_And_Lapses_by_Agent
                @AgentName varchar(255),
                @Period varchar(255),
                @Source_id int
AS
/**********************************************************************************************************************************
** Created by Jude Killip
** 03/12/2001
** Reports - Renewals_And_Lapses_by_Agent.rpt
** 1.00
**
** Running totals are performed here rather than in the report so that Cumulative totals can appear alonside Month totals
** (avoiding the use of Crystal7 running totals)
**
** @Period: Current Period or Prior Period
**********************************************************************************************************************************
** 1.01 14/12/2001  JMK     rsa_transfer! change to "broking"
**
** 1.02 14/12/2001  JMK     initialise running total variables to 0 (zero) at start of cursor 2
**
** 1.03 14/12/2001  JMK     add parameter
**
** 1.04 17/12/2001  JMK     add period parameter
**                          get values from Stats
** 1.05 05/08/2002  CMG/PB  Add source_id parameter to give filtering by Sub Branch
***********************************************************************************************************************************/
SET NOCOUNT ON
/*
-- test
DECLARE @AgentName varchar (100), @Period varchar (20)
SELECT @AgentName = 'ALL', @Period = 'Current Period'
*/

DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime

-- get party_cnt from @Agent (resolved_name)
DECLARE @AgentID int
IF @AgentName <> 'ALL'
    BEGIN
        SELECT @AgentID = party_cnt
        FROM Party
        WHERE resolved_name = @AgentName
    END

-- get default sub-branch for supplied source_id
DECLARE @sub_branch_id int
EXEC spu_sub_branch_default @source_id, @sub_branch_id OUTPUT

-- get current period values
DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod @sub_branch_id, @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT


-- find out whether Current or Prior Period has been selected
IF @Period = 'Prior Period'
    BEGIN
        SELECT @SelectedPeriodID = @CurrentPeriodID -1
    END
    ELSE
    BEGIN
        SELECT @SelectedPeriodID = @CurrentPeriodID
    END

CREATE TABLE #tmp12PeriodDates
(
    dateID int IDENTITY,
    dtPeriodEnd datetime NULL
)

--print 'get current and previous 12 period end dates'
-- based on selected Period
INSERT INTO #tmp12PeriodDates
    SELECT top 13 period_end_date
    FROM period
    WHERE period_id <= @SelectedPeriodID
    AND sub_branch_id = @sub_branch_id
    ORDER BY period_end_date DESC
/*
print 'test dates'
SELECT * FROM #tmp12PeriodDates
*/

-- get the end date of the Period selected
SELECT @dtSelectedPeriodEnd = dtPeriodEnd
FROM #tmp12PeriodDates
WHERE dateID = 1

CREATE TABLE #tmpRnwlsAndLapses
(
    Agent varchar (100) NULL,
    RenewalDate datetime NULL,
    MonthID int NULL,
    StatsFolder int NULL,
    RenValue money NULL,
    LapValue money NULL,
    InvValue money NULL
)

-- Insert Renewals
INSERT INTO #tmpRnwlsAndLapses
    SELECT (SELECT resolved_name FROM Party where party_cnt = sf.agent_cnt),
        sf.cover_start_date,
        NULL,
        sf.stats_folder_cnt,
        sd.this_premium_home,
        NULL,
        NULL
    FROM stats_folder sf
    JOIN stats_detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
    WHERE sf.transaction_type_code = 'REN'
    AND sd.stats_detail_type = 'GRS'
    AND sf.cover_start_date > (SELECT dtPeriodEnd FROM #tmp12PeriodDates WHERE dateID = 13)
    AND sf.cover_start_date <= (SELECT dtPeriodEnd FROM #tmp12PeriodDates WHERE dateID = 1)
    AND (sf.agent_cnt = @AgentID
        OR @AgentName = 'ALL')

-- Insert Lapses
INSERT INTO #tmpRnwlsAndLapses
    SELECT (SELECT resolved_name FROM Party where party_cnt = sf.agent_cnt),
        sf.expiry_date,
        NULL,
        sf.stats_folder_cnt,
        NULL,
        sd.this_premium_home,
        NULL
    FROM stats_folder sf
    JOIN stats_detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
    WHERE sf.transaction_type_code = 'NB'
    AND sd.stats_detail_type = 'GRS'
    AND sf.insurance_file_cnt IN                -- Get the latest live policy (status = null)
                                                -- linked to a folder with a lapsed policy (status = 2)
        (
            SELECT max(insurance_file_cnt)
            FROM insurance_file
            WHERE insurance_folder_cnt IN
                (
                    SELECT insurance_folder_cnt
                    FROM insurance_file
                    WHERE insurance_file_status_id = 2
                )
            AND insurance_file_status_id is NULL
            GROUP BY insurance_folder_cnt
        )
    AND (sf.agent_cnt = @AgentID
        OR @AgentName = 'ALL')
    AND sf.expiry_date > (SELECT dtPeriodEnd FROM #tmp12PeriodDates WHERE dateID = 13)
    AND sf.expiry_date <= (SELECT dtPeriodEnd FROM #tmp12PeriodDates WHERE dateID = 1)

-- Insert Not Invited
INSERT INTO #tmpRnwlsAndLapses
    SELECT (SELECT resolved_name FROM Party where party_cnt = sf.agent_cnt),
        sf.expiry_date,
        NULL,
        sf.stats_folder_cnt,
        NULL,
        NULL,
        sd.this_premium_home
    FROM stats_folder sf
    JOIN stats_detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
    WHERE sf.transaction_type_code = 'NB'
    AND sd.stats_detail_type = 'GRS'
    AND sf.insurance_file_cnt IN                -- Get policies with a stop code
        (
            SELECT insurance_file_cnt
            FROM insurance_file
            WHERE isnull(renewal_stop_code_id,0) <> 0
        )
    AND (sf.agent_cnt = @AgentID
        OR @AgentName = 'ALL')
    AND sf.expiry_date > (SELECT dtPeriodEnd FROM #tmp12PeriodDates WHERE dateID = 13)
    AND sf.expiry_date <= (SELECT dtPeriodEnd FROM #tmp12PeriodDates WHERE dateID = 1)


-- CURSOR 1 START ***************************************************************************
-- Sort premium into date periods and renewal types

-- Cursor variables
DECLARE @RenewalDate datetime,
    @MonthID int

DECLARE RandL_cursor CURSOR FAST_FORWARD FOR
    SELECT RenewalDate, MonthID FROM #tmpRnwlsAndLapses

OPEN    RandL_cursor

    FETCH NEXT FROM RandL_cursor
    INTO    @RenewalDate,
            @MonthID

    WHILE @@FETCH_STATUS = 0 BEGIN
        -- find out which relative month renewal date falls in
        SELECT @MonthID = max(dateID)
            FROM #tmp12PeriodDates
            WHERE @RenewalDate <= dtPeriodEnd

        UPDATE #tmpRnwlsAndLapses
           SET MonthID = @MonthID

        FETCH NEXT FROM RandL_cursor
        INTO    @RenewalDate,
                @MonthID

    END

CLOSE RandL_cursor
DEALLOCATE RandL_cursor
-- CURSOR 1 END ***************************************************************************
/*
print 'test #tmpRnwlsAndLapses'
SELECT * from #tmpRnwlsAndLapses
*/
-- Temp Table for output
CREATE TABLE #tmpRnwlsAndLapsesSum
(
    Agent varchar (100) NULL,
    MonthID int,
    MonthName varchar (20),
    CurrentPeriodEnd datetime NULL,
    RenMonthCount int NULL,
    RenMonthValue money NULL,
    LapMonthCount int NULL,
    LapMonthValue money NULL,
    InvMonthCount int NULL,
    InvMonthValue money NULL,
    CRenMonthCount int NULL,
    CRenMonthValue money NULL,
    CLapMonthCount int NULL,
    CLapMonthValue money NULL,
    CInvMonthCount int NULL,
    CInvMonthValue money NULL
)
-- CURSOR 2 START ***************************************************************************
-- Get Monthly and Cumulative value totals and counts per Agent

-- Cursor variables
DECLARE @Agent2 varchar (100),
    @MonthID2 int,
    @StatsFolder int,
    @RenValue2 money,
    @LapValue2 money,
    @InvValue2 money
-- Additional variables
DECLARE @PrevAgent varchar (100),
    @PrevStatsFolder int,
    @PrevMonthID int
-- Output variables
DECLARE @MonthName varchar (20),
    @RenMonthCount int,
    @RenMonthValue money,
    @LapMonthCount int,
    @LapMonthValue money,
    @InvMonthCount int,
    @InvMonthValue money,
    @CRenMonthCount int,
    @CRenMonthValue money,
    @CLapMonthCount int,
    @CLapMonthValue money,
    @CInvMonthCount int,
    @CInvMonthValue money

DECLARE RandLSum_cursor CURSOR FAST_FORWARD FOR
    SELECT isnull(Agent,' Direct'),     -- If Agent is null, then Direct Client
            MonthID,
            StatsFolder,
            isnull(RenValue,0),
            isnull(LapValue,0),
            isnull(InvValue,0)
    FROM #tmpRnwlsAndLapses
    ORDER BY Agent, MonthID, StatsFolder DESC        -- Ordered for the running totals

OPEN    RandLSum_cursor

    FETCH NEXT FROM RandLSum_cursor
    INTO    @Agent2,
            @MonthID2,
            @StatsFolder,
            @RenValue2,
            @LapValue2,
            @InvValue2

    -- Initialise Month and Agent change variables
    SELECT @PrevMonthID = @MonthID2, @PrevStatsFolder = @StatsFolder, @PrevAgent = @Agent2

    -- Initialise cumulative running totals
    SELECT @CRenMonthCount = 0,
        @CRenMonthValue = 0,
        @CLapMonthCount = 0,
        @CLapMonthValue = 0,
        @CInvMonthCount = 0,
        @CInvMonthValue = 0

    -- Initialise month totals
    SELECT @RenMonthCount = 0,
        @RenMonthValue = 0,
        @LapMonthCount = 0,
        @LapMonthValue = 0,
        @InvMonthCount = 0,
        @InvMonthValue = 0

    WHILE @@FETCH_STATUS = 0 BEGIN
        SELECT  @RenMonthValue = @RenMonthValue + @RenValue2,                   -- values per month
                @LapMonthValue = @LapMonthValue + @LapValue2,
                @InvMonthValue = @InvMonthValue + @InvValue2,
                @CRenMonthValue = @CRenMonthValue + @RenValue2,                 -- Cumulative values per month
                @CLapMonthValue = @CLapMonthValue + @LapValue2,
                @CInvMonthValue = @CInvMonthValue + @InvValue2,
                @MonthName = CASE @MonthID2                                     -- Month description
                                WHEN 1 THEN 'Current Month'
                                WHEN 2 THEN 'Prior Month'
                                WHEN 3 THEN 'Pre-prior Month'
                                WHEN 4 THEN 'Month 4'
                                WHEN 5 THEN 'Month 5'
                                WHEN 6 THEN 'Month 6'
                                WHEN 7 THEN 'Month 7'
                                WHEN 8 THEN 'Month 8'
                                WHEN 9 THEN 'Month 9'
                                WHEN 10 THEN 'Month 10'
                                WHEN 11 THEN 'Month 11'
                                WHEN 12 THEN 'Month 12'
                                ELSE ''
                            END

        FETCH NEXT FROM RandLSum_cursor
        INTO    @Agent2,
                @MonthID2,
                @StatsFolder,
                @RenValue2,
                @LapValue2,
                @InvValue2


        IF @PrevStatsFolder <> @StatsFolder OR @@FETCH_STATUS <> 0
        BEGIN
            SELECT  @RenMonthCount = CASE   WHEN isnull(@RenMonthValue, 0) <> 0         -- count per month
                                                THEN @RenMonthCount + 1
                                                ELSE @RenMonthCount
                                            END,
                    @LapMonthCount = CASE   WHEN isnull(@LapMonthValue, 0) <> 0
                                                THEN @LapMonthCount + 1
                                                ELSE @LapMonthCount
                                            END,
                    @InvMonthCount = CASE   WHEN isnull(@InvValue2, 0) <> 0
                                                THEN @InvMonthCount + 1
                                                ELSE @InvMonthCount
                                            END,
                    @CRenMonthCount = CASE   WHEN isnull(@InvMonthValue, 0) <> 0        -- Cumulative count per month
                                                THEN @CRenMonthCount + 1
                                                ELSE @CRenMonthCount
                                            END,
                    @CLapMonthCount = CASE   WHEN isnull(@InvMonthValue, 0) <> 0
                                                THEN @CLapMonthCount + 1
                                                ELSE @CLapMonthCount
                                            END,
                    @CInvMonthCount = CASE   WHEN isnull(@InvMonthValue, 0) <> 0
                                                THEN @CInvMonthCount + 1
                                                ELSE @CInvMonthCount
                                            END,
                    @PrevStatsFolder = @StatsFolder
        END


        IF @PrevMonthID <> @MonthID2 OR @@FETCH_STATUS <> 0
        BEGIN
            --print 'final values for this month'
            INSERT INTO #tmpRnwlsAndLapsesSum
                SELECT @PrevAgent,
                    @PrevMonthID,
                    @MonthName,
                    @dtSelectedPeriodEnd,
                    @RenMonthCount,
                    @RenMonthValue,
                    @LapMonthCount,
                    @LapMonthValue,
                    @InvMonthCount,
                    @InvMonthValue,
                    @CRenMonthCount,
                    @CRenMonthValue,
                    @CLapMonthCount,
                    @CLapMonthValue,
                    @CInvMonthCount,
                    @CInvMonthValue
        END

        IF @PrevAgent <> @Agent2
        BEGIN
            --print 'zero cumulative running totals'
            SELECT @CRenMonthCount = 0,
                @CRenMonthValue = 0,
                @CLapMonthCount = 0,
                @CLapMonthValue = 0,
                @CInvMonthCount = 0,
                @CInvMonthValue = 0,
                @PrevAgent = @Agent2
        END

        IF @PrevMonthID <> @MonthID2
        BEGIN
            --print 'zero month totals'
            SELECT @RenMonthCount = 0,
                @RenMonthValue = 0,
                @LapMonthCount = 0,
                @LapMonthValue = 0,
                @InvMonthCount = 0,
                @InvMonthValue = 0,
                @PrevMonthID = @MonthID2
        END
    END

CLOSE RandLSum_cursor
DEALLOCATE RandLSum_cursor
-- CURSOR 2 END ***************************************************************************

-- get month totals for all agents - ALL agents only
IF @AgentName = 'ALL'
BEGIN
    INSERT INTO #tmpRnwlsAndLapsesSum
        SELECT 'ZZZZ',      -- to force to the end of the report
            MonthID,
            MonthName,
            CurrentPeriodEnd,
            Sum(RenMonthCount),
            Sum(RenMonthValue),
            Sum(LapMonthCount),
            Sum(LapMonthValue),
            Sum(InvMonthCount),
            Sum(InvMonthValue),
            Sum(CRenMonthCount),
            Sum(CRenMonthValue),
            Sum(CLapMonthCount),
            Sum(CLapMonthValue),
            Sum(CInvMonthCount),
            Sum(CInvMonthValue)
        FROM #tmpRnwlsAndLapsesSum
        GROUP BY Agent, MonthID, MonthName, CurrentPeriodEnd
END
SET NOCOUNT OFF

SELECT Agent,
        MonthID,
        MonthName,
        CurrentPeriodEnd,
        RenMonthCount,
        RenMonthValue,
        LapMonthCount,
        LapMonthValue,
        InvMonthCount,
        InvMonthValue,
        CRenMonthCount,
        CRenMonthValue,
        CLapMonthCount,
        CLapMonthValue,
        CInvMonthCount,
        CInvMonthValue
FROM #tmpRnwlsAndLapsesSum

DROP TABLE #tmpRnwlsAndLapses
DROP TABLE #tmpRnwlsAndLapsesSum
DROP TABLE #tmp12PeriodDates

GO
