SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


EXECUTE DDLDropProcedure 'spu_Report_Claims_Bordereau_SFU'
GO



/**********************************************************************************************************************************
** Created by Kerry Butler
** 17/10/2001
**
** NAME:        spu_Report_Claims_Bordereau_SFU
**
**
**********************************************************************************************************************************
** 1.1      JMK 29/11/2001  Change Parameter from Reinsurer to Treaty
**                          Remove link to Insurance_file (to prevent duplication of records)
**                          Get Client resolved name

** 1.2     SLJ  10/7/2002  Change 
** 1.3     TOM  02/06/2003  More accurate matching of ri_shortname with Treaty description
** 1.4		JT  11/08/2004	MultiCurrency Feature
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_Claims_Bordereau_SFU
        @PeriodDate varchar (20),
        @Treaty varchar (100),
        @TypeOfCurrency	Varchar(30),
        @GroupByCode	VARCHAR(30)

AS
/*
-- TEST
DECLARE @PeriodDate varchar (20),
        @Treaty varchar (100)
SELECT @perioddate = 'May 31 2003'
SELECT @Treaty = 'ALL'
*/


-- PW110402 - find which period we want to base this report on
DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime

SELECT @PeriodDate = @PeriodDate + " 23:59:59.000"
SELECT @dtSelectedPeriodEnd = CONVERT (Datetime, @PeriodDate)

SELECT @SelectedPeriodID = period_id
FROM Period
WHERE period_end_date = @dtSelectedPeriodEnd

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


CREATE TABLE #tempClaimsBordereau
(
    TreatyId        int NULL,
    TreatyCode      varchar(20) NULL,
    Treaty          varchar(100) NULL,
    PeriodYear      int         NULL,
    PeriodNo        datetime    NULL,
    PeriodEnd       datetime    NULL,
    ClientName      varchar(100) NULL,
    PolicyNumber    varchar(30) NULL,
    ClaimNumber     varchar(30) NULL,
    AnalysisCode    int         NULL,
    AnalysisDesc    varchar(255) NULL,
    CatLossCode     int         NULL,
    CatCode         varchar (50) NULL,
    CausationCode   int     NULL,
    PrimaryCause    varchar (50)    NULL,
    PaidClaim       decimal (19,4)  NULL,
    LossDesc        varchar (255)   NULL,
    StatsFolderCnt  int     NULL,
    StatsDetType    varchar (3) NULL,
    dtCurrentPeriodEnd datetime,
    SourceID			INT	NULL
)

DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod_SFU @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT

if @Treaty = 'ALL'
BEGIN
    INSERT into #tempClaimsBordereau
        SELECT sd.ri_party_cnt,
                sd.ri_shortname,
                NULL,
                sf.posting_period_year,
                sf.posting_period_number,
                NULL,
                (SELECT resolved_name FROM Party WHERE shortname = sf.insurance_holder_shortname),
                sf.insurance_ref,
                sf.loss_code,
                (SELECT max(analysis_code_id) FROM insurance_file WHERE insurance_ref = sf.insurance_ref),
                NULL,
                c.catastrophe_code_id,
                NULL,
                c.primary_cause_id,
                NULL,
                CASE @TypeOfCurrency 
                	When 'Base' THEN sd.this_premium_original
                	When 'System' THEN sd.this_premium_system
                END,
                LEFT(c.description,255),
                sf.stats_folder_cnt,
                sd.stats_detail_type,
                @dtCurrentPeriodEnd,sf.source_id
                
            FROM stats_folder sf
            JOIN stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt
            JOIN claim c ON c.claim_id = sf.loss_id
            WHERE stats_detail_type in ('TTY', 'COI')
            AND transaction_type_code in ('C_CP', 'C_RV')
            AND sf.posting_period_number = @SelectedPeriodID
END
ELSE
BEGIN
    INSERT into #tempClaimsBordereau
        SELECT sd.ri_party_cnt,
                sd.ri_shortname,
                NULL,
                sf.posting_period_year,
                sf.posting_period_number,
                NULL,
                sf.insurance_holder_shortname,
                sf.insurance_ref, 
                sf.loss_code,
                (SELECT max(analysis_code_id) FROM insurance_file WHERE insurance_ref = sf.insurance_ref),
                NULL,
                c.catastrophe_code_id,
                NULL,
                c.primary_cause_id,
                NULL,
                CASE @TypeOfCurrency 
					When 'Base' THEN sd.this_premium_original
					When 'System' THEN sd.this_premium_system
                END,
                LEFT(c.description,255),
                sf.stats_folder_cnt,
                sd.stats_detail_type,
                @dtCurrentPeriodEnd,sf.source_id

            FROM stats_folder sf
            JOIN stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt
            JOIN claim c ON c.claim_id = sf.loss_id
            WHERE stats_detail_type in ('TTY', 'COI')
            AND transaction_type_code in ('C_CP', 'C_RV')
            AND sf.posting_period_number = @SelectedPeriodID
            AND sd.ri_shortname = (SELECT code from Treaty where description = @Treaty)
END

UPDATE #tempClaimsBordereau
SET AnalysisDesc = a.description
FROM analysis_code a
WHERE a.analysis_code_id = AnalysisCode

UPDATE #tempClaimsBordereau
SET Primarycause = pc.description
FROM primary_cause pc
WHERE pc.primary_cause_id = CausationCode

UPDATE #tempClaimsBordereau
SET CatCode = cc.description
FROM catastrophe_code cc
WHERE cc.catastrophe_code_id = CatLossCode

UPDATE #tempClaimsBordereau
SET PeriodEnd = p.period_end_date
FROM period p
WHERE PeriodNo = p.period_id

UPDATE #tempClaimsBordereau
SET Treaty = t.description
FROM treaty t
WHERE convert(varchar(20),description) = TreatyCode
AND TreatyId is null

UPDATE #tempClaimsBordereau
SET Treaty = t.description
FROM treaty t
WHERE convert(varchar(20),description) = TreatyCode
AND TreatyId is not null

/*
UPDATE #tempAUAClaimsBordereau
SET Treaty = TreatyCode
WHERE Treaty is NULL
*/

SELECT TreatyCode,
    Treaty,
    PeriodYear,
    PeriodNo,
    PeriodEnd,
    ClientName,
    PolicyNumber,
    ClaimNumber,
    AnalysisCode,
    AnalysisDesc,
    CatLossCode,
    CatCode,
    CausationCode,
    PrimaryCause,
    PaidClaim,
    LossDesc,
    StatsFolderCnt,
    StatsDetType,
    Case @TypeOfCurrency
    	WHEN 'Base' THEN cb.Code
    	WHEN 'System' THEN @SystemCurrencyCode END CurrencyCode,
    Case @TypeOfCurrency
    	WHEN 'Base' THEN CB.description
    	WHEN 'System' THEN @SystemCurrencyDesc END CurrencyDesc,
    s.Code CompanyCode,s.Description CompanyDesc,
    Case @GroupByCode 
    	WHEN 'Branch' Then S.Code
    	WHEN 'Branch And Currency' THEN S.Code
    	ELSE ''
    END GroupByCode
    
    
FROM #tempClaimsBordereau TB
INNER JOIN SOURCE S ON S.source_id = TB.sourceid
INNER JOIN CURRENCY CB ON CB.currency_id= S.Base_currency_id
DROP TABLE #tempClaimsBordereau




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

