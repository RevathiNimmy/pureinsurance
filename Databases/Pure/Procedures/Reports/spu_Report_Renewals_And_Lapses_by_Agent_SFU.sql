SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


EXECUTE DDLDropProcedure 'spu_Report_Renewals_And_Lapses_by_Agent_SFU'
GO

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
** 1.05 18/08/2004	JT		MultiCurrency changes 
** 1.06 14Jun2006   RC      Filter by Agent Group
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_Renewals_And_Lapses_by_Agent_SFU
(               @AgentName varchar (100),
                @PeriodDate varchar (20),
                @TypeOfcurrency	Varchar(30),
                @GroupbyCode	Varchar(30),
				@AgentGroupCode Varchar(30)
)
AS

SET NOCOUNT ON

/*-- test
DECLARE @AgentName varchar (100), @PeriodDate varchar (20)
SELECT @AgentName = 'ALL', @PeriodDate = "2004/08/09"
*/

--DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime

-- get party_cnt from @Agent (resolved_name)
DECLARE @AgentID int
IF @AgentName <> 'ALL'
    BEGIN
        SELECT @AgentID = party_cnt
        FROM Party
        WHERE shortname = @AgentName
    END

/* AGS 190704
-- get current period values
DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod_SFU @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT

-- find out whether Current or Prior Period has been selected
IF @Period = 'Prior Period'
    BEGIN
        SELECT @SelectedPeriodID = @CurrentPeriodID -1
    END
    ELSE
    BEGIN
        SELECT @SelectedPeriodID = @CurrentPeriodID
    END
*/
DECLARE @SelectedPeriodID int, @dtPeriodEndDate datetime, @dtSelectedPeriodEnd datetime
DECLARE @dtMinPeriodEnd datetime, @dtMaxPeriodEnd datetime

SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @dtPeriodEndDate = CONVERT (Datetime, @PeriodDate)

SELECT @SelectedPeriodID = period_id
FROM Period
WHERE period_end_date = @dtPeriodEndDate

/*Get System Currency Details--jitendra*/
	declare @SystemCurrencyCode varchar(10)
	declare @SystemCurrencyDesc varchar(255)
    SELECT
    	@SystemCurrencyCode = c.iso_code,
    	@SystemCurrencyDesc = c.description
    FROM PMSystem pms
    JOIN currency c
    	ON c.currency_id = pms.currency_id
    WHERE pms.system_id = 1
/*end  Get System Currency*/

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
    ORDER BY 1 DESC
/*
print 'test dates'
SELECT * FROM #tmp12PeriodDates
*/

-- get the end date of the Period selected
SELECT @dtSelectedPeriodEnd = dtPeriodEnd
FROM #tmp12PeriodDates
WHERE dateID = 1

-- get the max and min end dates
SELECT @dtMinPeriodEnd = dtPeriodEnd FROM #tmp12PeriodDates WHERE dateID = 13
SELECT @dtMaxPeriodEnd = dtPeriodEnd FROM #tmp12PeriodDates WHERE dateID = 1

CREATE TABLE #tmpRnwlsAndLapses
(
    Agent varchar (100) NULL,
    RenewalDate datetime NULL,
    MonthID int NULL,
    StatsFolder int NULL,
    RenValue money NULL,
    LapValue money NULL,
    InvValue money NULL,
    SourceId	INT NULL
)

-- Insert Renewals
INSERT INTO #tmpRnwlsAndLapses
    SELECT (SELECT resolved_name FROM Party where party_cnt = sf.agent_cnt),
        sf.cover_start_date,
        NULL,
        sf.stats_folder_cnt,
        Case @TypeOfCurrency WHEN 'Base' THEN sd.this_premium_home
        	WHEN 'System' THEN sd.this_premium_system
        END ,
        	
        NULL,
        NULL,sf.source_id
    FROM stats_folder sf
    JOIN stats_detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt
    WHERE sf.transaction_type_code = 'REN'
    AND sd.stats_detail_type = 'GRS'
    AND sf.cover_start_date > @dtMinPeriodEnd
    AND sf.cover_start_date <= @dtMaxPeriodEnd
    AND (sf.agent_cnt = @AgentID
        OR @AgentName = 'ALL')

