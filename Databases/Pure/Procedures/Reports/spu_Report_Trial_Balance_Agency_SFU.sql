SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Trial_Balance_Agency_SFU'
GO

/**********************************************************************************************************************************
** Created by Rajesh Choudhary
** 17 Feb 2006
** S4I Reports - Trial_Balance_Agency_U.rpt
**
***********************************************************************************************************************************/

CREATE  PROCEDURE spu_Report_Trial_Balance_Agency_SFU
	@branch_id      int,
	@PeriodDate varchar(20),
	@sType varchar(30),
	@Basis varchar(50),
	@TypeOfCurrency VARCHAR(15),
	@GroupBy VARCHAR(20),
	@IncludeClaims VARCHAR(3)
AS

/****** Object:  Stored Procedure dbo.sp_Report_Trial_Balance    Script Date: 16/10/00 12:18:33 ******/
/*** eck 110902 Round the amounts to 2 decimal places before adding them together to remove penny discrepancies ***/
/**********************************************************************************************************************/
-- Jude Killip	23 Jul 2003
-- Optimisation rework - use temp tables instead of work tables.
-- Major time improvement on large databases
/**********************************************************************************************************************/
DECLARE @period_end_date    datetime
SELECT @PeriodDate = @PeriodDate + ' 23:59:59.000'
SELECT @period_end_date = CONVERT (Datetime, @PeriodDate)

-- JMK 23/Jul/2003 - this section more or less unchanged
-- TB 11/11/02:  report basis. Select by document date or by transaction period
DECLARE @iBasis INT
DECLARE @iPeriod INT
DECLARE @iBranchPeriod INT

DECLARE
    @sBalBF         	char(25),
    @sBalCF         	char(25),
    @iBranchID      	int,
    @dPeriodEndDateIn   datetime,
    @dNextMonth     	datetime,
    @dLastDay       	datetime,
    @d1stPeriodEndDate  datetime,
    @dBFEndDate     	datetime,            -- period BeFore current
    @dPeriodEndDate     datetime,
    @dNextPeriodStartDate   datetime,
    @sCurrentYearName   varchar(20),
    @iCompanyid     	int,
    @dtLastYearEnd 		datetime

-- TB 11/11/02
DECLARE  @iPeriodBFEnd int,
         @iPeriodLastYearEnd int,
         @iPeriodEnd int

DECLARE @CompanyCode VARCHAR(10)
DECLARE @CompanyDesc VARCHAR(255)
DECLARE @SystemCurrencyCode VARCHAR(10)
DECLARE @SystemCurrencyDesc VARCHAR(255)
DECLARE @BaseCurrencyCode VARCHAR(10)
DECLARE @BaseCurrencyDesc VARCHAR(255)

    SET NOCOUNT ON

	/*Get System Currency Details*/
	SELECT
		@SystemCurrencyCode = c.iso_code,
		@SystemCurrencyDesc = c.description
	FROM PMSystem pms
	JOIN currency c
		ON c.currency_id = pms.currency_id
	WHERE pms.system_id = 1

    SELECT @iBranchID = ISNULL(@branch_id, 0)

    IF @iBranchID = 0
    BEGIN
        DECLARE company_cursor CURSOR FAST_FORWARD FOR
			SELECT
				co.company_id,
				co.code,
				co.description,
				cu.iso_code,
				cu.description
        	FROM company co
			JOIN currency cu
				ON cu.currency_id = co.base_currency
    END
    ELSE
    BEGIN
        DECLARE company_cursor CURSOR FAST_FORWARD FOR
			SELECT
				co.company_id,
				co.code,
				co.description,
				cu.iso_code,
				cu.description
        	FROM company co
			JOIN currency cu
				ON cu.currency_id = co.base_currency
            where company_id = @iBranchID
    END

    IF @Basis = 'Transaction Date'
        SELECT @iBasis = 1    -- Transaction Date
    ELSE
    BEGIN
        SELECT @iBasis = 0    -- Transaction Period
    END

    SELECT @dPeriodEndDateIn = ISNULL(@period_end_date, GETDATE())

    SELECT @sBalBF = 'Balance Brought Forward'
    SELECT @sBalCF = 'Balance Carried Forward'

