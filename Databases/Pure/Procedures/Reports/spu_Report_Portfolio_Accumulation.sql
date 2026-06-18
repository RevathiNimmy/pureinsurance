SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Portfolio_Accumulation'
GO


CREATE PROCEDURE spu_Report_Portfolio_Accumulation
                    @AccumCode varchar(255)
AS
/**********************************************************************************************************************************
** Created by Jude Killip
** 05/11/2001
** Reports -  Portfolio_by_Accumulation.rpt
**
**********************************************************************************************************************************
** 1.1      09/11/2001  JMK     prevent "Warning: Null value eliminated from aggregate." appearing on server (sometimes)
**
** 1.2      09/11/2001  JMK     'group by' error in #tmpParty select
***********************************************************************************************************************************/
/*
-- start for testing
DECLARE @AccumCode varchar (10)
SELECT @AccumCode = 'ALL'
-- level 2 'NP', 'B97'
-- level 3 'ADE', 'BTN', 'AAT', 'B97 4', 'NASSAU'
-- level 1 'BHS001', 'B'
-- end for testing
*/

CREATE TABLE #tempStats
    (
        CobID int NULL,
        COB varchar (255) NULL,
        RiskID int NULL,
        InsRef varchar (30) NULL,
        InsHolderCnt int NULL,
        AccumulationID int NULL,
        SumInsured money NULL,
        GrossPrem money NULL
    )

INSERT INTO #tempStats
    SELECT  sd.class_of_business_id,
        (SELECT description FROM Class_Of_Business WHERE class_of_business_id = sd.class_of_business_id),
        sd.risk_id,
        sf.insurance_ref,
        sf.insurance_holder_cnt,
        (SELECT accumulation_id FROM Risk WHERE risk_cnt = sd.risk_id),
        (SELECT sd.sum_insured_home
        WHERE sd.peril_id =
            (SELECT min(isnull(sd2.peril_id,9999))  -- '09/11/2001  JMK
            FROM stats_detail sd2
            WHERE sd2.stats_folder_cnt = sd.stats_folder_cnt
            AND sd.stats_detail_type <> 'TTY'
            )
        ),
        sd.this_premium_home
    FROM stats_folder sf
    JOIN stats_detail sd ON sf.stats_folder_cnt = sd.stats_folder_cnt AND sd.stats_detail_type = 'GRS'
    WHERE sf.insurance_file_cnt IN
            (
            SELECT  max(insurance_file_cnt)
            FROM    insurance_file
            WHERE   cover_start_date <= getdate()
            AND     expiry_date >= getdate()
            GROUP BY insurance_folder_cnt
            )
    AND sf.transaction_type_code NOT LIKE ('C_%')

CREATE TABLE #tempParty
    (
        InsHolderCnt int NULL,
        InsuredFullName varchar(100) NULL,
        ABI varchar(70) NULL,
        Parent varchar (100) NULL

    )
INSERT INTO #tempParty
        SELECT pc.party_cnt,
        pc.resolved_name,
        (SELECT min(pcc.trade_code)
            FROM party_corporate_client pcc
            WHERE pcc.party_cnt = pc.party_cnt
            GROUP BY pcc.trade_code),
        (SELECT p.resolved_name
            FROM Party p
            WHERE p.party_cnt
            IN (SELECT min(pr.relation_cnt)
                FROM party_relationship pr
                WHERE pr.party_cnt = pc.party_cnt
                GROUP BY pr.relation_cnt))
        FROM party pc
        JOIN #tempStats t ON t.InsHolderCnt = pc.party_cnt
        WHERE pc.party_type_id = 4
    UNION
        SELECT pp.party_cnt,
        pp.resolved_name,
        (SELECT min(pl.occupation_code)
            FROM party_lifestyle pl
            WHERE pl.party_cnt = pp.party_cnt
            GROUP BY pl.occupation_code),
        (SELECT p.resolved_name
            FROM Party p
            WHERE p.party_cnt
            IN (SELECT min(pr.relation_cnt)
                FROM party_relationship pr
                WHERE pr.party_cnt = pp.party_cnt
                GROUP BY pr.party_cnt ))            -- JMK 09/11/2001
        FROM party pp
        JOIN #tempStats t ON t.InsHolderCnt = pp.party_cnt
        WHERE pp.party_type_id <> 4
--*************************************************************************************************************
DECLARE @CodeLevel int

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

