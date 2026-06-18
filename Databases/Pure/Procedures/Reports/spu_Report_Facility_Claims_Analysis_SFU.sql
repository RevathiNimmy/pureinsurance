SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


EXECUTE DDLDropProcedure 'spu_Report_Facility_Claims_Analysis_SFU'
GO


/**********************************************************************************************************************************
** Created by Jude Killip
** 03/12/2001
** Reports - Facility_Claims_Analysis.rpt
** 1.00
**********************************************************************************************************************************
** 1.01     03/12/2001  Amend Treaty (date_to hh:mm:ss = 00:00:00)
** 1.02	JT	12/08/2004	MultiCurrency feature
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_Facility_Claims_Analysis_SFU
	@TypeOfCurrency	Varchar(30),
	@GroupByCode	Varchar(30)
AS
/*
    Claim status id constants
    1 = Provisional Open Claim
    2 = Live Open Claim
    3 = Closed
    4 = ReOpen
    5 = ReClosed
*/

-- get current period values
DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod_SFU @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT

-- get current year values
DECLARE @CurrentYearStartPeriodID int, @dtLastYearPeriodEndDate datetime
EXECUTE spu_Report_GetCurrentYear_SFU @CurrentYearStartPeriodID OUTPUT, @dtLastYearPeriodEndDate OUTPUT

/*Get System Currency Details*/
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
CREATE TABLE #tmpStats
(
    StatsFolderCnt int,
    CobID int NULL,
    CurrentTreaty varchar (20) NULL,
    Treaty varchar (20) NULL,
    CausationCodeID int NULL,
    Amount money NULL,
    SourceID	INT	--**added for multi currency 11/08/2004
)

--print 'OS Reserve Records'
INSERT INTO #tmpStats
    SELECT sd.stats_folder_cnt,
        sd.class_of_business_id,
        NULL,
        sd.ri_shortname,
        (SELECT top 1 primary_cause_id FROM Claim WHERE claim_number = sf.loss_code order by primary_cause_id Desc), --PN71312
        --sd.this_premium_home
        Case @TypeOfCurrency 
		        	WHEN 'Base' THEN sd.this_premium_home * -1
		        	WHEN 'System' THEN	sd.this_premium_system * -1
        END,sf.Source_id
        FROM  stats_detail sd
        JOIN stats_folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
        WHERE stats_detail_type = ('TTY')
        AND sf.transaction_type_code in ('C_CO', 'C_CR')           -- claims opened and maintained
    	AND(
			(@TypeOfCurrency = 'Base' and isnull(sd.this_premium_home,0) <> 0) 
			or 
			(@TypeOfCurrency = 'System' and isnull(sd.this_premium_system,0) <> 0)
		)
        AND(
            SELECT isnull(max(tef.accounts_export_status),'x')
            FROM transaction_export_folder tef WHERE sf.document_ref = tef.document_ref
            ) = 'c'
        AND sf.stats_folder_cnt IN (SELECT stats_folder_cnt
                                    FROM stats_detail
                                    WHERE ri_shortname IN (SELECT code FROM treaty))
        --AND (SELECT Claim_Status_id FROM claim WHERE Claim_Number = sf.loss_code) IN (2,4) -- open or reopened

--*********************************************************************************************************************************
-- START    Populate temporary table #tmpTreaty
--          Treaty details going back 6 Treaties
--*********************************************************************************************************************************
CREATE TABLE #tmpTreaty
    (
        tempID int IDENTITY,
        code1 varchar (10),
        replaces_id1 int,
        code2 varchar (10),
        replaces_id2 int,
        code3 varchar (10),
        replaces_id3 int,
        code4 varchar (10),
        replaces_id4 int,
        code5 varchar (10),
        replaces_id5 int,
        code6 varchar (10),
        replaces_id6 int,
    )

INSERT INTO #tmpTreaty
    SELECT t.code,
        t.replaces_treaty_id,
        NULL, NULL,
        NULL, NULL,
        NULL, NULL,
        NULL, NULL,
        NULL, NULL
    FROM treaty t
    WHERE t.treaty_id NOT IN (SELECT isnull(t1.replaces_treaty_id,'')
                        FROM treaty t1
                        WHERE t1.replaces_treaty_id = t.treaty_id
                        AND datediff(day, @dtCurrentPeriodEnd, t1.expiry_date) >=0
                        AND datediff(day, @dtCurrentPeriodEnd, t1.effective_date) <=0
                        AND t1.is_deleted = 0)
    AND datediff(day, @dtCurrentPeriodEnd, t.expiry_date) >=0
    AND datediff(day, @dtCurrentPeriodEnd, t.effective_date) <=0
    AND t.is_deleted = 0