-- Empty the temporary table
    --DELETE FROM Report_Transaction

    CREATE TABLE #Report_Transaction
    	(TransdetailID int,
    	AmountBF money,
    	Amount money,
    	AmountCF money,
    	RecordDescription varchar(150),
    	AccountId int,
    	AccountCode varchar(150),
		AccountName varchar(150),
    	EndDate datetime,
    	LedgerType varchar(150),
    	AccountType varchar(150),
    	RecordType int,
		CompanyCode VARCHAR(10),
		CompanyDesc VARCHAR(255),
		CurrencyCode VARCHAR(10),
		CurrencyDesc VARCHAR(255)
    	)

    OPEN company_cursor

    FETCH NEXT FROM company_cursor INTO @iCompanyid, @CompanyCode, @CompanyDesc, @BaseCurrencyCode, @BaseCurrencyDesc
    WHILE @@FETCH_STATUS = 0
    BEGIN

		/*Use correct branch to work out period*/
		IF EXISTS(SELECT NULL FROM period WHERE company_id > 1)
		BEGIN
			SELECT @iBranchPeriod = @iCompanyid
		END
		ELSE
		BEGIN
			SELECT @iBranchPeriod = 1
		END

        SELECT @iPeriod =
			(
				SELECT TOP 1 period_id
				FROM period
				WHERE period_end_Date = @period_end_date
				AND company_id = @iBranchPeriod
				ORDER BY period_id DESC
			)

        SELECT @dNextMonth = DATEADD(month, 1, @dPeriodEndDateIn)
        SELECT @dLastDay = DATEADD(day, DATEPART(dd, @dNextMonth) * -1, @dNextMonth )

        SELECT @dPeriodEndDate = (
        		SELECT 	ISNULL(MIN(P.period_end_date), @dLastDay)
				FROM   	Period    P
				WHERE   P.period_end_date >= @dPeriodEndDateIn
				AND     p.company_id = @iBranchPeriod
				)

        SELECT @dLastDay = DATEADD(day, DATEPART(dd, @dPeriodEndDate) * -1, @dPeriodEndDate)

        SELECT @dBFEndDate = (
        		SELECT 	ISNULL(MAX(P.period_end_date), @dLastDay)
				FROM  	Period P
				WHERE 	P.period_end_date < @dPeriodEndDate
				AND   	P.company_id = @iBranchPeriod
				)

        SELECT @sCurrentYearName = (
        		SELECT	TOP 1 ISNULL(P.year_name, '')
				FROM    Period P
				WHERE   P.period_end_date = @dPeriodEndDate
				AND     P.company_id = @iBranchPeriod
				ORDER BY period_id DESC
				)

-- If 1st period end date not found, set it to the current period
        SELECT @d1stPeriodEndDate = (
        		SELECT 	ISNULL(MIN(P.period_end_date), @dPeriodEndDate)
				FROM    Period P
				WHERE   p.company_id = @iBranchPeriod
				AND     P.year_name = @sCurrentYearName
				)

-- TB 10/10/02 : Get current year start date (i.e. last year end date)
-- This will be the end date of the period prior to @d1stPeriodEndDate
        SELECT @dtLastYearEnd = (
        		SELECT 	ISNULL(MAX(P.Period_end_date), @d1stPeriodEndDate)
				FROM 	Period P
				WHERE 	P.company_id = @iBranchPeriod
				AND 	P.period_End_date < @d1stPeriodEndDate)

		-- TB 16/10/02: Restore to original, set last year end to 1/1/1900, to select ALL data
        IF @sType = 'All Dates'
        BEGIN
		-- This effectivly restores the report to the Broking Version.
            SELECT @dtLastYearEnd = '1900-01-01'
        END

		-- TB 11/11/02: Report Basis, need the period ID's of those end dates
        IF @iBasis = 0
        BEGIN
            SELECT @iPeriodBFEnd = (
            	SELECT TOP 1 period_id
				FROM 	Period P
				WHERE 	P.company_id = @iBranchPeriod
				AND 	P.period_end_date = @dBFEndDate
				ORDER BY period_id DESC
				)

            IF @sType = 'All Dates'
            BEGIN
                SELECT @iPeriodLastYearEnd = (
                		SELECT 	period_id
						FROM 	Period P
						WHERE 	P.company_id = @iBranchPeriod
						AND 	P.period_end_Date = (
									SELECT min(period_end_Date)
									FROM Period
									WHERE company_id = @iBranchPeriod
									)
						)
            END
            ELSE
            BEGIN
                SELECT @iPeriodLastYearEnd = 
						(
                		SELECT TOP 1 period_id
						FROM 	Period P
						WHERE 	P.company_id = @iBranchPeriod
						AND 	P.period_end_Date = @dtLastYearEnd
						ORDER BY period_id DESC
						)

                IF @iPeriodLastYearEnd IS NULL SELECT @iPeriodLastYearEnd = 0
            END

            SELECT @iPeriodEnd = 
					(
            		SELECT 	TOP 1 period_id
					FROM 	Period P
					WHERE 	P.company_id = @iBranchPeriod
					AND 	P.period_end_date = @dPeriodEndDate 
					ORDER BY period_id DESC
					)
        END

