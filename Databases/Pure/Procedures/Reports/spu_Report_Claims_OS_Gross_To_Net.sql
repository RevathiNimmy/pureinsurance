SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Claims_OS_Gross_To_Net'
GO


CREATE PROCEDURE spu_Report_Claims_OS_Gross_To_Net
        @company_id int,
        @sub_branch_id int=NULL, --AMJ
                @SalvageAndTPRecovery varchar(255),
                @PeriodDate varchar(255)
AS
/**********************************************************************************************************************************
** Created by Jude Killip
** 03/07/2001
** RSA Reports - Outstanding_Claims_Gross_To_Net.rpt
**
**********************************************************************************************************************************
** 25/10/2001   JMK add @SalvageAndTPRecovery parameter
**
** 14/11/2001   JMK include claims payments *-1
**                  take out sign change of TTY and NET records
**
** 14/11/2001   JMK don't include payments when Salvage and Recovery only
**
** 01/08/2002   AMJ - branch specific change
***********************************************************************************************************************************
** VER      DATE        WHO     WHAT
** 1.01     29/01/2002  JMK     use new lookup parameter "Period" - user selects from list of
**                              current and previous period_end_dates (as a string)
***********************************************************************************************************************************/

/*
    Claim status id constants
    1 = Provisional Open Claim
    2 = Live Open Claim
    3 = Closed
    4 = ReOpen
    5 = ReClosed

    @SalvageAndTPRecovery:  Exclude - Exclude Salvage and Recovery
                            Include - All
                            Only    - Salvage and Recovery Only

*/
SET NOCOUNT ON

/*
--FOR TESTING
DECLARE @SalvageAndTPRecovery varchar (10),
    @PeriodDate varchar (20)

SELECT @SalvageAndTPRecovery = 'exclude',
    @PeriodDate = 'Oct 31 2001'
*/
IF @sub_branch_id IS NULL
    EXEC spu_sub_branch_default @source_id=@company_id, @sub_branch_id=@sub_branch_id OUTPUT

-- which period do we want to base this report on?
DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime

SELECT @PeriodDate = @PeriodDate + " 23:59:59.000"
SELECT @dtSelectedPeriodEnd = CONVERT (Datetime, @PeriodDate)

SELECT @SelectedPeriodID = period_id
FROM Period
WHERE period_end_date = @dtSelectedPeriodEnd
AND   sub_branch_id = @sub_branch_id

-- Get IDs of all outstanding claims
CREATE TABLE #tempRSAClmOSGrossNet1
(
    ClaimID int
)
INSERT INTO #tempRSAClmOSGrossNet1
    SELECT c.claim_id
    FROM claim c
    WHERE c.claim_status_id not in (3,5)

CREATE TABLE #tempRSAClmOSGrossNet
(
    ClaimNum varchar (30) NULL,
    PolNum varchar (30) NULL,
    Client varchar (20) NULL,
    DocRef varchar (25) NULL,
    Agency varchar (20) NULL,
    ProductCode varchar (10) NULL,
    ProductDesc varchar (255) NULL,
    RiskTypeCode varchar (10) NULL,
    RiskTypeDescription varchar (255) NULL,
    Gross decimal (19,4) NULL,
    Coinsurance decimal (19,4) NULL,
    Treaty decimal (19,4) NULL,
    Facultative decimal (19,4) NULL,
    Retained decimal (19,4) NULL,
    TransDate datetime NULL,
    dtSelectedPeriodEnd datetime,
    PostingPeriodID int,
    SelectedPeriodID int
)

