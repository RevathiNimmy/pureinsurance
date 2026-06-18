SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Report_AUA_SPU1'
GO

CREATE PROCEDURE spu_Report_AUA_SPU1
                    @branch_id int,
                    @PeriodDate varchar (20)
AS
SET NOCOUNT ON
CREATE TABLE #tempAUASPU1
(
    PeriodName      varchar (15) NULL,
    ReinsurerShortName varchar (25) NULL,
    ReinsurerName   varchar (100) NULL,
    ClassOfBusiness varchar (100) NULL,
    GrossPremium    decimal (19,4) NULL,
    IPT             decimal (19,4) NULL,
    FinanceCharge   decimal (19,4) NULL,
    AgentCommission decimal (19,4) NULL,
    VAT             decimal (19,4) NULL,
    BranchId        int NULL,
    BranchDesc      varchar (255) NULL,
    StatsCount      int NULL,
    PerilType       int NULL,
    SharePercent    decimal (12,8) NULL,
    UWType          char(1)     NULL
)
CREATE TABLE #tempAUASPU11
(
    Premium         decimal (19,4) NULL,
    AgentCommission decimal (19,4) NULL,
    FinanceCharge   decimal (19,4) NULL,
    StatsCount      int NULL,
    PerilType       int NULL,
    UWType          char(1)     NULL
)
CREATE TABLE #tempAUASPU12
(
    StatsCount      int NULL,
    Premium         decimal (19,4) NULL,
    commission      decimal (19,4) null,
    financecharge   decimal (19,4) null
)
DECLARE @UWType char(1)
SELECT @UWType = UW_type FROM hidden_options where branch_id = 1 and option_number = 1
DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime
SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @dtSelectedPeriodEnd = CONVERT (Datetime, @PeriodDate)
SELECT @SelectedPeriodID = period_id
FROM Period
WHERE period_end_date = @dtSelectedPeriodEnd
IF @branch_id > 0
BEGIN
    INSERT INTO #tempAUASPU1
        SELECT
            '',
            sd.ri_shortname,
            '',
            sf.product_code,
            0,
            (SELECT -1 * (sd.tax_value) WHERE sd.tax_type_code = 'IPT'),
            0, 0,
            (SELECT -1 * (sd.tax_value) WHERE sd.tax_type_code = 'VAT'),
            sf.branch_id,
            '', 0, 0, 0,
            @UWType
        FROM stats_folder sf
        JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'TAT')
        AND sf.branch_id = @branch_id
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
    INSERT INTO #tempAUASPU1
        SELECT
            '',
            sd.ri_shortname,
            '',
            sf.product_code,
            0,
            0,
            0, 0,
            (SELECT -1 * (sd.tax_value) WHERE sd.tax_type_code = 'VAT'),
            sf.branch_id,
            '', 0, 0, 0,
            @UWType
        FROM stats_folder sf
        JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'TAN')
    AND sd.tax_type_code = 'VAT'
        AND sf.branch_id = @branch_id
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
    INSERT INTO #tempAUASPU1
        SELECT
            '',
            ISNULL(sd.ri_shortname, ''),
            '',
            ISNULL(sf.product_code, ''),
            -1 * (ISNULL(sd.this_premium_home,0)),
            0,
            0,
            -1 * (ISNULL(sd.lead_commission_value_home, 0)),
            0,
            ISNULL(sf.branch_id, 0),
            '',
            sd.stats_folder_cnt,
            sd.peril_type_id,
            ISNULL(sd.ri_share_percent, 0),
            @UWType
        FROM stats_folder sf
        JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'TTY')
        AND sf.branch_id = @branch_id
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
    INSERT INTO #tempAUASPU11
        SELECT
            SUM(ISNULL(sd.this_premium_home, 0)),
            SUM(ISNULL(sd.lead_commission_value_home, 0)),
            0,

            sd.stats_folder_cnt,
            sd.peril_type_id,
            @UWType
        FROM stats_folder sf
        JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'GRS')
        AND sf.branch_id = @branch_id
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
        GROUP BY sd.peril_type_id, sd.stats_folder_cnt
END
ELSE
BEGIN
    INSERT INTO #tempAUASPU1
        SELECT
            '',
            sd.ri_shortname,
            '',
            sf.product_code,
            0,
            (SELECT -1 * (sd.tax_value) WHERE sd.tax_type_code = 'IPT'),
            0, 0,
            (SELECT -1 * (sd.tax_value) WHERE sd.tax_type_code = 'VAT'),
            sf.branch_id,
            '', 0, 0, 0,
            @UWType
        FROM stats_folder sf
        JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'TAT')
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
    INSERT INTO #tempAUASPU1
        SELECT
            '',
            sd.ri_shortname,
            '',
            sf.product_code,
            0,
            0,
            0, 0,
            (SELECT -1 * (sd.tax_value) WHERE sd.tax_type_code = 'VAT'),
            sf.branch_id,
            '', 0, 0, 0,
            @UWType
        FROM stats_folder sf
        JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'TAN')
    AND sd.tax_type_code = 'VAT'
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
    INSERT INTO #tempAUASPU1
        SELECT
            '',
            ISNULL(sd.ri_shortname, ''),
            '',
            ISNULL(sf.product_code, ''),
            sum(-1 * (ISNULL(sd.this_premium_home,0))),
            0,
            0,
            sum(-1 * (ISNULL(sd.lead_commission_value_home, 0))),
            0,
            ISNULL(sf.branch_id, 0),
            '',
            sd.stats_folder_cnt,
            sd.peril_type_id,
            ISNULL(sd.ri_share_percent, 0),
            @UWType
        FROM stats_folder sf
        JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'TTY')
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
      GROUP BY sd.ri_shortname, sf.product_code, sf.branch_id, sd.stats_folder_cnt, sd.peril_type_id, sd.ri_share_percent
    INSERT INTO #tempAUASPU11
        SELECT
            SUM(ISNULL(sd.this_premium_home, 0)),
            SUM(ISNULL(sd.lead_commission_value_home, 0)),
            0,
            sd.stats_folder_cnt,
            sd.peril_type_id,
            @UWType
        FROM stats_folder sf
        JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'GRS')
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
        GROUP BY sd.peril_type_id, sd.stats_folder_cnt