-- Insert Lapses
INSERT INTO #tmpRnwlsAndLapses
    SELECT (SELECT resolved_name FROM Party where party_cnt = sf.agent_cnt),
        sf.expiry_date,
        NULL,
        sf.stats_folder_cnt,
        NULL,
        Case @TypeOfCurrency WHEN 'Base' THEN sd.this_premium_home
		        	WHEN 'System' THEN sd.this_premium_system
        END ,
        NULL,sf.source_id
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
    AND sf.expiry_date > @dtMinPeriodEnd
    AND sf.expiry_date <= @dtMaxPeriodEnd

-- Insert Not Invited
INSERT INTO #tmpRnwlsAndLapses
    SELECT (SELECT resolved_name FROM Party where party_cnt = sf.agent_cnt),
        sf.expiry_date,
        NULL,
        sf.stats_folder_cnt,
        NULL,
        NULL,
        Case @TypeOfCurrency WHEN 'Base' THEN sd.this_premium_home
		        	WHEN 'System' THEN sd.this_premium_system
        END ,sf.Source_id
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
    AND sf.expiry_date > @dtMinPeriodEnd
    AND sf.expiry_date <= @dtMaxPeriodEnd


-- Sort premium into date periods and renewal types
DECLARE @MonthIDCounter int
SELECT @MonthIDCounter = 13
WHILE @MonthIDCounter > 0
BEGIN
	UPDATE #tmpRnwlsAndLapses
	SET monthID = dateID
	FROM #tmp12PeriodDates
	WHERE RenewalDate <= dtPeriodEnd 
	AND dateID = @MonthIDCounter
	AND monthID is null
	
select @MonthIDCounter = @MonthIDCounter - 1
END

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
    CInvMonthValue money NULL,
    SourceID	INT	NULL
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
    @PrevMonthID int,
    @PrevRenValue money,
    @PrevLapValue money,
    @PrevInvValue money
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
    @CInvMonthValue money,
    @SourceId	Int

DECLARE RandLSum_cursor CURSOR FOR
    SELECT isnull(Agent,' Direct'),     -- If Agent is null, then Direct Client
            MonthID,
            StatsFolder,
            isnull(RenValue,0),
            isnull(LapValue,0),
            isnull(InvValue,0),SourceId
    FROM #tmpRnwlsAndLapses
    ORDER BY Agent, MonthID, StatsFolder DESC        -- Ordered for the running totals