--JMK 23/J8ul/2003 - Major changes from this point onwards

		INSERT INTO #Report_Transaction
		(
			TransdetailID,
			AmountBF,
			Amount,
			AmountCF,
			RecordDescription,
			AccountId,
			AccountCode,
			AccountName,
			EndDate,
			LedgerType,
			AccountType,
			RecordType,
			CompanyCode,
			CompanyDesc,
			CurrencyCode,
			CurrencyDesc
		)
		SELECT
			0,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN ISNULL(SUM(ROUND(TD.amount,2)), 0.0)
				WHEN 'System' THEN ISNULL(SUM(ROUND(TD.system_amount,2)), 0.0)
				WHEN 'Transaction' THEN ISNULL(SUM(ROUND(TD.currency_amount,2)), 0.0)
			END,
			0.0,
			0.0,
			@sBalBF,
			A.Account_ID,
			A.short_code,
			MAX(A.account_name),
			@dBFEndDate,
			MAX(ISNULL(LT.description, '')),
			MAX(ISNULL(ATp.description, '')),
			0,
			@CompanyCode,
			@CompanyDesc,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN @BaseCurrencyCode
				WHEN 'System' THEN @SystemCurrencyCode
				WHEN 'Transaction' THEN C.iso_code
			END,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN @BaseCurrencyDesc
				WHEN 'System' THEN @SystemCurrencyDesc
				WHEN 'Transaction' THEN C.description
			END
		FROM Account A
		LEFT OUTER JOIN Ledger L 		ON A.ledger_id = L.ledger_id
		LEFT OUTER JOIN LedgerType LT 	ON L.ledgertype_id = LT.ledgertype_id
		LEFT OUTER JOIN AccountType ATp ON A.accounttype_id = ATp.accounttype_id
		JOIN TransDetail TD             ON A.account_id = TD.account_id
		JOIN Document D                	ON TD.document_id = D.document_id
		JOIN Currency C					ON C.currency_id = TD.currency_id
		WHERE D.company_id = @iCompanyid
		AND (
			(@iBasis = 0 AND ( TD.period_id <= @iPeriodBFEnd AND TD.period_id >= @iPeriodLastYearEnd ))
			OR
			(@iBasis = 1 AND ( D.document_date <= @dBFEndDate AND D.document_Date >= @dtLastYearEnd ))
			)

		GROUP BY
			A.account_id,
			A.short_code,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN @BaseCurrencyCode
				WHEN 'System' THEN @SystemCurrencyCode
				WHEN 'Transaction' THEN C.iso_code
			END,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN @BaseCurrencyDesc
				WHEN 'System' THEN @SystemCurrencyDesc
				WHEN 'Transaction' THEN C.description
			END

    -- Get Any details in the current period
		INSERT INTO #Report_Transaction
		(
			TransdetailID,
			AmountBF,
			Amount,
			AmountCF,
			RecordDescription,
			AccountId,
			AccountCode,
			AccountName,
			EndDate,
			LedgerType,
			AccountType,
			RecordType,
			CompanyCode,
			CompanyDesc,
			CurrencyCode,
			CurrencyDesc
		)
		SELECT  0,
			0.0,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN ISNULL(SUM(ROUND(TD.amount,2)), 0.0)
				WHEN 'System' THEN ISNULL(SUM(ROUND(TD.system_amount,2)), 0.0)
				WHEN 'Transaction' THEN ISNULL(SUM(ROUND(TD.currency_amount,2)), 0.0)
			END,
			0.0,
			'This Period',
			a.account_id,
			A.short_code,
			MAX(A.account_name),
			@dPeriodEndDate,
			MAX(ISNULL(LT.description, '')),
			MAX(ISNULL(ATp.description, '')),
			1,
			@CompanyCode,
			@CompanyDesc,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN @BaseCurrencyCode
				WHEN 'System' THEN @SystemCurrencyCode
				WHEN 'Transaction' THEN C.iso_code
			END,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN @BaseCurrencyDesc
				WHEN 'System' THEN @SystemCurrencyDesc
				WHEN 'Transaction' THEN C.description
			END
		FROM Account A
		LEFT OUTER JOIN Ledger L 		ON A.ledger_id = L.ledger_id
		LEFT OUTER JOIN LedgerType LT 	ON L.ledgertype_id = LT.ledgertype_id
		LEFT OUTER JOIN AccountType ATp	ON A.accounttype_id = ATp.accounttype_id
		JOIN TransDetail TD				ON A.account_id = TD.account_id
		JOIN Document D					ON TD.document_id = D.document_id
		JOIN Currency C					ON C.currency_id = TD.currency_id
		WHERE D.company_id = @iCompanyid
		AND (
			(@iBasis = 0 AND (TD.period_id > @iPeriodBFEnd AND TD.period_id <= @iPeriodEnd))
			OR
			(@iBasis = 1 AND (D.document_date > @dBFEndDate AND D.document_date <= @dPeriodEndDate))
			)

		GROUP BY
			A.account_id,
			A.short_code,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN @BaseCurrencyCode
				WHEN 'System' THEN @SystemCurrencyCode
				WHEN 'Transaction' THEN C.iso_code
			END,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN @BaseCurrencyDesc
				WHEN 'System' THEN @SystemCurrencyDesc
				WHEN 'Transaction' THEN C.description
			END

