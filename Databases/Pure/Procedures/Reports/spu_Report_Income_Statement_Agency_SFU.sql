SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Income_Statement_Agency_SFU'
GO

/**********************************************************************************************************************************
** Created by Rajesh Choudhary
** 17 Feb 2006
** S4I Reports - Income_Statement_Agency.rpt
**
**********************************************************************************************************************************
** Based on Back Office Income Statement Report
** Uses existing Orion SPs to get Tree Structure information
** Use node_id = 3 because existing Back Office report does! But Why?
**                                                   Because:   - 1 = Balance Sheet & Profit and Loss
**                                                              - 2 = Balance Sheet
**                                                              - 3 = Profit and Loss
**
** NB - Make sure Orion referred to as "Orion_For_Broking"
**********************************************************************************************************************************
***********************************************************************************************************************************/

CREATE PROCEDURE spu_Report_Income_Statement_Agency_SFU
        @branch_id int,
--        @period_end_date datetime,
        @PeriodDate varchar(20),
        @sType varchar(30),
        @sBasis varchar(50),
        @TypeOfCurrency varchar(15),
        @GroupBYCode	Varchar(50),
        @IncludeClaims	Varchar(3)
AS

DECLARE @DEBUG INT
SELECT @DEBUG = 0    --- 0 = OFF,  1 = ON
DECLARE @period_end_date    datetime
SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @period_end_date = CONVERT (Datetime, @PeriodDate)

/************************ Populate the Orion Tables ************************************/
/* sp_Report_ShortTreePathNames calls sp_Report_Treepath.
   Both SPs use the Orion tables: Report_Nodes, Report_Treepath, Report_TreepathNames
   ... which are cleared down and re-populated */


--SET NOCOUNT ON

-- which period do we want to base this report on?
DECLARE @SelectedPeriodEndID int, @dtSelectedPeriodEnd datetime
DECLARE @SelectedPeriodStartID int, @dtSelectedPeriodStart datetime
DECLARE @CurrentYearPeriodStartID int, @dtLastYearPeriodEnd datetime
DECLARE @LastYearPeriodStartID int,  @dt2YearsAgoPeriodEnd datetime
DECLARE @12MonthsAgoPeriodEndID int, @dt12MonthsAgoPeriodEnd datetime
DECLARE @12MonthsAgoPeriodStartID int, @dt12MonthsAgoPeriodStart datetime

DECLARE @AnyID int, @dtAnyDate datetime


--SELECT @sPeriodEndDate = @sPeriodEndDate + " 23:59:59.000"
SELECT @dtSelectedPeriodEnd = ISNULL(@Period_End_Date, GETDATE())  -- CONVERT (Datetime, @sPeriodEndDate)

DECLARE @iBasis INT
DECLARE @iPeriod INT
DECLARE @iBranchPeriod INT
-- Always use Branch 1 period table
-- if anyone sets up different periods for different branches this will
-- need to be revisited and worked out in the cursor
SELECT @iBranchPeriod = 1
    IF @sBasis = 'Transaction Date'
        SELECT @iBasis = 1    -- Transaction Date
    ELSE
        SELECT @iBasis = 0    -- Transaction Period


--EXEC Orion_for_rsa_transfer..sp_Report_ShortTreePathNames 3
EXECUTE spu_Report_FullTreePathNames_SFU 3


/************************ Get Account details based on Orion.Report_TreePathNames ******/
CREATE TABLE #tmpRSAIncomeStatement
(
        ReportMapID1 int NULL,
        ElementName1 varchar (30) NULL,
        ReportMapID2 int NULL,
        ElementName2 varchar (30) NULL,
        ReportMapID3 int NULL,
        ElementName3 varchar (30) NULL,
        ReportMapID4 int NULL,
        ElementName4 varchar (30) NULL,
        ReportMapID5 int NULL,
        ElementName5 varchar (30) NULL,
        ReportMapID6 int NULL,
        ElementName6 varchar (30) NULL,
        ReportMapID7 int NULL,
        ElementName7 varchar (30) NULL,
        ReportMapID8 int NULL,
        ElementName8 varchar (30) NULL,
        ReportMapID9 int NULL,
        ElementName9 varchar (30) NULL,
        ReportMapID10 int NULL,
        ElementName10 varchar (30) NULL,
        AccountID int,
        Account varchar (60) NULL,
        AccountCode varchar (30) NULL,
        amountTMTY money NULL,             /* Current Period */
        amountTMLY money NULL,             /* Equivalent Period Last Year */
        amountYTDTY money NULL,            /* Current Year To Date */
        amountYTDLY money NULL,            /* Same Period Last Year */
        LedgerType varchar (255) NULL,
        AccountType varchar (255) NULL,
        CurrencyCode varchar(15) NULL,
        CurrencyDesc varchar(255) NULL,
        CompanyCode	Varchar(15) NULL,
        CompanyDesc	Varchar(255) Null,
        GroupByCode	Varchar(50) NULL
)

DECLARE @iBranchID INT
DECLARE @iCompanyID INT
DECLARE @iCompanyCode varchar(15)
DECLARE @iCompanyDesc varchar(255)
DECLARE @iBaseCurrency varchar(255)
DECLARE @SystemCurrencyCode varchar(15)
DECLARE @SystemCurrencyDesc varchar(255)
SELECT @iBranchID = ISNULL(@branch_id, 0)
--SELECT @iBranchID = 0

/*Get System Currency Details*/
	SELECT
		@SystemCurrencyCode = c.iso_code,
		@SystemCurrencyDesc = c.description
	FROM PMSystem pms
	JOIN currency c
		ON c.currency_id = pms.currency_id
	WHERE pms.system_id = 1
/*******************/

IF @iBranchID = 0
BEGIN
    DECLARE company_cursor CURSOR
    FOR     select company_id,Company.code,Company.Description,Base_Currency
            from  company
END
ELSE
BEGIN
    DECLARE company_cursor CURSOR
    FOR     select company_id,Company.code,Company.Description,Base_Currency
            from  company
            where company_id = @iBranchID
END

OPEN company_cursor

FETCH NEXT FROM company_cursor INTO @iCompanyid,@iCompanyCode,@iCompanyDesc,@iBaseCurrency
WHILE @@FETCH_STATUS = 0
BEGIN        -- Company Cursor


    -- Report uses these Dates

    -- Report on Selected Period
        -- Selected Period End Date         @dtSelectedPeriodEnd
        -- Selected Period Start Date       @dtSelectedPeriodStart

    -- Report on Current Year to Date
        -- Current Year Start Date (or Previous Year End Date)  @dtLastYearPeriodEnd

    -- Report on Equivalent periods of Last year
        -- Last Year Start Date (or 2 Years ago End Date)       @dt2YearsAgoPeriodEnd

    -- 12 Months Ago - report on single period
        -- 12 Months Ago Period End Date    @dt12MonthsAgoPeriodEnd
        -- 12 Months Ago Perios Start Date  @dt12MonthsAgoPeriodStart