OPEN    RandLSum_cursor

    FETCH NEXT FROM RandLSum_cursor
    INTO    @Agent2,
            @MonthID2,
            @StatsFolder,
            @RenValue2,
            @LapValue2,
            @InvValue2,
            @SourceID

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

    WHILE @@FETCH_STATUS = 0

    BEGIN
        SELECT  @RenMonthValue = @RenMonthValue + @RenValue2,                   -- values per month
                @LapMonthValue = @LapMonthValue + @LapValue2,
                @InvMonthValue = @InvMonthValue + @InvValue2,
                @CRenMonthValue = @CRenMonthValue + @RenValue2,                 -- Cumulative values per month
                @CLapMonthValue = @CLapMonthValue + @LapValue2,
                @CInvMonthValue = @CInvMonthValue + @InvValue2,
                @PrevRenValue = @RenValue2,
                @PrevLapValue = @LapValue2,
                @PrevInvValue = @InvValue2,
                @MonthName = CASE @MonthID2                                     -- Month description
                                WHEN 1 THEN 'Current Month'
                                WHEN 2 THEN 'Prior Month'
                                WHEN 3 THEN 'Month 3'
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
                @InvValue2,
                @SourceID


        IF @PrevStatsFolder <> @StatsFolder OR @@FETCH_STATUS <> 0
        BEGIN
            SELECT  @RenMonthCount = CASE   WHEN isnull(@PrevRenValue, 0) <> 0         -- count per month
                                                THEN @RenMonthCount + 1
                                                ELSE @RenMonthCount
                                            END,
                    @LapMonthCount = CASE   WHEN isnull(@PrevLapValue, 0) <> 0
                                                THEN @LapMonthCount + 1
                                                ELSE @LapMonthCount
                                            END,
                    @InvMonthCount = CASE   WHEN isnull(@PrevInvValue, 0) <> 0
                                                THEN @InvMonthCount + 1
                                                ELSE @InvMonthCount
                                            END,
                    @CRenMonthCount = CASE   WHEN isnull(@PrevRenValue, 0) <> 0        -- Cumulative count per month
                                                THEN @CRenMonthCount + 1
                                                ELSE @CRenMonthCount
                                            END,
                    @CLapMonthCount = CASE   WHEN isnull(@PrevLapValue, 0) <> 0
                                                THEN @CLapMonthCount + 1
                                                ELSE @CLapMonthCount
                                            END,
                    @CInvMonthCount = CASE   WHEN isnull(@PrevInvValue, 0) <> 0
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
                    @CInvMonthValue,@SourceID
                    	
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
            Sum(CInvMonthValue),
            SourceID
        FROM #tmpRnwlsAndLapsesSum
        GROUP BY Agent, MonthID, MonthName, CurrentPeriodEnd,SourceID
END
SET NOCOUNT OFF


IF LOWER(@AgentGroupCode) = 'all'
BEGIN

	PRINT 'ENTER1'

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
	        CInvMonthValue,
	        SourceID,S.Code CompanyCode,
	        S.Description CompanyDesc,
	        Case @TypeOfCurrency WHEN 'Base' THEN CB.Code
	        	WHEN 'System' THEN @SystemcurrencyCode
	        END CurrencyCode,
	        Case @TypeOfCurrency WHEN 'Base' THEN CB.Description
	        	WHEN 'System' THEN @SystemCurrencyDesc
	        END CurrencyDesc,
	        Case @GroupByCode WHEN 'Branch' THEN S.Code
	        	WHEN 'Branch And Currency' THEN S.Code
	        	ELSE ''
	        END 'GroupBycode'
	        
	FROM #tmpRnwlsAndLapsesSum TS
	JOIN Source S ON S.source_id = TS.Sourceid
	JOIN Currency CB ON S.base_currency_id = CB.Currency_id
END

IF LOWER(@AgentGroupCode) <> 'all'
BEGIN

	PRINT 'ENTER2'

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
	        CInvMonthValue,
	        SourceID,S.Code CompanyCode,
	        S.Description CompanyDesc,
	        Case @TypeOfCurrency WHEN 'Base' THEN CB.Code
	        	WHEN 'System' THEN @SystemcurrencyCode
	        END CurrencyCode,
	        Case @TypeOfCurrency WHEN 'Base' THEN CB.Description
	        	WHEN 'System' THEN @SystemCurrencyDesc
	        END CurrencyDesc,
	        Case @GroupByCode WHEN 'Branch' THEN S.Code
	        	WHEN 'Branch And Currency' THEN S.Code
	        	ELSE ''
	        END 'GroupBycode'
	        
	FROM #tmpRnwlsAndLapsesSum TS
	JOIN Source S ON S.source_id = TS.Sourceid
	JOIN Currency CB ON S.base_currency_id = CB.Currency_id
    --RC-- 14 Jun 2006
    WHERE Agent IN(
    select trading_name from party_agent where linked_account_group = (
    select  party_cnt from party where shortname = @AgentGroupCode) )
    --RC-- 14 Jun 2006
END


DROP TABLE #tmpRnwlsAndLapses
DROP TABLE #tmpRnwlsAndLapsesSum
DROP TABLE #tmp12PeriodDates


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
