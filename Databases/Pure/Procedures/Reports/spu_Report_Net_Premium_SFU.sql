
EXECUTE DDLDropProcedure 'spu_Report_Net_Premium_SFU'
GO
/****** Object:  Stored Procedure dbo.sp_Report_Net_Premium    Script Date: 16/10/00 12:25:59 ******/
/**********************************************************************************************************************************
** Created by Jude Killip
** 15/08/2000
** RSA Reports - NetPremium.rpt
**  Created with dummy data to build the report
**********************************************************************************************************************************
** 27/10/2000 JMK       - Update to use DB.
**
** 15/01/2001 JMK       - use document_date, amend date criteria
**
** 05/05/2001 JMK       - amend to conform to summary of premium (base dates on Current Period)
**                          **make sure Orion_For_Broking named correctly**
**
** 08/06/2001 JMK       - redo above: this SP got lost somehow...
**                      - amend selection codes, still guessing
**
** 04/09/2001 JMK       - rewrite: use Stats NOT Orion, similar to sp_Report_Summary_of_Premium
** 06/10/2004	JT		- For showing base currency when all Branches are selected
***********************************************************************************************************************************/
CREATE PROCEDURE spu_Report_Net_Premium_SFU
	@branch_id int,
	@PeriodDate varchar (20),
	@sBasis varchar(50),
	@TypeOfCurrency VARCHAR(15)

AS


-- $Author: Tom.brown $
-- $Revision: 7 $
-- $Modtime: 19/11/02 16:21 $
-- $Workfile: sp_Report_Net_Premium.sql $
-- $Logfile: /Sirius For Underwriting/Crystal Reports/Stored Procedures/sp_Report_Net_Premium.sql $
-- $History: sp_Report_Net_Premium.sql $
-- 
-- *****************  Version 7  *****************
-- User: Tom.brown    Date: 20/11/02   Time: 15:13
-- Updated in $/Sirius For Underwriting/Crystal Reports/Stored Procedures
-- Overhaul:  Standardise parameters as Branch, Period End Date, <others>
-- and Basis (Date/Period)
-- 
-- *****************  Version 1  *****************
-- User: Tom.brown    Date: 19/11/02   Time: 16:42
-- Created in $/Work/SWIssues/Reports/ParamChanges
-- Interim:  Work to end 19 Nov 2002

SET NOCOUNT ON
DECLARE @DEBUG INT
SELECT @DEBUG = 0    -- 0 = OFF, 1 = ON

-- Report Basis:  By Date or by Period ID
DECLARE @iBasis INT
-- Branch (Company No)
DECLARE @iBranchID INT
-- Branch to select from Period Table
DECLARE @iBranchPeriod INT

-- Report Data selection Dates and Period ID
DECLARE @SelectedPeriodID int
DECLARE @iPeriod INT
DECLARE @dtSelectedPeriodEnd datetime
DECLARE @period_end_date    datetime
DECLARE @prev_period_end_date datetime

DECLARE @YearStartPeriodID int
DECLARE @dtYearStart datetime

DECLARE @12PeriodsAgoID int
DECLARE @dt12PeriodsAgo datetime

DECLARE @system_currency_id INT
DECLARE @CurrencyCode VARCHAR(10)
DECLARE @CurrencyDesc VARCHAR(255)

/*Use correct branch to work out period*/
IF EXISTS(SELECT NULL FROM period WHERE company_id > 1)
BEGIN
	SELECT @iBranchPeriod = ISNULL(@branch_id,1)
END
ELSE
BEGIN
	SELECT @iBranchPeriod = 1
END

/*Get System Currency*/
SELECT
	@system_currency_id = currency_id
FROM PMSystem
WHERE system_id = 1


/*Set currency code and description for report header
IF @TypeOfCurrency = 'Base'
BEGIN
	SELECT
		@CurrencyCode = c.iso_code,
		@CurrencyDesc = c.description
	FROM currency c
	JOIN source s
		ON s.base_currency_id = c.currency_id
	WHERE s.source_id = @branch_id
END*/

IF @TypeOfCurrency = 'System'
BEGIN
	SELECT
		@CurrencyCode = c.iso_code,
		@CurrencyDesc = c.description
	FROM currency c
	WHERE c.currency_id = @system_currency_id
END

SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @period_end_date = CONVERT (Datetime, @PeriodDate)
SELECT @dtSelectedPeriodEnd = @period_end_date

IF @branch_id IS NULL
    SELECT @iBranchID = 0
ELSE
    SELECT @iBranchID = @branch_id

IF @sBasis = 'Transaction Date'
BEGIN
    SELECT @iBasis = 1    -- Transaction Date
END
ELSE
BEGIN
    SELECT @iBasis = 0    -- Transaction Period
END
    SELECT @prev_period_end_date = ( SELECT max(period_end_date)
                                       FROM Period
                                      WHERE period_end_date < @period_end_date
                                        AND company_id = @iBranchPeriod )

    SELECT @iPeriod = ( SELECT period_id
                        FROM period
                        WHERE period_end_Date = @period_end_date
                         AND company_id = @iBranchPeriod )
-- which period do we want to base this report on?
-- Selected Period
SELECT @SelectedPeriodID = @iPeriod
-- get the year start period ID
SELECT @YearStartPeriodID = ( SELECT  min(period_id)
                                FROM period
                                WHERE year_name = ( SELECT year_name
                                                      FROM period
                                                     WHERE period_id = @SelectedPeriodID
                                                       AND company_id = @iBranchPeriod)
                                  AND company_id = @iBranchPeriod )
SELECT @dtYearStart = ( SELECT max(period_end_date)
                          FROM Period
                         WHERE period_id < @YearStartPeriodID
                           AND company_id = @iBranchPeriod )
-- get the 12 period ID
SELECT @dt12PeriodsAgo = DATEADD(year, -1, @period_end_date)
SELECT @12PeriodsAgoID = (SELECT period_id
                            FROM period
                           WHERE period_end_date = @dt12PEriodsAgo
                             AND company_id = @iBranchPeriod )
IF @12PeriodsAgoID IS NULL
BEGIN
    SELECT @12PeriodsAgoID = 1
    SELECT @dt12PeriodsAgo = ( SELECT period_end_date
                                 FROM period
                                WHERE period_id = @12PeriodsAgoID )
END
ELSE
BEGIN
    SELECT @dt12PeriodsAgo =
        ( SELECT max(period_end_Date)
            FROM Period
           WHERE period_id < @12PeriodsAgoID
             AND company_id = @iBranchPeriod )
END

/*
-- get period end complete date
DECLARE @dtPEndComplete datetime
EXECUTE spu_Report_GetPeriodEndComplete_Date_SFU @dtPEndComplete OUTPUT

-- get current period values
DECLARE @CurrentPeriodID int, @dtCurrentPeriodEnd datetime
EXECUTE spu_Report_GetCurrentPeriod_SFU @CurrentPeriodID OUTPUT,  @dtCurrentPeriodEnd OUTPUT
*/
-- get current year values
DECLARE @CurrentYearStartPeriodID int, @dtLastYearPeriodEndDate datetime
EXECUTE spu_Report_GetCurrentYear_SFU @CurrentYearStartPeriodID OUTPUT, @dtLastYearPeriodEndDate OUTPUT


IF @DEBUG = 1
BEGIN
    SELECT ' @SelectedPeriodID = ',  @SelectedPeriodID
    SELECT ' @iPeriod = ',  @iPeriod
    SELECT ' @dtSelectedPeriodEnd = ',  @dtSelectedPeriodEnd
    SELECT ' @period_end_date   = ',  @period_end_date
    SELECT ' @prev_period_end_date = ',  @prev_period_end_date
    SELECT ' @YearStartPeriodID= ',  @YearStartPeriodID
    SELECT ' @dtYearStart = ',  @dtYearStart
    SELECT ' @12PeriodsAgoID = ',  @12PeriodsAgoID
    SELECT ' @dt12PeriodsAgo = ',  @dt12PeriodsAgo
    SELECT ' @CurrentYearStartPeriodID= ', @CurrentYearStartPeriodID
    SELECT ' @dtLastYearPeriodEndDate= ', @dtLastYearPeriodEndDate
END