CREATE TABLE #tmpAccumulation6
(
    acc_id int NULL,
    code varchar(10) NULL,
    description varchar(255) NULL,
    parent_id int NULL
)
--print 'level 6'
INSERT INTO #tmpAccumulation6
    SELECT accumulation_id,
        code,
        description,
        parent_id
    FROM Accumulation
    WHERE parent_id IN (SELECT acc_id FROM #tmpAccumulation5)

CREATE TABLE #tmpAccumulation7
(
    acc_id int NULL,
    code varchar(10) NULL,
    description varchar(255) NULL,
    parent_id int NULL
)
--print 'level 7'
INSERT INTO #tmpAccumulation7
    SELECT accumulation_id,
        code,
        description,
        parent_id
    FROM Accumulation
    WHERE parent_id IN (SELECT acc_id FROM #tmpAccumulation6)

CREATE TABLE #tmpAccumulation8
(
    acc_id int NULL,
    code varchar(10) NULL,
    description varchar(255) NULL,
    parent_id int NULL
)
--print 'level 8'
INSERT INTO #tmpAccumulation8
    SELECT accumulation_id,
        code,
        description,
        parent_id
    FROM Accumulation
    WHERE parent_id IN (SELECT acc_id FROM #tmpAccumulation7)

CREATE TABLE #tmpAccumulation9
(
    acc_id int NULL,
    code varchar(10) NULL,
    description varchar(255) NULL,
    parent_id int NULL
)
--print 'level 9'
INSERT INTO #tmpAccumulation9
    SELECT accumulation_id,
        code,
        description,
        parent_id
    FROM Accumulation
    WHERE parent_id IN (SELECT acc_id FROM #tmpAccumulation8)

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
    level5_description varchar(255) NULL,
    level6_id int NULL,
    level6_description varchar(255) NULL,
    level7_id int NULL,
    level7_description varchar(255) NULL,
    level8_id int NULL,
    level8_description varchar(255) NULL,
    level9_id int NULL,
    level9_description varchar(255) NULL
    )

IF @AccumCode = 'ALL'
    SELECT @CodeLevel = 0
ELSE
IF EXISTS (SELECT * FROM #tmpAccumulation1 WHERE code = @AccumCode)
    SELECT @CodeLevel = 1
ELSE
IF EXISTS (SELECT * FROM #tmpAccumulation2 WHERE code = @AccumCode)
    SELECT @CodeLevel = 2
ELSE
IF EXISTS (SELECT * FROM #tmpAccumulation3 WHERE code = @AccumCode)
    SELECT @CodeLevel = 3
ELSE
IF EXISTS (SELECT * FROM #tmpAccumulation4 WHERE code = @AccumCode)
    SELECT @CodeLevel = 4
ELSE
IF EXISTS (SELECT * FROM #tmpAccumulation5 WHERE code = @AccumCode)
    SELECT @CodeLevel = 5
ELSE
IF EXISTS (SELECT * FROM #tmpAccumulation6 WHERE code = @AccumCode)
    SELECT @CodeLevel = 6
ELSE
IF EXISTS (SELECT * FROM #tmpAccumulation7 WHERE code = @AccumCode)
    SELECT @CodeLevel = 7
ELSE
IF EXISTS (SELECT * FROM #tmpAccumulation8 WHERE code = @AccumCode)
    SELECT @CodeLevel = 8
ELSE
IF EXISTS (SELECT * FROM #tmpAccumulation9 WHERE code = @AccumCode)
    SELECT @CodeLevel = 9

IF @CodeLevel = 0
BEGIN
    INSERT INTO #tmpAccumulationFull
        SELECT 1,
            t1.acc_id, t1.description,
            t2.acc_id, t2.description,
            t3.acc_id, t3.description,
            t4.acc_id, t4.description,
            t5.acc_id, t5.description,
            t6.acc_id, t6.description,
            t7.acc_id, t7.description,
            t8.acc_id, t8.description,
            t9.acc_id, t9.description
        FROM #tmpAccumulation1 t1
        LEFT OUTER JOIN  #tmpAccumulation2 t2 ON t2.parent_id = t1.acc_id
        LEFT OUTER JOIN  #tmpAccumulation3 t3 ON t3.parent_id = t2.acc_id
        LEFT OUTER JOIN  #tmpAccumulation4 t4 ON t4.parent_id = t3.acc_id
        LEFT OUTER JOIN  #tmpAccumulation5 t5 ON t5.parent_id = t4.acc_id
        LEFT OUTER JOIN  #tmpAccumulation6 t6 ON t6.parent_id = t5.acc_id
        LEFT OUTER JOIN  #tmpAccumulation7 t7 ON t7.parent_id = t6.acc_id
        LEFT OUTER JOIN  #tmpAccumulation8 t8 ON t8.parent_id = t7.acc_id
        LEFT OUTER JOIN  #tmpAccumulation9 t9 ON t9.parent_id = t8.acc_id
END

IF @CodeLevel = 1
BEGIN
    INSERT INTO #tmpAccumulationFull
        SELECT @CodeLevel,
            t1.acc_id, t1.description,
            t2.acc_id, t2.description,
            t3.acc_id, t3.description,
            t4.acc_id, t4.description,
            t5.acc_id, t5.description,
            t6.acc_id, t6.description,
            t7.acc_id, t7.description,
            t8.acc_id, t8.description,
            t9.acc_id, t9.description
        FROM #tmpAccumulation1 t1
        LEFT OUTER JOIN  #tmpAccumulation2 t2 ON t2.parent_id = t1.acc_id
        LEFT OUTER JOIN  #tmpAccumulation3 t3 ON t3.parent_id = t2.acc_id
        LEFT OUTER JOIN  #tmpAccumulation4 t4 ON t4.parent_id = t3.acc_id
        LEFT OUTER JOIN  #tmpAccumulation5 t5 ON t5.parent_id = t4.acc_id
        LEFT OUTER JOIN  #tmpAccumulation6 t6 ON t6.parent_id = t5.acc_id
        LEFT OUTER JOIN  #tmpAccumulation7 t7 ON t7.parent_id = t6.acc_id
        LEFT OUTER JOIN  #tmpAccumulation8 t8 ON t8.parent_id = t7.acc_id
        LEFT OUTER JOIN  #tmpAccumulation9 t9 ON t9.parent_id = t8.acc_id
        WHERE t1.code = @AccumCode
END
IF @CodeLevel = 2
BEGIN
    INSERT INTO #tmpAccumulationFull
        SELECT @CodeLevel,
            t2.acc_id, t2.description,
            t3.acc_id, t3.description,
            t4.acc_id, t4.description,
            t5.acc_id, t5.description,
            t6.acc_id, t6.description,
            t7.acc_id, t7.description,
            t8.acc_id, t8.description,
            t9.acc_id, t9.description,
            NULL, NULL
        FROM  #tmpAccumulation2 t2
        LEFT OUTER JOIN  #tmpAccumulation3 t3 ON t3.parent_id = t2.acc_id
        LEFT OUTER JOIN  #tmpAccumulation4 t4 ON t4.parent_id = t3.acc_id
        LEFT OUTER JOIN  #tmpAccumulation5 t5 ON t5.parent_id = t4.acc_id
        LEFT OUTER JOIN  #tmpAccumulation6 t6 ON t6.parent_id = t5.acc_id
        LEFT OUTER JOIN  #tmpAccumulation7 t7 ON t7.parent_id = t6.acc_id
        LEFT OUTER JOIN  #tmpAccumulation8 t8 ON t8.parent_id = t7.acc_id
        LEFT OUTER JOIN  #tmpAccumulation9 t9 ON t9.parent_id = t8.acc_id
        WHERE t2.code = @AccumCode
END
If @CodeLevel = 3
BEGIN
    INSERT INTO #tmpAccumulationFull
        SELECT @CodeLevel,
            t3.acc_id, t3.description,
            t4.acc_id, t4.description,
            t5.acc_id, t5.description,
            t6.acc_id, t6.description,
            t7.acc_id, t7.description,
            t8.acc_id, t8.description,
            t9.acc_id, t9.description,
            NULL, NULL,
            NULL, NULL
        FROM  #tmpAccumulation3 t3
        LEFT OUTER JOIN  #tmpAccumulation4 t4 ON t4.parent_id = t3.acc_id
        LEFT OUTER JOIN  #tmpAccumulation5 t5 ON t5.parent_id = t4.acc_id
        LEFT OUTER JOIN  #tmpAccumulation6 t6 ON t6.parent_id = t5.acc_id
        LEFT OUTER JOIN  #tmpAccumulation7 t7 ON t7.parent_id = t6.acc_id
        LEFT OUTER JOIN  #tmpAccumulation8 t8 ON t8.parent_id = t7.acc_id
        LEFT OUTER JOIN  #tmpAccumulation9 t9 ON t9.parent_id = t8.acc_id
        WHERE t3.code = @AccumCode
END

DROP TABLE #tmpAccumulation1
DROP TABLE #tmpAccumulation2
DROP TABLE #tmpAccumulation3
DROP TABLE #tmpAccumulation4
DROP TABLE #tmpAccumulation5
DROP TABLE #tmpAccumulation6
DROP TABLE #tmpAccumulation7
DROP TABLE #tmpAccumulation8
DROP TABLE #tmpAccumulation9

--*************************************************************************************************************

--SELECT * FROM #tmpAccumulationFull


SELECT ts.COB,
    ts.InsRef,
    ts.InsHolderCnt,
    ts.SumInsured,
    ts.GrossPrem,
    tp.InsuredFullName,
    tp.ABI,
    tp.Parent,
    ta.level1_description,
    ta.level2_description,
    ta.level3_description,
    ta.level4_description,
    ta.level5_description,
    ta.level6_description,
    ta.level7_description,
    ta.level8_description,
    ta.level9_description
FROM #tempStats ts
JOIN #tempParty tp ON ts.InsHolderCnt = tp.InsHolderCnt
JOIN #tmpAccumulationFull ta ON
    (ts.accumulationId = ta.level1_id OR
    ts.accumulationId = ta.level2_id OR
    ts.accumulationId = ta.level3_id OR
    ts.accumulationId = ta.level4_id OR
    ts.accumulationId = ta.level5_id OR
    ts.accumulationId = ta.level6_id OR
    ts.accumulationId = ta.level7_id OR
    ts.accumulationId = ta.level8_id OR
    ts.accumulationId = ta.level9_id
    )

DROP TABLE #tempStats
DROP TABLE #tempParty
DROP TABLE #tmpAccumulationFull

GO