END
INSERT INTO #tempAUASPU12
SELECT      sd.stats_folder_cnt,
            sum(isnull(sd.this_premium_home, 0)),
            SUM(ISNULL(sd.lead_commission_value_home, 0)),
            0
FROM        stats_detail sd
WHERE       sd.stats_folder_cnt in (
            SELECT statscount
            FROM #tempAUASPU11
            )
AND         sd.stats_detail_type = 'GRS'
GROUP BY    sd.stats_folder_cnt
UPDATE #tempAUASPU12
SET    financecharge = sd.lead_commission_value_home
FROM   stats_detail sd
WHERE  statscount = sd.stats_folder_cnt
AND    sd.stats_detail_type = 'SUG'
DECLARE @stats_folder_cnt int
DECLARE @stats_folder_cnt_2 int
DECLARE @total_prem numeric (19,4)
DECLARE @total_finance numeric (19,4)
DECLARE @premium numeric (19,4)
DECLARE @charge_share numeric (19,4)
DECLARE @peril_type int
DECLARE c_finchg_br CURSOR FAST_FORWARD FOR
        SELECT  sd.stats_folder_cnt
        FROM    stats_folder sf
        JOIN    stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'SUG')
        AND
        sf.branch_id = @branch_id
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
DECLARE c_finchg_nb CURSOR FAST_FORWARD FOR
        SELECT  sd.stats_folder_cnt
        FROM    stats_folder sf
        JOIN    stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'SUG')
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
IF @branch_id > 0
BEGIN
    OPEN c_finchg_br
    FETCH NEXT FROM c_finchg_br INTO @stats_folder_cnt
END
ELSE
BEGIN
    OPEN c_finchg_nb
    FETCH NEXT FROM c_finchg_nb INTO @stats_folder_cnt
END
WHILE (@@FETCH_STATUS = 0)
BEGIN
    SELECT      @total_prem = SUM(sd.this_premium_home)
    FROM        stats_detail sd
    WHERE       sd.stats_folder_cnt = @stats_folder_cnt
    AND         sd.stats_detail_type = 'GRS'
    GROUP BY    sd.stats_folder_cnt
    SELECT  @total_finance = sd.lead_commission_value_home
    FROM    stats_detail sd
    WHERE   sd.stats_folder_cnt = @stats_folder_cnt
    AND     sd.stats_detail_type = 'SUG'
    DECLARE c_tempgrs CURSOR FAST_FORWARD FOR
            SELECT  Premium, StatsCount, PerilType
            FROM    #tempAUASPU11
            WHERE   StatsCount = @stats_folder_cnt
    OPEN c_tempgrs
    FETCH NEXT FROM c_tempgrs INTO @premium,
                                   @stats_folder_cnt_2,
                                   @peril_type
    WHILE (@@FETCH_STATUS = 0)
    BEGIN
        SELECT @charge_share = (@premium / @total_prem) * @total_finance
        UPDATE #tempAUASPU11
        SET FinanceCharge = @charge_share
        WHERE #tempAUASPU11.StatsCount = @stats_folder_cnt_2
        AND #tempAUASPU11.PerilType = @peril_type
        FETCH NEXT FROM c_tempgrs INTO @premium,
                                       @stats_folder_cnt_2,
                                       @peril_type
    END
    CLOSE c_tempgrs
    DEALLOCATE c_tempgrs
    IF @branch_id > 0
    BEGIN
        FETCH NEXT FROM c_finchg_br INTO @stats_folder_cnt
    END
    ELSE
    BEGIN
        FETCH NEXT FROM c_finchg_nb INTO @stats_folder_cnt
    END
END
IF @branch_id > 0
BEGIN
    CLOSE c_finchg_br
END
ELSE
BEGIN
    CLOSE c_finchg_nb
END
DEALLOCATE c_finchg_br
DEALLOCATE c_finchg_nb
UPDATE #tempAUASPU1
SET AgentCommission = (#tempAUASPU1.grosspremium / t3.premium * t3.Commission),
    FinanceCharge = (#tempAUASPU1.grosspremium / t3.premium * t3.FinanceCharge)
FROM #tempAUASPU12 t3
WHERE #tempAUASPU1.StatsCount = t3.StatsCount
UPDATE #tempAUASPU1
SET ReinsurerName = t.description
FROM treaty t
WHERE ReinsurerShortName = t.code
SELECT * FROM #tempAUASPU1
DROP TABLE #tempAUASPU1
DROP TABLE #tempAUASPU11
DROP TABLE #tempAUASPU12
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