IF @SalvageAndTPRecovery = 'exclude'
BEGIN
-- print 'get outstanding claims with Reserves '
INSERT INTO #tempRSAClmOSGrossNet
    SELECT sf.loss_code,
        sf.insurance_ref,
        sf.insurance_holder_shortname,
        sf.document_ref,
        sf.agent_shortname,
        sf.product_code,
        p.description,
        sd.risk_type_code,
        rt.description,
        CASE sf.transaction_type_code
            WHEN 'C_CP' THEN
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS') *-1
            ELSE
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS')
            END,
        CASE sf.transaction_type_code
            WHEN 'C_CP' THEN
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI') *-1
            ELSE
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI')
            END,
        CASE sf.transaction_type_code
            WHEN 'C_CP' THEN
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'TTY') *-1
            ELSE
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'TTY')
            END,
        CASE sf.transaction_type_code
            WHEN 'C_CP' THEN
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'FAC') *-1
            ELSE
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'FAC')
            END,
        CASE sf.transaction_type_code
            WHEN 'C_CP' THEN
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'NET') *-1
            ELSE
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'NET')
            END,
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        sf.posting_period_number,
        @SelectedPeriodID
    FROM Stats_Folder sf
    JOIN #tempRSAClmOSGrossNet1 T1 ON ClaimID = sf.loss_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND isnull(sd.this_premium_home,0) <> 0
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE sf.transaction_type_code IN ('C_CO', 'C_CR', 'C_CP')
    AND sf.posting_period_number = @SelectedPeriodID
END
ELSE
IF @SalvageAndTPRecovery = 'only'
BEGIN
-- print 'get outstanding claims with Recoveries '
INSERT INTO #tempRSAClmOSGrossNet
    SELECT sf.loss_code,
        sf.insurance_ref,
        sf.insurance_holder_shortname,
        sf.document_ref,
        sf.agent_shortname,
        sf.product_code,
        p.description,
        sd.risk_type_code,
        rt.description,
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS'),
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI'),
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'TTY'),
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'FAC'),
        (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'NET'),
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        sf.posting_period_number,
        @SelectedPeriodID
    FROM Stats_Folder sf
    JOIN #tempRSAClmOSGrossNet1 T1 ON ClaimID = sf.loss_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND isnull(sd.this_premium_home,0) <> 0
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE sf.transaction_type_code IN ('C_SA', 'C_RV')
    AND sf.posting_period_number = @SelectedPeriodID
END
ELSE
BEGIN
-- print 'get outstanding claims, Reserves & Recoveries'
INSERT INTO #tempRSAClmOSGrossNet
    SELECT sf.loss_code,
        sf.insurance_ref,
        sf.insurance_holder_shortname,
        sf.document_ref,
        sf.agent_shortname,
        sf.product_code,
        p.description,
        sd.risk_type_code,
        rt.description,
        CASE sf.transaction_type_code
            WHEN 'C_CP' THEN
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS') *-1
            ELSE
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'GRS')
            END,
        CASE sf.transaction_type_code
            WHEN 'C_CP' THEN
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI') *-1
            ELSE
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'COI')
            END,
        CASE sf.transaction_type_code
            WHEN 'C_CP' THEN
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'TTY') *-1
            ELSE
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'TTY')
            END,
        CASE sf.transaction_type_code
            WHEN 'C_CP' THEN
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'FAC') *-1
            ELSE
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'FAC')
            END,
        CASE sf.transaction_type_code
            WHEN 'C_CP' THEN
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'NET') *-1
            ELSE
                (SELECT sd.this_premium_home WHERE sd.stats_detail_type = 'NET')
            END,
        sf.transaction_date,
        @dtSelectedPeriodEnd,
        sf.posting_period_number,
        @SelectedPeriodID
    FROM Stats_Folder sf
    JOIN #tempRSAClmOSGrossNet1 T1 ON ClaimID = sf.loss_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND isnull(sd.this_premium_home,0) <> 0
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    WHERE sf.transaction_type_code IN ('C_CO', 'C_CR', 'C_SA', 'C_RV', 'C_CP')
    AND sf.posting_period_number = @SelectedPeriodID
END

DROP TABLE #tempRSAClmOSGrossNet1

SET NOCOUNT OFF

SELECT * FROM #tempRSAClmOSGrossNet

DROP TABLE #tempRSAClmOSGrossNet
GO