SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Accumulations'
GO


CREATE PROCEDURE spu_Report_Accumulations
    @PerilDesc varchar(255),
    @AccumDesc varchar(255),
    @company_id int,
    @sub_branch_id int=NULL --AMJ
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 20/09/2000
** RSA Reports - Accumulations.rpt
**      *in progress- fields not complete
**********************************************************************************************************************************
** 22/09/2000 Jude Killip       use Accumulation_Values table
**
** 20/11/2000 Jude Killip       DB Change - link between Insurance_File and Accumulation_Values
**
** 02/05/2001 Jude Killip       DB Change - new fields in Accumulation_Values
**                              Add insurance_file_cnt filter to get current policies
**                              move parameter here from report
**                              base dates on Period
**
** 18/06/2001 Jude Killip       Redo based on premium Gross to Net
**
** 20/11/2001   JMK             Add Accumulation Description parameter
**                              Get accumulation hierarchy
**                              Tidy up the rest
**
** 05/12/2001   JMK             Remove levels beyond level 5
**                              summarise accumulation values (to fix duplication error)
** 01/08/2002   AMJ - branch specific change
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     31/01/2002  JMK     re-released separately - (missed from patch 1.6.15)
***********************************************************************************************************************************/
SET NOCOUNT ON
IF @sub_branch_id IS NULL
    EXEC spu_sub_branch_default @source_id=@company_id, @sub_branch_id=@sub_branch_id OUTPUT

CREATE TABLE #tempRSAAccumulation
(
    Level1Code int NULL,
    Level2Code int NULL,
    Level3Code int NULL,
    Level4Code int NULL,
    Level5Code int NULL,
    Level1Desc varchar(255) NULL,
    Level2Desc varchar(255) NULL,
    Level3Desc varchar(255) NULL,
    Level4Desc varchar(255) NULL,
    Level5Desc varchar(255) NULL,
    StartLevel int NULL,
    CountLevels int NULL,
    PerilID int NULL,
    PerilType varchar (255) NULL,
    PeriodRangeID int,                      -- 1=Current, 2=YTD, 3=12Months
    PeriodRangeName varchar(30),
    Gross decimal (19,4) NULL,
    Coinsurance decimal (19,4) NULL,
    Treaty decimal (19,4) NULL,
    Facultative decimal (19,4) NULL,
    Retained decimal (19,4) NULL,
    dtCurrentPeriodEnd datetime,
    dtLastYearEnd datetime,
    dt12MonthsAgo datetime,
    CurrentPeriodID int
)
-- get current period values
DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod @sub_branch_id, @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT

-- get current year values
DECLARE @CurrentYearStartPeriodID int, @dtLastYearPeriodEndDate datetime
EXECUTE spu_Report_GetCurrentYear @sub_branch_id, @CurrentYearStartPeriodID OUTPUT, @dtLastYearPeriodEndDate OUTPUT

-- get current 12 month period values
DECLARE  @12MonthPeriodID int, @dt12MonthPeriodEnd datetime
EXECUTE spu_Report_GetCurrent12MonthPeriod @sub_branch_id, @12MonthPeriodID OUTPUT, @dt12MonthPeriodEnd OUTPUT

--*************************************************************************************************************
-- Accumulation table hierarchy
DECLARE @CodeLevel int, @CountLevels int

CREATE TABLE #tmpAccumulation1
(
    acc_id int NULL,
    code varchar(10) NULL,
    description varchar(255) NULL,
    parent_id int NULL
)
--print 'level 1'
INSERT INTO #tmpAccumulation1
    SELECT accumulation_id,
        code,
        description,
        parent_id
    FROM Accumulation
    WHERE is_deleted = 0
    AND parent_id IS NULL