-- Get the year to date balance
		INSERT INTO #Report_Transaction
		(
			TransdetailID,
			AmountBF,
			Amount,
			AmountCF,
			RecordDescription,
			AccountId,
			AccountCode,
			AccountName,
			EndDate,
			LedgerType,
			AccountType,
			RecordType,
			CompanyCode,
			CompanyDesc,
			CurrencyCode,
			CurrencyDesc
		)
		SELECT  0,
			0.0,
			0.0,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN ISNULL(SUM(ROUND(TD.amount,2)), 0.0)
				WHEN 'System' THEN ISNULL(SUM(ROUND(TD.system_amount,2)), 0.0)
				WHEN 'Transaction' THEN ISNULL(SUM(ROUND(TD.currency_amount,2)), 0.0)
			END,
			@sBalCF,
			a.account_id,
			A.short_code,
			MAX(A.account_name),
			@dPeriodEndDate,
			MAX(ISNULL(LT.description, '')),
			MAX(ISNULL(ATp.description, '')),
			9,
			@CompanyCode,
			@CompanyDesc,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN @BaseCurrencyCode
				WHEN 'System' THEN @SystemCurrencyCode
				WHEN 'Transaction' THEN C.iso_code
			END,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN @BaseCurrencyDesc
				WHEN 'System' THEN @SystemCurrencyDesc
				WHEN 'Transaction' THEN C.description
			END
		FROM Account A
		LEFT OUTER JOIN Ledger L			ON A.ledger_id = L.ledger_id
		LEFT OUTER JOIN LedgerType LT		ON L.ledgertype_id = LT.ledgertype_id
		LEFT OUTER JOIN AccountType ATp		ON A.accounttype_id = ATp.accounttype_id
		JOIN TransDetail TD					ON A.account_id = TD.account_id
		JOIN Document D						ON TD.document_id = D.document_id
		JOIN Currency C						ON C.currency_id = TD.currency_id
		WHERE D.company_id = @iCompanyid
		AND (
			(@iBasis = 0 AND (TD.period_id <= @iPeriodEnd AND TD.period_id >= @iPeriodLastYearEnd))
			OR
			(@iBasis = 1 AND (D.document_date <= @dPeriodEndDate AND D.document_date >= @dtLastYearEnd))
			)

		GROUP BY
			A.account_id,
			A.short_code,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN @BaseCurrencyCode
				WHEN 'System' THEN @SystemCurrencyCode
				WHEN 'Transaction' THEN C.iso_code
			END,
			CASE @TypeOfCurrency
				WHEN 'Base' THEN @BaseCurrencyDesc
				WHEN 'System' THEN @SystemCurrencyDesc
				WHEN 'Transaction' THEN C.description
			END

		FETCH NEXT FROM company_cursor INTO @iCompanyid, @CompanyCode, @CompanyDesc, @BaseCurrencyCode, @BaseCurrencyDesc

    END

    CLOSE company_cursor
    DEALLOCATE company_cursor





