EXECUTE DDLDropProcedure 'spu_Report_Accumulations_SFU'
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO


/**********************************************************************************************************************************
** Created by Jude Killip
** 20/09/2000
** RSA Reports - Accumulations.rpt
**
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
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     31/01/2002  JMK     re-released separately - (missed from patch 1.6.15)
**
** 1.02     22/07/2002  JMK     F0047028
**                              - rolling 12 months only
**                              - Limit to In Force policies
**                              - Add @PeriodDate and @DetailOrSummary parameters
**                              - Add Policy information
**
** 1.03     16/08/2002  JMK     F0047028
**                              - limit to risks from the latest version of the policy, to pick up the total sum insured at that time
**                              - amend date selection and policy info retrieval
**
** 1.04     21/08/2002  JMK     F0047028
**                              - Missing policy details on some Policies where MTA extended cover. Amend selection
**
** 1.05     11/09/2002  JMK     - limit policies to: cover start <= selected date and expiry >= selected date
**
** 1.06     08/10/2002  JMK     - Amend current policy selection (include MTA Temp)
**                              - Amend risk selection (get info from Risk rather than Stats)
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_Accumulations_SFU
                        @PerilDesc varchar (255),
                        @AccumDesc varchar (255),
                        @Period_End_Date DateTime,   
                        @DetailOrSummary varchar (10)
AS

/*
-- for testing @PeriodDate parameter
DECLARE @PeriodDate varchar (20)
SELECT @PeriodDate = 'Aug 31 2002'

--for testing @AccumDesc and @PerilDesc and @DetailOrSummary parameters
-- level 2 'NP', 'B97'
-- level 3 'ADE', 'BTN', 'AAT', 'B97 4', 'NASSAU'
-- level 1 'BHS001', 'B','Eqecat - FREEPORT'
DECLARE @PerilDesc varchar (255),
        @AccumDesc varchar (255),
        @DetailOrSummary varchar(10)

SELECT @PerilDesc = 'ALL',
        @AccumDesc = 'ALL',
        @DetailOrSummary = 'Detail'

*/
/*
-- for testing a single policy
DECLARE @InsRef varchar (50)

SELECT @PerilDesc = 'All', @AccumDesc = 'all',
--@InsRef = 'c/35/co/100019'
*/

SET NOCOUNT ON

--*************************************************************************************************************
-- which period do we want to base this report on?
DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime

SELECT @Period_End_Date = convert(datetime, convert(varchar(70), @Period_End_Date, 111)) 
Select @dtSelectedPeriodEnd = CONVERT (Datetime, @Period_End_Date)

--*************************************************************************************************************
-- Get Policy Information

CREATE TABLE #PolicyInfo
(
    PolicyNum varchar (50),
    InsFolderCnt int,
    FirstInsFileCnt int,
    CoverStartDate datetime,
    CurrentInsFileCnt int,
    ExpiryDate datetime
)

