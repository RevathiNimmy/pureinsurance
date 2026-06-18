SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Trial_Balance_Summary_Agency'
GO


CREATE PROCEDURE spu_Report_Trial_Balance_Summary_Agency
    @branch_id int,
    @PeriodDate varchar(20),
    @sType varchar(30),
    @Basis varchar(50)
AS


/**********************************************************************************************************************/
-- Jude Killip	03 Oct 2003
-- PN 6499: Optimisation rework - Based on spu_Report_Trial_Balance_SFU
-- Final Summary section is essentially as original version 
/**********************************************************************************************************************/
/*
--Testing
DECLARE  @branch_id      int,
    @PeriodDate varchar(20),
    @sType varchar(30),
    @Basis varchar(50)
    
SELECT  @branch_id = 0,
    @PeriodDate = '30 Sep 2003',
    @sType = 'All Dates',
    @Basis = 'Transaction Date'
--End Testing
*/

DECLARE @period_end_date    datetime
SELECT @PeriodDate = @PeriodDate + ' 23:59:59'
SELECT @period_end_date = CONVERT (Datetime, @PeriodDate)

-- @sType 'Current Year' or 'All Dates'
-- @Basis 'Transaction Date' or 'Transaction Period'

DECLARE @dtSelectedPeriodEnd datetime,  @dtPrevPeriodEnd datetime, @dtYearStart datetime, @dtPrevYearEnd datetime
DECLARE @SelectedPeriodID int
DECLARE @iBranchPeriod INT
-- Always use Branch 1 period table
-- if anyone sets up different periods for different branches this will
-- need to be revisited and worked out in the cursor
SELECT @iBranchPeriod = 1