CREATE TABLE #tmpAccumulation2
(
    acc_id int NULL,
    code varchar(10) NULL,
    description varchar(255) NULL,
    parent_id int NULL
)
--print 'level 2'
INSERT INTO #tmpAccumulation2
    SELECT accumulation_id,
        code,
        description,
        parent_id
    FROM Accumulation
    WHERE parent_id IN (SELECT acc_id FROM #tmpAccumulation1)

CREATE TABLE #tmpAccumulation3
(
    acc_id int NULL,
    code varchar(10) NULL,
    description varchar(255) NULL,
    parent_id int NULL
)
--print 'level 3'
INSERT INTO #tmpAccumulation3
    SELECT accumulation_id,
        code,
        description,
        parent_id
    FROM Accumulation
    WHERE parent_id IN (SELECT acc_id FROM #tmpAccumulation2)

CREATE TABLE #tmpAccumulation4
(
    acc_id int NULL,
    code varchar(10) NULL,
    description varchar(255) NULL,
    parent_id int NULL
)
--print 'level 4'
INSERT INTO #tmpAccumulation4
    SELECT accumulation_id,
        code,
        description,
        parent_id
    FROM Accumulation
    WHERE parent_id IN (SELECT acc_id FROM #tmpAccumulation3)

CREATE TABLE #tmpAccumulation5
(
    acc_id int NULL,
    code varchar(10) NULL,
    description varchar(255) NULL,
    parent_id int NULL
)
--print 'level 5'
INSERT INTO #tmpAccumulation5
    SELECT accumulation_id,
        code,
        description,
        parent_id
    FROM Accumulation
    WHERE parent_id IN (SELECT acc_id FROM #tmpAccumulation4)


-- table for bringing all levels together
CREATE TABLE #tmpAccumulationFull
(
    StartLevel int,
    level1_id int,
    level1_description varchar(255) NULL,
    level2_id int NULL,
    level2_description varchar(255) NULL,
    level3_id int NULL,
    level3_description varchar(255) NULL,
    level4_id int NULL,
    level4_description varchar(255) NULL,
    level5_id int NULL,
    level5_description varchar(255) NULL
    )

IF @AccumDesc = 'ALL'
    SELECT @CodeLevel = 0
ELSE
IF EXISTS (SELECT * FROM #tmpAccumulation1 WHERE description = @AccumDesc)
    SELECT @CodeLevel = 1
ELSE
IF EXISTS (SELECT * FROM #tmpAccumulation2 WHERE description = @AccumDesc)
    SELECT @CodeLevel = 2
ELSE
IF EXISTS (SELECT * FROM #tmpAccumulation3 WHERE description = @AccumDesc)
    SELECT @CodeLevel = 3
ELSE
IF EXISTS (SELECT * FROM #tmpAccumulation4 WHERE description = @AccumDesc)
    SELECT @CodeLevel = 4
ELSE
IF EXISTS (SELECT * FROM #tmpAccumulation5 WHERE description = @AccumDesc)
    SELECT @CodeLevel = 5


-- Table for summarising values by peril and code
CREATE TABLE #tempvalues
(
    perilTypeID int,
    PeriodID int,
    PeriodName varchar (30),
    code1 int,
    code2 int,
    code3 int,
    code4 int,
    code5 int,
    SI money,
    COI money,
    TTY money,
    FAC money,
    RET money
)
--*********************************************************************************************************************
IF @CodeLevel = 0
BEGIN
    -- get all levels of accumulations
    INSERT INTO #tmpAccumulationFull
        SELECT 1,
            ISNULL(t1.acc_id,0), t1.description,
            ISNULL(t2.acc_id,0), t2.description,
            ISNULL(t3.acc_id,0), t3.description,
            ISNULL(t4.acc_id,0), t4.description,
            ISNULL(t5.acc_id,0), t5.description
        FROM #tmpAccumulation1 t1
        LEFT OUTER JOIN  #tmpAccumulation2 t2 ON t2.parent_id = t1.acc_id
        LEFT OUTER JOIN  #tmpAccumulation3 t3 ON t3.parent_id = t2.acc_id
        LEFT OUTER JOIN  #tmpAccumulation4 t4 ON t4.parent_id = t3.acc_id
        LEFT OUTER JOIN  #tmpAccumulation5 t5 ON t5.parent_id = t4.acc_id

    -- update CURRENT PERIOD values for this level
    INSERT INTO #tempvalues
        SELECT av.peril_type_id,
            1,      -- Current Period
            "Current Period",
            av.accumulation_code_1,
            av.accumulation_code_2,
            av.accumulation_code_3,
            av.accumulation_code_4,
            av.accumulation_code_5,
            sum(av.sum_insured),
            sum(av.coinsured_sum_insured),
            sum(av.treaty_sum_insured),
            sum(av.fac_sum_insured),
            sum(av.retained_sum_insured)
        FROM Accumulation_Values av
        WHERE av.risk_cnt IN(
            SELECT  DISTINCT(sd.risk_id)
            FROM    stats_detail sd
            WHERE   sd.stats_folder_cnt in (
                SELECT  sf.stats_folder_cnt
                FROM    stats_folder sf
                WHERE sf.posting_period_number = @CurrentPeriodID)
            )
        AND (isnull(av.sum_insured,0) <> 0
            OR isnull(av.coinsured_sum_insured,0) <> 0
            OR isnull(av.retained_sum_insured,0) <> 0
            OR isnull(av.treaty_sum_insured,0) <> 0
            OR isnull(av.fac_sum_insured,0) <> 0
            )
        AND av.accumulation_code_1 in (SELECT level1_id FROM #tmpAccumulationFull)
        GROUP BY av.peril_type_id,
                av.accumulation_code_1,
                av.accumulation_code_2,
                av.accumulation_code_3,
                av.accumulation_code_4,
                av.accumulation_code_5

    -- update CURRENT YEAR values for this level
    INSERT INTO #tempvalues
        SELECT av.peril_type_id,
            2,      -- Current Year
            "Current Year",
            av.accumulation_code_1,
            av.accumulation_code_2,
            av.accumulation_code_3,
            av.accumulation_code_4,
            av.accumulation_code_5,
            sum(av.sum_insured),
            sum(av.coinsured_sum_insured),
            sum(av.treaty_sum_insured),
            sum(av.fac_sum_insured),
            sum(av.retained_sum_insured)
        FROM Accumulation_Values av
        WHERE av.risk_cnt IN(
            SELECT  DISTINCT(sd.risk_id)
            FROM    stats_detail sd
            WHERE   sd.stats_folder_cnt in (
                SELECT  sf.stats_folder_cnt
                FROM    stats_folder sf
                WHERE sf.posting_period_number BETWEEN @CurrentYearStartPeriodID AND @CurrentPeriodID)
            )
        AND (isnull(av.sum_insured,0) <> 0
            OR isnull(av.coinsured_sum_insured,0) <> 0
            OR isnull(av.retained_sum_insured,0) <> 0
            OR isnull(av.treaty_sum_insured,0) <> 0
            OR isnull(av.fac_sum_insured,0) <> 0
            )
        AND av.accumulation_code_1 in (SELECT level1_id FROM #tmpAccumulationFull)
        GROUP BY av.peril_type_id,
                av.accumulation_code_1,
                av.accumulation_code_2,
                av.accumulation_code_3,
                av.accumulation_code_4,
                av.accumulation_code_5


    -- update 12 MONTH values for this level
    INSERT INTO #tempvalues
        SELECT av.peril_type_id,
            3,      -- 12 Month Period
            "12 Month Period",
            av.accumulation_code_1,
            av.accumulation_code_2,
            av.accumulation_code_3,
            av.accumulation_code_4,
            av.accumulation_code_5,
            sum(av.sum_insured),
            sum(av.coinsured_sum_insured),
            sum(av.treaty_sum_insured),
            sum(av.fac_sum_insured),
            sum(av.retained_sum_insured)
        FROM Accumulation_Values av
        WHERE av.risk_cnt IN(
            SELECT  DISTINCT(sd.risk_id)
            FROM    stats_detail sd
            WHERE   sd.stats_folder_cnt in (
                SELECT  sf.stats_folder_cnt
                FROM    stats_folder sf
                WHERE sf.posting_period_number BETWEEN @12MonthPeriodID + 1 AND @CurrentPeriodID)
            )
        AND (isnull(av.sum_insured,0) <> 0
            OR isnull(av.coinsured_sum_insured,0) <> 0
            OR isnull(av.retained_sum_insured,0) <> 0
            OR isnull(av.treaty_sum_insured,0) <> 0
            OR isnull(av.fac_sum_insured,0) <> 0
            )
        AND av.accumulation_code_1 in (SELECT level1_id FROM #tmpAccumulationFull)
        GROUP BY av.peril_type_id,
                av.accumulation_code_1,
                av.accumulation_code_2,
                av.accumulation_code_3,
                av.accumulation_code_4,
                av.accumulation_code_5
END
--*********************************************************************************************************************
IF @CodeLevel = 1
BEGIN
    -- get all levels of accumulations
     INSERT INTO #tmpAccumulationFull
         SELECT 1,
             ISNULL(t1.acc_id,0), t1.description,
             ISNULL(t2.acc_id,0), t2.description,
             ISNULL(t3.acc_id,0), t3.description,
             ISNULL(t4.acc_id,0), t4.description,
             ISNULL(t5.acc_id,0), t5.description
         FROM #tmpAccumulation1 t1
         LEFT OUTER JOIN  #tmpAccumulation2 t2 ON t2.parent_id = t1.acc_id
         LEFT OUTER JOIN  #tmpAccumulation3 t3 ON t3.parent_id = t2.acc_id
         LEFT OUTER JOIN  #tmpAccumulation4 t4 ON t4.parent_id = t3.acc_id
         LEFT OUTER JOIN  #tmpAccumulation5 t5 ON t5.parent_id = t4.acc_id
         WHERE t1.description = @AccumDesc

     -- update CURRENT PERIOD values for this level
     INSERT INTO #tempvalues
         SELECT av.peril_type_id,
            1,      -- Current Period
            "Current Period",
             av.accumulation_code_1,
             av.accumulation_code_2,
             av.accumulation_code_3,
             av.accumulation_code_4,
             av.accumulation_code_5,
             sum(av.sum_insured),
             sum(av.coinsured_sum_insured),
             sum(av.treaty_sum_insured),
             sum(av.fac_sum_insured),
             sum(av.retained_sum_insured)
         FROM Accumulation_Values av
         WHERE av.risk_cnt IN(
             SELECT  DISTINCT(sd.risk_id)
             FROM    stats_detail sd
             WHERE   sd.stats_folder_cnt in (
                 SELECT  sf.stats_folder_cnt
                 FROM    stats_folder sf
                 WHERE sf.posting_period_number = @CurrentPeriodID)
             )
         AND (isnull(av.sum_insured,0) <> 0
             OR isnull(av.coinsured_sum_insured,0) <> 0
             OR isnull(av.retained_sum_insured,0) <> 0
             OR isnull(av.treaty_sum_insured,0) <> 0
             OR isnull(av.fac_sum_insured,0) <> 0
             )
         AND av.accumulation_code_1 in (SELECT level1_id FROM #tmpAccumulationFull)
         GROUP BY av.peril_type_id,
                 av.accumulation_code_1,
                 av.accumulation_code_2,
                 av.accumulation_code_3,
                 av.accumulation_code_4,
                 av.accumulation_code_5

     -- update CURRENT YEAR values for this level
     INSERT INTO #tempvalues
         SELECT av.peril_type_id,
            2,      -- Current Year
            "Current Year",
             av.accumulation_code_1,
             av.accumulation_code_2,
             av.accumulation_code_3,
             av.accumulation_code_4,
             av.accumulation_code_5,
             sum(av.sum_insured),
             sum(av.coinsured_sum_insured),
             sum(av.treaty_sum_insured),
             sum(av.fac_sum_insured),
             sum(av.retained_sum_insured)
         FROM Accumulation_Values av
         WHERE av.risk_cnt IN(
             SELECT  DISTINCT(sd.risk_id)
             FROM    stats_detail sd
             WHERE   sd.stats_folder_cnt in (
                 SELECT  sf.stats_folder_cnt
                 FROM    stats_folder sf
                 WHERE sf.posting_period_number BETWEEN @CurrentYearStartPeriodID AND @CurrentPeriodID)
             )
         AND (isnull(av.sum_insured,0) <> 0
             OR isnull(av.coinsured_sum_insured,0) <> 0
             OR isnull(av.retained_sum_insured,0) <> 0
             OR isnull(av.treaty_sum_insured,0) <> 0
             OR isnull(av.fac_sum_insured,0) <> 0
             )
         AND av.accumulation_code_1 in (SELECT level1_id FROM #tmpAccumulationFull)
         GROUP BY av.peril_type_id,
                 av.accumulation_code_1,
                 av.accumulation_code_2,
                 av.accumulation_code_3,
                 av.accumulation_code_4,
                 av.accumulation_code_5


     -- update 12 MONTH values for this level
     INSERT INTO #tempvalues
         SELECT av.peril_type_id,
            3,      -- 12 Month Period
            "12 Month Period",
             av.accumulation_code_1,
             av.accumulation_code_2,
             av.accumulation_code_3,
             av.accumulation_code_4,
             av.accumulation_code_5,
             sum(av.sum_insured),
             sum(av.coinsured_sum_insured),
             sum(av.treaty_sum_insured),
             sum(av.fac_sum_insured),
             sum(av.retained_sum_insured)
         FROM Accumulation_Values av
         WHERE av.risk_cnt IN(
             SELECT  DISTINCT(sd.risk_id)
             FROM    stats_detail sd
             WHERE   sd.stats_folder_cnt in (
                 SELECT  sf.stats_folder_cnt
                 FROM    stats_folder sf
                 WHERE sf.posting_period_number BETWEEN @12MonthPeriodID + 1 AND @CurrentPeriodID)
             )
         AND (isnull(av.sum_insured,0) <> 0
             OR isnull(av.coinsured_sum_insured,0) <> 0
             OR isnull(av.retained_sum_insured,0) <> 0
             OR isnull(av.treaty_sum_insured,0) <> 0
             OR isnull(av.fac_sum_insured,0) <> 0
             )
         AND av.accumulation_code_1 in (SELECT level1_id FROM #tmpAccumulationFull)
         GROUP BY av.peril_type_id,
                 av.accumulation_code_1,
                 av.accumulation_code_2,
                 av.accumulation_code_3,
                 av.accumulation_code_4,
                 av.accumulation_code_5
END
--*********************************************************************************************************************
IF @CodeLevel = 2
BEGIN
    INSERT INTO #tmpAccumulationFull
        SELECT @CodeLevel,
            ISNULL(t2.acc_id,0), t2.description,
            ISNULL(t3.acc_id,0), t3.description,
            ISNULL(t4.acc_id,0), t4.description,
            ISNULL(t5.acc_id,0), t5.description,
            NULL, NULL
        FROM  #tmpAccumulation2 t2
        LEFT OUTER JOIN  #tmpAccumulation3 t3 ON t3.parent_id = t2.acc_id
        LEFT OUTER JOIN  #tmpAccumulation4 t4 ON t4.parent_id = t3.acc_id
        LEFT OUTER JOIN  #tmpAccumulation5 t5 ON t5.parent_id = t4.acc_id
        WHERE t2.description = @AccumDesc

     -- update CURRENT PERIOD values for this level
     INSERT INTO #tempvalues
         SELECT av.peril_type_id,
            1,      -- Current Period
            "Current Period",
             av.accumulation_code_2,
             av.accumulation_code_3,
             av.accumulation_code_4,
             av.accumulation_code_5,
             NULL,
             sum(av.sum_insured),
             sum(av.coinsured_sum_insured),
             sum(av.treaty_sum_insured),
             sum(av.fac_sum_insured),
             sum(av.retained_sum_insured)
         FROM Accumulation_Values av
         WHERE av.risk_cnt IN(
             SELECT  DISTINCT(sd.risk_id)
             FROM    stats_detail sd
             WHERE   sd.stats_folder_cnt in (
                 SELECT  sf.stats_folder_cnt
                 FROM    stats_folder sf
                 WHERE sf.posting_period_number = @CurrentPeriodID)
             )
         AND (isnull(av.sum_insured,0) <> 0
             OR isnull(av.coinsured_sum_insured,0) <> 0
             OR isnull(av.retained_sum_insured,0) <> 0
             OR isnull(av.treaty_sum_insured,0) <> 0
             OR isnull(av.fac_sum_insured,0) <> 0
             )
         AND av.accumulation_code_2 in (SELECT level1_id FROM #tmpAccumulationFull)
         GROUP BY av.peril_type_id,
                 av.accumulation_code_2,
                 av.accumulation_code_3,
                 av.accumulation_code_4,
                 av.accumulation_code_5

     -- update CURRENT YEAR values for this level
     INSERT INTO #tempvalues
         SELECT av.peril_type_id,
            2,      -- Current Year
            "Current Year",
             av.accumulation_code_2,
             av.accumulation_code_3,
             av.accumulation_code_4,
             av.accumulation_code_5,
             NULL,
             sum(av.sum_insured),
             sum(av.coinsured_sum_insured),
             sum(av.treaty_sum_insured),
             sum(av.fac_sum_insured),
             sum(av.retained_sum_insured)
         FROM Accumulation_Values av
         WHERE av.risk_cnt IN(
             SELECT  DISTINCT(sd.risk_id)
             FROM    stats_detail sd
             WHERE   sd.stats_folder_cnt in (
                 SELECT  sf.stats_folder_cnt
                 FROM    stats_folder sf
                 WHERE sf.posting_period_number BETWEEN @CurrentYearStartPeriodID AND @CurrentPeriodID)
             )
         AND (isnull(av.sum_insured,0) <> 0
             OR isnull(av.coinsured_sum_insured,0) <> 0
             OR isnull(av.retained_sum_insured,0) <> 0
             OR isnull(av.treaty_sum_insured,0) <> 0
             OR isnull(av.fac_sum_insured,0) <> 0
             )
         AND av.accumulation_code_2 in (SELECT level1_id FROM #tmpAccumulationFull)
         GROUP BY av.peril_type_id,
                 av.accumulation_code_2,
                 av.accumulation_code_3,
                 av.accumulation_code_4,
                 av.accumulation_code_5


     -- update 12 MONTH values for this level
     INSERT INTO #tempvalues
         SELECT av.peril_type_id,
            3,      -- 12 Month Period
            "12 Month Period",
             av.accumulation_code_2,
             av.accumulation_code_3,
             av.accumulation_code_4,
             av.accumulation_code_5,
             NULL,
             sum(av.sum_insured),
             sum(av.coinsured_sum_insured),
             sum(av.treaty_sum_insured),
             sum(av.fac_sum_insured),
             sum(av.retained_sum_insured)
         FROM Accumulation_Values av
         WHERE av.risk_cnt IN(
             SELECT  DISTINCT(sd.risk_id)
             FROM    stats_detail sd
             WHERE   sd.stats_folder_cnt in (
                 SELECT  sf.stats_folder_cnt
                 FROM    stats_folder sf
                 WHERE sf.posting_period_number BETWEEN @12MonthPeriodID + 1 AND @CurrentPeriodID)
             )
         AND (isnull(av.sum_insured,0) <> 0
             OR isnull(av.coinsured_sum_insured,0) <> 0
             OR isnull(av.retained_sum_insured,0) <> 0
             OR isnull(av.treaty_sum_insured,0) <> 0
             OR isnull(av.fac_sum_insured,0) <> 0
             )
         AND av.accumulation_code_2 in (SELECT level1_id FROM #tmpAccumulationFull)
         GROUP BY av.peril_type_id,
                 av.accumulation_code_2,
                 av.accumulation_code_3,
                 av.accumulation_code_4,
                 av.accumulation_code_5
END
--*********************************************************************************************************************
If @CodeLevel = 3
BEGIN
    INSERT INTO #tmpAccumulationFull
        SELECT @CodeLevel,
            ISNULL(t3.acc_id,0), t3.description,
            ISNULL(t4.acc_id,0), t4.description,
            ISNULL(t5.acc_id,0), t5.description,
            NULL, NULL,
            NULL, NULL
        FROM  #tmpAccumulation3 t3
        LEFT OUTER JOIN  #tmpAccumulation4 t4 ON t4.parent_id = t3.acc_id
        LEFT OUTER JOIN  #tmpAccumulation5 t5 ON t5.parent_id = t4.acc_id
        WHERE t3.description = @AccumDesc

     -- update CURRENT PERIOD values for this level
     INSERT INTO #tempvalues
         SELECT av.peril_type_id,
            1,      -- Current Period
            "Current Period",
             av.accumulation_code_3,
             av.accumulation_code_4,
             av.accumulation_code_5,
             NULL,
             NULL,
             sum(av.sum_insured),
             sum(av.coinsured_sum_insured),
             sum(av.treaty_sum_insured),
             sum(av.fac_sum_insured),
             sum(av.retained_sum_insured)
         FROM Accumulation_Values av
         WHERE av.risk_cnt IN(
             SELECT  DISTINCT(sd.risk_id)
             FROM    stats_detail sd
             WHERE   sd.stats_folder_cnt in (
                 SELECT  sf.stats_folder_cnt
                 FROM    stats_folder sf
                 WHERE sf.posting_period_number = @CurrentPeriodID)
             )
         AND (isnull(av.sum_insured,0) <> 0
             OR isnull(av.coinsured_sum_insured,0) <> 0
             OR isnull(av.retained_sum_insured,0) <> 0
             OR isnull(av.treaty_sum_insured,0) <> 0
             OR isnull(av.fac_sum_insured,0) <> 0
             )
         AND av.accumulation_code_3 in (SELECT level1_id FROM #tmpAccumulationFull)
         GROUP BY av.peril_type_id,
                 av.accumulation_code_3,
                 av.accumulation_code_4,
                 av.accumulation_code_5

     -- update CURRENT YEAR values for this level
     INSERT INTO #tempvalues
         SELECT av.peril_type_id,
            2,      -- Current Year
            "Current Year",
             av.accumulation_code_3,
             av.accumulation_code_4,
             av.accumulation_code_5,
             NULL,
             NULL,
             sum(av.sum_insured),
             sum(av.coinsured_sum_insured),
             sum(av.treaty_sum_insured),
             sum(av.fac_sum_insured),
             sum(av.retained_sum_insured)
         FROM Accumulation_Values av
         WHERE av.risk_cnt IN(
             SELECT  DISTINCT(sd.risk_id)
             FROM    stats_detail sd
             WHERE   sd.stats_folder_cnt in (
                 SELECT  sf.stats_folder_cnt
                 FROM    stats_folder sf
                 WHERE sf.posting_period_number BETWEEN @CurrentYearStartPeriodID AND @CurrentPeriodID)
             )
         AND (isnull(av.sum_insured,0) <> 0
             OR isnull(av.coinsured_sum_insured,0) <> 0
             OR isnull(av.retained_sum_insured,0) <> 0
             OR isnull(av.treaty_sum_insured,0) <> 0
             OR isnull(av.fac_sum_insured,0) <> 0
             )
         AND av.accumulation_code_3 in (SELECT level1_id FROM #tmpAccumulationFull)
         GROUP BY av.peril_type_id,
                 av.accumulation_code_3,
                 av.accumulation_code_4,
                 av.accumulation_code_5


     -- update 12 MONTH values for this level
     INSERT INTO #tempvalues
         SELECT av.peril_type_id,
            3,      -- 12 Month Period
            "12 Month Period",
             av.accumulation_code_3,
             av.accumulation_code_4,
             av.accumulation_code_5,
             NULL,
             NULL,
             sum(av.sum_insured),
             sum(av.coinsured_sum_insured),
             sum(av.treaty_sum_insured),
             sum(av.fac_sum_insured),
             sum(av.retained_sum_insured)
         FROM Accumulation_Values av
         WHERE av.risk_cnt IN(
             SELECT  DISTINCT(sd.risk_id)
             FROM    stats_detail sd
             WHERE   sd.stats_folder_cnt in (
                 SELECT  sf.stats_folder_cnt
                 FROM    stats_folder sf
                 WHERE sf.posting_period_number BETWEEN @12MonthPeriodID + 1 AND @CurrentPeriodID)
             )
         AND (isnull(av.sum_insured,0) <> 0
             OR isnull(av.coinsured_sum_insured,0) <> 0
             OR isnull(av.retained_sum_insured,0) <> 0
             OR isnull(av.treaty_sum_insured,0) <> 0
             OR isnull(av.fac_sum_insured,0) <> 0
             )
         AND av.accumulation_code_3 in (SELECT level1_id FROM #tmpAccumulationFull)
         GROUP BY av.peril_type_id,
                 av.accumulation_code_3,
                 av.accumulation_code_4,
                 av.accumulation_code_5
END
--*********************************************************************************************************************
If @CodeLevel = 4
BEGIN
    INSERT INTO #tmpAccumulationFull
        SELECT @CodeLevel,
            ISNULL(t4.acc_id,0), t4.description,
            ISNULL(t5.acc_id,0), t5.description,
            NULL, NULL,
            NULL, NULL,
            NULL, NULL
        FROM  #tmpAccumulation4 t4
        LEFT OUTER JOIN  #tmpAccumulation5 t5 ON t5.parent_id = t4.acc_id
        WHERE t4.description = @AccumDesc

     -- update CURRENT PERIOD values for this level
     INSERT INTO #tempvalues
         SELECT av.peril_type_id,
            1,      -- Current Period
            "Current Period",
             av.accumulation_code_4,
             av.accumulation_code_5,
             NULL,
             NULL,
             NULL,
             sum(av.sum_insured),
             sum(av.coinsured_sum_insured),
             sum(av.treaty_sum_insured),
             sum(av.fac_sum_insured),
             sum(av.retained_sum_insured)
         FROM Accumulation_Values av
         WHERE av.risk_cnt IN(
             SELECT  DISTINCT(sd.risk_id)
             FROM    stats_detail sd
             WHERE   sd.stats_folder_cnt in (
                 SELECT  sf.stats_folder_cnt
                 FROM    stats_folder sf
                 WHERE sf.posting_period_number = @CurrentPeriodID)
             )
         AND (isnull(av.sum_insured,0) <> 0
             OR isnull(av.coinsured_sum_insured,0) <> 0
             OR isnull(av.retained_sum_insured,0) <> 0
             OR isnull(av.treaty_sum_insured,0) <> 0
             OR isnull(av.fac_sum_insured,0) <> 0
             )
         AND av.accumulation_code_4 in (SELECT level1_id FROM #tmpAccumulationFull)
         GROUP BY av.peril_type_id,
                 av.accumulation_code_4,
                 av.accumulation_code_5

     -- update CURRENT YEAR values for this level
     INSERT INTO #tempvalues
         SELECT av.peril_type_id,
            2,      -- Current Year
            "Current Year",
             av.accumulation_code_4,
             av.accumulation_code_5,
             NULL,
             NULL,
             NULL,
             sum(av.sum_insured),
             sum(av.coinsured_sum_insured),
             sum(av.treaty_sum_insured),
             sum(av.fac_sum_insured),
             sum(av.retained_sum_insured)
         FROM Accumulation_Values av
         WHERE av.risk_cnt IN(
             SELECT  DISTINCT(sd.risk_id)
             FROM    stats_detail sd
             WHERE   sd.stats_folder_cnt in (
                 SELECT  sf.stats_folder_cnt
                 FROM    stats_folder sf
                 WHERE sf.posting_period_number BETWEEN @CurrentYearStartPeriodID AND @CurrentPeriodID)
             )
         AND (isnull(av.sum_insured,0) <> 0
             OR isnull(av.coinsured_sum_insured,0) <> 0
             OR isnull(av.retained_sum_insured,0) <> 0
             OR isnull(av.treaty_sum_insured,0) <> 0
             OR isnull(av.fac_sum_insured,0) <> 0
             )
         AND av.accumulation_code_4 in (SELECT level1_id FROM #tmpAccumulationFull)
         GROUP BY av.peril_type_id,
                 av.accumulation_code_4,
                 av.accumulation_code_5


     -- update 12 MONTH values for this level
     INSERT INTO #tempvalues
         SELECT av.peril_type_id,
            3,      -- 12 Month Period
            "12 Month Period",
             av.accumulation_code_4,
             av.accumulation_code_5,
             NULL,
             NULL,
             NULL,
             sum(av.sum_insured),
             sum(av.coinsured_sum_insured),
             sum(av.treaty_sum_insured),
             sum(av.fac_sum_insured),
             sum(av.retained_sum_insured)
         FROM Accumulation_Values av
         WHERE av.risk_cnt IN(
             SELECT  DISTINCT(sd.risk_id)
             FROM    stats_detail sd
             WHERE   sd.stats_folder_cnt in (
                 SELECT  sf.stats_folder_cnt
                 FROM    stats_folder sf
                 WHERE sf.posting_period_number BETWEEN @12MonthPeriodID + 1 AND @CurrentPeriodID)
             )
         AND (isnull(av.sum_insured,0) <> 0
             OR isnull(av.coinsured_sum_insured,0) <> 0
             OR isnull(av.retained_sum_insured,0) <> 0
             OR isnull(av.treaty_sum_insured,0) <> 0
             OR isnull(av.fac_sum_insured,0) <> 0
             )
         AND av.accumulation_code_4 in (SELECT level1_id FROM #tmpAccumulationFull)
         GROUP BY av.peril_type_id,
                 av.accumulation_code_4,
                 av.accumulation_code_5
END
--*********************************************************************************************************************
If @CodeLevel = 5
BEGIN
    INSERT INTO #tmpAccumulationFull
        SELECT @CodeLevel,
            ISNULL(t5.acc_id,0), t5.description,
            NULL, NULL,
            NULL, NULL,
            NULL, NULL,
            NULL, NULL
        FROM  #tmpAccumulation5 t5
        WHERE t5.description = @AccumDesc

     -- update CURRENT PERIOD values for this level
     INSERT INTO #tempvalues
         SELECT av.peril_type_id,
            1,      -- Current Period
            "Current Period",
             av.accumulation_code_5,
             NULL,
             NULL,
             NULL,
             NULL,
             sum(av.sum_insured),
             sum(av.coinsured_sum_insured),
             sum(av.treaty_sum_insured),
             sum(av.fac_sum_insured),
             sum(av.retained_sum_insured)
         FROM Accumulation_Values av
         WHERE av.risk_cnt IN(
             SELECT  DISTINCT(sd.risk_id)
             FROM    stats_detail sd
             WHERE   sd.stats_folder_cnt in (
                 SELECT  sf.stats_folder_cnt
                 FROM    stats_folder sf
                 WHERE sf.posting_period_number = @CurrentPeriodID)
             )
         AND (isnull(av.sum_insured,0) <> 0
             OR isnull(av.coinsured_sum_insured,0) <> 0
             OR isnull(av.retained_sum_insured,0) <> 0
             OR isnull(av.treaty_sum_insured,0) <> 0
             OR isnull(av.fac_sum_insured,0) <> 0
             )
         AND av.accumulation_code_5 in (SELECT level1_id FROM #tmpAccumulationFull)
         GROUP BY av.peril_type_id,
                 av.accumulation_code_5

     -- update CURRENT YEAR values for this level
     INSERT INTO #tempvalues
         SELECT av.peril_type_id,
            2,      -- Current Year
            "Current Year",
             av.accumulation_code_5,
             NULL,
             NULL,
             NULL,
             NULL,
             sum(av.sum_insured),
             sum(av.coinsured_sum_insured),
             sum(av.treaty_sum_insured),
             sum(av.fac_sum_insured),
             sum(av.retained_sum_insured)
         FROM Accumulation_Values av
         WHERE av.risk_cnt IN(
             SELECT  DISTINCT(sd.risk_id)
             FROM    stats_detail sd
             WHERE   sd.stats_folder_cnt in (
                 SELECT  sf.stats_folder_cnt
                 FROM    stats_folder sf
                 WHERE sf.posting_period_number BETWEEN @CurrentYearStartPeriodID AND @CurrentPeriodID)
             )
         AND (isnull(av.sum_insured,0) <> 0
             OR isnull(av.coinsured_sum_insured,0) <> 0
             OR isnull(av.retained_sum_insured,0) <> 0
             OR isnull(av.treaty_sum_insured,0) <> 0
             OR isnull(av.fac_sum_insured,0) <> 0
             )
         AND av.accumulation_code_5 in (SELECT level1_id FROM #tmpAccumulationFull)
         GROUP BY av.peril_type_id,
                 av.accumulation_code_5


     -- update 12 MONTH values for this level
     INSERT INTO #tempvalues
         SELECT av.peril_type_id,
            3,      -- 12 Month Period
            "12 Month Period",
             av.accumulation_code_5,
             NULL,
             NULL,
             NULL,
             NULL,
             sum(av.sum_insured),
             sum(av.coinsured_sum_insured),
             sum(av.treaty_sum_insured),
             sum(av.fac_sum_insured),
             sum(av.retained_sum_insured)
         FROM Accumulation_Values av
         WHERE av.risk_cnt IN(
             SELECT  DISTINCT(sd.risk_id)
             FROM    stats_detail sd
             WHERE   sd.stats_folder_cnt in (
                 SELECT  sf.stats_folder_cnt
                 FROM    stats_folder sf
                 WHERE sf.posting_period_number BETWEEN @12MonthPeriodID + 1 AND @CurrentPeriodID)
             )
         AND (isnull(av.sum_insured,0) <> 0
             OR isnull(av.coinsured_sum_insured,0) <> 0
             OR isnull(av.retained_sum_insured,0) <> 0
             OR isnull(av.treaty_sum_insured,0) <> 0
             OR isnull(av.fac_sum_insured,0) <> 0
             )
         AND av.accumulation_code_5 in (SELECT level1_id FROM #tmpAccumulationFull)
         GROUP BY av.peril_type_id,
                 av.accumulation_code_5
END
DROP TABLE #tmpAccumulation1
DROP TABLE #tmpAccumulation2
DROP TABLE #tmpAccumulation3
DROP TABLE #tmpAccumulation4
DROP TABLE #tmpAccumulation5

IF @CodeLevel = 0 SELECT @CodeLevel = 1

--*************************************************************************************************************
-- OUTPUT
INSERT INTO #tempRSAAccumulation
    SELECT tv.code1,
        tv.code2,
        tv.code3,
        tv.code4,
        tv.code5,
        (SELECT DISTINCT level1_description FROM #tmpAccumulationFull WHERE tv.code1 = level1_id),
        (SELECT DISTINCT level2_description FROM #tmpAccumulationFull WHERE tv.code2 = level2_id),
        (SELECT DISTINCT level3_description FROM #tmpAccumulationFull WHERE tv.code3 = level3_id),
        (SELECT DISTINCT level4_description FROM #tmpAccumulationFull WHERE tv.code4 = level4_id),
        (SELECT DISTINCT level5_description FROM #tmpAccumulationFull WHERE tv.code5 = level5_id),
        @CodeLevel,
        NULL,
        tv.PerilTypeID,
        pt.description,
        tv.PeriodID,
        tv.PeriodName,
        tv.SI,
        tv.COI,
        tv.TTY,
        tv.FAC,
        tv.RET,
        @dtCurrentPeriodEnd,
        @dtLastYearPeriodEndDate,
        @dt12MonthPeriodEnd,
        @CurrentPeriodID
    FROM #tempvalues tv
    JOIN Peril_Type pt ON tv.perilTypeID = pt.peril_type_id
    WHERE (pt.description = @PerilDesc
        OR @PerilDesc = 'ALL')

DROP TABLE #tmpAccumulationFull
DROP TABLE #tempvalues

IF EXISTS (SELECT * FROM #tempRSAAccumulation WHERE Level5Desc IS NOT NULL)
    SELECT @CountLevels = 5
ELSE
IF EXISTS (SELECT * FROM #tempRSAAccumulation WHERE Level4Desc IS NOT NULL)
    SELECT @CountLevels = 4
ELSE
IF EXISTS (SELECT * FROM #tempRSAAccumulation WHERE Level3Desc IS NOT NULL)
    SELECT @CountLevels = 3
ELSE
IF EXISTS (SELECT * FROM #tempRSAAccumulation WHERE Level2Desc IS NOT NULL)
    SELECT @CountLevels = 2
ELSE
    SELECT @CountLevels = 1

UPDATE #tempRSAAccumulation
    SET CountLevels = @CountLevels

--print 'test'
--select @CountLevels '@CountLevels', @CodeLevel '@CodeLevel'
SET NOCOUNT OFF
SELECT Level1Desc,
    Level2Desc,
    Level3Desc,
    Level4Desc,
    Level5Desc,
    StartLevel,
    CountLevels,
    PerilType,
    PeriodRangeID,
    PeriodRangeName,
    Gross,
    Coinsurance,
    Treaty,
    Facultative,
    Retained,
    dtCurrentPeriodEnd,
    dtLastYearEnd,
    dt12MonthsAgo,
    CurrentPeriodID
    FROM #tempRSAAccumulation
--ORDER BY 1, 12
DROP TABLE #tempRSAAccumulation

GO