--  selected period End
    SELECT @SelectedPeriodEndID = period_id
      FROM  Period
     WHERE period_end_date = @dtSelectedPeriodEnd
       AND company_id = @iBranchPeriod

--  selected period Start, Take off a month
--    SELECT @dtSelectedPeriodStart = DATEADD(month, -1, @dtSelectedPeriodEnd)
-- work out a temporary date
    SELECT @dtAnyDate = DATEADD(day, DATEPART(dd, @dtSelectedPeriodEnd) * -1, @dtSelectedPeriodEnd)
-- Look up the corresponding period end
    SELECT @dtSelectedPeriodStart = (SELECT ISNULL(MAX(P.period_end_date), @dtAnyDate)
                           FROM  Period    P
                      WHERE P.period_end_date < @dtSelectedPeriodEnd
                        AND     p.company_id = @iBranchPeriod )

    SELECT @SelectedPeriodStartID = period_id
      FROM  Period
     WHERE period_end_date = @dtSelectedPeriodStart
       AND company_id = @iBranchPeriod

    -- current year start period ID
    SELECT @CurrentYearPeriodStartID = period_id
      FROM  period
     WHERE period_end_date = (SELECT MIN(period_end_date)
                                FROM  Period
                               WHERE year_name = (SELECT year_name
                                                 FROM  period
                                                WHERE period_id = @SelectedPeriodEndID)
                            )
       AND company_id = @iBranchPeriod
    -- Get the date of current year start, then subtract a month
    SELECT @dtAnyDate = period_end_date
      FROM  Period
     WHERE period_id = @CurrentYearPeriodStartID
       AND company_id = @iBranchPeriod
    -- Take off the days
    SELECT @dtLastYEarPeriodEnd =DATEADD(day, DATEPART(dd, @dtAnyDate) * -1, @dtAnyDate)
    -- Get previous period
    SELECT @dtLastYearPeriodEnd = ( SELECT MAX(period_end_date)
                                      FROM  Period
                                     WHERE period_end_date <= @dtLastYearPeriodEnd
                                       AND company_id = @iBranchPeriod )

    -- Get the ID of the above Date
    SELECT @AnyID = period_id
      FROM  Period
     WHERE period_end_date = @dtLastYEarPeriodEnd
       AND company_id = @iBranchPeriod

    -- last years start period ID - using LastYearPeriodEnd
    SELECT @LastYEarPeriodStartID = period_id
      FROM  Period
     WHERE period_end_date = (SELECT MIN(Period_end_Date)
                                FROM  Period
                               WHERE year_name = (SELECT year_name
                                                    FROM  PEriod
                                                   WHERE period_id = @AnyID)
                              )
      AND company_id = @iBranchPeriod

    IF @LastYearPeriodStartID IS NULL
    BEGIN
        -- No Last year! - use the minimum date on the system
        SELECT @LastYearPeriodStartID =  period_id
          FROM  Period
         WHERE Period_End_Date = (SELECT MIN(period_end_date)
                                    FROM  Period
                                   WHERE company_id = @iBranchPeriod)
           AND company_id = @iBranchPeriod
        -- Then Get the Date of that period
        SELECT @dtLastYearPeriodEnd = period_end_date
          FROM  Period
         WHERE period_id = @LastYearPeriodStartID
    END
    ELSE
    BEGIN
        -- subtract 1 month from lastyear start to get 2years ago end
        SELECT @dtAnyDate = period_end_date
          FROM  Period
         WHERE period_id = @LastYearPeriodStartID
           AND company_id = @iBranchPeriod
        SELECT @dt2YearsAgoPeriodEnd = DATEADD(month, -1, @dtAnyDate)
    END



    -- get the 12 periods Ago ID take 12 months off current Date
    SELECT @dt12MonthsAgoPeriodEnd = DATEADD(month, -12, @dtSelectedPeriodEnd)
    -- get the ID of this Date
    SELECT @12MonthsAgoPeriodEndID = period_id
      FROM  Period
     WHERE period_end_date = @dt12MonthsAgoPeriodEnd
       AND company_id = @iBranchPeriod

    -- If there is no 12 months ago date, use the oldest period for that company
    IF @12MonthsAgoPeriodEndID IS NULL
    BEGIN
        SELECT @12MonthsAgoPeriodEndID =  period_id
          FROM  Period
         WHERE Period_End_Date = (SELECT MIN(period_end_date)
                                    FROM  Period
                                   WHERE company_id = @iBranchPeriod)
           AND  company_id = @iBranchPeriod
        -- Then Get the Date of that period
        SELECT @dt12MonthsAgoPeriodEnd = period_end_date
          FROM  Period
         WHERE period_id = @12MonthsAgoPeriodEndID
    END
    ELSE
    BEGIN
        -- Get the previous period by subtracting a month
