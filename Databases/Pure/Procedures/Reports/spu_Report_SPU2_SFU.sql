SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


EXECUTE DDLDropProcedure 'spu_Report_SPU2_SFU'
GO

/**********************************************************************************************************************************
** Created by Kerry Butler
** 19/9/2001
** Agency Reports - SPU2
**      *in progress- fields not complete
** v1.1 2/1/2 Added values for IPT and VAT
** v1.2     9/1/02 KB   Use Source table rather than Branch table to pick up the branch name.
** v1.3     1/3/02 KB   Populate finance charge from the premium finance tables now that
**              instalments are available (Totalcost -  AmountToFinance)
**                              Also use source_id rather than code to pick up Branch name.
** v1.4 PW090402 - get agent commission from GRS records because it is not on TTY records
** v1.5 PW180402    Get the finance charge from the new SUG records
** v1.6 PW010502    Get rid of nulls from aggregate
** v1.7 JMK 14/06/02    Change parameter - use period for date selection
**********************************************************************************************************************************
**
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_SPU2_SFU
                        (@branch_id int,
                        @PeriodDate varchar (20),
                           @TypeOfCurrency Varchar(30)
                        )
AS




SET NOCOUNT ON

CREATE TABLE #tempSPU2
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

CREATE TABLE #tempSPU21
(
    Premium             decimal (19,4)      NULL,
    AgentCommission     decimal (19,4)      NULL,
    FinanceCharge       decimal (19,4)      NULL,
    StatsCount          int                 NULL,
    PerilType           int                 NULL
)

CREATE TABLE #tempSPU22
(
    StatsCount      int NULL,
    Premium         decimal (19,4) NULL,
    commission      decimal (19,4) null,
    financecharge   decimal (19,4) null
)

-- which period do we want to base this report on?
DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime

SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @dtSelectedPeriodEnd = CONVERT (Datetime, @PeriodDate)

SELECT @SelectedPeriodID = period_id
FROM Period
WHERE period_end_date = @dtSelectedPeriodEnd

IF @branch_id > 0
BEGIN

-- PW090402 - insert TAT and COI records into report temp table

INSERT INTO #tempSPU2
    SELECT
        sf.stats_folder_cnt,
        NULL,
        (SELECT sd.ri_shortname where (sd.stats_detail_type = 'COI') or (sd.stats_detail_type = 'TAT') ),
        NULL,
        sd.risk_type_code,
        (SELECT -1 * (
        Case @TypeOfCurrency When 'Base' THEN  sd.this_premium_home
        	WHEN 'System' THEN sd.this_premium_system
        END
        ) where (sd.stats_detail_type = 'COI')),
        --sd.taxes_total,
        (SELECT -1 * (sd.tax_value) WHERE sd.stats_detail_type = 'TAT' and sd.tax_type_code = 'IPT'),
        0,
        --sd.charges_total,
        sd.lead_commission_value_home,
        (SELECT -1 * (sd.tax_value) WHERE sd.stats_detail_type = 'TAT' and sd.tax_type_code = 'VAT'),
        sf.source_id,
        NULL,
        NULL,
        NULL
    FROM stats_folder sf
    JOIN stats_detail sd on   sd.stats_folder_cnt = sf.stats_folder_cnt
    WHERE  sf.source_id = @branch_id
    --AND sf.transaction_date >= @start_date
    --AND sf.transaction_date <= @end_date
    AND sf.posting_period_number = @SelectedPeriodID
    AND sf.transaction_type_code not like 'C%'

-- PW090402 - insert TTY records, grouped by peril type and reinsurer, into temp report table

INSERT INTO #tempSPU2
    SELECT
        sf.stats_folder_cnt,
        NULL,
        MAX(ISNULL(sd.ri_shortname,'')),
        NULL,
        MAX(ISNULL(sd.risk_type_code,'')),
        Case @TypeOfcurrency 
        	WHEN 'Base' THEN
        		-1 * (SUM(ISNULL(sd.this_premium_home,0)))
        	WHEN 'System' THEN 
        		-1 * (SUM(ISNULL(sd.this_premium_System,0)))
        END
        ,
        --sd.taxes_total,
        NULL,
        0,
        --sd.charges_total,
        Case @TypeOfcurrency 
		       	WHEN 'Base' THEN
         SUM(ISNULL(sd.lead_commission_value_home,0))
               	WHEN 'System' THEN
         SUM(ISNULL(sd.lead_commission_value_system,0))
         END,
        NULL,
        MAX(ISNULL(sf.source_id,'')),
        NULL,
        sd.peril_type_id,
        MAX(ISNULL(sd.ri_share_percent,0))

    FROM stats_folder sf
    JOIN stats_detail sd on   sd.stats_folder_cnt = sf.stats_folder_cnt
    WHERE  sd.stats_detail_type = 'TTY'
    AND sf.source_id = @branch_id
    --AND sf.transaction_date >= @start_date
    --AND sf.transaction_date <= @end_date
    AND sf.posting_period_number = @SelectedPeriodID
    AND sf.transaction_type_code not like 'C%'
    GROUP BY sd.peril_type_id, sf.stats_folder_cnt

-- PW090402 - insert GRS records into temp table for commission, grouped by peril type

INSERT INTO #tempSPU21
    SELECT

        Case @TypeOfcurrency
        WHEN 'Base' THEN
        	SUM(ISNULL(sd.this_premium_home,0))
        WHEN 'System' THEN
        	SUM(ISNULL(sd.this_premium_system,0))
        END	,
        Case @TypeOfcurrency
        WHEN 'Base' THEN
        	SUM(ISNULL(sd.lead_commission_value_home,0))
        WHEN 'system' THEN 
        	SUM(ISNULL(sd.lead_commission_value_system,0)) END,
        0,
        sd.stats_folder_cnt,
        sd.peril_type_id

    FROM stats_folder sf
    JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
    WHERE (sd.stats_detail_type = 'GRS')
    AND sf.source_id = @branch_id
    --AND sf.transaction_date >= @start_date
    --AND sf.transaction_date <= @end_date
    AND sf.posting_period_number = @SelectedPeriodID
    AND sf.transaction_type_code NOT LIKE 'C%'
    GROUP BY sd.peril_type_id, sd.stats_folder_cnt

END

ELSE

BEGIN

-- PW090402 - insert TAT and COI records into report temp table

INSERT INTO #tempSPU2
    SELECT
        sf.stats_folder_cnt,
        NULL,
        (SELECT sd.ri_shortname where (sd.stats_detail_type = 'COI') or (sd.stats_detail_type = 'TAT') ),
        NULL,
        sd.risk_type_code,
        (SELECT 
        Case @TypeOfCurrency WHEN 'Base' THEN -1 * (sd.this_premium_home) 
        	WHEN 'System' THEN -1 * (sd.this_premium_System) 
        END
        where (sd.stats_detail_type = 'COI')),
        --sd.taxes_total,
        (SELECT -1 * (sd.tax_value) WHERE sd.stats_detail_type = 'TAT' and sd.tax_type_code = 'IPT'),
        0,
        --sd.charges_total,
        Case @TypeOfCurrency WHEN 'Base' THEN 
        	sd.lead_commission_value_home
        	WHEN 'System' THEN 
        	sd.lead_commission_value_system
        END,
        (SELECT -1 * (sd.tax_value) WHERE sd.stats_detail_type = 'TAT' and sd.tax_type_code = 'VAT'),
        sf.source_id,
        NULL,
        NULL,
        NULL
    FROM stats_folder sf
    JOIN stats_detail sd on   sd.stats_folder_cnt = sf.stats_folder_cnt
    --WHERE  sf.transaction_date >= @start_date
    --AND sf.transaction_date <= @end_date
    AND sf.posting_period_number = @SelectedPeriodID
    AND sf.transaction_type_code not like 'C%'

-- PW090402 - insert TTY records, grouped by peril type and reinsurer, into temp report table

INSERT INTO #tempSPU2
    SELECT
        sf.stats_folder_cnt,
        NULL,
        ISNULL(sd.ri_shortname,''),
        NULL,
        ISNULL(sd.risk_type_code,''),
        Case @TypeOfCurrency 
        	WHEN 'Base' THEN 
        -1 * (ISNULL(sd.this_premium_home,0))
        	WHEN 'System' THEN 
        -1 * (ISNULL(sd.this_premium_system,0))
        END
        ,
        --sd.taxes_total,
        NULL,
        0,
        --sd.charges_total,
        Case @TypeOfCurrency
        	WHEN 'Base'THEN
        ISNULL(sd.lead_commission_value_home,0)
        	WHEN 'System' THEN
        	ISNULL(sd.lead_commission_value_System,0)
        END,
        NULL,
        ISNULL(sf.source_id,''),
        NULL,
        sd.peril_type_id,
        ISNULL(sd.ri_share_percent,0)

    FROM stats_folder sf
    JOIN stats_detail sd on   sd.stats_folder_cnt = sf.stats_folder_cnt
    WHERE  sd.stats_detail_type = 'TTY'
    --AND sf.transaction_date >= @start_date
    --AND sf.transaction_date <= @end_date
    AND sf.posting_period_number = @SelectedPeriodID
    AND sf.transaction_type_code not like 'C%'
--    GROUP BY sd.peril_type_id, sf.stats_folder_cnt

-- PW090402 - insert GRS records into temp table for commission, grouped by peril type

INSERT INTO #tempSPU21
    SELECT

        Case @TypeOfCurrency 
        	WHEN 'Base' THEN 
        SUM(ISNULL(sd.this_premium_home,0))
        	WHEN 'System' THEN 
        SUM(ISNULL(sd.this_premium_System,0))
        END,
        Case @TypeOfCurrency 
        	WHEN 'Base' THEN 
        SUM(ISNULL(sd.lead_commission_value_home,0))
        	WHEN 'System' THEN 
        SUM(ISNULL(sd.lead_commission_value_System,0))
        END  ,
        0,
        sd.stats_folder_cnt,
        sd.peril_type_id

    FROM stats_folder sf
    JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
    WHERE (sd.stats_detail_type = 'GRS')
    --AND sf.transaction_date >= @start_date
    --AND sf.transaction_date <= @end_date
    AND sf.posting_period_number = @SelectedPeriodID
    AND sf.transaction_type_code NOT LIKE 'C%'
    GROUP BY sd.peril_type_id, sd.stats_folder_cnt

END

INSERT INTO #tempSPU22
SELECT      sd.stats_folder_cnt,
			Case @TypeOfCurrency WHEN 'Base' THEN
			     sum(isnull(sd.this_premium_home, 0))
			    WHEN 'System' THEN 
			    sum(isnull(sd.this_premium_system, 0))
			END
			 ,
			 Case @TypeOfCurrency WHEN 'Base' THEN
            	SUM(ISNULL(sd.lead_commission_value_home, 0))
            	WHEN 'System' THEN
            	SUM(ISNULL(sd.lead_commission_value_System, 0))
            END,
            	
            0
FROM        stats_detail sd
WHERE       sd.stats_folder_cnt in (
            SELECT statscount
            FROM #tempSPU21
            )
AND         sd.stats_detail_type = 'GRS'
GROUP BY    sd.stats_folder_cnt

UPDATE #tempSPU22
SET    financecharge = 
	Case @TypeOfCurrency WHEN 'Base' THEN
		(ISNULL(sd.lead_commission_value_home, 0))
		WHEN 'System' THEN
		(ISNULL(sd.lead_commission_value_System, 0))
    END
FROM   stats_detail sd
WHERE  statscount = sd.stats_folder_cnt
AND    sd.stats_detail_type = 'SUG'

-- PW180402 - update temporary GRS records with their share of the finance charge

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
        sf.source_id = @branch_id
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

    SELECT      @total_prem = SUM(sd.stats_folder_cnt)
    FROM        stats_detail sd
    WHERE       sd.stats_folder_cnt = @stats_folder_cnt
    AND         sd.stats_detail_type = 'GRS'
    GROUP BY    sd.stats_folder_cnt

    SELECT  @total_finance = 
    Case @TypeOfCurrency WHEN 'Base' THEN
		(ISNULL(sd.lead_commission_value_home, 0))
		WHEN 'System' THEN 
		(ISNULL(sd.lead_commission_value_System, 0))
    END
    FROM    stats_detail sd
    WHERE   sd.stats_folder_cnt = @stats_folder_cnt
    AND     sd.stats_detail_type = 'SUG'

    DECLARE c_tempgrs CURSOR FAST_FORWARD FOR
            SELECT  Premium, StatsCount, PerilType
            FROM    #tempSPU21
            WHERE   StatsCount = @stats_folder_cnt

    OPEN c_tempgrs
    FETCH NEXT FROM c_tempgrs INTO @premium,
                                   @stats_folder_cnt_2,
                                   @peril_type
    WHILE (@@FETCH_STATUS = 0)
    BEGIN
        SELECT @charge_share = (@premium / @total_prem) * @total_finance

        UPDATE #tempSPU21
        SET FinanceCharge = @charge_share
        WHERE #tempSPU21.StatsCount = @stats_folder_cnt_2
        AND #tempSPU21.PerilType = @peril_type

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

-- PW090402 - update report temporary table with commission shares

/*
UPDATE #tempSPU2
SET AgentCommission = (SharePercent / 100) * t2.AgentCommission,
    FinanceCharge = (SharePercent / 100) * t2.FinanceCharge
FROM #tempSPU21 t2
WHERE #tempSPU2.StatsFolder = t2.StatsCount
AND #tempSPU2.PerilType = t2.PerilType
*/

