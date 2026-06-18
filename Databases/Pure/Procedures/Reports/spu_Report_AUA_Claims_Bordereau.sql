SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_AUA_Claims_Bordereau'
GO


CREATE PROCEDURE spu_Report_AUA_Claims_Bordereau
        @Start_date datetime,
        @End_date datetime,
        @Treaty varchar (100),
        @Source_id int
AS
/**********************************************************************************************************************************
** Created by Kerry Butler
** 17/10/2001
**
** NAME:        spu_Report_AUA_Claims_Bordereau
**
**
**********************************************************************************************************************************
** 1.1      JMK 29/11/2001  Change Parameter from Reinsurer to Treaty
**                          Remove link to Insurance_file (to prevent duplication of records)
**                          Get Client resolved name
** 1.2      CMG/PB 05082002 Add source_id parameter to give filtering by Sub Branch
***********************************************************************************************************************************/
/*
-- test
declare @Start_date datetime,
        @End_date datetime,
        @Treaty varchar (100),
        @sub_branch_id int
select @Start_date = convert (datetime, '01-01-2001'),
@end_date = convert (datetime, '11-28-2001'),
@Treaty = 'Property Quota Share 01'
*/
declare @sub_branch_id int

CREATE TABLE #tempAUAClaimsBordereau
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
    StatsDetType    varchar (3) NULL

)

-- get default sub-branch for supplied source_id
EXEC spu_sub_branch_default @source_id, @sub_branch_id OUTPUT

-- get current period values
DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod @sub_branch_id, @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT

if @Treaty = 'ALL'
BEGIN
    INSERT into #tempAUAClaimsBordereau
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
                sd.this_premium_original,
                LEFT(c.description,255),
                sf.stats_folder_cnt,
                sd.stats_detail_type
            FROM stats_folder sf
            JOIN stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt
            JOIN claim c ON c.claim_id = sf.loss_id
            WHERE stats_detail_type in ('TTY', 'COI')
            AND transaction_type_code in ('C_CP', 'C_RV')
            AND sf.transaction_date BETWEEN @Start_date AND @End_date
END
ELSE
BEGIN
    INSERT into #tempAUAClaimsBordereau
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
                sd.this_premium_original,
                LEFT(c.description,255),
                sf.stats_folder_cnt,
                sd.stats_detail_type
            FROM stats_folder sf
            JOIN stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt
            JOIN claim c ON c.claim_id = sf.loss_id
            WHERE stats_detail_type in ('TTY', 'COI')
            AND transaction_type_code in ('C_CP', 'C_RV')
            AND sf.transaction_date BETWEEN @Start_date AND @End_date
            AND sd.ri_shortname = (SELECT code from Treaty where description = @Treaty)
END

UPDATE #tempAUAClaimsBordereau
SET AnalysisDesc = a.description
FROM analysis_code a
WHERE a.analysis_code_id = AnalysisCode

UPDATE #tempAUAClaimsBordereau
SET Primarycause = pc.description
FROM primary_cause pc
WHERE pc.primary_cause_id = CausationCode

UPDATE #tempAUAClaimsBordereau
SET CatCode = cc.description
FROM catastrophe_code cc
WHERE cc.catastrophe_code_id = CatLossCode

UPDATE #tempAUAClaimsBordereau
SET PeriodEnd = p.period_end_date
FROM period p
WHERE PeriodNo = p.period_id

UPDATE #tempAUAClaimsBordereau
SET Treaty = t.description
FROM treaty t
WHERE code = TreatyCode
AND TreatyId IS NULL

UPDATE #tempAUAClaimsBordereau
SET Treaty = p.resolved_name
FROM party p
WHERE party_cnt = TreatyId
AND TreatyId IS NOT NULL

UPDATE #tempAUAClaimsBordereau
SET Treaty = TreatyCode
WHERE Treaty is NULL

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
    StatsDetType
FROM #tempAUAClaimsBordereau

DROP TABLE #tempAUAClaimsBordereau

GO