-- Get all CURRENT versions of Policies that fall within the selected time period
INSERT INTO #PolicyInfo
    SELECT  max(insurance_ref),
        insurance_folder_cnt,
        NULL,
        NULL,
        max(insurance_file_cnt),
        max(expiry_date)
    FROM    insurance_file i
   WHERE   i.cover_start_date <= @dtSelectedPeriodEnd  
        AND (i.lapsed_date IS NULL OR (i.lapsed_date IS NOT NULL AND i.lapsed_date < 'Jan 01 1901') 
               OR (i.lapsed_date IS NOT NULL AND i.lapsed_date >= @dtSelectedPeriodEnd))  -- cover starts before selected date    
        --  PM020702 expires after selected date OR (PM020702 - 27/06/2012) policy is still live
        AND (i.expiry_date >= @dtSelectedPeriodEnd And ((SELECT TOP 1 i1.insurance_file_status_id  
                     FROM insurance_file i1 WHERE i1.insurance_folder_cnt = i.insurance_folder_cnt 
                     ORDER BY i1.insurance_file_cnt DESC) IS NULL or i.lapsed_date IS NULL or i.lapsed_date >=@dtSelectedPeriodEnd))        
        AND    
        (    
            (i.insurance_file_status_id IS NULL    
            AND    
            i.insurance_file_type_id IN (2,5,6,9))          -- 2 = Live Policy, 5 = MTA Permanent, 6 = MTA Temp    
            OR    
            i.insurance_file_status_id IN (3,4,2,5,6)           -- 3 = Under Renewal, 4 = Replaced    
            OR 
            (i.insurance_file_status_id = 1 AND           -- 1 = Cancelled 
               EXISTS( select cover_start_date from insurance_file where @dtSelectedPeriodEnd < cover_start_date 
                AND insurance_file_cnt in ( select MAX(insurance_file_cnt) from insurance_file 
                                           where insurance_folder_cnt = i.insurance_folder_cnt 
                                           and insurance_file_type_id = 8))
            )
        )    
        AND     NOT EXISTS(SELECT ifl1.insurance_file_cnt   
				FROM insurance_file_risk_link ifl1 INNER JOIN insurance_file_risk_link ifl2   
				ON ifl1.original_risk_cnt = ifl2.risk_cnt INNER JOIN insurance_file inf   
				ON ifl2.insurance_file_cnt = inf.insurance_file_cnt   
				WHERE ISNULL(inf.insurance_file_status_id, 0) in (5,6)   
				AND ifl1.insurance_file_cnt = ifl2.insurance_file_cnt)         
		GROUP BY insurance_folder_cnt    

-- Table for all ORIGINAL versions of policies (exclude MTAs) relative to the Current versions
CREATE TABLE #OriginalPolicies
(
    OriginalInsFolder int,
    OriginalInsFile int,
    OriginalCoverStart datetime
)

INSERT INTO #OriginalPolicies
    SELECT  insurance_folder_cnt,
            max(insurance_file_cnt),
            max(cover_start_date)
    FROM    insurance_file
    JOIN    #PolicyInfo ON insurance_folder_cnt = InsFolderCnt
    WHERE   insurance_file_type_id = 2
    AND     (isnull(insurance_file_status_id,3) IN (3,4,2,6)     -- 3 = Under Renewal, 4 = Replaced
		OR	(isnull(insurance_file_status_id,3) = 1 AND           -- 1 = Cancelled 
               EXISTS( select cover_start_date from insurance_file where @dtSelectedPeriodEnd < cover_start_date 
                AND insurance_file_cnt in ( select MAX(insurance_file_cnt) from insurance_file 
                                           where insurance_folder_cnt = insurance_folder_cnt 
                                           and insurance_file_type_id = 8))
			)
		)
    AND     insurance_file_cnt <= CurrentInsFileCnt         -- most recent original version relative to Current
    GROUP BY insurance_folder_cnt

-- Update PolicyInfo table
UPDATE #PolicyInfo
    SET FirstInsFileCnt = OriginalInsFile,
        CoverStartDate = OriginalCoverStart
    FROM #OriginalPolicies
    WHERE InsFolderCnt = OriginalInsFolder

-- Delete any without policy details - during test these few were all Cancelled policies
DELETE #PolicyInfo
WHERE FirstInsFileCnt is null

--*************************************************************************************************************
-- Select Risks from In Force policies
CREATE TABLE #tempRiskCnt
(
    RiskCnt int
)
INSERT INTO #tempRiskCnt
    SELECT  DISTINCT(r.risk_cnt)
    FROM    risk r
    JOIN    insurance_file_risk_link ifrl   ON ifrl.risk_cnt = r.risk_cnt
    JOIN    #PolicyInfo                     ON ifrl.insurance_file_cnt = CurrentInsFileCnt
    WHERE   isnull(accumulation_id,0) <> 0
    AND     ifrl.status_flag <> 'D'

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

--*************************************************************************************************************
-- Bring all Accumulation levels together
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