--print 'current treaty -1'
UPDATE  #tmpTreaty
    SET code2 = t.code,
        replaces_id2 = t.replaces_treaty_id
    FROM treaty t
    JOIN #tmpTreaty tt ON t.treaty_id = tt.replaces_id1
    WHERE t.is_deleted = 0

--print 'current treaty -2'
UPDATE  #tmpTreaty
    SET code3 = t.code,
        replaces_id3 = t.replaces_treaty_id
    FROM treaty t
    JOIN #tmpTreaty tt ON t.treaty_id = tt.replaces_id2
    WHERE t.is_deleted = 0

--print 'current treaty -3'
UPDATE  #tmpTreaty
    SET code4 = t.code,
        replaces_id4 = t.replaces_treaty_id
    FROM treaty t
    JOIN #tmpTreaty tt ON t.treaty_id = tt.replaces_id3
    WHERE t.is_deleted = 0

--print 'current treaty -4'
UPDATE  #tmpTreaty
    SET code5 = t.code,
        replaces_id5 = t.replaces_treaty_id
    FROM treaty t
    JOIN #tmpTreaty tt ON t.treaty_id = tt.replaces_id4
    WHERE t.is_deleted = 0

--print 'current treaty -5'
UPDATE  #tmpTreaty
    SET code6 = t.code,
        replaces_id6 = t.replaces_treaty_id
    FROM treaty t
    JOIN #tmpTreaty tt ON t.treaty_id = tt.replaces_id5
    WHERE t.is_deleted = 0

--*********************************************************************************************************************************
-- END    Populate temporary table #tmpTreaty
--*********************************************************************************************************************************
--*********************************************************************************************************************************
-- START    Populate temporary table #tmpTreatyCurrent
--          Store Current Treaty against all previous Treaties
--*********************************************************************************************************************************
CREATE TABLE #tmpTreatyCurrent
    (
        CurrentTreaty varchar (20),
        Treaty varchar (20) NULL
    )

INSERT INTO #tmpTreatyCurrent
    SELECT tt.code1,
        tt.code1
    FROM #tmpTreaty tt
    WHERE tt.code1 IS NOT NULL

INSERT INTO #tmpTreatyCurrent
    SELECT tt.code1,
        tt.code2
    FROM #tmpTreaty tt
    WHERE tt.code2 IS NOT NULL

INSERT INTO #tmpTreatyCurrent
    SELECT tt.code1,
        tt.code3
    FROM #tmpTreaty tt
    WHERE tt.code3 IS NOT NULL

INSERT INTO #tmpTreatyCurrent
    SELECT tt.code1,
        tt.code4
    FROM #tmpTreaty tt
    WHERE tt.code4 IS NOT NULL

INSERT INTO #tmpTreatyCurrent
    SELECT tt.code1,
        tt.code5
    FROM #tmpTreaty tt
    WHERE tt.code5 IS NOT NULL

INSERT INTO #tmpTreatyCurrent
    SELECT tt.code1,
        tt.code6
    FROM #tmpTreaty tt
    WHERE tt.code6 IS NOT NULL

--*********************************************************************************************************************************
-- END    Populate temporary table #tmpTreatyCurrent
--*********************************************************************************************************************************
CREATE TABLE #tmpFacilityClmAnal
    (
        ClassOfBusiness varchar (255) NULL,
        CausationCode varchar (50) NULL,
        CurrentTreaty varchar (20) NULL,
        dtCurrentPeriodEnd datetime,
        RecordTypeID int,
        RecordType varchar (20),
        TreatyName1 varchar (20) NULL,
        TreatyName2 varchar (20) NULL,
        TreatyName3 varchar (20) NULL,
        TreatyName4 varchar (20) NULL,
        TreatyName5 varchar (20) NULL,
        TreatyName6 varchar (20) NULL,
        CurrentYear1 money NULL,
        PreviousYear2 money NULL,
        PreviousYear3 money NULL,
        PreviousYear4 money NULL,
        PreviousYear5 money NULL,
        PreviousYear6 money NULL,
        CountCurrentYear1 int NULL,
        CountPreviousYear2 int NULL,
        CountPreviousYear3 int NULL,
        CountPreviousYear4 int NULL,
        CountPreviousYear5 int NULL,
        CountPreviousYear6 int NULL,
        SourceID			INT NULL
    )

--
UPDATE #tmpStats
    SET CurrentTreaty = tc.CurrentTreaty
    FROM #tmpTreatyCurrent tc
    WHERE tc.Treaty = #tmpStats.Treaty

