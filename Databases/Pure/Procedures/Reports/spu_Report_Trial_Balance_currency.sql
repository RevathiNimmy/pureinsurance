--DC040803 ISS5754 modded rounding to 3dp not 2 for totalling
--eck09062003 Fixed for PN4589 Included changes made by MKW to Trial Balance for PN3365
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Trial_Balance_Currency'
GO

 

CREATE PROCEDURE spu_Report_Trial_Balance_Currency (
	@branch_id		int,
	@period_end_date	datetime)
AS

DECLARE	@iAccountID		int,
	@sBalBF			char(25),
	@sBalCF			char(25),
	@iBranchID		int,
	@dPeriodEndDateIn	datetime,
	@dNextMonth		datetime,
	@sYear			char(4),
	@sMonth			char(2),
	@dFirstDay		datetime,
	@dLastDay		datetime,
-- TF040800	@dPrevPeriodEndDate	datetime,
	@d1stPeriodEndDate	datetime,
	@dBFEndDate		datetime,
	@dPeriodEndDate		datetime,
	@dNextPeriodStartDate	datetime,
	@sCurrentYearName	varchar(20),
	@iCompanyid		int,
--DC101202
	@AgentUnderwriter 	varchar(1)

	SET NOCOUNT ON

	--DC101202 -start -a check ia required for broking or underwriting for changes to end date used for report
	SELECT 	@AgentUnderwriter = value
	FROM	hidden_options
	WHERE	branch_id = 1 and option_number = 1
	IF @AgentUnderwriter is null
		SELECT @AgentUnderwriter = 'A'
	IF @AgentUnderwriter = ''
		SELECT @AgentUnderwriter = 'A'
	--DC101202 -end
	
	SELECT @iBranchID = ISNULL(@branch_id, 0)

	IF @iBranchID = 0
	BEGIN
		DECLARE company_cursor CURSOR FAST_FORWARD
		FOR	select company_id 
			from Company 
	END
	ELSE
	BEGIN
		DECLARE company_cursor CURSOR FAST_FORWARD
		FOR	select company_id 
			from Company 
			where company_id = @iBranchID
	END

	SELECT @dPeriodEndDateIn = ISNULL(@period_end_date, GETDATE())

    --MKW300503 PN3365  If multi accounting use node 0 (therefore all nodes). START
    --EXECUTE spu_Report_FullTreePathNames 1
    if exists(select null from hidden_options where option_number=16)
        begin
            EXEC  spu_Report_FullTreePathNames 0
        end
    else
        begin
            EXEC  spu_Report_FullTreePathNames 1
        end
    --MKW300503 PN3365  If multi accounting use node 0 (therefore all nodes). END


	SELECT @sBalBF = 'Balance Brought Forward'
	SELECT @sBalCF = 'Balance Carried Forward'

-- Empty the temporary table
	DELETE FROM  Report_Transaction

	OPEN company_cursor

	FETCH NEXT FROM company_cursor INTO @iCompanyid
	WHILE @@FETCH_STATUS = 0
	BEGIN

		SELECT @dNextMonth = DATEADD(month, 1, @dPeriodEndDateIn)
		SELECT @dLastDay = DATEADD(day, DATEPART(dd, @dNextMonth) * -1, @dNextMonth )

		--DC101202 -start -now use actual date specified (for Broking), not find end date of the period the given date is in
		IF @AgentUnderwriter = 'A' 
		BEGIN
			SELECT @dPeriodEndDate = @dPeriodEndDateIn
		END
		ELSE
		BEGIN
			SELECT @dPeriodEndDate = (SELECT ISNULL(MIN(P.period_end_date), @dLastDay)
			FROM  Period 	P
			WHERE P.period_end_date >= @dPeriodEndDateIn
			AND p.company_id = @iCompanyid)		
		END
		--DC101202 -end


		SELECT @dLastDay = DATEADD(day, DATEPART(dd, @dPeriodEndDate) * -1, @dPeriodEndDate)

		SELECT @dBFEndDate = (SELECT	ISNULL(MAX(P.period_end_date), @dLastDay)
				      FROM	Period P
				      WHERE	P.period_end_date < @dPeriodEndDate
				      AND 	p.company_id = @iCompanyid)

		SELECT @sCurrentYearName = (SELECT	ISNULL(P.year_name, '')
					    FROM	Period P
					    WHERE	P.period_end_date = @dPeriodEndDate
					    AND 	p.company_id = @iCompanyid)