--        SELECT @dt12MonthsAgoPeriodStart = DATEADD(month, -1, @dt12MonthsAgoPeriodEnd)
        -- work out a temporary date
        SELECT @dtAnyDate = DATEADD(day, DATEPART(dd, @dt12MonthsAgoPeriodEnd ) * -1, @dt12MonthsAgoPeriodEnd )
        -- Look up the corresponding period end
        SELECT @dt12MonthsAgoPeriodStart = ( SELECT ISNULL(MAX(P.period_end_date), @dtAnyDate)
                                   FROM  Period    P
                              WHERE P.period_end_date < @dt12MonthsAgoPeriodEnd
                                AND     p.company_id = @iBranchPeriod)
        SELECT @12MonthsAgoPeriodStartID = (SELECT period_id
                                              FROM  period P
                                             WHERE p.period_end_date = @dt12MonthsAgoPeriodStart
                                               AND p.company_id = @iBranchPeriod)
    END

    -- TB 09/10/02 put these back in as RSA want Income statement
    -- Print out the dates used in the report
    IF @DEBUG = 1
    BEGIN
        SELECT 'Selected Period  End  ID = ', @SelectedPeriodEndID, ' Date = ', Convert(varchar(20),@dtSelectedPeriodEnd,113)
        SELECT 'Selected Period Start ID = ', @SelectedPeriodStartID, ' Date = ', Convert(varchar(20),@dtSelectedPeriodStart,113)
        SELECT '12 Periods Ago End ID    = ', @12MonthsAgoPeriodEndID, ' Date = ', Convert(Varchar(20), @dt12MonthsAgoPeriodEnd,113)
        SELECT '12 Periods Ago Start ID  = ', @12MonthsAgoPeriodStartID, ' Date = ', Convert(Varchar(20), @dt12MonthsAgoPeriodStart,113)
        SELECT 'Current Year Start ID ', @CurrentYearPeriodStartID, ' Date = ', convert(varchar(20), @dtLastYearPeriodEnd, 113)
        SELECT 'Last YEar Start ID    = ', @LastYEarPeriodStartID, ' Date = ', Convert(varchar(20), @dt2YEarsAgoPeriodEnd, 113)
    END
    -- TB 16/10/02 Report Type,
    IF @sType = 'All Dates'
    BEGIN
        SELECT @dtLastYearPeriodEnd = '1900-01-01'
        SELECT @dt2YEarsAgoPeriodEnd = '1900-01-01'
        SELECT @CurrentYearPeriodStartID = 0
        SELECT @LastYEarPeriodStartID = 0
    END

    IF @DEBUG = 1
        PRINT ' Current Period'


    -- TB 12/11/02:  Report Basis, by Transaction Date or Period
    IF @iBasis = 0   --- Transaction Period
    BEGIN
        -- Current Period
        INSERT INTO #tmpRSAIncomeStatement
                SELECT
                        RTPN.Report_Map_Id1,
                        RTPN.element_name1,
                        RTPN.Report_Map_Id2,
                        RTPN.element_name2,
                        RTPN.Report_Map_Id3,
                        RTPN.element_name3,
                        RTPN.Report_Map_Id4,
                        RTPN.element_name4,
                        RTPN.Report_Map_Id5,
                        RTPN.element_name5,
                        RTPN.Report_Map_Id6,
                        RTPN.element_name6,
                        RTPN.Report_Map_Id7,
                        RTPN.element_name7,
                        RTPN.Report_Map_Id8,
                        RTPN.element_name8,
                        RTPN.Report_Map_Id9,
                        RTPN.element_name9,
                        RTPN.Report_Map_Id10,
                        RTPN.element_name10,
                        a.account_id,
                        a.account_name,
                        a.short_code,
                -- Current Period
                  --      sum(td.amount),
	                   CASE @TypeOfCurrency
							WHEN 'Base' THEN ISNULL(SUM(ROUND(TD.amount,2)), 0.0)
							WHEN 'System' THEN ISNULL(SUM(ROUND(TD.system_amount,2)),0.0)
						END,
                        NULL,
                        NULL,
                        NULL,
        --        lt.description,
        --        act.description
                        NULL,
                        NULL,
                         CASE @TypeOfCurrency
							WHEN 'Base' THEN cb.iso_code
							WHEN 'System' THEN @SystemCurrencyCode
						END,
						CASE @TypeOfCurrency
							WHEN 'Base' THEN cb.description
							WHEN 'System' THEN @SystemCurrencyDesc
						END, --added later for multicurrency
                        @iCompanyCode,@iCompanyDesc,
                         CASE  @Groupbycode 
						 	WHEN 'Branch' THEN @iCompanyCode
						    WHEN 'Branch and Currency' THEN @iCompanyCode
					 		ELSE ''
	 		   			END 'GroupByCode'
                        
        FROM     Report_TreePathNames RTPN
                INNER JOIN  Account a                ON RTPN.account_id = a.account_id
                INNER JOIN  TransDetail td           ON a.account_id = td.account_id
                INNER JOIN  Document d               ON td.document_id = d.document_id
                INNER JOIN	Currency CB				 ON CB.Currency_id = @iBaseCurrency
        --        LEFT OUTER JOIN  Ledger l            ON a.ledger_id = l.ledger_id
        --        LEFT OUTER JOIN  LedgerType lt       ON l.ledgertype_id = lt.ledgertype_id
        --        LEFT OUTER JOIN  AccountType act     ON a.accounttype_id = act.accounttype_id
        WHERE D.company_id = @iCompanyid
            AND ( ( TD.period_id = @SelectedPeriodEndID )
                  AND ISNULL(TD.spare, '') NOT IN ('COMM ADJ', 'AGENT ADJ')
                  AND TD.Document_Sequence NOT IN
                    ( SELECT  Document_Sequence + 1
                      FROM     TransDetail
                      WHERE   document_id = D.document_id
                      AND spare = 'COMM ADJ' )
                )
            OR ( ( TD.period_id = @SelectedPeriodEndID )
                  AND ISNULL(TD.spare, '') IN ('COMM ADJ', 'AGENT ADJ')
                  OR TD.Document_Sequence IN
                    ( SELECT  Document_Sequence + 1
                      FROM     TransDetail
                      WHERE   document_id = D.document_id
                      AND spare = 'COMM ADJ' )
                )
        GROUP BY
                        RTPN.Report_Map_Id1,
                        RTPN.element_name1,
                        RTPN.Report_Map_Id2,
                        RTPN.element_name2,
                        RTPN.Report_Map_Id3,
                        RTPN.element_name3,
                        RTPN.Report_Map_Id4,
                        RTPN.element_name4,
                        RTPN.Report_Map_Id5,
                        RTPN.element_name5,
                        RTPN.Report_Map_Id6,
                        RTPN.element_name6,
                        RTPN.Report_Map_Id7,
                        RTPN.element_name7,
                        RTPN.Report_Map_Id8,
                        RTPN.element_name8,
                        RTPN.Report_Map_Id9,
                        RTPN.element_name9,
                        RTPN.Report_Map_Id10,
                        RTPN.element_name10,
                        a.account_id,
                        a.account_name,
                        a.short_code,
                        CB.iso_Code,CB.Description --added later for multicurrency
                        --@CompanyCode,@CompanyDesc

        -- Date range 2
        IF @DEBUG = 1
            Print ' current period last year'

        INSERT INTO #tmpRSAIncomeStatement
                SELECT
                        RTPN.Report_Map_Id1,
                        RTPN.element_name1,
                        RTPN.Report_Map_Id2,
                        RTPN.element_name2,
                        RTPN.Report_Map_Id3,
                        RTPN.element_name3,
                        RTPN.Report_Map_Id4,
                        RTPN.element_name4,
                        RTPN.Report_Map_Id5,
                        RTPN.element_name5,
                        RTPN.Report_Map_Id6,
                        RTPN.element_name6,
                        RTPN.Report_Map_Id7,
                        RTPN.element_name7,
                        RTPN.Report_Map_Id8,
                        RTPN.element_name8,
                        RTPN.Report_Map_Id9,
                        RTPN.element_name9,
                        RTPN.Report_Map_Id10,
                        RTPN.element_name10,
                        a.account_id,
                        a.account_name,
                        a.short_code,
                        NULL,
        -- Current Period last year
                        --sum(td.amount),
                        CASE @TypeOfCurrency
							WHEN 'Base' THEN ISNULL(SUM(ROUND(TD.amount,2)), 0.0)
							WHEN 'System' THEN ISNULL(SUM(ROUND(TD.system_amount,2)),0.0) 
			   			END,
						NULL,
                        NULL,
        --        lt.description,
        --        act.description
                        NULL,
                        NULL,
                        CASE @TypeOfCurrency
							WHEN 'Base' THEN cb.iso_code
							WHEN 'System' THEN @SystemCurrencyCode
						END,
						CASE @TypeOfCurrency
							WHEN 'Base' THEN cb.description
							WHEN 'System' THEN @SystemCurrencyDesc
						END, --added later for multicurrency
                        @iCompanyCode,@iCompanyDesc,
  					   CASE  @Groupbycode
							WHEN 'Branch' THEN @iCompanyCode
							WHEN 'Branch and Currency' THEN @iCompanyCode
							ELSE ''
						END 'GroupByCode'

        FROM     Report_TreePathNames RTPN
                INNER JOIN  Account a                ON RTPN.account_id = a.account_id
                INNER JOIN  TransDetail td           ON a.account_id = td.account_id
                INNER JOIN  Document d               ON td.document_id = d.document_id
                INNER JOIN	Currency CB				 ON td.Currency_id= @iBaseCurrency
        --        LEFT OUTER JOIN  Ledger l            ON a.ledger_id = l.ledger_id
        --        LEFT OUTER JOIN  LedgerType lt       ON l.ledgertype_id = lt.ledgertype_id
        --        LEFT OUTER JOIN  AccountType act     ON a.accounttype_id = act.accounttype_id
        WHERE D.company_id = @iCompanyid
            AND ( (TD.period_id = @12MonthsAgoPeriodEndID )
                  AND ISNULL(TD.spare, '') NOT IN ('COMM ADJ', 'AGENT ADJ')
                  AND TD.Document_Sequence NOT IN
                    ( SELECT  Document_Sequence + 1
                      FROM     TransDetail
                      WHERE   document_id = D.document_id
                      AND spare = 'COMM ADJ' )
                )
             OR ( (TD.period_id = @12MonthsAgoPeriodEndID )
                  AND ISNULL(TD.spare, '') IN ('COMM ADJ', 'AGENT ADJ')
                  OR TD.Document_Sequence IN
                    ( SELECT  Document_Sequence + 1
                      FROM     TransDetail
                      WHERE   document_id = D.document_id
                      AND spare = 'COMM ADJ' )
                )
        GROUP BY
                        RTPN.Report_Map_Id1,
                        RTPN.element_name1,
                        RTPN.Report_Map_Id2,
                        RTPN.element_name2,
                        RTPN.Report_Map_Id3,
                        RTPN.element_name3,
                        RTPN.Report_Map_Id4,
                        RTPN.element_name4,
                        RTPN.Report_Map_Id5,
                        RTPN.element_name5,
                        RTPN.Report_Map_Id6,
                        RTPN.element_name6,
                        RTPN.Report_Map_Id7,
                        RTPN.element_name7,
                        RTPN.Report_Map_Id8,
                        RTPN.element_name8,
                        RTPN.Report_Map_Id9,
                        RTPN.element_name9,
                        RTPN.Report_Map_Id10,
                        RTPN.element_name10,
                        a.account_id,
                        a.account_name,
                        a.short_code,
                        CB.iso_code,CB.Description --added later for multicurrency
                        --@CompanyCode,@CompanyDesc


        -- Date Range 3
        IF @DEBUG = 1
            print 'This year, year to date'

        INSERT INTO #tmpRSAIncomeStatement
            SELECT
                    RTPN.Report_Map_Id1,
                    RTPN.element_name1,
                    RTPN.Report_Map_Id2,
                    RTPN.element_name2,
                    RTPN.Report_Map_Id3,
                    RTPN.element_name3,
                    RTPN.Report_Map_Id4,
                    RTPN.element_name4,
                    RTPN.Report_Map_Id5,
                    RTPN.element_name5,
                    RTPN.Report_Map_Id6,
                    RTPN.element_name6,
                    RTPN.Report_Map_Id7,
                    RTPN.element_name7,
                    RTPN.Report_Map_Id8,
                    RTPN.element_name8,
                    RTPN.Report_Map_Id9,
                    RTPN.element_name9,
                    RTPN.Report_Map_Id10,
                    RTPN.element_name10,
                    a.account_id,
                    a.account_name,
                    a.short_code,
                    NULL,
                    NULL,
                    --sum(td.amount),
                    CASE @TypeOfCurrency
 						WHEN 'Base' THEN ISNULL(SUM(ROUND(TD.amount,2)), 0.0)
						WHEN 'System' THEN ISNULL(SUM(ROUND(TD.system_amount,2)),0.0)
		    		END,
                    NULL,
        --        lt.description,
        --        act.description
                    NULL,
                    NULL,
                    CASE @TypeOfCurrency
						WHEN 'Base' THEN cb.iso_code
						WHEN 'System' THEN @SystemCurrencyCode
					END,
					CASE @TypeOfCurrency
						WHEN 'Base' THEN cb.description
						WHEN 'System' THEN @SystemCurrencyDesc
					END, --added later for multicurrency
                    @iCompanyCode,@iCompanyDesc,
				CASE  @Groupbycode 
					WHEN 'Branch' THEN @iCompanyCode
					WHEN 'Branch and Currency' THEN @iCompanyCode
					ELSE '' 
				END 'GroupByCode'

        FROM     Report_TreePathNames RTPN
            INNER JOIN  Account a                ON RTPN.account_id = a.account_id
            INNER JOIN  TransDetail td           ON a.account_id = td.account_id
            INNER JOIN  Document d               ON td.document_id = d.document_id
            INNER JOIN  Currency CB				 ON CB.currency_id = @iBaseCurrency
        --        LEFT OUTER JOIN  Ledger l            ON a.ledger_id = l.ledger_id
        --        LEFT OUTER JOIN  LedgerType lt       ON l.ledgertype_id = lt.ledgertype_id
        --        LEFT OUTER JOIN  AccountType act     ON a.accounttype_id = act.accounttype_id
        WHERE D.company_id = @iCompanyid
        AND    ( (TD.period_id <= @SelectedPeriodEndID AND TD.period_id > @CurrentYearPeriodStartID )
                  AND ISNULL(TD.spare, '') NOT IN ('COMM ADJ', 'AGENT ADJ')
                  AND TD.Document_Sequence NOT IN
                    ( SELECT  Document_Sequence + 1
                      FROM     TransDetail
                      WHERE   document_id = D.document_id
                      AND spare = 'COMM ADJ' )
               )
        OR ( (TD.period_id <= @SelectedPeriodEndID AND TD.period_id > @CurrentYearPeriodStartID )
                  AND ISNULL(TD.spare, '') IN ('COMM ADJ', 'AGENT ADJ')
                  OR TD.Document_Sequence IN
                    ( SELECT  Document_Sequence + 1
                      FROM     TransDetail
                      WHERE   document_id = D.document_id
                      AND spare = 'COMM ADJ' )
               )
        GROUP BY
                    RTPN.Report_Map_Id1,
                    RTPN.element_name1,
                    RTPN.Report_Map_Id2,
                    RTPN.element_name2,
                    RTPN.Report_Map_Id3,
                    RTPN.element_name3,
                    RTPN.Report_Map_Id4,
                    RTPN.element_name4,
                    RTPN.Report_Map_Id5,
                    RTPN.element_name5,
                    RTPN.Report_Map_Id6,
                    RTPN.element_name6,
                    RTPN.Report_Map_Id7,
                    RTPN.element_name7,
                    RTPN.Report_Map_Id8,
                    RTPN.element_name8,
                    RTPN.Report_Map_Id9,
                    RTPN.element_name9,
                    RTPN.Report_Map_Id10,
                    RTPN.element_name10,
                    a.account_id,
                    a.account_name,
                    a.short_code,
                    CB.iso_code,CB.Description --added later for multicurrency
                    --@CompanyCode,@CompanyDesc


       -- last year Year To Date
       IF @DEBUG = 1
            PRINT 'Last Year, Year to Date'

        INSERT INTO #tmpRSAIncomeStatement
            SELECT
                    RTPN.Report_Map_Id1,
                    RTPN.element_name1,
                    RTPN.Report_Map_Id2,
                    RTPN.element_name2,
                    RTPN.Report_Map_Id3,
                    RTPN.element_name3,
                    RTPN.Report_Map_Id4,
                    RTPN.element_name4,
                    RTPN.Report_Map_Id5,
                    RTPN.element_name5,
                    RTPN.Report_Map_Id6,
                    RTPN.element_name6,
                    RTPN.Report_Map_Id7,
                    RTPN.element_name7,
                    RTPN.Report_Map_Id8,
                    RTPN.element_name8,
                    RTPN.Report_Map_Id9,
                    RTPN.element_name9,
                    RTPN.Report_Map_Id10,
                    RTPN.element_name10,
                    a.account_id,
                    a.account_name,
                    a.short_code,
                    NULL,
                    NULL,
                    NULL,
                   --sum(td.amount),
                    CASE @TypeOfCurrency
		   				WHEN 'Base' THEN ISNULL(SUM(ROUND(TD.amount,2)), 0.0)
						WHEN 'System' THEN ISNULL(SUM(ROUND(TD.system_amount,2)),0.0) 
		    		END,
        --        lt.description,
        --        act.description
                    NULL,
                    NULL,
                    CASE @TypeOfCurrency
						WHEN 'Base' THEN cb.iso_code
						WHEN 'System' THEN @SystemCurrencyCode
					END,
					CASE @TypeOfCurrency
						WHEN 'Base' THEN cb.description
						WHEN 'System' THEN @SystemCurrencyDesc
					END, --added later for multicurrency
                    @iCompanyCode,@iCompanyDesc,
                    CASE  @Groupbycode 
						WHEN 'Branch' THEN @iCompanyCode
						WHEN 'Branch and Currency' THEN @iCompanyCode
					ELSE '' 
					END 'GroupByCode'
                    
        FROM     Report_TreePathNames RTPN
            INNER JOIN  Account a                ON RTPN.account_id = a.account_id
            INNER JOIN  TransDetail td           ON a.account_id = td.account_id
            INNER JOIN  Document d               ON td.document_id = d.document_id
            INNER JOIN	Currency CB				 ON CB.Currency_id = @iBaseCurrency
        --        LEFT OUTER JOIN  Ledger l            ON a.ledger_id = l.ledger_id
        --        LEFT OUTER JOIN  LedgerType lt       ON l.ledgertype_id = lt.ledgertype_id
        --        LEFT OUTER JOIN  AccountType act     ON a.accounttype_id = act.accounttype_id
        WHERE D.company_id = @iCompanyid
        AND	D.document_date > @dt2YearsAgoPeriodEnd AND D.document_date <= @dtLastYearPeriodEnd
        AND ISNULL(TD.spare, '') NOT IN ('COMM ADJ', 'AGENT ADJ')
        AND  TD.Document_Sequence NOT IN
                ( SELECT  Document_Sequence + 1
                  FROM     TransDetail
                  WHERE   document_id = D.document_id
                 AND spare = 'COMM ADJ' )
        AND ( td.document_sequence = 2  )
        GROUP BY
                    RTPN.Report_Map_Id1,
                    RTPN.element_name1,
                    RTPN.Report_Map_Id2,
                    RTPN.element_name2,
                    RTPN.Report_Map_Id3,
                    RTPN.element_name3,
                    RTPN.Report_Map_Id4,
                    RTPN.element_name4,
                    RTPN.Report_Map_Id5,
                    RTPN.element_name5,
                    RTPN.Report_Map_Id6,
                    RTPN.element_name6,
                    RTPN.Report_Map_Id7,
                    RTPN.element_name7,
                    RTPN.Report_Map_Id8,
                    RTPN.element_name8,
                    RTPN.Report_Map_Id9,
                    RTPN.element_name9,
                    RTPN.Report_Map_Id10,
                    RTPN.element_name10,
                    a.account_id,
                    a.account_name,
                    a.short_code,
                    CB.iso_code,CB.Description --added later for multicurrency
                    --@CompanyCode,@CompanyDesc


    END            -- @iBasis = 0, Transaction Period
    ELSE
    BEGIN            --- @iBasis <> 0,  Transaction Date
        -- Current Period
        INSERT INTO #tmpRSAIncomeStatement
                SELECT
                        RTPN.Report_Map_Id1,
                        RTPN.element_name1,
                        RTPN.Report_Map_Id2,
                        RTPN.element_name2,
                        RTPN.Report_Map_Id3,
                        RTPN.element_name3,
                        RTPN.Report_Map_Id4,
                        RTPN.element_name4,
                        RTPN.Report_Map_Id5,
                        RTPN.element_name5,
                        RTPN.Report_Map_Id6,
                        RTPN.element_name6,
                        RTPN.Report_Map_Id7,
                        RTPN.element_name7,
                        RTPN.Report_Map_Id8,
                        RTPN.element_name8,
                        RTPN.Report_Map_Id9,
                        RTPN.element_name9,
                        RTPN.Report_Map_Id10,
                        RTPN.element_name10,
                        a.account_id,
                        a.account_name,
                        a.short_code,
                -- Current Period
                --        sum(td.amount),
                	CASE @TypeOfCurrency
						WHEN 'Base' THEN ISNULL(SUM(ROUND(TD.amount,2)), 0.0)
						WHEN 'System' THEN ISNULL(SUM(ROUND(TD.system_amount,2)),0.0) 
					END,
                        NULL,
                        NULL,
                        NULL,
        --        lt.description,
        --        act.description
                        NULL,
                        NULL,
                        CASE @TypeOfCurrency
							WHEN 'Base' THEN cb.iso_code
							WHEN 'System' THEN @SystemCurrencyCode
						END,
						CASE @TypeOfCurrency
							WHEN 'Base' THEN cb.description
							WHEN 'System' THEN @SystemCurrencyDesc
						END, --added later for multicurrency
                        @iCompanyCode,@iCompanyDesc,
                        CASE  @Groupbycode
							WHEN 'Branch' THEN @iCompanyCode
							WHEN 'Branch and Currency' THEN @iCompanyCode  
						ELSE '' 
						END 'GroupByCode'
        FROM     Report_TreePathNames RTPN
                INNER JOIN  Account a                ON RTPN.account_id = a.account_id
                INNER JOIN  TransDetail td           ON a.account_id = td.account_id
                INNER JOIN  Document d               ON td.document_id = d.document_id
                INNER JOIN	Currency CB				 ON CB.Currency_id = @iBaseCurrency
        --        LEFT OUTER JOIN  Ledger l            ON a.ledger_id = l.ledger_id
        --        LEFT OUTER JOIN  LedgerType lt       ON l.ledgertype_id = lt.ledgertype_id
        --        LEFT OUTER JOIN  AccountType act     ON a.accounttype_id = act.accounttype_id
        WHERE D.company_id = @iCompanyid
            AND ( ( D.document_date <= @dtSelectedPeriodEnd  AND D.document_date > @dtSelectedPeriodStart )
                  AND ISNULL(TD.spare, '') NOT IN ('COMM ADJ', 'AGENT ADJ')
                  AND TD.Document_Sequence NOT IN
                    ( SELECT  Document_Sequence + 1
                      FROM     TransDetail
                      WHERE   document_id = D.document_id
                      AND spare = 'COMM ADJ' )
                )
            OR ( ( TD.ref_date <= @dtSelectedPeriodEnd AND  TD.ref_date > @dtSelectedPeriodStart)
                  AND ISNULL(TD.spare, '') IN ('COMM ADJ', 'AGENT ADJ')
                  OR TD.Document_Sequence IN
                    ( SELECT  Document_Sequence + 1
                      FROM     TransDetail
                      WHERE   document_id = D.document_id
                      AND spare = 'COMM ADJ' )
                )
        GROUP BY
                        RTPN.Report_Map_Id1,
                        RTPN.element_name1,
                        RTPN.Report_Map_Id2,
                        RTPN.element_name2,
                        RTPN.Report_Map_Id3,
                        RTPN.element_name3,
                        RTPN.Report_Map_Id4,
                        RTPN.element_name4,
                        RTPN.Report_Map_Id5,
                        RTPN.element_name5,
                        RTPN.Report_Map_Id6,
                        RTPN.element_name6,
                        RTPN.Report_Map_Id7,
                        RTPN.element_name7,
                        RTPN.Report_Map_Id8,
                        RTPN.element_name8,
                        RTPN.Report_Map_Id9,
                        RTPN.element_name9,
                        RTPN.Report_Map_Id10,
                        RTPN.element_name10,
                        a.account_id,
                        a.account_name,
                        a.short_code,
                        CB.iso_code,CB.Description --added later for multicurrency
                        --@CompanyCode,@CompanyDesc

        -- Date range 2
        IF @DEBUG = 1
            Print ' current period last year'

        INSERT INTO #tmpRSAIncomeStatement
                SELECT
                        RTPN.Report_Map_Id1,
                        RTPN.element_name1,
                        RTPN.Report_Map_Id2,
                        RTPN.element_name2,
                        RTPN.Report_Map_Id3,
                        RTPN.element_name3,
                        RTPN.Report_Map_Id4,
                        RTPN.element_name4,
                        RTPN.Report_Map_Id5,
                        RTPN.element_name5,
                        RTPN.Report_Map_Id6,
                        RTPN.element_name6,
                        RTPN.Report_Map_Id7,
                        RTPN.element_name7,
                        RTPN.Report_Map_Id8,
                        RTPN.element_name8,
                        RTPN.Report_Map_Id9,
                        RTPN.element_name9,
                        RTPN.Report_Map_Id10,
                        RTPN.element_name10,
                        a.account_id,
                        a.account_name,
                        a.short_code,
                        NULL,
        -- Current Period last year
                       -- sum(td.amount),
                        CASE @TypeOfCurrency
		       				WHEN 'Base' THEN ISNULL(SUM(ROUND(TD.amount,2)), 0.0)
							WHEN 'System' THEN ISNULL(SUM(ROUND(TD.system_amount,2)),0.0)
						END,
                        NULL,
                        NULL,
        --        lt.description,
        --        act.description
                        NULL,
                        NULL,
                        CASE @TypeOfCurrency
							WHEN 'Base' THEN cb.iso_code
							WHEN 'System' THEN @SystemCurrencyCode
						END,
						CASE @TypeOfCurrency
							WHEN 'Base' THEN cb.description
							WHEN 'System' THEN @SystemCurrencyDesc
						END, --added later for multicurrency
                        @iCompanyCode,@iCompanyDesc,
                        CASE  @Groupbycode 
							WHEN 'Branch' THEN @iCompanyCode
							WHEN 'Branch and Currency' THEN @iCompanyCode  
						ELSE '' 
						END 'GroupByCode'
        FROM     Report_TreePathNames RTPN
                INNER JOIN  Account a                ON RTPN.account_id = a.account_id
                INNER JOIN  TransDetail td           ON a.account_id = td.account_id
                INNER JOIN  Document d               ON td.document_id = d.document_id
                INNER JOIN	Currency CB				 ON CB.Currency_id=@iBaseCurrency
        --        LEFT OUTER JOIN  Ledger l            ON a.ledger_id = l.ledger_id
        --        LEFT OUTER JOIN  LedgerType lt       ON l.ledgertype_id = lt.ledgertype_id
        --        LEFT OUTER JOIN  AccountType act     ON a.accounttype_id = act.accounttype_id
        WHERE D.company_id = @iCompanyid
            AND ( (D.document_date <= @dt12MonthsAgoPeriodEnd AND D.Document_Date > @dt12MonthsAgoPeriodStart )
                  AND ISNULL(TD.spare, '') NOT IN ('COMM ADJ', 'AGENT ADJ')
                  AND TD.Document_Sequence NOT IN
                    ( SELECT  Document_Sequence + 1
                      FROM     TransDetail
                      WHERE   document_id = D.document_id
                      AND spare = 'COMM ADJ' )
                )
             OR ( (TD.ref_date <= @dt12MonthsAgoPeriodEnd AND TD.ref_date > @dt12MonthsAgoPeriodStart )
                  AND ISNULL(TD.spare, '') IN ('COMM ADJ', 'AGENT ADJ')
                  OR TD.Document_Sequence IN
                    ( SELECT  Document_Sequence + 1
                      FROM     TransDetail
                      WHERE   document_id = D.document_id
                      AND spare = 'COMM ADJ' )
                )
        GROUP BY
                        RTPN.Report_Map_Id1,
                        RTPN.element_name1,
                        RTPN.Report_Map_Id2,
                        RTPN.element_name2,
                        RTPN.Report_Map_Id3,
                        RTPN.element_name3,
                        RTPN.Report_Map_Id4,
                        RTPN.element_name4,
                        RTPN.Report_Map_Id5,
                        RTPN.element_name5,
                        RTPN.Report_Map_Id6,
                        RTPN.element_name6,
                        RTPN.Report_Map_Id7,
                        RTPN.element_name7,
                        RTPN.Report_Map_Id8,
                        RTPN.element_name8,
                        RTPN.Report_Map_Id9,
                        RTPN.element_name9,
                        RTPN.Report_Map_Id10,
                        RTPN.element_name10,
                        a.account_id,
                        a.account_name,
                        a.short_code,
                        CB.iso_code,CB.Description --added later for multicurrency
                        --@CompanyCode,@CompanyDesc


        -- Date Range 3
        IF @DEBUG = 1
            print 'This year, year to date'

        INSERT INTO #tmpRSAIncomeStatement
            SELECT
                    RTPN.Report_Map_Id1,
                    RTPN.element_name1,
                    RTPN.Report_Map_Id2,
                    RTPN.element_name2,
                    RTPN.Report_Map_Id3,
                    RTPN.element_name3,
                    RTPN.Report_Map_Id4,
                    RTPN.element_name4,
                    RTPN.Report_Map_Id5,
                    RTPN.element_name5,
                    RTPN.Report_Map_Id6,
                    RTPN.element_name6,
                    RTPN.Report_Map_Id7,
                    RTPN.element_name7,
                    RTPN.Report_Map_Id8,
                    RTPN.element_name8,
                    RTPN.Report_Map_Id9,
                    RTPN.element_name9,
                    RTPN.Report_Map_Id10,
                    RTPN.element_name10,
                    a.account_id,
                    a.account_name,
                    a.short_code,
                    NULL,
                    NULL,