DECLARE 
    @sBalBF         	char(25),
    @sBalCF         	char(25),
    @iBranchID      	int,
    @iCompanyid     	int

    SET NOCOUNT ON

    SELECT @iBranchID = ISNULL(@branch_id, 0)

    IF @iBranchID = 0
    BEGIN
        DECLARE company_cursor CURSOR FAST_FORWARD
        FOR select company_id
            from company
    END
    ELSE
    BEGIN
        DECLARE company_cursor CURSOR FAST_FORWARD
        FOR select company_id
            from company
            where company_id = @iBranchID
    END


    SELECT @sBalBF = 'Balance Brought Forward'
    SELECT @sBalCF = 'Balance Carried Forward'

    
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
    	RecordType int
    	)

    OPEN company_cursor

    FETCH NEXT FROM company_cursor INTO @iCompanyid
    WHILE @@FETCH_STATUS = 0
    BEGIN
    	
    	CREATE TABLE #PeriodIDRange
    		(Period_id int)

		-- Selected period values
		SELECT @dtSelectedPeriodEnd = max(period_end_date)
		FROM period
		WHERE period_end_Date <= @period_end_date
		AND company_id = @iBranchPeriod

		SELECT @SelectedPeriodID = period_id 
		FROM period
		WHERE period_end_Date  = @dtSelectedPeriodEnd
		AND company_id = @iBranchPeriod

		-- Previous period values
		SELECT @dtPrevPeriodEnd = max(period_end_date)
		FROM Period
		WHERE period_end_date < @period_end_date
		AND company_id = @iBranchPeriod

		-- *If current period is the first period set up
		-- (no need for exact date)
		IF @dtPrevPeriodEnd IS NULL
		BEGIN
			SELECT  @dtPrevPeriodEnd = dateadd(month, -1, @dtSelectedPeriodEnd),
					@dtPrevYearEnd = @dtPrevPeriodEnd
		END
		ELSE
		BEGIN
			-- year start period values
			SELECT @dtYearStart = min(period_end_date)
			FROM period
			WHERE year_name = (SELECT year_name
							   FROM period
							   WHERE period_id = @SelectedPeriodID
							   AND company_id = @iBranchPeriod)
			AND company_id = @iBranchPeriod 

			-- previous year end values
			SELECT @dtPrevYearEnd = max(period_end_date)
			FROM period
			WHERE period_end_date < @dtYearStart
			AND company_id = @iBranchPeriod


			-- *If there are no periods set up for the previous year
			IF @dtPrevYearEnd IS NULL
			BEGIN
				SELECT  @dtPrevYearEnd = dateadd(month, -1, @dtYearStart)
			END
		END
		
		-- get all period ids up to selected period
		-- cannot assume ids are in date order
		
		IF @sType = 'Current Year'
		BEGIN
			INSERT INTO #PeriodIDRange
				SELECT period_id
				FROM Period
				WHERE period_end_date > @dtPrevYearEnd
				AND period_end_date <= @dtPrevPeriodEnd 
				AND company_id = @iBranchPeriod 
		END		
		ELSE
		BEGIN
			INSERT INTO #PeriodIDRange
				SELECT period_id
				FROM Period
				WHERE period_end_date <= @dtPrevPeriodEnd 
				AND company_id = @iBranchPeriod 
		
			SELECT @dtPrevYearEnd =  '1900-01-01'
		END
		

	-- BF details, not including selected period
		INSERT INTO #Report_Transaction    
			(TransdetailID,
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
			RecordType
			)
		SELECT  
			0,
			isnull(sum(ROUND(TD.amount,2)), 0.0),       -- eck 110902 added rounded
			0.0,
			0.0,
			@sBalBF,
			A.Account_ID,
			A.short_code,
			MAX(A.account_name),
			@dtPrevPeriodEnd,
			MAX(ISNULL(LT.description, '')),
			MAX(ISNULL(ATp.description, '')),
			0
		FROM Account A
		LEFT OUTER JOIN Ledger L 		ON A.ledger_id = L.ledger_id
		LEFT OUTER JOIN LedgerType LT 	ON L.ledgertype_id = LT.ledgertype_id
		LEFT OUTER JOIN AccountType ATp ON A.accounttype_id = ATp.accounttype_id
		JOIN TransDetail TD             ON A.account_id = TD.account_id
		JOIN Document D                	ON TD.document_id = D.document_id
		WHERE D.company_id = @iCompanyid
		AND (
			(@Basis = 'Transaction Period' AND (TD.period_id IN (SELECT Period_id FROM #PeriodIDRange)))
			OR
			(@Basis = 'Transaction Date' AND (D.document_date > @dtPrevYearEnd AND D.document_Date <= @dtPrevPeriodEnd))
			)
		GROUP BY    A.account_id, A.short_code

    -- Get Any details in the selected period
		INSERT INTO #Report_Transaction
			(TransdetailID,
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
			RecordType
			)
		SELECT  0,
			0.0,
			isnull(sum(ROUND(TD.amount,2)), 0.0),       -- eck 110902 added rounded
			0.0,
			'This Period',
			a.account_id,
			A.short_code,
			MAX(A.account_name),
			@dtSelectedPeriodEnd,
			MAX(ISNULL(LT.description, '')),
			MAX(ISNULL(ATp.description, '')),
			1
		FROM Account A
		LEFT OUTER JOIN Ledger L 		ON A.ledger_id = L.ledger_id
		LEFT OUTER JOIN LedgerType LT 	ON L.ledgertype_id = LT.ledgertype_id
		LEFT OUTER JOIN AccountType ATp	ON A.accounttype_id = ATp.accounttype_id
		JOIN TransDetail TD				ON A.account_id = TD.account_id
		JOIN Document D					ON TD.document_id = D.document_id
		WHERE D.company_id = @iCompanyid
		AND (
			(@Basis = 'Transaction Period' AND (TD.period_id = @SelectedPeriodID ))
			OR
			(@Basis = 'Transaction Date' AND (D.document_date > @dtPrevPeriodEnd AND D.document_date <= @dtSelectedPeriodEnd))
			)
		GROUP BY A.account_id, A.short_code


-- Get the to date balance
		INSERT INTO #Report_Transaction    
			(TransdetailID,
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
			RecordType
			)
		SELECT  0,
			0.0,
			0.0,
			isnull(sum(ROUND(TD.amount,2)), 0.0),       -- eck 110902 added rounded
			@sBalCF,
			a.account_id,
			A.short_code,
			MAX(A.account_name),
			@dtSelectedPeriodEnd,
			MAX(ISNULL(LT.description, '')),
			MAX(ISNULL(ATp.description, '')),
			9
		FROM Account A
		LEFT OUTER JOIN Ledger L			ON A.ledger_id = L.ledger_id
		LEFT OUTER JOIN LedgerType LT		ON L.ledgertype_id = LT.ledgertype_id
		LEFT OUTER JOIN AccountType ATp		ON A.accounttype_id = ATp.accounttype_id
		JOIN TransDetail TD					ON A.account_id = TD.account_id
		JOIN Document D						ON TD.document_id = D.document_id
		WHERE D.company_id = @iCompanyid
		AND (
			(@Basis = 'Transaction Period' AND (TD.period_id IN (SELECT Period_id FROM #PeriodIDRange)
							OR TD.period_id = @SelectedPeriodID))
			OR
			(@Basis = 'Transaction Date' AND (D.document_date > @dtPrevYearEnd AND D.document_Date <= @dtSelectedPeriodEnd))
		)
		GROUP BY A.account_id, A.short_code
		
		
		DROP TABLE #PeriodIDRange

        FETCH NEXT FROM company_cursor INTO @iCompanyid

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
CREATE TABLE #TBSummary
(
	Node1 varchar(30),
	Map1 int,
	Node2 varchar(30),
	Map2 int,
	Node3 varchar(30),
	Map3 int,
	Node4 varchar(30),
	Map4 int,
	Node5 varchar(30),
	Map5 int,
	Node6 varchar(30),
	Map6 int,
	Node7 varchar(30),
	Map7 int,
	Node8 varchar(30),
	Map8 int,
	Node9 varchar(30),
	Map9 int,
	Node10 varchar(30),
	Map10 int,
	Node11 varchar(30),
	Map11 int,
	AmountBF money,
	Amount money,
	AmountCF money,
	AccountId int,
	AccountName varchar(150),
	AccountCode varchar(150),
	RecordType int,
	EndDate datetime,
	RecordDescription varchar(150),
	TransdetailID int,
	LedgerType varchar(150),
	AccountType varchar(150)
)
-- combine Transactions and Account Structure

INSERT INTO #TBSummary
	SELECT  e1.element_name,	ee1.report_map_id,
		e2.element_name,		ee2.report_map_id,
		e3.element_name,		ee3.report_map_id,
		e4.element_name,		ee4.report_map_id,
		e5.element_name,		ee5.report_map_id,
		e6.element_name,		ee6.report_map_id,
		e7.element_name,		ee7.report_map_id,
		e8.element_name,		ee8.report_map_id,
		e9.element_name,		ee9.report_map_id,
		e10.element_name,	ee10.report_map_id,
		e11.element_name,	ee11.report_map_id,
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
		RT.AccountType
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


DROP TABLE #Report_Transaction
DROP TABLE #AccountNodes

-- Tom Brown
-- All data now in temp tables.  
-- get out the values we want by names selections
------------------------------------------
-- Sub-total 1
--    Clients, Agent, Sub-Agent, Third-Parties
-- Sub-total 2
--    Insurers & ReInsurers,  (we only have RI Payable)
-- Sub-total 3
--    Banks, Income, Expenditure, OtherAssets, OtherLiabilities
/************************************************************
Some of those heading items are combinations of items in our ledgers
Definitions of those combinations (according to Barrie Mitchell at AUA.
    Clients = Claims Receivable + Direct + Claims Payable
    Third Parties = Other Party Receivable + Other Party Payable
    Other Assets = Fees Recievable + O/S Claims RI TTY
    Other Liabilities = O/S Claims Adj + Tax + Share Capital & Reserves
****************************************************************/
-- This really is the long way round as far as SQL is concerned, BUT
-- its easy to follow, its obvious what's going on, and its quicker than
-- sending it all to Crystal and then sorting out the results there, as
-- that would be a nightmare to follow.

-- Other points:  I've deliberatly selected on element name = 'Text' 
-- rather than element_ID because this makes it easier to follow.
-- The SQL won't run as fast - but I think its worth it for clarity.

-- Create a table to accept this data
CREATE TABLE #TBSummaryOutput (
    TotalID INT,
    SubTotalID INT,
    Heading varchar(50),
    BF_Amount decimal(19,4),
    Amount decimal(19,4),
    Positives decimal(19,4),
    Negatives decimal(19,4),
    CF_Amount decimal(19,4)
)

-- Variables to separate payments from receipts.  
DECLARE @Positives decimal(19,4)
DECLARE @Negatives decimal(19,4)

-- Clients = Sum 'Direct' and 'Claims Receivable' and 'Claims Payable'
/************************************************************************** Client *********/
INSERT INTO #TBSummaryOutput
SELECT  1,
        1,
        'Clients',
        SUM(TB.AmountBF),
        SUM(TB.Amount),
        NULL,
        NULL,
        SUM(TB.AmountCF)
FROM #TBSummary TB
WHERE   TB.Node1 = 'Nominal Ledger' --Node1
    AND TB.Node2 = 'Balance Sheet' --Node2
    AND ( TB.Node3 = 'Assets' --Node3
          AND TB.Node4 = 'Current Assets'
          AND TB.Node5 = 'Sales Ledger'
          AND (ISNULL(TB.Node6, '') = 'Claims Receivable' OR
               ISNULL(TB.Node6, '') = 'Direct' ) )
     OR ( TB.Node3 = 'Liabilities'
          AND TB.Node4 = 'Current Liabilities'
          AND TB.Node5 = 'Claims Payable' )

-- TB 17/2/03: Change to use cf_amount instead of amount
-- Set the Positives and Negatives
SELECT @Positives = ( SELECT -- SUM(TB.Amount)
                             SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE -- ( TB.Amount > 0 ) AND
                            (TB.AmountCF > 0 ) AND
                            ( TB.Node1 = 'Nominal Ledger'
                              AND TB.Node2 = 'Balance Sheet'
                              AND ( TB.Node3 = 'Assets'
                                    AND TB.Node4 = 'Current Assets'
                                    AND TB.Node5 = 'Sales Ledger'
                                    AND (ISNULL(TB.Node6, '') = 'Claims Receivable' OR
                                         ISNULL(TB.Node6, '') = 'Direct' ) )
                               OR ( TB.Node3 = 'Liabilities'
                                    AND TB.Node4 = 'Current Liabilities'
                                    AND TB.Node5 = 'Claims Payable' )
                              )
                        )


SELECT @Negatives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE ( TB.AmountCF < 0 ) AND
                            ( TB.Node1 = 'Nominal Ledger'
                              AND TB.Node2 = 'Balance Sheet'
                              AND ( TB.Node3 = 'Assets'
                                    AND TB.Node4 = 'Current Assets'
                                    AND TB.Node5 = 'Sales Ledger'
                                    AND (ISNULL(TB.Node6, '') = 'Claims Receivable' OR
                                         ISNULL(TB.Node6, '') = 'Direct' ) )
                               OR ( TB.Node3 = 'Liabilities'
                                    AND TB.Node4 = 'Current Liabilities'
                                    AND TB.Node5 = 'Claims Payable' )
                              )
                        )
UPDATE #TBSummaryOutput
    SET Positives = ISNULL(@Positives,0),
        Negatives = ISNULL(@Negatives,0)
    WHERE Heading = 'Clients'

/*************************************** Agent ******************************************/
INSERT INTO #TBSummaryOutput
SELECT  2,
        1,
        'Agent',
        SUM(TB.AmountBF),
        SUM(TB.Amount),
        NULL,
        NULL,
        SUM(TB.AmountCF)
FROM #TBSummary TB
WHERE   TB.Node1 = 'Nominal Ledger'
    AND TB.Node2 = 'Balance Sheet'
    AND TB.Node3 = 'Assets'
    AND TB.Node4 = 'Current Assets'
    AND TB.Node5 = 'Sales Ledger'
    AND (ISNULL(TB.Node6, '') = 'Agents' )

SELECT @Positives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE  ( TB.AmountCF > 0 )
                          AND TB.Node1 = 'Nominal Ledger'
                          AND TB.Node2 = 'Balance Sheet'
                          AND TB.Node3 = 'Assets'
                          AND TB.Node4 = 'Current Assets'
                          AND TB.Node5 = 'Sales Ledger'
                          AND (ISNULL(TB.Node6, '') = 'Agents' )
                    )
SELECT @Negatives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE  ( TB.AmountCF  < 0 )
                          AND TB.Node1 = 'Nominal Ledger'
                          AND TB.Node2 = 'Balance Sheet'
                          AND TB.Node3 = 'Assets'
                          AND TB.Node4 = 'Current Assets'
                          AND TB.Node5 = 'Sales Ledger'
                          AND (ISNULL(TB.Node6, '') = 'Agents' )
                    )
