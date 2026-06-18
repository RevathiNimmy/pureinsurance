SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_AUA_TransProcessing'
GO


CREATE PROCEDURE spu_Report_AUA_TransProcessing
    @Source_id int
AS

/**********************************************************************************************************************************
** Created by Kerry Butler
** 12/10/2001
**
** NAME:        spu_Report_AUA_TransProcessing
**      v1.1    7/12/01 Pick up one record per policy for count
**      v1.2    9/02/02 KB.  Use product rather than class of business but leave the variable name
**              unchanged to avoid changing the report.
**      v1.3    CMG/PB 05082002 Add source_id parameter to give filtering by Sub Branch
**
**********************************************************************************************************************************
**
***********************************************************************************************************************************/


DECLARE @CurrentPeriodID int
DECLARE @Current12PeriodID int
DECLARE @Current24PeriodID int
DECLARE @sub_branch_id int

-- get default sub-branch for supplied source_id
EXEC spu_sub_branch_default @source_id, @sub_branch_id OUTPUT

EXECUTE spu_Report_GetCurrentPeriod @sub_branch_id, @CurrentPeriodID OUTPUT,  NULL


EXECUTE spu_report_getcurrent12monthperiod @sub_branch_id, @Current12PeriodID OUTPUT, NULL


EXECUTE spu_report_getcurrent24monthperiod @sub_branch_id, @Current24PeriodID OUTPUT, NULL

print @CurrentPeriodID
print @Current12PeriodID
print @Current24PeriodID

CREATE TABLE #tempTransProc

(
    StatsFolder int     NULL,
    BranchCode  int         NULL,
    BranchDesc  varchar (255)   NULL,
    ClassofBusiness int     NULL,
    ClassOfBusinessDesc varchar (255) NULL,
    TransType   varchar(20)     NULL,
    PremiumNB   decimal (19,4)  NULL,       -- New Business
    NBCount     int     NULL,
    PremiumNBR  decimal (19,4)  NULL,       -- New Business Return
    NBRCount    int     NULL,
    PremiumREN  decimal (19,4)  NULL,       -- Renewal
    RENCount    int     NULL,
    PremiumRENR decimal (19,4)  NULL,       -- Renewal return
    RENRCount   int     NULL,
    AddPrem     decimal (19,4)  NULL,       -- AP
    APCount     int     NULL,
    RetPrem     decimal (19,4)  NULL,       -- RP
    RPCount     int     NULL,
    ClaimOpen   decimal (19,4)  NULL,       -- Claim open
    COCount     int     NULL,
    ClaimRevise decimal (19,4)  NULL,       -- Claim revision
    CRCount     int     NULL,
    ClaimPaid   decimal (19,4)  NULL,       -- Claim payment
    CPCount     int     NULL,
    ClaimSalvage    decimal (19,4)  NULL,       -- Salvage
    CSCount     int     NULL,
    ClaimRecovery   decimal (19,4)  NULL,       -- Recovery
    CVCount     int     NULL,

    PeriodNo    int     NULL,
    PeriodYear  int     NULL,
    PeriodRange int     NULL        -- 1 this period
                            -- 2 this year to date (exc this period)
                            -- 3 this period last year
                            -- 4 last year cumulative
)

INSERT INTO #tempTransProc

SELECT
    sf.stats_folder_cnt,
    branch_id,
    NULL,
    class_of_business_id,
    --NULL,
    sf.product_code,
    transaction_type_code,
    (SELECT this_premium_home WHERE transaction_type_code = 'NB'),
    (SELECT 1 WHERE transaction_type_code = 'NB' and stats_detail_id = 1),
    (SELECT this_premium_home WHERE transaction_type_code = 'NBR'),
    (SELECT 1 WHERE transaction_type_code = 'NBR'and stats_detail_id = 1),
    (SELECT this_premium_home WHERE transaction_type_code = 'REN'),
    (SELECT 1 WHERE transaction_type_code = 'REN'and stats_detail_id = 1),
    (SELECT this_premium_home WHERE transaction_type_code = 'RENR'),
    (SELECT 1 WHERE transaction_type_code = 'RENR'and stats_detail_id = 1),
    (SELECT this_premium_home WHERE transaction_type_code = 'MTA'),
    (SELECT 1 WHERE transaction_type_code = 'MTA'and stats_detail_id = 1),
    (SELECT this_premium_home WHERE transaction_type_code = 'MTC'),
    (SELECT 1 WHERE transaction_type_code = 'MTC'and stats_detail_id = 1),
    (SELECT this_premium_home WHERE transaction_type_code = 'C_CO'),
    (SELECT 1 WHERE transaction_type_code = 'C_CO'),
    (SELECT this_premium_home WHERE transaction_type_code = 'C_CR'),
    (SELECT 1 WHERE transaction_type_code = 'C_CR'),
    (SELECT this_premium_home WHERE transaction_type_code = 'C_CP'),
    (SELECT 1 WHERE transaction_type_code = 'C_CP'),
    (SELECT this_premium_home WHERE transaction_type_code = 'C_SA'),
    (SELECT 1 WHERE transaction_type_code = 'C_SA'),
    (SELECT this_premium_home WHERE transaction_type_code = 'C_RV'),
    (SELECT 1 WHERE transaction_type_code = 'C_RV'),
    posting_period_number,
    posting_period_year,
    NULL
FROM stats_folder sf


LEFT OUTER JOIN stats_detail sd ON sd.stats_folder_cnt = sf.stats_folder_cnt

WHERE  stats_detail_type = 'GRS'

UPDATE
    #tempTransProc
    SET periodrange = 1
    WHERE periodno = @CurrentPeriodID

UPDATE
    #tempTransProc
    SET periodrange = 2
    WHERE periodno BETWEEN @Current12PeriodID AND ( @CurrentPeriodID - 1)

UPDATE
    #tempTransProc
    SET periodrange = 3
    WHERE periodno = @Current12PeriodID

UPDATE
    #tempTransProc
    SET periodrange = 4
    WHERE periodno BETWEEN @Current24PeriodID AND ( @Current12PeriodID - 1 )


-- KB 9/1/2 No longer needed as we are now using product.
--UPDATE
--  #tempTransProc
--  SET ClassofBusinessDesc = c.description
--  FROM class_of_business c
--  WHERE c.class_of_business_id = ClassofBusiness

UPDATE
    #tempTransProc
    --SET BranchDesc = b.description
    --FROM branch b
    --WHERE b.branch_id = BranchCode
    SET BranchDesc = s.code
    FROM source s
    WHERE s.source_id = BranchCode


SET NOCOUNT OFF

-- Squirt it all out to the report

SELECT * FROM #tempTransProc
DROP TABLE #tempTransProc

GO