--                    sum(td.amount),
		    		CASE @TypeOfCurrency
						WHEN 'Base' THEN ISNULL(SUM(ROUND(TD.amount,2)), 0.0)
						WHEN 'System' THEN ISNULL(SUM(ROUND(TD.system_amount,2)),0.0)
		    		END,	
                    NULL,
        --        lt.description,
        --        act.description
                    NULL,
                    NULL,
                    CASE @TypeOfCurrency
						WHEN 'Base' THEN cb.iso_code
						WHEN 'System' THEN @SystemCurrencyCode
					END,
					CASE @TypeOfCurrency
						WHEN 'Base' THEN cb.description
						WHEN 'System' THEN @SystemCurrencyDesc
					END, --added later for multicurrency
                    @iCompanyCode,@iCompanyDesc,
                    CASE  @Groupbycode 
						WHEN 'Branch' THEN @iCompanyCode
						WHEN 'Branch and Currency' THEN @iCompanyCode
						ELSE '' 
					END 'GroupByCode'
        FROM     Report_TreePathNames RTPN
            INNER JOIN  Account a                ON RTPN.account_id = a.account_id
            INNER JOIN  TransDetail td           ON a.account_id = td.account_id
            INNER JOIN  Document d               ON td.document_id = d.document_id
            INNER JOIN	Currency CB				 ON CB.Currency_id = @iBaseCurrency
            
        --        LEFT OUTER JOIN  Ledger l            ON a.ledger_id = l.ledger_id
        --        LEFT OUTER JOIN  LedgerType lt       ON l.ledgertype_id = lt.ledgertype_id
        --        LEFT OUTER JOIN  AccountType act     ON a.accounttype_id = act.accounttype_id
        WHERE D.company_id = @iCompanyid
        AND    ( (D.document_date <= @dtSelectedPeriodEnd AND D.Document_Date > @dtLastYearPeriodEnd )
                  AND ISNULL(TD.spare, '') NOT IN ('COMM ADJ', 'AGENT ADJ')
                  AND TD.Document_Sequence NOT IN
                    ( SELECT  Document_Sequence + 1
                      FROM     TransDetail
                      WHERE   document_id = D.document_id
                      AND spare = 'COMM ADJ' )
               )
        OR ( (TD.ref_date <= @dtSelectedPeriodEnd AND TD.ref_date > @dtLastYearPeriodEnd )
                  AND ISNULL(TD.spare, '') IN ('COMM ADJ', 'AGENT ADJ')
                  OR TD.Document_Sequence IN
                    ( SELECT  Document_Sequence + 1
                      FROM     TransDetail
                      WHERE   document_id = D.document_id
                      AND spare = 'COMM ADJ' )
               )
        GROUP BY
                    RTPN.Report_Map_Id1,
                    RTPN.element_name1,
                    RTPN.Report_Map_Id2,
                    RTPN.element_name2,
                    RTPN.Report_Map_Id3,
                    RTPN.element_name3,
                    RTPN.Report_Map_Id4,
                    RTPN.element_name4,
                    RTPN.Report_Map_Id5,
                    RTPN.element_name5,
                    RTPN.Report_Map_Id6,
                    RTPN.element_name6,
                    RTPN.Report_Map_Id7,
                    RTPN.element_name7,
                    RTPN.Report_Map_Id8,
                    RTPN.element_name8,
                    RTPN.Report_Map_Id9,
                    RTPN.element_name9,
                    RTPN.Report_Map_Id10,
                    RTPN.element_name10,
                    a.account_id,
                    a.account_name,
                    a.short_code,
                    CB.iso_code,CB.Description --added later for multicurrency
                        --@CompanyCode,@CompanyDesc


       -- last year Year To Date
       IF @DEBUG = 1
            PRINT 'Last Year, Year to Date'

        INSERT INTO #tmpRSAIncomeStatement
            SELECT
                    RTPN.Report_Map_Id1,
                    RTPN.element_name1,
                    RTPN.Report_Map_Id2,
                    RTPN.element_name2,
                    RTPN.Report_Map_Id3,
                    RTPN.element_name3,
                    RTPN.Report_Map_Id4,
                    RTPN.element_name4,
                    RTPN.Report_Map_Id5,
                    RTPN.element_name5,
                    RTPN.Report_Map_Id6,
                    RTPN.element_name6,
                    RTPN.Report_Map_Id7,
                    RTPN.element_name7,
                    RTPN.Report_Map_Id8,
                    RTPN.element_name8,
                    RTPN.Report_Map_Id9,
                    RTPN.element_name9,
                    RTPN.Report_Map_Id10,
                    RTPN.element_name10,
                    a.account_id,
                    a.account_name,
                    a.short_code,
                    NULL,
                    NULL,
                    NULL,
                --   sum(td.amount),
                    CASE @TypeOfCurrency
						WHEN 'Base' THEN ISNULL(SUM(ROUND(TD.amount,2)), 0.0)
						WHEN 'System' THEN ISNULL(SUM(ROUND(TD.system_amount,2)),0.0)
		    		END,
        --        lt.description,
        --        act.description
                    NULL,
                    NULL,
                    CASE @TypeOfCurrency
						WHEN 'Base' THEN cb.iso_code
						WHEN 'System' THEN @SystemCurrencyCode
					END,
					CASE @TypeOfCurrency
						WHEN 'Base' THEN cb.description
						WHEN 'System' THEN @SystemCurrencyDesc
					END, --added later for multicurrency
                    @iCompanyCode,@iCompanyDesc,
                    CASE  @Groupbycode 
						WHEN 'Branch' THEN @iCompanyCode
						WHEN 'Branch and Currency' THEN @iCompanyCode  
						ELSE '' 
					END 'GroupByCode'
        FROM     Report_TreePathNames RTPN
            INNER JOIN  Account a                ON RTPN.account_id = a.account_id
            INNER JOIN  TransDetail td           ON a.account_id = td.account_id
            INNER JOIN  Document d               ON td.document_id = d.document_id
            INNER JOIN  Currency CB              ON CB.Currency_id = @iBaseCurrency
        --        LEFT OUTER JOIN  Ledger l            ON a.ledger_id = l.ledger_id
        --        LEFT OUTER JOIN  LedgerType lt       ON l.ledgertype_id = lt.ledgertype_id
        --        LEFT OUTER JOIN  AccountType act     ON a.accounttype_id = act.accounttype_id
        WHERE D.company_id = @iCompanyid
        AND	D.document_date > @dt2YearsAgoPeriodEnd AND D.document_date <= @dtLastYearPeriodEnd
        AND ISNULL(TD.spare, '') NOT IN ('COMM ADJ', 'AGENT ADJ')
        AND  TD.Document_Sequence NOT IN
                ( SELECT  Document_Sequence + 1
                  FROM     TransDetail
                  WHERE   document_id = D.document_id
                 AND spare = 'COMM ADJ' )
        AND ( td.document_sequence = 2  )
        GROUP BY
                    RTPN.Report_Map_Id1,
                    RTPN.element_name1,
                    RTPN.Report_Map_Id2,
                    RTPN.element_name2,
                    RTPN.Report_Map_Id3,
                    RTPN.element_name3,
                    RTPN.Report_Map_Id4,
                    RTPN.element_name4,
                    RTPN.Report_Map_Id5,
                    RTPN.element_name5,
                    RTPN.Report_Map_Id6,
                    RTPN.element_name6,
                    RTPN.Report_Map_Id7,
                    RTPN.element_name7,
                    RTPN.Report_Map_Id8,
                    RTPN.element_name8,
                    RTPN.Report_Map_Id9,
                    RTPN.element_name9,
                    RTPN.Report_Map_Id10,
                    RTPN.element_name10,
                    a.account_id,
                    a.account_name,
                    a.short_code,
                    CB.iso_code,CB.Description --added later for multicurrency
					--@CompanyCode,@CompanyDesc

                    

    END             --- @iBasis <> 0, Transaction Date

    FETCH NEXT FROM company_cursor INTO @iCompanyid,@iCompanyCode,@iCompanyDesc,@iBaseCurrency