-- If 1st period end date not found, set it to the current period
		SELECT @d1stPeriodEndDate = (SELECT	ISNULL(MIN(P.period_end_date), @dPeriodEndDate)
					    FROM	Period P
					    WHERE	p.company_id = @iCompanyid
					    AND		P.year_name = @sCurrentYearName)

--MJ 10/10/01 simplified the statements somewhat
-- Brought forward amount
		INSERT INTO Report_Transaction	(
									transdetail_id,
									extra_numeric1,
									amount,
									extra_numeric2,
									document_ref,
									account_id,
									account_code,
									account_name,
									extra_datetime1,
									ledger_type,
									account_type,
									record_type
									)
		SELECT	0,
			isnull(sum(round(isnull(TD.currency_amount,0.00),2)), 0.0), -- eck 110902 added rounded --DC040803 -modded rounding to 3dp
			0.0,
			0.0,
			@sBalBF,
			A.Account_ID,
			A.short_code,
			MAX(A.account_name),
			@dBFEndDate,
			MAX(ISNULL(LT.description, '')),
			MAX(ISNULL(ATp.description, '')),
			0
		FROM	Account A
			LEFT OUTER JOIN  Ledger L
			ON A.ledger_id = L.ledger_id
			LEFT OUTER JOIN  LedgerType LT
			ON L.ledgertype_id = LT.ledgertype_id
			LEFT OUTER JOIN  AccountType ATp
			ON A.accounttype_id = ATp.accounttype_id
			JOIN  TransDetail TD
			ON A.account_id = TD.account_id
			JOIN  Document D
			ON TD.document_id = D.document_id
		where 	A.account_id in (SELECT account_id FROM  Report_TreePathNames)
		and	D.company_id = @iCompanyid
        AND     
        (
            (
                D.document_date <= @dBFEndDate
                AND 
                TD.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
            )
            OR
            (
                TD.ref_date <= @dBFEndDate
                AND   
                TD.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
            )
        )
		group by	A.account_id, A.short_code

-- Get Any details in the current period
		INSERT INTO  Report_Transaction(
							transdetail_id,
							extra_numeric1,
							amount,
							extra_numeric2,
							document_ref,
							account_id,
							account_code,
							account_name,
							account_type,
							ledger_type,
							record_type)
		SELECT	0,
			0.0,
			isnull(sum(round(isnull(TD.currency_amount,0.00),2)), 0.0), -- eck 110902 added rounded --DC040803 modded rounding to 3dp
			0.0,
			'This Period',
			A.Account_ID,
			A.short_code,
			MAX(A.account_name),
			ISNULL(Max(ATp.description), ''),
			ISNULL(Max(LT.description), ''),
			1
		FROM	Account A
			LEFT OUTER JOIN  Ledger L
			ON A.ledger_id = L.ledger_id
			LEFT OUTER JOIN  LedgerType LT
			ON L.ledgertype_id = LT.ledgertype_id
			LEFT OUTER JOIN  AccountType ATp
			ON A.accounttype_id = ATp.accounttype_id
			JOIN  TransDetail TD
			ON A.account_id = TD.account_id
			JOIN  Document D
			ON TD.document_id = D.document_id
			JOIN  Period P
			ON TD.period_id = P.period_id
		where 	A.account_id in (SELECT account_id FROM  Report_TreePathNames)
		and	D.company_id = @iCompanyid
        AND
        (
            (
                D.document_date > @dBFEndDate
                AND 
                D.document_date <= @dPeriodEndDate
                AND 
                TD.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
            )
            OR
            (
                TD.ref_date > @dBFEndDate
                AND 
                TD.ref_date <= @dPeriodEndDate
                AND   
                TD.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
            )
        )
		group by	A.account_id, A.short_code