UPDATE #tempSPU2
SET AgentCommission = (#tempSPU2.grosspremium / t3.premium * t3.Commission),
    FinanceCharge = (#tempSPU2.grosspremium / t3.premium * t3.FinanceCharge)
FROM #tempSPU22 t3
WHERE #tempSPU2.StatsFolder = t3.StatsCount
AND t3.premium <> 0


--UPDATE #tempSPU2
--SET ReinsurerShortName = sd.ri_shortname
--FROM stats_detail sd
--WHERE sd.stats_folder_cnt = statsfolder

UPDATE #tempSPU2
SET ReinsurerName = t.description
FROM treaty t
WHERE ReinsurerShortName = t.code

--KB 9/1/02
UPDATE #tempSPU2
SET BranchDesc = s.description
FROM source s
WHERE BranchID = s.source_id
/*Get System Currency Details--jitendra*/
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
SELECT *, 
S.Code CompanyCode,
S.description CompanyDesc,
Case @TypeOfCurrency 
	WHEN 'System' THEN  @Systemcurrencycode
	WHEN 'Base' THEN C.Code
END CurrencyCode,
Case @TypeOfCurrency 
	WHEN 'System' THEN @SystemCurrencyDesc
	WHEN  'Base' THEN C.description
END CurrencyDesc 

FROM #tempSPU2  TS

INNER JOIN Source S ON S.source_id = TS.BranchID
INNER JOIN CURRENCY C ON C.Currency_id= S.base_currency_id

where isnull(ReinsurerName,'')<>''

DROP TABLE #tempSPU2
DROP TABLE #tempSPU21

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

