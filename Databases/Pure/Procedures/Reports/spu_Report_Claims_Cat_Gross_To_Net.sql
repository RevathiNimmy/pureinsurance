SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Claims_Cat_Gross_To_Net'
GO


CREATE PROCEDURE spu_Report_Claims_Cat_Gross_To_Net
                @CatastropheCode varchar(255),
                @IncludeClosed varchar(255),
                @PeriodDate varchar(255),
        @company_id int,
        @sub_branch_id int=NULL --AMJ
AS
/**********************************************************************************************************************************
** Created by Jude Killip
** 21/12/2001
** Reports - Claims_by_Catastrophe_Gross_To_Net.rpt
**
**********************************************************************************************************************************
** VER  DATE        WHO DESC
** 1.01 02/01/2002  JMK Exclude payments
**
** 1.02 03/01/2002  JMK Include Risk and Product for grouping
**
** 1.03 18/12/2001  JMK use new lookup parameter "Period" - user's selection from list of
**                      ..current and previous period_end_dates (as a string)
** 01/08/2002   AMJ - branch specific change
***********************************************************************************************************************************/
/*
    Claim status id constants
    1 = Provisional Open Claim
    2 = Live Open Claim
    3 = Closed
    4 = ReOpen
    5 = ReClosed

*/
SET NOCOUNT ON
IF @sub_branch_id IS NULL
    EXEC spu_sub_branch_default @source_id=@company_id, @sub_branch_id=@sub_branch_id OUTPUT

/*
--for testing
DECLARE @CatastropheCode varchar (60),
            @IncludeClosed varchar (5),
            @PeriodDate varchar (20)
SELECT @CatastropheCode = 'fire', @IncludeClosed = 'no', @PeriodDate = 'Oct 31 2001'
*/

-- which period do we want to base this report on?
DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime

SELECT @PeriodDate = @PeriodDate + " 23:59:59.000"
SELECT @dtSelectedPeriodEnd = CONVERT (Datetime, @PeriodDate)

SELECT @SelectedPeriodID = period_id
FROM Period
WHERE period_end_date = @dtSelectedPeriodEnd
AND   sub_branch_id = @sub_branch_id

-- Get IDs of all outstanding claims
CREATE TABLE #tempClmCatGrossNet1
(
    ClaimID int,
    CatastropheID int
)

DECLARE @SelectedCatID int
IF @CatastropheCode = 'ALL'
    SELECT @SelectedCatID = 0
ELSE
    SELECT @SelectedCatID = catastrophe_code_id FROM catastrophe_code WHERE description = @CatastropheCode

INSERT INTO #tempClmCatGrossNet1
    SELECT claim_id,
           catastrophe_code_id
    FROM claim
    WHERE isnull(catastrophe_code_id,0) <> 0
    AND (
        (claim_status_id in (2, 4) AND @IncludeClosed = 'no')
        OR (claim_status_id <> 1 AND @IncludeClosed = 'yes')
        )
    AND (
    (@SelectedCatID = catastrophe_code_id)
    OR (@SelectedCatID = 0)
    )

CREATE TABLE #tempClmCatGrossNet
(
    ClaimNum varchar (30) NULL,
    CatastropheCode varchar (50) NULL,
    ProductDescription  varchar(255) null,
    RiskDescription     varchar(255) null,
    PolNum varchar (30) NULL,
    Client varchar (20) NULL,
    DocRef varchar (25) NULL,
    Agency varchar (20) NULL,
    Gross decimal (19,4) NULL,
    Coinsurance decimal (19,4) NULL,
    Treaty decimal (19,4) NULL,
    Facultative decimal (19,4) NULL,
    Retained decimal (19,4) NULL,
    TransDate datetime NULL,
    dtSelectedPeriod datetime
)

-- print 'get outstanding claims with Reserves '
INSERT INTO #tempClmCatGrossNet
    SELECT sf.loss_code,
        (SELECT description FROM catastrophe_code WHERE catastrophe_code_id = T1.CatastropheID),
        (SELECT description FROM product WHERE product_id = sf.product_id),
        (SELECT description FROM risk_type WHERE risk_type_id = sd.risk_type_id),
        sf.insurance_ref,
        sf.insurance_holder_shortname,
        sf.document_ref,
        isnull(sf.agent_shortname, ' Direct'),
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
        @dtSelectedPeriodEnd
    FROM Stats_Folder sf
    JOIN #tempClmCatGrossNet1 T1 ON ClaimID = sf.loss_id
    JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND isnull(sd.this_premium_home,0) <> 0
    JOIN Product p          ON sf.product_id = p.product_id
    JOIN Risk_Type rt       ON sd.risk_type_id = rt.risk_type_id
    --WHERE sf.transaction_type_code IN ('C_CO', 'C_CR', 'C_CP')
    WHERE sf.transaction_type_code IN ('C_CO', 'C_CR')
    AND sf.posting_period_number = @SelectedPeriodID

DROP TABLE #tempClmCatGrossNet1

SET NOCOUNT OFF

SELECT * FROM #tempClmCatGrossNet

DROP TABLE #tempClmCatGrossNet
GO