DROP TABLE #tmpTreatyCurrent

 --print 'get details together'
 --print 'Value'
INSERT INTO #tmpFacilityClmAnal
    SELECT (SELECT description FROM Class_Of_Business WHERE class_of_business_id = ts.CobID),
    (SELECT description FROM Primary_Cause where primary_cause_id = ts.CausationCodeID),
    ts.CurrentTreaty,
    @dtCurrentPeriodEnd,
    1,
    'By Value',
    tt.code1,
    tt.code2,
    tt.code3,
    tt.code4,
    tt.code5,
    tt.code6,
    CASE WHEN tt.code1 = ts.Treaty THEN
        ts.Amount
        END,
    CASE WHEN tt.code2 = ts.Treaty THEN
        ts.Amount
        END,
    CASE WHEN tt.code3 = ts.Treaty THEN
        ts.Amount
        END,
    CASE WHEN tt.code4 = ts.Treaty THEN
        ts.Amount
        END,
    CASE WHEN tt.code5 = ts.Treaty THEN
        ts.Amount
        END,
    CASE WHEN tt.code6 = ts.Treaty THEN
        ts.Amount
        END,
    NULL, NULL, NULL, NULL, NULL, NULL,ts.SourceID
    FROM #tmpStats ts
    JOIN #tmpTreaty tt ON tt.code1 = ts.CurrentTreaty

 --print 'Number'
INSERT INTO #tmpFacilityClmAnal
    SELECT ClassOfBusiness,
        CausationCode,
        CurrentTreaty,
        dtCurrentPeriodEnd,
        2,
        'By Number',
        TreatyName1,
        TreatyName2,
        TreatyName3,
        TreatyName4,
        TreatyName5,
        TreatyName6,
        NULL, NULL, NULL, NULL, NULL, NULL,
        CASE WHEN CurrentYear1 IS NOT NULL THEN
            1
            END,
        CASE WHEN PreviousYear2 IS NOT NULL THEN
            1
            END,
        CASE WHEN PreviousYear3 IS NOT NULL THEN
            1
            END,
        CASE WHEN PreviousYear4 IS NOT NULL THEN
            1
            END,
        CASE WHEN PreviousYear5 IS NOT NULL THEN
            1
            END,
        CASE WHEN PreviousYear6 IS NOT NULL THEN
            1
        END,SourceID
    FROM #tmpFacilityClmAnal
    WHERE RecordTypeID = 1

 --print 'Average'
INSERT INTO #tmpFacilityClmAnal
    SELECT ClassOfBusiness,
        CausationCode,
        CurrentTreaty,
        dtCurrentPeriodEnd,
        3,
        'Average Cost',
        TreatyName1,
        TreatyName2,
        TreatyName3,
        TreatyName4,
        TreatyName5,
        TreatyName6,
        CurrentYear1,
        PreviousYear2,
        PreviousYear3,
        PreviousYear4,
        PreviousYear5,
        PreviousYear6,
        CASE WHEN CurrentYear1 IS NOT NULL THEN
            1
            END,
        CASE WHEN PreviousYear2 IS NOT NULL THEN
            1
            END,
        CASE WHEN PreviousYear3 IS NOT NULL THEN
            1
            END,
        CASE WHEN PreviousYear4 IS NOT NULL THEN
            1
            END,
        CASE WHEN PreviousYear5 IS NOT NULL THEN
            1
            END,
        CASE WHEN PreviousYear6 IS NOT NULL THEN
            1
        END,SourceID
    FROM #tmpFacilityClmAnal
    WHERE RecordTypeID = 1

DROP TABLE #tmpStats
DROP TABLE  #tmpTreaty

SELECT *,S.Code CompanyCode,S.description CompanyDesc,
Case @TypeOfCurrency 
	WHEN 'System' THEN  @Systemcurrencycode
	WHEN 'Base' THEN C.Code
END CurrencyCode,
Case @TypeOfCurrency 
	WHEN 'System' THEN @SystemCurrencyDesc
	WHEN 'Base' THEN C.description
END CurrencyDesc,
Case @GroupbyCode 
	WHEN 'Branch' THEN S.Code
	WHEN 'Branch And Currency' THEN S.Code
ELSE ''
END 'GroupByCode'

FROM #tmpFacilityClmAnal TA
INNER JOIN SOurce S ON S.source_id = ta.sourceid
INNER JOIN CURRENCY C ON C.Currency_id= S.base_currency_id

DROP TABLE #tmpFacilityClmAnal

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