END

CLOSE company_cursor
DEALLOCATE company_cursor


-- Now reselect these into another table, combining all RTPN.element con

CREATE TABLE #RSAIncomeStatementOutput
(
        dtCurrentPeriodEnd datetime NULL,
        ReportMapID1 int NULL,
        ElementName1 varchar (30) NULL,
        ReportMapID2 int NULL,
        ElementName2 varchar (30) NULL,
        ReportMapID3 int NULL,
        ElementName3 varchar (30) NULL,
        ReportMapID4 int NULL,
        ElementName4 varchar (30) NULL,
        ReportMapID5 int NULL,
        ElementName5 varchar (30) NULL,
        ReportMapID6 int NULL,
        ElementName6 varchar (30) NULL,
        ReportMapID7 int NULL,
        ElementName7 varchar (30) NULL,
        ReportMapID8 int NULL,
        ElementName8 varchar (30) NULL,
        ReportMapID9 int NULL,
        ElementName9 varchar (30) NULL,
        ReportMapID10 int NULL,
        ElementName10 varchar (30) NULL,
        AccountID int,
        Account varchar (60) NULL,
        AccountCode varchar (30) NULL,
        amountTMTY money NULL,             /* Current Period */
        amountTMLY money NULL,             /* Equivalent Period Last Year */
        amountYTDTY money NULL,            /* Current Year To Date */
        amountYTDLY money NULL,            /* Same Period Last Year */
        LedgerType varchar (255) NULL,
        AccountType varchar (255) NULL,
        CompanyCode	Varchar(20)	Null,
        CompanyDesc	Varchar(255)	Null,
        CurrencyCode	Varchar(20)	Null,
        CurrencyDesc	Varchar(255)	Null,
        GroupyCode		Varchar(50)
        
)


