SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_AUA_SPU2'
GO


CREATE PROCEDURE spu_Report_AUA_SPU2
                    @branch_id int,
                    @start_date datetime,
                    @end_date datetime
AS

/**********************************************************************************************************************************
** Created by Kerry Butler
** 19/9/2001
** AUA Reports - SPU2
**      *in progress- fields not complete
** v1.1 2/1/2 Added values for IPT and VAT
** v1.2     9/1/02 KB   Use Source table rather than Branch table to pick up the branch name.
** v1.3     1/3/02 KB   Populate finance charge from the premium finance tables now that
**              instalments are available (Totalcost -  AmountToFinance)
**                              Also use source_id rather than code to pick up Branch name.
** v1.4 PW090402 - get agent commission from GRS records because it is not on TTY records
**
**********************************************************************************************************************************
**
***********************************************************************************************************************************/
SET NOCOUNT ON

CREATE TABLE #tempAUASPU2
(
    StatsFolder     int                 NULL,
    PeriodName      varchar (15)        NULL,
    ReinsurerShortName  varchar (25)    NULL,
    ReinsurerName       varchar (100)   NULL, -- actually Treaty name
    ClassOfBusiness     varchar (100)   NULL,
    GrossPremium        decimal (19,4)  NULL,
    IPT         decimal (19,4)          NULL,
    FinanceCharge       decimal (19,4)  NULL,
    AgentCommission     decimal (19,4)  NULL,
    VAT                 decimal (19,4)  NULL,
    BranchId            int             NULL,
    BranchDesc          varchar (255)   NULL,
-- PW090402 - add required fields to link TTY records back to GRS
    PerilType           int             NULL,
    SharePercent        decimal (12,8)  NULL
)

-- PW090402 - Create temp table to hold the agent commission grouped by peril type

CREATE TABLE #tempAUASPU21
(
    AgentCommission decimal (19,4)      NULL,
    StatsCount      int                 NULL,
    PerilType       int                 NULL
)

IF @branch_id > 0
BEGIN

-- PW090402 - insert TAT and COI records into report temp table

INSERT INTO #tempAUASPU2
    SELECT
        sf.stats_folder_cnt,
        NULL,
        (SELECT sd.ri_shortname where (sd.stats_detail_type = 'COI') or (sd.stats_detail_type = 'TAT') ),
        NULL,
        sd.risk_type_code,
        (SELECT -1 * (sd.this_premium_home) where (sd.stats_detail_type = 'COI')),
        --sd.taxes_total,
        (SELECT -1 * (sd.tax_value) WHERE sd.stats_detail_type = 'TAT' and sd.tax_type_code = 'IPT'),
        (SELECT PF.TotalCost - PF.AmountToFinance from pfPremiumFinance pf WHERE PF.insurance_file_cnt = sf.insurance_file_cnt),
        --sd.charges_total,
        sd.lead_commission_value_home,
        (SELECT -1 * (sd.tax_value) WHERE sd.stats_detail_type = 'TAT' and sd.tax_type_code = 'VAT'),
        sf.branch_id,
        NULL,
        NULL,
        NULL
    FROM stats_folder sf
    JOIN stats_detail sd on   sd.stats_folder_cnt = sf.stats_folder_cnt
    WHERE  sf.branch_id = @branch_id
    AND sf.transaction_date >= @start_date
    AND sf.transaction_date <= @end_date
    AND sf.transaction_type_code not like 'C%'

-- PW090402 - insert TTY records, grouped by peril type and reinsurer, into temp report table

INSERT INTO #tempAUASPU2
    SELECT
        sf.stats_folder_cnt,
        NULL,
        MAX(sd.ri_shortname),
        NULL,
        MAX(sd.risk_type_code),
        -1 * (SUM(sd.this_premium_home)),
        --sd.taxes_total,
        NULL,
        SUM(PF.TotalCost - PF.AmountToFinance),
        --sd.charges_total,
        SUM(sd.lead_commission_value_home),
        NULL,
        MAX(sf.branch_id),
        NULL,
        sd.peril_type_id,
        MAX(sd.ri_share_percent)

    FROM stats_folder sf
    LEFT JOIN PFPremiumFinance pf on sf.insurance_file_cnt = PF.insurance_file_cnt
    JOIN stats_detail sd on   sd.stats_folder_cnt = sf.stats_folder_cnt
    WHERE  sd.stats_detail_type = 'TTY'
    AND sf.branch_id = @branch_id
    AND sf.transaction_date >= @start_date
    AND sf.transaction_date <= @end_date
    AND sf.transaction_type_code not like 'C%'
    GROUP BY sd.peril_type_id, sf.stats_folder_cnt

-- PW090402 - insert GRS records into temp table for commission, grouped by peril type