--**************************************************************************************
-- this section sorts out the Account positions in the Account Structure
-- replaces use of work tables (e.g. Report_Structure_Tree)
-- and related Stored Procs (e.g. sp_report_get_full_treepath)
--**************************************************************************************
-- Get all the Account nodes

CREATE TABLE #AccountNodes
(account_id int, id1 int, id2 int, id3 int, id4 int, id5 int, id6 int, id7 int, id8 int, id9 int, id10 int, id11 int)

DECLARE @account_id int, @id1 int, @id2 int, @id3 int, @id4 int, @id5 int, @id6 int, @id7 int, @id8 int, @id9 int, @id10 int, @id11 int
DECLARE @new_id1 int, @new_id2 int, @new_id3 int, @new_id4 int, @new_id5 int, @new_id6 int, @new_id7 int, @new_id8 int, @new_id9 int, @new_id10 int, @new_id11 int
DECLARE @iStartNode int

DECLARE c_nodes CURSOR FAST_FORWARD
FOR
	SELECT distinct ST.account_id,
		st10.element_id,
		st9.element_id,
		st8.element_id,
		st7.element_id,
		st6.element_id,
		st5.element_id,
		st4.element_id,
		st3.element_id,
		st2.element_id,
		st1.element_id,
		st.element_id
    FROM StructureTree ST
	JOIN #Report_Transaction RT ON RT.AccountID = ST.account_id
	LEFT OUTER JOIN StructureTree ST1 	ON st1.node_id = st.parent_node_id
	LEFT OUTER JOIN StructureTree ST2 	ON st2.node_id = st1.parent_node_id
	LEFT OUTER JOIN StructureTree ST3 	ON st3.node_id = st2.parent_node_id
	LEFT OUTER JOIN StructureTree ST4 	ON st4.node_id = st3.parent_node_id
	LEFT OUTER JOIN StructureTree ST5 	ON st5.node_id = st4.parent_node_id
	LEFT OUTER JOIN StructureTree ST6 	ON st6.node_id = st5.parent_node_id
	LEFT OUTER JOIN StructureTree ST7 	ON st7.node_id = st6.parent_node_id
	LEFT OUTER JOIN StructureTree ST8 	ON st8.node_id = st7.parent_node_id
	LEFT OUTER JOIN StructureTree ST9 	ON st9.node_id = st8.parent_node_id
	LEFT OUTER JOIN StructureTree ST10 	ON st10.node_id = st9.parent_node_id

OPEN c_nodes

FETCH NEXT FROM c_nodes
INTO @account_id, @id1, @id2, @id3, @id4, @id5, @id6, @id7, @id8, @id9, @id10, @id11