INSERT INTO #RSAIncomeStatementOutput
SELECT  @dtSelectedPeriodEnd,
        ReportMapID1,
        ElementName1,
        ReportMapID2,
        ElementName2,
        ReportMapID3,
        ElementName3,
        ReportMapID4,
        ElementName4,
        ReportMapID5,
        ElementName5,
        ReportMapID6,
        ElementName6,
        ReportMapID7,
        ElementName7,
        ReportMapID8,
        ElementName8,
        ReportMapID9,
        ElementName9,
        ReportMapID10,
        ElementName10,
        AccountID,
        Account,
        AccountCode,
        SUM(ISNULL(amountTMTY,0)),
        SUM(ISNULL(amountTMLY,0)),
        SUM(ISNULL(amountYTDTY,0)),
        SUM(ISNULL(amountYTDLY,0)),
        NULL,      -- LedgerType
        NULL,       -- AccountType
        CompanyCode,
        CompanyDesc,
        CurrencyCode,
        CurrencyDesc,
        GroupByCode
FROM #TmpRSAIncomeStatement
GROUP BY
        ReportMapID1,
        ElementName1,
        ReportMapID2,
        ElementName2,
        ReportMapID3,
        ElementName3,
        ReportMapID4,
        ElementName4,
        ReportMapID5,
        ElementName5,
        ReportMapID6,
        ElementName6,
        ReportMapID7,
        ElementName7,
        ReportMapID8,
        ElementName8,
        ReportMapID9,
        ElementName9,
        ReportMapID10,
        ElementName10,
        AccountID,
        Account,
        AccountCode,
        CompanyCode,
		CompanyDesc,
		CurrencyCode,
        CurrencyDesc,GroupByCode