UPDATE #TBSummaryOutput
    SET Positives = ISNULL(@Positives,0),
        Negatives = ISNULL(@Negatives,0)
    WHERE Heading = 'Agent'

/********************************************** Sub-Agent *********************************/
INSERT INTO #TBSummaryOutput
SELECT  3,
        1,
        'Sub Agent',
        SUM(TB.AmountBF),
        SUM(TB.Amount),
        NULL,
        NULL,
        SUM(TB.AmountCF)
FROM #TBSummary TB
WHERE   TB.Node1 = 'Nominal Ledger'
    AND TB.Node2 = 'Balance Sheet'
    AND TB.Node3 = 'Liabilities'
    AND TB.Node4 = 'Current Liabilities'
    AND TB.Node5 = 'Sub Agents'

SELECT @Positives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE  ( TB.AmountCF > 0 )
                          AND TB.Node1 = 'Nominal Ledger'
                          AND TB.Node2 = 'Balance Sheet'
                          AND TB.Node3 = 'Liabilities'
                          AND TB.Node4 = 'Current Liabilities'
                          AND TB.Node5 = 'Sub Agents'
                     )

SELECT @Negatives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE  ( TB.AmountCF < 0 )
                          AND TB.Node1 = 'Nominal Ledger'
                          AND TB.Node2 = 'Balance Sheet'
                          AND TB.Node3 = 'Liabilities'
                          AND TB.Node4 = 'Current Liabilities'
                          AND TB.Node5 = 'Sub Agents'
                     )