-- then shuffle the Account nodes into line
WHILE @@fetch_status = 0
BEGIN
	IF isnull(@id10,0) = 0
	SELECT @new_id1 = @id11, @new_id2 = 0, @new_id3 = 0, @new_id4 = 0, @new_id5 = 0, @new_id6 = 0, @new_id7 = 0, @new_id8 = 0, @new_id9 = 0, @new_id10 = 0, @new_id11 = 0
	ELSE

	IF isnull(@id9,0) = 0
	SELECT @new_id1 = @id10, @new_id2 = @id11, @new_id3 = 0, @new_id4 = 0, @new_id5 = 0, @new_id6 = 0, @new_id7 = 0, @new_id8 = 0, @new_id9 = 0, @new_id10 = 0, @new_id11 = 0
	ELSE

	IF isnull(@id8,0) = 0
	SELECT @new_id1 = @id9, @new_id2 = @id10, @new_id3 = @id11, @new_id4 = 0, @new_id5 = 0, @new_id6 = 0, @new_id7 = 0, @new_id8 = 0, @new_id9 = 0, @new_id10 = 0, @new_id11 = 0
	ELSE

	IF isnull(@id7,0) = 0
	SELECT @new_id1 = @id8, @new_id2 = @id9, @new_id3 = @id10, @new_id4 = @id11, @new_id5 = 0, @new_id6 = 0, @new_id7 = 0, @new_id8 = 0, @new_id9 = 0, @new_id10 = 0, @new_id11 = 0
	ELSE

	IF isnull(@id6,0) = 0
	SELECT @new_id1 = @id7, @new_id2 = @id8, @new_id3 = @id9, @new_id4 = @id10, @new_id5 = @id11, @new_id6 = 0, @new_id7 = 0, @new_id8 = 0, @new_id9 = 0, @new_id10 = 0, @new_id11 = 0
	ELSE

	IF isnull(@id5,0) = 0
	SELECT @new_id1 = @id6, @new_id2 = @id7, @new_id3 = @id8, @new_id4 = @id9, @new_id5 = @id10, @new_id6 = @id11, @new_id7 = 0, @new_id8 = 0, @new_id9 = 0, @new_id10 = 0, @new_id11 = 0
	ELSE

	IF isnull(@id4,0) = 0
	SELECT @new_id1 = @id5, @new_id2 = @id6, @new_id3 = @id7, @new_id4 = @id8, @new_id5 = @id9, @new_id6 = @id10, @new_id7 = @id11, @new_id8 = 0, @new_id9 = 0, @new_id10 = 0, @new_id11 = 0
	ELSE

	IF isnull(@id3,0) = 0
	SELECT @new_id1 = @id4, @new_id2 = @id5, @new_id3 = @id6, @new_id4 = @id7, @new_id5 = @id8, @new_id6 = @id9, @new_id7 = @id10, @new_id8 = @id11, @new_id9 = 0, @new_id10 = 0, @new_id11 = 0
	ELSE

	IF isnull(@id2,0) = 0
	SELECT @new_id1 = @id3, @new_id2 = @id4, @new_id3 = @id5, @new_id4 = @id6, @new_id5 = @id7, @new_id6 = @id8, @new_id7 = @id9, @new_id8 = @id10, @new_id9 = @id11, @new_id10 = 0, @new_id11 = 0
	ELSE

	IF isnull(@id1,0) = 0
	SELECT @new_id1 = @id2, @new_id2 = @id3, @new_id3 = @id4, @new_id4 = @id5, @new_id5 = @id6, @new_id6 = @id7, @new_id7 = @id8, @new_id8 = @id9, @new_id9 = @id10, @new_id10 = @id11, @new_id11 = 0
	ELSE

	SELECT @new_id1 = @id1, @new_id2 = @id2, @new_id3 = @id3, @new_id4 = @id4, @new_id5 = @id5, @new_id6 = @id6, @new_id7 = @id7, @new_id8 = @id8, @new_id9 = @id9, @new_id10 = @id10, @new_id11 = @id11

	-- and store them line by line
	INSERT INTO #AccountNodes
	SELECT @account_id, @new_id1, @new_id2, @new_id3, @new_id4, @new_id5, @new_id6, @new_id7, @new_id8, @new_id9, @new_id10, @new_id11

	FETCH NEXT FROM c_nodes
	INTO @account_id, @id1, @id2, @id3, @id4, @id5, @id6, @id7, @id8, @id9, @id10, @id11

END
CLOSE c_nodes
DEALLOCATE c_nodes

--**************************************************************************************
-- end of Account structure section
--**************************************************************************************

-- combine Transactions and Account Structure
SET NOCOUNT OFF