-- Next look up the accouttype and ledger types
UPDATE #RSAIncomeStatementOutput
  SET AccountType = act.Description,
      LedgerType = lt.Description
FROM #TmpRSAIncomeStatement TRIS
     INNER JOIN  Account A                ON TRIS.AccountID = A.Account_ID
     LEFT OUTER JOIN  Ledger l            ON a.ledger_id = l.ledger_id
     LEFT OUTER JOIN  LedgerType lt       ON l.ledgertype_id = lt.ledgertype_id
     LEFT OUTER JOIN  AccountType act     ON a.accounttype_id = act.accounttype_id


SET NOCOUNT OFF

--RC-- RENAME 'RI Treaty Commission' AND 'RI Other Commission' FOLDERS
update #RSAIncomeStatementOutput
set ElementName4 = 'Facility / Binder Income'
where ElementName4 = 'RI Treaty Commission'

update #RSAIncomeStatementOutput
set ElementName4 = 'FAC Income'
where ElementName4 = 'RI Other Commission'


--RC-- FILTER CLAIMS FOLDERS AND ACCOUNTS IF CHOSEN
--RC-- REMOVE Gross Written Premium', 'RI Treaty Premium', 'RI Other Premium' FOLDERS
--RC-- get data back from temporary table
IF @IncludeClaims = 'Yes'
	BEGIN
		SELECT * FROM #RSAIncomeStatementOutput
		where
		ElementName4 Not IN ('Gross Written Premium', 'RI Treaty Premium', 'RI Other Premium')
	END
ELSE
	BEGIN
		SELECT * FROM #RSAIncomeStatementOutput
		where
		ElementName4 Not IN ('Gross Written Premium', 'RI Treaty Premium', 'RI Other Premium')
		and
		ElementName5 NOT Like 'CLM%'
	END



-- delete temporary table
DROP TABLE #tmpRSAIncomeStatement
DROP TABLE #RSAIncomeStatementOutput

-- clear down Orion Table
DELETE FROM  Report_TreePathNames
GO