--MJ 10/10/01 simplified the statements somewhat
-- Get the year to date balance
		INSERT INTO  Report_Transaction(
							transdetail_id,
							extra_numeric1,
							amount,
							extra_numeric2,
							document_ref,
							account_id,
							account_code,
							account_name,
							extra_datetime1,
							ledger_type,
							account_type,
							record_type)
		SELECT	0,
			0.0,
			0.0,
			isnull(sum(round(isnull(TD.currency_amount,0.00),2)), 0.0), -- eck 110902 added rounded --DC300703 modded rounding to 3dp
			@sBalCF,
			a.account_id,
			A.short_code,
			MAX(A.account_name),
			@dPeriodEndDate,
			MAX(ISNULL(LT.description, '')),
			MAX(ISNULL(ATp.description, '')),
			9
		FROM	Account A
			LEFT OUTER JOIN  Ledger L
			ON A.ledger_id = L.ledger_id
			LEFT OUTER JOIN  LedgerType LT
			ON L.ledgertype_id = LT.ledgertype_id
			LEFT OUTER JOIN  AccountType ATp
			ON A.accounttype_id = ATp.accounttype_id
			JOIN  TransDetail TD
			ON A.account_id = TD.account_id
			JOIN  Document D
			ON TD.document_id = D.document_id
		where 	A.account_id in (SELECT account_id FROM  Report_TreePathNames)
		and	D.company_id = @iCompanyid
        AND     
        (
            (
                D.document_date <= @dPeriodEndDate
                AND
                TD.spare NOT IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
            )
            OR
            (
                TD.ref_date <= @dPeriodEndDate
                AND
                TD.spare IN ('COMM ADJ', 'AGENT ADJ', 'BROK ADJ')
            )
        )
		group by	A.account_id, A.short_code

		FETCH NEXT FROM company_cursor INTO @iCompanyid

	END

	CLOSE company_cursor
	DEALLOCATE company_cursor

-- Get the data from the temporary tables

	SET NOCOUNT OFF

	SELECT	RTPN.element_name1,
		RTPN.Report_Map_Id1,
		RTPN.element_name2,
		RTPN.Report_Map_Id2,
		RTPN.element_name3,
		RTPN.Report_Map_Id3,
		RTPN.element_name4,
		RTPN.Report_Map_Id4,
		ISNULL(RTPN.element_name5, '') element_name5,
		ISNULL(RTPN.Report_Map_Id5, 0) Report_Map_Id5,
		ISNULL(RTPN.element_name6, '') element_name6,
		ISNULL(RTPN.Report_Map_Id6, 0) Report_Map_Id6,
		ISNULL(RTPN.element_name7, '') element_name7,
		ISNULL(RTPN.Report_Map_Id7, 0) Report_Map_Id7,
		ISNULL(RTPN.element_name8, '') element_name8,
		ISNULL(RTPN.Report_Map_Id8, 0) Report_Map_Id8,
		ISNULL(RTPN.element_name9, '') element_name9,
		ISNULL(RTPN.Report_Map_Id9, 0) Report_Map_Id9,
		ISNULL(RTPN.element_name10, '') element_name10,
		ISNULL(RTPN.Report_Map_Id10, 0) Report_Map_Id10,
		RT.extra_numeric1 bf_amount,
		RT.amount amount, 
		RT.extra_numeric2 cf_amount,
		RT.account_name,
		RT.account_code,
		RT.record_type,

		RT.extra_char1 period_name,
		RT.extra_datetime1 period_end_date,
		RT.document_date,
		ISNULL(RT.documenttype_id, 0) documenttype_id,
		RT.document_ref,
		RT.transdetail_id,
		RT.ledger_type,
		RT.account_type
	FROM	Report_Transaction RT
		JOIN  Report_TreePathNames RTPN
		ON RT.account_id = RTPN.account_id
	ORDER BY Report_Map_Id1, Report_Map_Id2, Report_Map_Id3, Report_Map_Id4, Report_Map_Id5, Report_Map_Id6, Report_Map_Id7, Report_Map_Id8, Report_Map_Id9, Report_Map_Id10, account_code, record_type, period_end_date, document_date

	SET NOCOUNT ON

	DELETE FROM  Report_Transaction
	DELETE FROM  Report_TreePathNames

	SET NOCOUNT OFF	






GO
 