--******************************************BEGIN RC CHNAGES********************************************
--RC	Store report results in temp table 
SELECT * INTO #Report_Transaction_Temp FROM
(SELECT
	e1.element_name Node1,	ee1.report_map_id Map1,
	e2.element_name Node2,		ee2.report_map_id Map2,
	e3.element_name Node3,		ee3.report_map_id Map3,
	e4.element_name Node4,		ee4.report_map_id Map4,
	e5.element_name Node5,		ee5.report_map_id Map5,
	e6.element_name Node6,		ee6.report_map_id Map6,
	e7.element_name Node7,		ee7.report_map_id Map7,
	e8.element_name Node8,		ee8.report_map_id Map8,
	e9.element_name Node9,		ee9.report_map_id Map9,
	e10.element_name Node10,	ee10.report_map_id Map10,
	e11.element_name Node11,	ee11.report_map_id Map11,
	RT.AmountBF,
	RT.Amount,
	RT.AmountCF,
	RT.AccountID,
	RT.AccountName,
	RT.AccountCode,
	RT.RecordType,
	RT.EndDate,
	RT.RecordDescription,
	RT.TransdetailID,
	RT.LedgerType,
	RT.AccountType,
	RT.CompanyCode,
	RT.CompanyDesc,
	RT.CurrencyCode,
	RT.CurrencyDesc,
	CASE @GroupBy
		WHEN 'Branch' THEN CompanyCode
		WHEN 'Branch and Currency' THEN CompanyCode
		WHEN 'Currency' THEN CurrencyCode
		ELSE ''
	END 'GroupByCode1',
	CASE @GroupBy
		WHEN 'Branch' THEN CompanyDesc
		WHEN 'Branch and Currency' THEN CompanyDesc
		WHEN 'Currency' THEN CurrencyDesc
		ELSE ''
	END 'GroupByDesc1'
FROM #Report_Transaction RT
JOIN #AccountNodes an 				ON RT.AccountID = an.account_id
LEFT OUTER JOIN Element e1 			ON e1.element_id = an.id1
LEFT OUTER JOIN ElementExtras ee1 	ON ee1.element_id = e1.element_id
LEFT OUTER JOIN Element e2 			ON e2.element_id = an.id2
LEFT OUTER JOIN ElementExtras ee2 	ON ee2.element_id = e2.element_id
LEFT OUTER JOIN Element e3 			ON e3.element_id = an.id3
LEFT OUTER JOIN ElementExtras ee3 	ON ee3.element_id = e3.element_id
LEFT OUTER JOIN Element e4 			ON e4.element_id = an.id4
LEFT OUTER JOIN ElementExtras ee4 	ON ee4.element_id = e4.element_id
LEFT OUTER JOIN Element e5 			ON e5.element_id = an.id5
LEFT OUTER JOIN ElementExtras ee5 	ON ee5.element_id = e5.element_id
LEFT OUTER JOIN Element e6 			ON e6.element_id = an.id6
LEFT OUTER JOIN ElementExtras ee6 	ON ee6.element_id = e6.element_id
LEFT OUTER JOIN Element e7 			ON e7.element_id = an.id7
LEFT OUTER JOIN ElementExtras ee7 	ON ee7.element_id = e7.element_id
LEFT OUTER JOIN Element e8 			ON e8.element_id = an.id8
LEFT OUTER JOIN ElementExtras ee8 	ON ee8.element_id = e8.element_id
LEFT OUTER JOIN Element e9 			ON e9.element_id = an.id9
LEFT OUTER JOIN ElementExtras ee9 	ON ee9.element_id = e9.element_id
LEFT OUTER JOIN Element e10 		ON e10.element_id = an.id10
LEFT OUTER JOIN ElementExtras ee10 	ON ee10.element_id = e10.element_id
LEFT OUTER JOIN Element e11 		ON e11.element_id = an.id11
LEFT OUTER JOIN ElementExtras ee11 	ON ee11.element_id = e11.element_id
) DERIVEDTBL


--RC-- RENAME 'RI Treaty Commission' FOLDER
UPDATE #Report_Transaction_Temp
SET Node4 = 'Facility / Binder Income'
WHERE Node4 = 'RI Treaty Commission'

--RC-- RENAME 'RI Other Commission' FOLDER
UPDATE #Report_Transaction_Temp
SET Node4 = 'FAC Income'
WHERE Node4 = 'RI Other Commission'

IF @IncludeClaims = 'Yes'
	BEGIN
		--RC-- REMOVE Gross Written Premium', 'RI Treaty Premium', 'RI Other Premium' FOLDERS
		--RC-- SRF27740, SRF27757 (S4I CORE) -- To take care of 'Gross Written Premium' which is truncated as 'Gross Written Premiu'
		SELECT * FROM #Report_Transaction_Temp
		WHERE Node4 Not IN ('Gross Written Premium', 'Gross Written Premiu', 'RI Treaty Premium', 'RI Other Premium')
	END
ELSE
	BEGIN
		--RC-- REMOVE Gross Written Premium', 'RI Treaty Premium', 'RI Other Premium' FOLDERS
		--RC-- SRF27740, SRF27757 (S4I CORE) -- To take care of 'Gross Written Premium' which is truncated as 'Gross Written Premiu'
		SELECT * FROM #Report_Transaction_Temp
		WHERE Node4 Not IN ('Gross Written Premium', 'Gross Written Premiu', 'RI Treaty Premium', 'RI Other Premium')
		AND
		--RC-- FILTER CLAIMS FOLDERS AND ACCOUNTS IF CHOSEN		
		(AccountName NOT Like 'CLM%')
    END


DROP TABLE #Report_Transaction_Temp
--******************************************END RC CHNAGES********************************************

DROP TABLE #Report_Transaction
DROP TABLE #AccountNodes



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