UPDATE #TBSummaryOutput
    SET Positives = ISNULL(@Positives,0),
        Negatives = ISNULL(@Negatives,0)
    WHERE Heading = 'Sub Agent'



/******************************************************** Third Party **************/
INSERT INTO #TBSummaryOutput
SELECT  4,
        1,
        'Third Parties',
        SUM(TB.AmountBF),
        SUM(TB.Amount),
        NULL,
        NULL,
        SUM(TB.AmountCF)
FROM #TBSummary TB
WHERE   TB.Node1 = 'Nominal Ledger'
    AND TB.Node2 = 'Balance Sheet'
  AND  ( TB.Node3 = 'Liabilities'
         AND TB.Node4 = 'Current Liabilities'
         AND TB.Node5 = 'Other Party Payable' )
   OR  ( TB.Node3 = 'Assets'
         AND TB.Node4 = 'Current Assets'
         AND TB.Node5 = 'Sales Ledger'
         AND ISNULL(TB.Node6,'') = 'Other Party R''''able' )

SELECT @Positives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM #TBSummary TB
                      WHERE ( TB.AmountCF > 0 ) AND 
                            ( TB.Node1 = 'Nominal Ledger'
                              AND TB.Node2 = 'Balance Sheet'
                              AND  ( TB.Node3 = 'Liabilities'
                                    AND TB.Node4 = 'Current Liabilities'
                                    AND TB.Node5 = 'Other Party Payable' )
                               OR  ( TB.Node3 = 'Assets'
                                    AND TB.Node4 = 'Current Assets'
                                    AND TB.Node5 = 'Sales Ledger'
                                    AND ISNULL(TB.Node6,'') = 'Other Party R''''able' )
                              )
                     )