CREATE TABLE #tempRSANetPrem
(
    ProductCode varchar (10) NULL,
    ProductDesc varchar (255) NULL,
    RiskTypeID int NULL,
    RiskTypeDesc varchar (255) NULL,
    PeriodFlag int NULL,           -- 0=Current Period, 1 = Cumulative (current year to date)
    PeriodName varchar (20) NULL,
    ShortName varchar (30) NULL,
    PremiumAmount decimal (19,4) NULL,
    CommissionAmount decimal (19,4) NULL,
    ClaimAmount decimal (19,4) NULL,
    TransDate datetime NULL,
    dtCurrentPeriodEnd datetime,
    dtLastYearEnd datetime,
    PostingPeriodID int,
    CurrentPeriodID int,
	CurrencyCode VARCHAR(10),
	CurrencyDesc VARCHAR(255)
)

-- CURRENT PERIOD

-- SELECTED PERIOD
IF @iBasis = 0 -- Transaction Period
BEGIN

    -- Add Premium Records
    INSERT INTO #tempRSANetPrem
        SELECT sf.product_code,
            p.description,
            sd.risk_type_id,
            (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
            0,
            'Current Period',
            (select isnull(agent_shortname, insurance_holder_shortname)),
            CASE @TypeOfCurrency
				WHEN 'Base' THEN sd.this_premium_home
				WHEN 'System' THEN sd.this_premium_system
			END,/*Premium*/
            NULL,
            NULL,
            sf.transaction_date,
            @dtSelectedPeriodEnd,
            @dtLastYearPeriodEndDate,
            sf.posting_period_number,
            @SelectedPeriodID,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Code
				WHEN 'System' THEN @CurrencyCode
			END,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Description
				WHEN 'System' THEN @CurrencyDesc
			END 
        FROM Stats_Folder sf
        JOIN Product p          ON sf.product_id = p.product_id
        JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
        AND sd.stats_detail_type in ('GRS', 'TTY', 'FAC', 'COI')
        JOIN Currency CB ON sd.Currency_Code= CB.Code
        WHERE isnull(sd.this_premium_home,0) <> 0
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )

    -- Add Commission Records
    INSERT INTO #tempRSANetPrem
        SELECT sf.product_code,
            p.description,
            sd.risk_type_id,
            (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
            0,
            'Current Period',
            (select isnull(agent_shortname, insurance_holder_shortname)),
            NULL,
            CASE @TypeOfCurrency
				WHEN 'Base' THEN ISNULL(sd.lead_commission_value_home,0) + ISNULL(sd.sub_commission_value_home,0)
				WHEN 'System' THEN ISNULL(sd.lead_commission_value_system,0) + ISNULL(sd.sub_commission_value_system,0)
			END,/*Commission*/   
            NULL,
            sf.transaction_date,
            @dtSelectedPeriodEnd,
            @dtLastYearPeriodEndDate,
            sf.posting_period_number,
            @SelectedPeriodID,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Code
				WHEN 'System' THEN @CurrencyCode
			END,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Description
				WHEN 'System' THEN @CurrencyDesc
			END 
        FROM Stats_Folder sf
        JOIN Product p          ON sf.product_id = p.product_id
        JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND sd.stats_detail_type  in ('GRS', 'TTY', 'FAC', 'COI', 'SUB')
        JOIN Currency CB ON SD.Currency_Code = CB.Code
        WHERE isnull(sd.this_premium_home,0) <> 0
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )

    -- Add Claims Paid Records
    INSERT INTO #tempRSANetPrem
        SELECT sf.product_code,
            p.description,
            sd.risk_type_id,
            (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
            0,
            'Current Period',
            (select isnull(agent_shortname, insurance_holder_shortname)),
            NULL,
            NULL,
            CASE @TypeOfCurrency
				WHEN 'Base' THEN sd.this_premium_home
				WHEN 'System' THEN sd.this_premium_system
			END,-- Claims Paid
            sf.transaction_date,
            @dtSelectedPeriodEnd,
            @dtLastYearPeriodEndDate,
            sf.posting_period_number,
            @SelectedPeriodID,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Code
				WHEN 'System' THEN @CurrencyCode
			END,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Description
				WHEN 'System' THEN @CurrencyDesc
			END 
        FROM Stats_Folder sf
        JOIN Product p          ON sf.product_id = p.product_id
        JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND sd.stats_detail_type ='NET' --in ('GRS', 'TTY', 'FAC', 'COI')
        JOIN Currency CB ON CB.Code= Sd.Currency_Code
        WHERE isnull(sd.this_premium_home,0) <> 0
        AND sf.posting_period_number = @SelectedPeriodID
        AND sf.transaction_type_code = ('C_CP')                   -- claims payments only
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )

    -- CURRENT YEAR TO DATE, inclusive
    -- Add Premium Records
    INSERT INTO #tempRSANetPrem
        SELECT sf.product_code,
            p.description,
            sd.risk_type_id,
            (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
            1,
            'Cumulative Period',
            (select isnull(agent_shortname, insurance_holder_shortname)),
            CASE @TypeOfCurrency
				WHEN 'Base' THEN sd.this_premium_home
				WHEN 'System' THEN sd.this_premium_system
			END,--Premium
            NULL,
            NULL,
            sf.transaction_date,
            @dtSelectedPeriodEnd,
            @dtLastYearPeriodEndDate,
            sf.posting_period_number,
            @SelectedPeriodID,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Code
				WHEN 'System' THEN @CurrencyCode
			END,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Description
				WHEN 'System' THEN @CurrencyDesc
			END 
        FROM Stats_Folder sf
        JOIN Product p          ON sf.product_id = p.product_id
        JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND sd.stats_detail_type in ('GRS', 'TTY', 'FAC', 'COI')
        JOIN Currency CB ON CB.code = SD.Currency_code
        WHERE isnull(sd.this_premium_home,0) <> 0
        AND sf.posting_period_number BETWEEN  @CurrentYearStartPeriodID AND @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )

    -- Add Commission Records
    INSERT INTO #tempRSANetPrem
        SELECT sf.product_code,
            p.description,
            sd.risk_type_id,
            (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
            1,
            'Cumulative Period',
            (select isnull(agent_shortname, insurance_holder_shortname)),
            NULL,
            CASE @TypeOfCurrency
				WHEN 'Base' THEN ISNULL(sd.lead_commission_value_home,0) + ISNULL(sd.sub_commission_value_home,0)
				WHEN 'System' THEN ISNULL(sd.lead_commission_value_system,0) + ISNULL(sd.sub_commission_value_system,0)
			END,--Commission
            NULL,
            sf.transaction_date,
            @dtSelectedPeriodEnd,
            @dtLastYearPeriodEndDate,
            sf.posting_period_number,
            @SelectedPeriodID,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Code
				WHEN 'System' THEN @CurrencyCode
			END,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Description
				WHEN 'System' THEN @CurrencyDesc
			END 
        FROM Stats_Folder sf
        JOIN Product p          ON sf.product_id = p.product_id
        JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND sd.stats_detail_type in ('GRS', 'TTY', 'FAC', 'COI', 'SUB')
         JOIN Currency CB ON CB.Code = SD.Currency_Code
        WHERE isnull(sd.this_premium_home,0) <> 0
        AND sf.posting_period_number BETWEEN  @CurrentYearStartPeriodID AND @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )

    -- Add Claims Paid Records
    INSERT INTO #tempRSANetPrem
        SELECT sf.product_code,
            p.description,
            sd.risk_type_id,
            (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
            1,
            'Cumulative Period',
            (select isnull(agent_shortname, insurance_holder_shortname)),
            NULL,
            NULL,
            CASE @TypeOfCurrency
				WHEN 'Base' THEN sd.this_premium_home
				WHEN 'System' THEN sd.this_premium_system
			END,-- Claims Paid
            sf.transaction_date,
            @dtSelectedPeriodEnd,
            @dtLastYearPeriodEndDate,
            sf.posting_period_number,
            @SelectedPeriodID,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Code
				WHEN 'System' THEN @CurrencyCode
			END,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Description
				WHEN 'System' THEN @CurrencyDesc
			END 
        FROM Stats_Folder sf
        JOIN Product p          ON sf.product_id = p.product_id
        JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND sd.stats_detail_type='NET' -- in ('GRS', 'TTY', 'FAC', 'COI')
        JOIN Currency CB ON CB.Code = SD.Currency_code
        WHERE isnull(sd.this_premium_home,0) <> 0
        AND sf.posting_period_number BETWEEN  @CurrentYearStartPeriodID AND @SelectedPeriodID
        AND sf.transaction_type_code = ('C_CP')                   -- claims payments only
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )

END  -- iBasis = 0 Transaction Period
ELSE
BEGIN -- iBasis <> 0 Transaction Date
    -- Add Premium Records
    INSERT INTO #tempRSANetPrem
        SELECT sf.product_code,
            p.description,
            sd.risk_type_id,
            (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
            0,
            'Current Period',
            (select isnull(agent_shortname, insurance_holder_shortname)),
            CASE @TypeOfCurrency
				WHEN 'Base' THEN sd.this_premium_home
				WHEN 'System' THEN sd.this_premium_system
			END,--Premium
            NULL,
            NULL,
            sf.transaction_date,
            @dtSelectedPeriodEnd,
            @dtLastYearPeriodEndDate,
            sf.posting_period_number,
            @SelectedPeriodID,
				Case @TypeOfCurrency
					WHEN 'Base' THEN CB.Code
					WHEN 'System' THEN @CurrencyCode
				END,
				Case @TypeOfCurrency
					WHEN 'Base' THEN CB.Description
					WHEN 'System' THEN @CurrencyDesc
				END 
        FROM Stats_Folder sf
        JOIN Product p          ON sf.product_id = p.product_id
        JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND sd.stats_detail_type in ('GRS', 'TTY', 'FAC', 'COI')
        JOIN Currency CB ON CB.Code = SD.Currency_code
        WHERE isnull(sd.this_premium_home,0) <> 0
        AND ( sf.document_date > @prev_period_end_date AND sf.document_date <= @period_end_date)
        AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )

    -- Add Commission Records
    INSERT INTO #tempRSANetPrem
        SELECT sf.product_code,
            p.description,
            sd.risk_type_id,
            (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
            0,
            'Current Period',
            (select isnull(agent_shortname, insurance_holder_shortname)),
            NULL,
            CASE @TypeOfCurrency
				WHEN 'Base' THEN ISNULL(sd.lead_commission_value_home,0) + ISNULL(sd.sub_commission_value_home,0)
				WHEN 'System' THEN ISNULL(sd.lead_commission_value_system,0) + ISNULL(sd.sub_commission_value_system,0)
			END,--Commission
            NULL,
            sf.transaction_date,
            @dtSelectedPeriodEnd,
            @dtLastYearPeriodEndDate,
            sf.posting_period_number,
            @SelectedPeriodID,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Code
				WHEN 'System' THEN @CurrencyCode
			END,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Description
				WHEN 'System' THEN @CurrencyDesc
			END         FROM Stats_Folder sf
        JOIN Product p          ON sf.product_id = p.product_id
        JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND sd.stats_detail_type  in ('GRS', 'TTY', 'FAC', 'COI', 'SUB')
        JOIN Currency CB ON CB.Code = SD.Currency_code
        WHERE isnull(sd.this_premium_home,0) <> 0
        AND ( sf.document_date > @prev_period_end_date AND sf.document_date <= @period_end_date)
        AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )

    -- Add Claims Paid Records
    INSERT INTO #tempRSANetPrem
        SELECT sf.product_code,
            p.description,
            sd.risk_type_id,
            (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
            0,
            'Current Period',
            (select isnull(agent_shortname, insurance_holder_shortname)),
            NULL,
            NULL,
            CASE @TypeOfCurrency
				WHEN 'Base' THEN sd.this_premium_home
				WHEN 'System' THEN sd.this_premium_system
			END,-- Claims Paid
            sf.transaction_date,
            @dtSelectedPeriodEnd,
            @dtLastYearPeriodEndDate,
            sf.posting_period_number,
            @SelectedPeriodID,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Code
				WHEN 'System' THEN @CurrencyCode
			END,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Description
				WHEN 'System' THEN @CurrencyDesc
			END         
			FROM Stats_Folder sf
        JOIN Product p          ON sf.product_id = p.product_id
        JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND sd.stats_detail_type ='NET' -- in ('GRS', 'TTY', 'FAC', 'COI')
        JOIN Currency CB ON CB.Code = SD.Currency_code
        WHERE isnull(sd.this_premium_home,0) <> 0
        AND ( sf.document_date > @prev_period_end_date AND sf.document_date <= @period_end_date)
        AND sf.transaction_type_code = ('C_CP')                   -- claims payments only
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )

    -- CURRENT YEAR TO DATE, inclusive
    -- Add Premium Records
    INSERT INTO #tempRSANetPrem
        SELECT sf.product_code,
            p.description,
            sd.risk_type_id,
            (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
            1,
            'Cumulative Period',
            (select isnull(agent_shortname, insurance_holder_shortname)),
            CASE @TypeOfCurrency
				WHEN 'Base' THEN sd.this_premium_home
				WHEN 'System' THEN sd.this_premium_system
			END,--Premium
            NULL,
            NULL,
            sf.transaction_date,
            @dtSelectedPeriodEnd,
            @dtLastYearPeriodEndDate,
            sf.posting_period_number,
            @SelectedPeriodID,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Code
				WHEN 'System' THEN @CurrencyCode
			END,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Description
				WHEN 'System' THEN @CurrencyDesc
			END         
			FROM Stats_Folder sf
        JOIN Product p          ON sf.product_id = p.product_id
        JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND sd.stats_detail_type in ('GRS', 'TTY', 'FAC', 'COI')
        JOIN Currency CB ON CB.Code = SD.Currency_code
        WHERE isnull(sd.this_premium_home,0) <> 0
        AND sf.posting_period_number BETWEEN  @CurrentYearStartPeriodID AND @SelectedPeriodID
        AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )

    -- Add Commission Records
    INSERT INTO #tempRSANetPrem
        SELECT sf.product_code,
            p.description,
            sd.risk_type_id,
            (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
            1,
            'Cumulative Period',
            (select isnull(agent_shortname, insurance_holder_shortname)),
            NULL,
            CASE @TypeOfCurrency
				WHEN 'Base' THEN ISNULL(sd.lead_commission_value_home,0) + ISNULL(sd.sub_commission_value_home,0)
				WHEN 'System' THEN ISNULL(sd.lead_commission_value_system,0) + ISNULL(sd.sub_commission_value_system,0)
			END,--Commission
            NULL,
            sf.transaction_date,
            @dtSelectedPeriodEnd,
            @dtLastYearPeriodEndDate,
            sf.posting_period_number,
            @SelectedPeriodID,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Code
				WHEN 'System' THEN @CurrencyCode
			END,
			Case @TypeOfCurrency
				WHEN 'Base' THEN CB.Description
				WHEN 'System' THEN @CurrencyDesc
			END         
			FROM Stats_Folder sf
        JOIN Product p          ON sf.product_id = p.product_id
        JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND sd.stats_detail_type in ('GRS', 'TTY', 'FAC', 'COI', 'SUB')
        JOIN Currency CB ON CB.Code = SD.Currency_code
        WHERE isnull(sd.this_premium_home,0) <> 0
        AND ( sf.document_date > @dtYearStart AND sf.document_date <= @period_end_date )
        AND sf.transaction_type_code NOT LIKE ('C_%')                   -- all but claims
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )

    -- Add Claims Paid Records
    INSERT INTO #tempRSANetPrem
        SELECT sf.product_code,
            p.description,
            sd.risk_type_id,
            (SELECT description FROM Risk_Type WHERE risk_type_id = sd.risk_type_id),
            1,
            'Cumulative Period',
            (select isnull(agent_shortname, insurance_holder_shortname)),
            NULL,
            NULL,
            CASE @TypeOfCurrency
				WHEN 'Base' THEN sd.this_premium_home
				WHEN 'System' THEN sd.this_premium_system
			END,-- Claims Paid
            sf.transaction_date,
            @dtSelectedPeriodEnd,
            @dtLastYearPeriodEndDate,
            sf.posting_period_number,
            @SelectedPeriodID,
			@CurrencyCode,
			@CurrencyDesc
        FROM Stats_Folder sf
        JOIN Product p          ON sf.product_id = p.product_id
        JOIN Stats_Detail sd    ON sf.stats_folder_cnt = sd.stats_folder_cnt
            AND sd.stats_detail_type='NET' -- in ('GRS', 'TTY', 'FAC', 'COI')
        JOIN Currency CB ON CB.Code = SD.Currency_code
        WHERE isnull(sd.this_premium_home,0) <> 0
        AND ( sf.document_date > @dtYearStart AND sf.document_date <= @period_end_date )
        AND sf.transaction_type_code = ('C_CP')                   -- claims payments only
        AND ( @iBranchID = 0
              or    (   @iBranchID <> 0 and sf.Branch_id = @iBranchID )
            )

END -- iBasis <> 0 Transaction Date



SET NOCOUNT OFF
SELECT * FROM #tempRSANetPrem

DROP TABLE #tempRSANetPrem

GO

-- End of $Workfile: sp_Report_Net_Premium.sql $