INSERT INTO #tempAUASPU21
    SELECT

        SUM(sd.lead_commission_value_home),
        sd.stats_folder_cnt,
        sd.peril_type_id

    FROM stats_folder sf
    JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
    WHERE (sd.stats_detail_type = 'GRS')
    AND
    sf.branch_id = @branch_id
    AND sf.transaction_date >= @start_date
    AND sf.transaction_date <= @end_date
    AND sf.transaction_type_code NOT LIKE 'C%'
    GROUP BY sd.peril_type_id, sd.stats_folder_cnt

END

ELSE

-- PW090402 - insert TAT and COI records into report temp table
BEGIN

INSERT INTO #tempAUASPU2
    SELECT
        sf.stats_folder_cnt,
        NULL,
        (SELECT sd.ri_shortname where (sd.stats_detail_type = 'COI') or (sd.stats_detail_type = 'TAT') ),
        NULL,
        sd.risk_type_code,
        (SELECT -1 * (sd.this_premium_home) where (sd.stats_detail_type = 'COI')),
        --sd.taxes_total,
        (SELECT -1 * (sd.tax_value) WHERE sd.stats_detail_type = 'TAT' and sd.tax_type_code = 'IPT'),
        (SELECT PF.TotalCost - PF.AmountToFinance from pfPremiumFinance pf WHERE PF.insurance_file_cnt = sf.insurance_file_cnt),
        --sd.charges_total,
        sd.lead_commission_value_home,
        (SELECT -1 * (sd.tax_value) WHERE sd.stats_detail_type = 'TAT' and sd.tax_type_code = 'VAT'),
        sf.branch_id,
        NULL,
        NULL,
        NULL
    FROM stats_folder sf
    JOIN stats_detail sd on   sd.stats_folder_cnt = sf.stats_folder_cnt
    WHERE  sf.transaction_date >= @start_date
    AND sf.transaction_date <= @end_date
    AND sf.transaction_type_code not like 'C%'

-- PW090402 - insert TTY records, grouped by peril type and reinsurer, into temp report table

INSERT INTO #tempAUASPU2
    SELECT
        sf.stats_folder_cnt,
        NULL,
        MAX(sd.ri_shortname),
        NULL,
        MAX(sd.risk_type_code),
        -1 * (SUM(sd.this_premium_home)),
        --sd.taxes_total,
        NULL,
        SUM(PF.TotalCost - PF.AmountToFinance),
        --sd.charges_total,
        SUM(sd.lead_commission_value_home),
        NULL,
        MAX(sf.branch_id),
        NULL,
        sd.peril_type_id,
        MAX(sd.ri_share_percent)

    FROM stats_folder sf
    LEFT JOIN PFPremiumFinance pf on sf.insurance_file_cnt = PF.insurance_file_cnt
    JOIN stats_detail sd on   sd.stats_folder_cnt = sf.stats_folder_cnt
    WHERE  sd.stats_detail_type = 'TTY'
    AND sf.transaction_date >= @start_date
    AND sf.transaction_date <= @end_date
    AND sf.transaction_type_code not like 'C%'
    GROUP BY sd.peril_type_id, sF.stats_folder_cnt

-- PW090402 - insert GRS records into temp table for commission, grouped by peril type

INSERT INTO #tempAUASPU21
    SELECT

        SUM(sd.lead_commission_value_home),
        sd.stats_folder_cnt,
        sd.peril_type_id

    FROM stats_folder sf
    JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
    WHERE (sd.stats_detail_type = 'GRS')
    AND sf.transaction_date >= @start_date
    AND sf.transaction_date <= @end_date
    AND sf.transaction_type_code NOT LIKE 'C%'
    GROUP BY sd.peril_type_id, sd.stats_folder_cnt

END

-- PW090402 - update report temporary table with commission shares

    UPDATE #tempAUASPU2
    SET AgentCommission = (SharePercent / 100) * t2.AgentCommission
    FROM #tempAUASPU21 t2
    WHERE #tempAUASPU2.StatsFolder = t2.StatsCount
    AND #tempAUASPU2.PerilType = t2.PerilType

    --UPDATE #tempAUASPU2
    --SET ReinsurerShortName = sd.ri_shortname
    --FROM stats_detail sd
    --WHERE sd.stats_folder_cnt = statsfolder

    UPDATE #tempAUASPU2
    SET ReinsurerName = t.description
    FROM treaty t
    WHERE ReinsurerShortName = t.code

--KB 9/1/02
    UPDATE #tempAUASPU2
    SET BranchDesc = s.description
    FROM source s
    WHERE BranchID = s.source_id

SELECT * FROM #tempAUASPU2 where isnull(ReinsurerName,'')<>''

DROP TABLE #tempAUASPU2
DROP TABLE #tempAUASPU21

GO