-- Table for values by peril and code
CREATE TABLE #tempvalues
(
    perilTypeID int,
    code1 int,
    code2 int,
    code3 int,
    code4 int,
    SI money,
    COI money,
    TTY money,
    FAC money,
    RET money,
    RiskCnt int
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
            ISNULL(t4.acc_id,0), t4.description
        FROM #tmpAccumulation1 t1
        LEFT OUTER JOIN  #tmpAccumulation2 t2 ON t2.parent_id = t1.acc_id
        LEFT OUTER JOIN  #tmpAccumulation3 t3 ON t3.parent_id = t2.acc_id
        LEFT OUTER JOIN  #tmpAccumulation4 t4 ON t4.parent_id = t3.acc_id

    -- update with values for this level
    INSERT INTO #tempvalues
        SELECT av.peril_type_id,
            av.accumulation_code_1,
            av.accumulation_code_2,
            av.accumulation_code_3,
            av.accumulation_code_4,
            av.sum_insured,
            av.coinsured_sum_insured,
            av.treaty_sum_insured,
            av.fac_sum_insured,
            av.retained_sum_insured,
            av.risk_cnt
        FROM Accumulation_Values av
        WHERE av.risk_cnt IN
            (
            SELECT  DISTINCT RiskCnt
            FROM #tempRiskCnt
            )
        AND (isnull(av.sum_insured,0) <> 0
            OR isnull(av.coinsured_sum_insured,0) <> 0
            OR isnull(av.retained_sum_insured,0) <> 0
            OR isnull(av.treaty_sum_insured,0) <> 0
            OR isnull(av.fac_sum_insured,0) <> 0
            )
        AND av.accumulation_code_1 IN (SELECT level1_id FROM #tmpAccumulationFull)

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
             ISNULL(t4.acc_id,0), t4.description
         FROM #tmpAccumulation1 t1
         LEFT OUTER JOIN  #tmpAccumulation2 t2 ON t2.parent_id = t1.acc_id
         LEFT OUTER JOIN  #tmpAccumulation3 t3 ON t3.parent_id = t2.acc_id
         LEFT OUTER JOIN  #tmpAccumulation4 t4 ON t4.parent_id = t3.acc_id
         WHERE t1.description = @AccumDesc
--print 'test contents of #tmpAccumulationFull'
--select * from #tmpAccumulationFull

     -- update with values for this level
    INSERT INTO #tempvalues
        SELECT av.peril_type_id,
            av.accumulation_code_1,
            av.accumulation_code_2,
            av.accumulation_code_3,
            av.accumulation_code_4,
            av.sum_insured,
            av.coinsured_sum_insured,
            av.treaty_sum_insured,
            av.fac_sum_insured,
            av.retained_sum_insured,
            av.risk_cnt
        FROM Accumulation_Values av
        WHERE av.risk_cnt IN
            (
            SELECT  DISTINCT RiskCnt
            FROM #tempRiskCnt
            )
        AND (isnull(av.sum_insured,0) <> 0
            OR isnull(av.coinsured_sum_insured,0) <> 0
            OR isnull(av.retained_sum_insured,0) <> 0
            OR isnull(av.treaty_sum_insured,0) <> 0
            OR isnull(av.fac_sum_insured,0) <> 0
            )
         AND av.accumulation_code_1 in (SELECT level1_id FROM #tmpAccumulationFull)
END

--*********************************************************************************************************************
IF @CodeLevel = 2
BEGIN
    INSERT INTO #tmpAccumulationFull
        SELECT @CodeLevel,
            ISNULL(t2.acc_id,0), t2.description,
            ISNULL(t3.acc_id,0), t3.description,
            ISNULL(t4.acc_id,0), t4.description,
            NULL, NULL
        FROM  #tmpAccumulation2 t2
        LEFT OUTER JOIN  #tmpAccumulation3 t3 ON t3.parent_id = t2.acc_id
        LEFT OUTER JOIN  #tmpAccumulation4 t4 ON t4.parent_id = t3.acc_id
        WHERE t2.description = @AccumDesc

     -- update with values for this level
    INSERT INTO #tempvalues
        SELECT av.peril_type_id,
            av.accumulation_code_2,
            av.accumulation_code_3,
            av.accumulation_code_4,
            NULL,
            av.sum_insured,
            av.coinsured_sum_insured,
            av.treaty_sum_insured,
            av.fac_sum_insured,
            av.retained_sum_insured,
            av.risk_cnt
        FROM Accumulation_Values av
        WHERE av.risk_cnt IN
            (
            SELECT  DISTINCT RiskCnt
            FROM #tempRiskCnt
            )
        AND (isnull(av.sum_insured,0) <> 0
            OR isnull(av.coinsured_sum_insured,0) <> 0
            OR isnull(av.retained_sum_insured,0) <> 0
            OR isnull(av.treaty_sum_insured,0) <> 0
            OR isnull(av.fac_sum_insured,0) <> 0
            )
         AND av.accumulation_code_2 in (SELECT level1_id FROM #tmpAccumulationFull)
END

--*********************************************************************************************************************
IF @CodeLevel = 3
BEGIN
    INSERT INTO #tmpAccumulationFull
        SELECT @CodeLevel,
            ISNULL(t3.acc_id,0), t3.description,
            ISNULL(t4.acc_id,0), t4.description,
            NULL, NULL,
            NULL, NULL
        FROM  #tmpAccumulation3 t3
        LEFT OUTER JOIN  #tmpAccumulation4 t4 ON t4.parent_id = t3.acc_id
        WHERE t3.description = @AccumDesc

     -- update with values for this level
    INSERT INTO #tempvalues
        SELECT av.peril_type_id,
            av.accumulation_code_3,
            av.accumulation_code_4,
            NULL,
            NULL,
            av.sum_insured,
            av.coinsured_sum_insured,
            av.treaty_sum_insured,
            av.fac_sum_insured,
            av.retained_sum_insured,
            av.risk_cnt
        FROM Accumulation_Values av
        WHERE av.risk_cnt IN
            (
            SELECT  DISTINCT RiskCnt
            FROM #tempRiskCnt
            )
        AND (isnull(av.sum_insured,0) <> 0
            OR isnull(av.coinsured_sum_insured,0) <> 0
            OR isnull(av.retained_sum_insured,0) <> 0
            OR isnull(av.treaty_sum_insured,0) <> 0
            OR isnull(av.fac_sum_insured,0) <> 0
            )
         AND av.accumulation_code_3 in (SELECT level1_id FROM #tmpAccumulationFull)
END

--*********************************************************************************************************************
IF @CodeLevel = 4
BEGIN
    INSERT INTO #tmpAccumulationFull
        SELECT @CodeLevel,
            ISNULL(t4.acc_id,0), t4.description,
            NULL, NULL,
            NULL, NULL,
            NULL, NULL
        FROM  #tmpAccumulation4 t4
        WHERE t4.description = @AccumDesc

     -- update with values for this level
    INSERT INTO #tempvalues
        SELECT av.peril_type_id,
            av.accumulation_code_4,
            NULL,
            NULL,
            NULL,
            av.sum_insured,
            av.coinsured_sum_insured,
            av.treaty_sum_insured,
            av.fac_sum_insured,
            av.retained_sum_insured,
            av.risk_cnt
        FROM Accumulation_Values av
        WHERE av.risk_cnt IN
            (
            SELECT  DISTINCT RiskCnt
            FROM #tempRiskCnt
            )
        AND (isnull(av.sum_insured,0) <> 0
            OR isnull(av.coinsured_sum_insured,0) <> 0
            OR isnull(av.retained_sum_insured,0) <> 0
            OR isnull(av.treaty_sum_insured,0) <> 0
            OR isnull(av.fac_sum_insured,0) <> 0
            )
         AND av.accumulation_code_4 in (SELECT level1_id FROM #tmpAccumulationFull)
END
--print 'test contents of #tempvalues'
--select * from #tempvalues

IF @CodeLevel = 0 SELECT @CodeLevel = 1
--*************************************************************************************************************
-- OUTPUT
CREATE TABLE #tempRSAAccumulation
(
    Level1Code int NULL,
    Level2Code int NULL,
    Level3Code int NULL,
    Level4Code int NULL,
    Level1Desc varchar(255) NULL,
    Level2Desc varchar(255) NULL,
    Level3Desc varchar(255) NULL,
    Level4Desc varchar(255) NULL,
    StartLevel int NULL,
    CountLevels int NULL,
    PerilID int NULL,
    PerilType varchar (255) NULL,
    Gross decimal (19,4) NULL,
    Coinsurance decimal (19,4) NULL,
    Treaty decimal (19,4) NULL,
    Facultative decimal (19,4) NULL,
    Retained decimal (19,4) NULL,
    dtSelectedPeriodEnd datetime,
    InsFolderCnt int
)

INSERT INTO #tempRSAAccumulation
    SELECT tv.code1,
        tv.code2,
        tv.code3,
        tv.code4,
        (SELECT DISTINCT level1_description FROM #tmpAccumulationFull WHERE tv.code1 = level1_id),
        (SELECT DISTINCT level2_description FROM #tmpAccumulationFull WHERE tv.code2 = level2_id),
        (SELECT DISTINCT level3_description FROM #tmpAccumulationFull WHERE tv.code3 = level3_id),
        (SELECT DISTINCT level4_description FROM #tmpAccumulationFull WHERE tv.code4 = level4_id),
        @CodeLevel,
        NULL,
        tv.PerilTypeID,
        pt.description,
        tv.SI,
        tv.COI,
        tv.TTY,
        tv.FAC,
        tv.RET,
        @dtSelectedPeriodEnd,
        (SELECT distinct insurance_folder_cnt FROM insurance_file WHERE insurance_file_cnt IN
            (SELECT distinct insurance_file_cnt FROM insurance_file_risk_link WHERE risk_cnt = tv.riskcnt)
        )
    FROM #tempvalues tv
    JOIN Peril_Type pt ON tv.perilTypeID = pt.peril_type_id
    WHERE (pt.description = @PerilDesc
        OR @PerilDesc = 'ALL')

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
IF @DetailOrSummary = 'Summary'
BEGIN
    SELECT Level1Desc,
        Level2Desc,
        Level3Desc,
        Level4Desc,
        Max(StartLevel) as StartLevel ,
        Max(CountLevels) as CountLevels,
        PerilType,
        Sum(Gross) as Gross,
        Sum(Coinsurance) as Coinsurance,
        Sum(Treaty) as Treaty,
        Sum(Facultative) as Facultative,
        Sum(Retained) as Retained,
        Max(dtSelectedPeriodEnd) as dtSelectedPeriodEnd,
        NULL PolicyNum,
        NULL CoverStartDate,
        NULL ExpiryDate
    FROM #tempRSAAccumulation
    GROUP BY PerilType, Level1Desc, Level2Desc, Level3Desc, Level4Desc
END
ELSE
BEGIN
    SELECT Level1Desc,
        Level2Desc,
        Level3Desc,
        Level4Desc,
        StartLevel,
        CountLevels,
        PerilType,
        Gross,
        Coinsurance,
        Treaty,
        Facultative,
        Retained,
        dtSelectedPeriodEnd,
        -- get Policy Info
        PolicyNum,
        CoverStartDate,
        ExpiryDate
    FROM #tempRSAAccumulation a
    JOIN #PolicyInfo p ON a.InsFolderCnt = p.InsFolderCnt
END

--DROP TABLE #tempInsFileCnt
DROP TABLE #OriginalPolicies
DROP TABLE #PolicyInfo
DROP TABLE #tempRiskCnt
DROP TABLE #tmpAccumulation1
DROP TABLE #tmpAccumulation2
DROP TABLE #tmpAccumulation3
DROP TABLE #tmpAccumulation4
DROP TABLE #tmpAccumulationFull
DROP TABLE #tempvalues
DROP TABLE #tempRSAAccumulation
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