SELECT @Negatives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM #TBSummary TB
                      WHERE ( TB.AmountCF < 0 ) AND
                            ( TB.Node1 = 'Nominal Ledger'
                              AND TB.Node2 = 'Balance Sheet'
                              AND  ( TB.Node3 = 'Liabilities'
                                    AND TB.Node4 = 'Current Liabilities'
                                    AND TB.Node5 = 'Other Party Payable' )
                               OR  ( TB.Node3 = 'Assets'
                                    AND TB.Node4 = 'Current Assets'
                                    AND TB.Node5 = 'Sales Ledger'
                                    AND ISNULL(TB.Node6,'') = 'Other Party R''''able' )
                              )
                      )
UPDATE #TBSummaryOutput
    SET Positives = ISNULL(@Positives,0),
        Negatives = ISNULL(@Negatives,0)
    WHERE Heading = 'Third Parties'

/*****************************************  Insurers  **********************************/
INSERT INTO #TBSummaryOutput
SELECT  1,
        2,
        '(Re)Insurers',
        SUM(TB.AmountBF),
        SUM(TB.Amount),
        NULL,
        NULL,
        SUM(TB.AmountCF)
FROM #TBSummary TB
WHERE   TB.Node1 = 'Nominal Ledger'
    AND TB.Node2 = 'Balance Sheet'
    AND TB.Node3 = 'Liabilities'
    AND TB.Node4 = 'Current Liabilities'
    AND TB.Node5 = 'RI Payable'

SELECT @Positives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE  ( TB.AmountCF > 0 )
                          AND TB.Node1 = 'Nominal Ledger'
                          AND TB.Node2 = 'Balance Sheet'
                          AND TB.Node3 = 'Liabilities'
                          AND TB.Node4 = 'Current Liabilities'
                          AND TB.Node5 = 'RI Payable'
                     )

SELECT @Negatives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE  ( TB.AmountCF < 0 )
                          AND TB.Node1 = 'Nominal Ledger'
                          AND TB.Node2 = 'Balance Sheet'
                          AND TB.Node3 = 'Liabilities'
                          AND TB.Node4 = 'Current Liabilities'
                          AND TB.Node5 = 'RI Payable'
                     )

UPDATE #TBSummaryOutput
    SET Positives = ISNULL(@Positives,0),
        Negatives = ISNULL(@Negatives,0)
    WHERE Heading = '(Re)Insurers'


/********************************************************  Bank  ************************/
INSERT INTO #TBSummaryOutput
SELECT  1,
        3,
        'Bank',
        SUM(TB.AmountBF),
        SUM(TB.Amount),
        NULL,
        NULL,
        SUM(TB.AmountCF)
FROM #TBSummary TB
WHERE   TB.Node1 = 'Nominal Ledger'
    AND TB.Node2 = 'Balance Sheet'
    AND TB.Node3 = 'Assets'
    AND TB.Node4 = 'Current Assets'
    AND TB.Node5 = 'Bank'


SELECT @Positives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE  ( TB.AmountCF > 0 )
                          AND TB.Node1 = 'Nominal Ledger'
                          AND TB.Node2 = 'Balance Sheet'
                          AND TB.Node3 = 'Assets'
                          AND TB.Node4 = 'Current Assets'
                          AND TB.Node5 = 'Bank'
                    )
SELECT @Negatives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE  ( TB.AmountCF  < 0 )
                          AND TB.Node1 = 'Nominal Ledger'
                          AND TB.Node2 = 'Balance Sheet'
                          AND TB.Node3 = 'Assets'
                          AND TB.Node4 = 'Current Assets'
                          AND TB.Node5 = 'Bank'
                    )
UPDATE #TBSummaryOutput
    SET Positives = ISNULL(@Positives,0),
        Negatives = ISNULL(@Negatives,0)
    WHERE Heading = 'Bank'

/*******************************************************   Income  *********************/
INSERT INTO #TBSummaryOutput
SELECT  2,
        3,
        'Income',
        SUM(TB.AmountBF),
        SUM(TB.Amount),
        NULL,
        NULL,
        SUM(TB.AmountCF)
FROM #TBSummary TB
WHERE   TB.Node1 = 'Nominal Ledger'
    AND TB.Node2 = 'Profit and Loss'
    AND TB.Node3 = 'Income'


SELECT @Positives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE  ( TB.AmountCF > 0 )
                          AND TB.Node1 = 'Nominal Ledger'
                          AND TB.Node2 = 'Profit and Loss'
                          AND TB.Node3 = 'Income'
                    )
SELECT @Negatives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE  ( TB.AmountCF  < 0 )
                          AND TB.Node1 = 'Nominal Ledger'
                          AND TB.Node2 = 'Profit and Loss'
                          AND TB.Node3 = 'Income'
                    )
UPDATE #TBSummaryOutput
    SET Positives = ISNULL(@Positives,0),
        Negatives = ISNULL(@Negatives,0)
    WHERE Heading = 'Income'

/****************************************************  Expenditure ***************************/
INSERT INTO #TBSummaryOutput
SELECT  3,
        3,
        'Expenditure',
        SUM(TB.AmountBF),
        SUM(TB.Amount),
        NULL,
        NULL,
        SUM(TB.AmountCF)
FROM #TBSummary TB
WHERE   TB.Node1 = 'Nominal Ledger'
    AND TB.Node2 = 'Profit and Loss'
    AND TB.Node3 = 'Expenditure'

SELECT @Positives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE  ( TB.AmountCF > 0 )
                          AND TB.Node1 = 'Nominal Ledger'
                          AND TB.Node2 = 'Profit and Loss'
                          AND TB.Node3 = 'Expenditure'
                    )
SELECT @Negatives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE  ( TB.AmountCF  < 0 )
                          AND TB.Node1 = 'Nominal Ledger'
                          AND TB.Node2 = 'Profit and Loss'
                          AND TB.Node3 = 'Expenditure'
                    )
UPDATE #TBSummaryOutput
    SET Positives = ISNULL(@Positives,0),
        Negatives = ISNULL(@Negatives,0)
    WHERE Heading = 'Expenditure'

/********************************************* Other Assets ****************************/
INSERT INTO #TBSummaryOutput
SELECT  4,
        3,
        'Other Assets',
        SUM(TB.AmountBF),
        SUM(TB.Amount),
        NULL,
        NULL,
        SUM(TB.AmountCF)
FROM #TBSummary TB
WHERE   TB.Node1 = 'Nominal Ledger'
    AND TB.Node2 = 'Balance Sheet'
    AND TB.Node3 = 'Assets'
    AND TB.Node4 = 'Current Assets'
    AND ( TB.Node5 = 'Fees Receivable'
          OR TB.Node5 = 'O/S Claims RI TTY' )

SELECT @Positives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE  ( TB.AmountCF > 0 )
                          AND TB.Node1 = 'Nominal Ledger'
                          AND TB.Node2 = 'Balance Sheet'
                          AND TB.Node3 = 'Assets'
                          AND TB.Node4 = 'Current Assets'
                          AND ( TB.Node5 = 'Fees Receivable'
                                OR TB.Node5 = 'O/S Claims RI TTY' )
                    )
SELECT @Negatives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE  ( TB.AmountCF  < 0 )
                          AND TB.Node1 = 'Nominal Ledger'
                          AND TB.Node2 = 'Balance Sheet'
                          AND TB.Node3 = 'Assets'
                          AND TB.Node4 = 'Current Assets'
                          AND ( TB.Node5 = 'Fees Receivable'
                                OR TB.Node5 = 'O/S Claims RI TTY' )
                    )
UPDATE #TBSummaryOutput
    SET Positives = ISNULL(@Positives,0),
        Negatives = ISNULL(@Negatives,0)
    WHERE Heading = 'Other Assets'

/***************************************************** Other Liabilities *******************/
INSERT INTO #TBSummaryOutput
SELECT  5,
        3,
        'Other Liabilities',
        SUM(TB.AmountBF),
        SUM(TB.Amount),
        NULL,
        NULL,
        SUM(TB.AmountCF)
FROM #TBSummary TB
WHERE   TB.Node1 = 'Nominal Ledger'
    AND TB.Node2 = 'Balance Sheet'
    AND TB.Node3 = 'Liabilities'
    AND ( TB.Node4 = 'Current Liabilities'
          AND ( TB.Node5 = 'O/S Claims Adj'
                OR TB.Node5 = 'Tax' ) )
     OR ( TB.Node4 = 'Share Capital & Res' )

SELECT @Positives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE ( TB.AmountCF > 0 ) AND
                            ( TB.Node1 = 'Nominal Ledger'
                              AND TB.Node2 = 'Balance Sheet'
                              AND TB.Node3 = 'Liabilities'
                              AND ( TB.Node4 = 'Current Liabilities'
                                    AND ( TB.Node5 = 'O/S Claims Adj'
                                    OR TB.Node5 = 'Tax' ) )
                               OR ( TB.Node4 = 'Share Capital & Res' )
                             )
                    )
SELECT @Negatives = ( SELECT -- SUM(TB.Amount)
				SUM(TB.AmountCF)
                      FROM  #TBSummary TB
                      WHERE ( TB.AmountCF < 0 ) AND
                            ( TB.Node1 = 'Nominal Ledger'
                              AND TB.Node2 = 'Balance Sheet'
                              AND TB.Node3 = 'Liabilities'
                              AND ( TB.Node4 = 'Current Liabilities'
                                    AND ( TB.Node5 = 'O/S Claims Adj'
                                    OR TB.Node5 = 'Tax' ) )
                               OR ( TB.Node4 = 'Share Capital & Res' )
                             )
                    )
UPDATE #TBSummaryOutput
    SET Positives = ISNULL(@Positives,0),
        Negatives = ISNULL(@Negatives,0)
    WHERE Heading = 'Other Liabilities'



-- Output required for the report
SELECT	TotalID,
		SubTotalID,
		Heading,
		BF_Amount,
		Amount,
		Positives,
		Negatives,
		CF_Amount
FROM #TBSummaryOutput


DROP TABLE #TBSummaryOutput
DROP TABLE #TBSummary


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON
GO

