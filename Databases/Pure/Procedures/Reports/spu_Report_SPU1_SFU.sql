SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF 
GO


EXECUTE DDLDropProcedure 'spu_Report_SPU1_SFU'
GO



/**********************************************************************************************************************************
** Created by Kerry Butler
** 19/9/2001
** Agency Reports - SPU1
**      *in progress- fields not complete
**  1.1 KB  3/12/01 Amended comparision criteria to use NOT LIKE rather than <>
**  1.2 KB 18/12/01 Use TAT lines to populate tax values
**                  Tax types (IPT, VAT) are hard-coded for now. This will require
**                      future development to cater for non-uk sites.
**                      Also multiply values by -1 so they don't come through as negative
**  1.3 KB 1/3/02   Use values from the premium finance table to determine the Finance Charge.
**                  (totalcost - amount to finance)
**  1.4 PW040402    Get the agent commission from the GRS records as it is not stored
**                  against the treaty
**  1.5 PW180402    Get the finance charge from the new SUG records
**  1.6 PW010502    Get rid of nulls from aggregate
**
**  1.7 JMK 14/06/02    Change parameter - use period for date selection
**  1.8 TB  24/01/02    Prevent divide by zero errors 
**	1.9	JT	20/08/2004	Multicurrency changes
**********************************************************************************************************************************
**
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_SPU1_SFU
                        (@branch_id int,
                        @PeriodDate varchar (20),
								@TypeOfCurrency Varchar(30),
								@GroupByCode	Varchar(30)
                        )
AS

SET NOCOUNT ON

-- Create temp table to hold records for the report

CREATE TABLE #tempSPU1
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

-- PW040402 - Create temp table to hold the agent commission grouped by peril type

CREATE TABLE #tempSPU11
(
    Premium         decimal (19,4) NULL,
    AgentCommission decimal (19,4) NULL,
    FinanceCharge   decimal (19,4) NULL,
    StatsCount      int NULL,
    PerilType       int NULL,
    UWType          char(1)     NULL

)

CREATE TABLE #tempSPU12
(
    StatsCount      int NULL,
    Premium         decimal (19,4) NULL,
    commission      decimal (19,4) null,
    financecharge   decimal (19,4) null
)

DECLARE @UWType char(1)
SELECT @UWType = UW_type FROM hidden_options

-- which period do we want to base this report on?
DECLARE @SelectedPeriodID int, @dtSelectedPeriodEnd datetime

SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @dtSelectedPeriodEnd = CONVERT (Datetime, @PeriodDate)

SELECT @SelectedPeriodID = period_id
FROM Period
WHERE period_end_date = @dtSelectedPeriodEnd

IF @branch_id > 0
BEGIN

-- PW040402 - insert TAT records into report temp table

    INSERT INTO #tempSPU1
        SELECT

            '',
            sd.ri_shortname,
            '',
            sf.product_code,
            0,
            (SELECT -1 * (sd.tax_value) WHERE sd.tax_type_code = 'IPT'),
            0, 0,
            (SELECT -1 * (sd.tax_value) WHERE sd.tax_type_code = 'VAT'),
            sf.source_id,
            '', 0, 0, 0,
            @UWType

        FROM stats_folder sf
        JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'TAT')
        AND sf.source_id = @branch_id
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
--  AND sf.stats_folder_cnt = 185621


--Tomo 10062002 - VAT isn't on TAT, it's on TAN
    INSERT INTO #tempSPU1
        SELECT

            '',
            sd.ri_shortname,
            '',
            sf.product_code,
            0,
            0,
            0, 0,
            (SELECT -1 * (sd.tax_value) WHERE sd.tax_type_code = 'VAT'),
            sf.source_id,
            '', 0, 0, 0,
            @UWType

        FROM stats_folder sf
        JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'TAN')
    AND sd.tax_type_code = 'VAT'
        AND sf.source_id = @branch_id
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
--  AND sf.stats_folder_cnt = 185621

-- PW040402 - insert TTY records, grouped by peril type and reinsurer, into temp report table
--Tomo 10062002 - Don't group - it makes no sense

    INSERT INTO #tempSPU1
        SELECT

            '',
            ISNULL(sd.ri_shortname, ''),
            '',
            ISNULL(sf.product_code, ''),
            Case @TypeOfCurrency
            	WHEN 'Base' THEN -1 * (ISNULL(sd.this_premium_home,0))
            	WHEN 'System' THEN -1 * (ISNULL(sd.this_premium_system,0))
            END,
            0,
            0,
            Case @TypeOfCurrency
					WHEN 'Base' THEN -1 * (ISNULL(sd.lead_commission_value_home, 0))
					WHEN 'System' THEN -1 * (ISNULL(sd.lead_commission_value_system, 0))
            END,
            0,
            ISNULL(sf.source_id, 0),
            '',
            sd.stats_folder_cnt,
            sd.peril_type_id,
            ISNULL(sd.ri_share_percent, 0),
            @UWType

        FROM stats_folder sf
        JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'TTY')
        AND sf.source_id = @branch_id
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
--  AND sf.stats_folder_cnt = 185621
--      GROUP BY sd.peril_type_id, sd.stats_folder_cnt

-- PW040402 - insert GRS records into temp table for commission, grouped by peril type

    INSERT INTO #tempSPU11
        SELECT

            Case @TypeOfCurrency
				WHEN 'Base' THEN  SUM(ISNULL(sd.this_premium_home,0))
				WHEN 'System' THEN SUM(ISNULL(sd.this_premium_system,0))
            END,
             Case @TypeOfCurrency
				WHEN 'Base' THEN SUM(ISNULL(sd.lead_commission_value_home, 0))
				WHEN 'System' THEN SUM(ISNULL(sd.lead_commission_value_system, 0))
            END,
            0,
            sd.stats_folder_cnt,
            sd.peril_type_id,
            @UWType

        FROM stats_folder sf
        JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'GRS')
        AND sf.source_id = @branch_id
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
--  AND sf.stats_folder_cnt = 185621
        GROUP BY sd.peril_type_id, sd.stats_folder_cnt

END
ELSE
BEGIN

-- PW040402 - insert TAT records into report temp table

    INSERT INTO #tempSPU1
        SELECT

            '',
            sd.ri_shortname,
            '',
            sf.product_code,
            0,
            (SELECT -1 * (sd.tax_value) WHERE sd.tax_type_code = 'IPT'),
            0, 0,
            (SELECT -1 * (sd.tax_value) WHERE sd.tax_type_code = 'VAT'),
            sf.source_id,
            '', 0, 0, 0,
            @UWType

        FROM stats_folder sf
        JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'TAT')
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
--  AND sf.stats_folder_cnt = 185621

--Tomo 10062002 - VAT isn't on TAT, it's on TAN
    INSERT INTO #tempSPU1
        SELECT

            '',
            sd.ri_shortname,
            '',
            sf.product_code,
            0,
            0,
            0, 0,
            (SELECT -1 * (sd.tax_value) WHERE sd.tax_type_code = 'VAT'),
            sf.source_id,
            '', 0, 0, 0,
            @UWType

        FROM stats_folder sf
        JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'TAN')
    AND sd.tax_type_code = 'VAT'
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
--  AND sf.stats_folder_cnt = 185621

-- PW040402 - insert TTY records, grouped by peril type and reinsurer, into temp report table

    INSERT INTO #tempSPU1
        SELECT

            '',
            ISNULL(sd.ri_shortname, ''),
            '',
            ISNULL(sf.product_code, ''),
            Case @TypeOfCurrency
				WHEN 'Base' THEN sum(-1 * (ISNULL(sd.this_premium_home,0)))
				WHEN 'System' THEN  sum(-1 * (ISNULL(sd.this_premium_system,0)))
			END
			,
            0,
            0,
            Case @TypeOfCurrency
            	WHEN 'Base' THEN sum(-1 * (ISNULL(sd.lead_commission_value_home, 0)))
            	WHEN 'System' THEN  sum(-1 * (ISNULL(sd.lead_commission_value_system, 0)))
            END,
            	
            0,
            ISNULL(sf.source_id, 0),
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
--  AND sf.stats_folder_cnt = 185621
      GROUP BY sd.ri_shortname, sf.product_code, sf.source_id, sd.stats_folder_cnt, sd.peril_type_id, sd.ri_share_percent

-- PW040402 - insert GRS records into temp table for commission, grouped by peril type

    INSERT INTO #tempSPU11
        SELECT
        	Case @TypeOfCurrency
				WHEN 'Base' THEN SUM(ISNULL(sd.this_premium_home, 0))
				WHEN 'System' THEN  SUM(ISNULL(sd.this_premium_system, 0))
            END,
            Case @TypeOfCurrency
				WHEN 'Base' THEN SUM(ISNULL(sd.lead_commission_value_home, 0))
				WHEN 'System' THEN  SUM(ISNULL(sd.lead_commission_value_system, 0))
            END,
            0,
            sd.stats_folder_cnt,
            sd.peril_type_id,
            @UWType

        FROM stats_folder sf
        JOIN stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'GRS')
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
--  AND sf.stats_folder_cnt = 185621
        GROUP BY sd.peril_type_id, sd.stats_folder_cnt

END

INSERT INTO #tempSPU12
SELECT      sd.stats_folder_cnt,
			Case @TypeOfCurrency
				WHEN 'Base' THEN SUM(ISNULL(sd.this_premium_home, 0))
				WHEN 'System' THEN  SUM(ISNULL(sd.this_premium_system, 0))
            END,
            Case @TypeOfCurrency
				WHEN 'Base' THEN SUM(ISNULL(sd.lead_commission_value_home, 0))
				WHEN 'System' THEN  SUM(ISNULL(sd.lead_commission_value_system, 0))
            END,
            0
FROM        stats_detail sd
WHERE       sd.stats_folder_cnt in (
            SELECT statscount
            FROM #tempSPU11
            )
AND         sd.stats_detail_type = 'GRS'
GROUP BY    sd.stats_folder_cnt

UPDATE #tempSPU12
SET    financecharge = 
Case @TypeOfCurrency
	WHEN 'Base' THEN sd.lead_commission_value_home
	WHEN 'System' THEN  sd.lead_commission_value_system
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
--  AND sf.stats_folder_cnt = 185621

DECLARE c_finchg_nb CURSOR FAST_FORWARD FOR
        SELECT  sd.stats_folder_cnt
        FROM    stats_folder sf
        JOIN    stats_detail sd on sf.stats_folder_cnt = sd.stats_folder_cnt
        WHERE (sd.stats_detail_type = 'SUG')
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE 'C%'
--  AND sf.stats_folder_cnt = 185621

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

    SELECT      @total_prem = 
    CASE @TypeOfCurrency 
    	WHEN  'Base' THEN SUM(sd.this_premium_home)
    	WHEN 'System' THEN SUM(sd.this_premium_system)
    END
    FROM        stats_detail sd
    WHERE       sd.stats_folder_cnt = @stats_folder_cnt
    AND         sd.stats_detail_type = 'GRS'
    GROUP BY    sd.stats_folder_cnt

    SELECT  @total_finance = 
    CASE @TypeOfCurrency 
	    	WHEN  'Base' THEN sd.lead_commission_value_home
	    	WHEN 'System' THEN sd.lead_commission_value_system
    END
    
    FROM    stats_detail sd
    WHERE   sd.stats_folder_cnt = @stats_folder_cnt
    AND     sd.stats_detail_type = 'SUG'

    DECLARE c_tempgrs CURSOR FAST_FORWARD FOR
            SELECT  Premium, StatsCount, PerilType
            FROM    #tempSPU11
            WHERE   StatsCount = @stats_folder_cnt

    OPEN c_tempgrs
    FETCH NEXT FROM c_tempgrs INTO @premium,
                                   @stats_folder_cnt_2,
                                   @peril_type
    WHILE (@@FETCH_STATUS = 0)
    BEGIN
        -- TB Prevent div by zero
        IF @total_prem = 0
            SELECT @charge_share = 0
        ELSE
            SELECT @charge_share = (@premium / @total_prem) * @total_finance

        UPDATE #tempSPU11
        SET FinanceCharge = @charge_share
        WHERE #tempSPU11.StatsCount = @stats_folder_cnt_2
        AND #tempSPU11.PerilType = @peril_type

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

-- PW040402 - update report temporary table with commission shares
-- PW180402 - and finance charge

--UPDATE #tempSPU1
--SET AgentCommission = (SharePercent / 100) * t2.AgentCommission,
--    FinanceCharge = (SharePercent / 100) * t2.FinanceCharge
--FROM #tempSPU11 t2
--WHERE #tempSPU1.StatsCount = t2.StatsCount
--AND #tempSPU1.PerilType = t2.PerilType

UPDATE #tempSPU1
SET AgentCommission = (#tempSPU1.grosspremium / t3.premium * t3.Commission),
    FinanceCharge = (#tempSPU1.grosspremium / t3.premium * t3.FinanceCharge)
FROM #tempSPU12 t3
WHERE #tempSPU1.StatsCount = t3.StatsCount
  AND t3.premium <> 0         -- TB to prevent div by zero error

UPDATE #tempSPU1
SET ReinsurerName = t.description
FROM treaty t
WHERE ReinsurerShortName = t.code

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
		END CurrencyDesc,
		Case @GroupbyCode 
			WHEN 'Branch' THEN S.Code
			WHEN 'Branch And Currency' THEN S.Code
		ELSE ''
		END 'GroupByCode'	
    FROM #tempSPU1 TS
	INNER JOIN Source S ON S.source_id = TS.BranchId
	INNER JOIN CURRENCY C ON C.Currency_id= S.base_currency_id

DROP TABLE #tempSPU1
DROP TABLE #tempSPU11
DROP TABLE #tempSPU12



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

