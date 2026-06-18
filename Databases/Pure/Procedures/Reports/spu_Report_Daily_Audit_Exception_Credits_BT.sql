SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Daily_Audit_Exception_Credits_BT'
GO
 
CREATE PROCEDURE spu_Report_Daily_Audit_Exception_Credits_BT
	@branch_id	int,
	@start_date	datetime,
	@end_date	datetime,
	@BA 		varchar(11)
AS

DECLARE	@iBranchID	int,
	@document_id	int

SELECT @iBranchID = ISNULL(@branch_id, 0)

set nocount on

-- report_audit_debit_table3 used to hold the basic transaction details for processing within the cursor
DELETE FROM report_audit_debit_table2
-- report_audit_debit_table3 used to hold the info we're passing to the report in the end
DELETE FROM report_audit_debit_table3

DBCC CHECKIDENT (report_audit_debit_table3, RESEED, 0)

INSERT INTO report_audit_debit_table3
(	year_name, 
	period_name, 
	transdetail_id, 
	accounting_date, 
	insurance_ref,  
	comment,
	short_code,
	Document_ref,
	document_type,
	party_type,
	type_description,
	risk_code,
	risk_description,
	cover_start_date,
	this_premium,
	insurer_premium,
	net_commission,
	fees,
	agent_fees,
	commission_amount,
	disc,
	sub_agent_fees,
	commission_percentage,
	resolved_name,
	document_date,
	account_id,
	account_handler,
	Company_id,
	Company_desc
)
--MJ 07/2002  select all debit transactions including reversals
SELECT DISTINCT
	P.year_name					as 'year_name',
	P.period_name					as 'period_name',
	D.document_id					as 'transdetail_id',  -- actually the document_id
	TD.accounting_date				as 'accounting_date',
	isnull(IFI.insurance_ref, '')			as 'insurance_ref',
	D.comment 					as 'comment',
	A.short_code 					as 'short_code',
	D.Document_ref 					as 'Document_ref',
	DT.description 					as 'document_type',
	ISNULL(PT.code, '') 				as 'party_type',
	ISNULL(PT.description, '')			as 'type_description',
	ISNULL(RC.code, '')				as 'risk_code',
	ISNULL(RC.description, '')			as 'risk_description',
	ISNULL(IFI.cover_start_date, '')		as 'cover_start_date',
	0						as 'this_premium',
	0						as 'insurer_premuim',
	0						as 'net_commission',
	0						as 'fees',
	0						as 'agent_fees',
	0						as 'commission_amount',
	0						as 'disc',
	0						as 'sub_agent_fees',
	ISNULL(IFI.commission_percentage, 0)		as 'commission_percentage',
	ISNULL(PY.resolved_name, '')			as 'resolved_name',
	D.document_date					as 'document_date',
	A.account_id					as 'account_id',
	ISNULL(BT.description, '')			as 'account_handler',
	D.company_id					as 'Company_id',
	Comp.description				as 'Company_desc'
FROM	TransDetail 					TD
JOIN	Document 						D
	ON TD.document_id = D.document_id
JOIN	Company						Comp
	ON D.company_id = Comp.company_id
JOIN	DocumentType	 				DT
	ON D.documenttype_id = DT.documenttype_id
JOIN	Period 						P
	ON TD.period_id = P.period_id
JOIN	Account		 				A
	ON TD.account_id = A.account_id
JOIN	party 									PY
	ON A.account_key = PY.party_cnt
JOIN	party_type 								PT
	ON PY.party_type_id = PT.party_type_id
left outer JOIN transaction_export_folder					TEF
	ON td.insurance_ref = TEF.insurance_ref
	AND D.company_id = TEF.source_id 
	AND D.document_ref = TEF.document_ref
left outer JOIN	insurance_file 							IFI
	ON TEF.insurance_file_cnt = IFI.insurance_file_cnt
--eck260902
left outer JOIN	insurance_file 							IFI2
	ON TD.insurance_ref = IFI2.insurance_ref
	AND IFI2.policy_version =	
	(	select max(policy_version)
		from insurance_file
		where insurance_ref = IFI2.insurance_ref
   		and lead_insurer_cnt IS NOT NULL	
	)	
--DJM
left outer JOIN	business_type					BT
		on BT.business_type_id = IFI.business_type_id
left outer JOIN	risk_code 							RC
	ON IFI.risk_code_id = RC.risk_code_id
WHERE	
	D.document_date >= @start_date
	AND D.document_date <= @end_date
AND	(
	(IFI2.cover_start_date < @start_date				--eck230902
	AND @BA = 'Before'
	)
	OR
	(IFI2.cover_start_date > @end_date				--eck230902
	AND @BA = 'After'
	)
	)
-- Not sure if we need this section at this point but it works so left it
AND	(	ISNULL(TD.spare, '') NOT IN ('COMM ADJ', 'AGENT ADJ')
		AND
		TD.Document_Sequence NOT IN 	
		(	SELECT	Document_Sequence + 1
			FROM	TransDetail 
			WHERE	document_id = TD.document_id
			AND	spare = 'COMM ADJ'
		)
	)
AND	PT.code IN ('PC', 'CC', 'GC')
AND	D.documenttype_id IN (3, 5, 16, 18, 32, 36)
AND	(	@iBranchID = 0
		OR	
		(	@iBranchID <> 0
			AND
			TD.company_id = @iBranchId				
		)
	)
-- only get the first CLient for split premiums or so I hope
AND TD.document_sequence =
(	select min(document_sequence)
	from 	TransDetail		TD1
	JOIN	Account 		A1
		ON A1.Account_id = TD1.Account_id
	where TD1.document_id = TD.document_id 
	and A1.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL16/06/2003 Remove Hard coded ledger_id
)
order by d.document_id

-- set the account handler and the other info requiring a Trans exp record for reversal items
UPDATE	radt
SET	radt.insurance_ref = isnull(IFI.insurance_ref, TD.insurance_ref),
	radt.party_type = ISNULL(PT.code, ''),
	radt.type_description = ISNULL(PT.description, ''),
	radt.risk_code = ISNULL(RC.code, ''),
	radt.risk_description = ISNULL(RC.description, ''),
	radt.cover_start_date = ISNULL(IFI.cover_start_date, ''),
	radt.commission_percentage = ISNULL(IFI.commission_percentage, 0),
	radt.resolved_name = ISNULL(PY.resolved_name, ''),
	radt.account_handler = ISNULL(BT.description, '')
FROM	report_audit_debit_table3				radt
JOIN	Document 				D
	ON D.document_ref = SUBSTRING(radt.comment, 22, 11)
	AND D.company_id = radt.company_id
JOIN	Transdetail			TD
	ON D.document_id = TD.document_id
JOIN	Account				A
	ON TD.account_id = A.account_id
left outer JOIN	party 						PY
	ON A.account_key = PY.party_cnt
left outer JOIN	party_type 					PT
	ON PY.party_type_id = PT.party_type_id
left outer JOIN transaction_export_folder			TEF
	ON td.insurance_ref = TEF.insurance_ref
	AND D.company_id = TEF.source_id 
	AND D.document_ref = TEF.document_ref
left outer JOIN	insurance_file 					IFI
	ON TEF.insurance_file_cnt = IFI.insurance_file_cnt
left outer JOIN	risk_code 					RC
	on RC.risk_code_id = IFI.risk_code_id
--DJM
left outer JOIN	business_type					BT
		on BT.business_type_id = IFI.business_type_id
WHERE radt.comment like 'Reversal of Document%'
AND PT.code IN ('PC', 'CC', 'GC')

-- Process each document one at a time slow but effective due to the nature of the info needed

DECLARE	c_Cursor CURSOR FAST_FORWARD FOR	
SELECT transdetail_id		-- which is actually the document_id
FROM report_audit_debit_table3
group BY transdetail_id

OPEN	c_Cursor

FETCH NEXT FROM c_Cursor INTO @document_id

WHILE @@FETCH_STATUS = 0
BEGIN	

	DELETE FROM report_audit_debit_table2

	-- Select the Whole Document into Table 2 except the adjustments
	-- basicall recreates core bits of the transdetail table
	INSERT INTO report_audit_debit_table2
	SELECT	TD.transdetail_id,
		A.account_id,
		A.Ledger_id,
		TD.amount,
		ISNULL(RTRIM(TD.spare),'')
	FROM	TransDetail 	TD
	JOIN	Account 		A
		ON A.Account_id = TD.Account_id
	WHERE	TD.document_id = @document_id
	AND	(	ISNULL(TD.spare, '') NOT IN ('COMM ADJ', 'AGENT ADJ')
			AND
			TD.Document_Sequence NOT IN 	
			(	SELECT	Document_Sequence + 1
				FROM	TransDetail 
				WHERE	document_id = TD.document_id
				AND	spare = 'COMM ADJ'
			)
		)

-- MJ get the premium as posted against the first client
-- MJ Will need to change this to get the total posted against all clients for shared premiums
-- MJ or break this down to multiple lines for each client
-- MJ changed to do the former for now
	update report_audit_debit_table3
	set this_premium =
	ISNULL ( 
	(	select sum(radt2.amount)
		FROM	TransDetail		TD
		JOIN 	report_audit_debit_table2			radt2
			on radt2.transdetail_id = TD.transdetail_id
		JOIN report_audit_debit_table3				radt3	
			on radt3.account_id = radt2.account_id
		where radt3.transdetail_id = @document_id	
		AND TD.document_sequence =
		(	select min(document_sequence)
			from TransDetail		TD1
			JOIN Account 			A1
				ON A1.Account_id = TD1.Account_id
			where TD1.document_id = TD.document_id 
			and A1.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA') --PSL16/06/2003 Remove Hard coded ledger_id
		)
--ECK 29/07/2002 Extra
		AND NOT EXISTS
		(	select transdetail_id
			from TransDetail		TD1
			JOIN Account 		A1
				ON A1.Account_id = TD1.Account_id
			where TD1.document_id = @document_id
			and A1.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB') --PSL16/06/2003 Remove Hard coded ledger_id
		) 
	)
	,0.00)
	where transdetail_id = @document_id	

-- get The First insurer premium
	update report_audit_debit_table3
	set 	insurer_premium_short_code = A3.short_code,
		insurer_premium = 
		(	select sum(radt.amount)
			from report_audit_debit_table2				radt
			where A3.Account_id = radt.Account_id
		)
	FROM   Account 				A3
	where A3.account_id = 
	(	Select TD.account_id 
		FROM	TransDetail		TD
		JOIN 	report_audit_debit_table2			radt2
			on radt2.transdetail_id = TD.transdetail_id
		where TD.document_sequence = 
		(	select min(document_sequence)
			from TransDetail		TD1
			JOIN Account 		A1
				ON A1.Account_id = TD1.Account_id
			where TD.document_id = TD1.document_id
			and A1.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN') --PSL16/06/2003 Remove Hard coded ledger_id
		)
	)
	and transdetail_id = @document_id
--	group by A3.short_code


-- There can be multiple specially where an extra has been applied.  At this point there will 
-- only be the one line in table 3 per transactions additional inurers require additional 
-- lines to hold the required values hence the if statement.
-- Again this may not be correct for shared premuims

	IF	(	SELECT	COUNT(DISTINCT account_id) 
			FROM 	report_audit_debit_table2
			WHERE 	ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN') --PSL16/06/2003 Remove Hard coded ledger_id
		) > 1 
	BEGIN
		insert into report_audit_debit_table3
		(	year_name,
			period_name,
			transdetail_id,
			accounting_date,
			insurance_ref,
			comment,
			short_code,
			Document_ref,
			document_type,
			party_type,
			type_description,
			risk_code,
			risk_description,
			cover_start_date,
			insurer_premium,
			insurer_premium_short_code,
			account_handler,
			Company_id,
			Company_desc,
			resolved_name,
			document_date,
			account_id
		)
		select 	radt3.year_name,
			radt3.period_name,
			radt3.transdetail_id,
			radt3.accounting_date,
			radt3.insurance_ref,
			radt3.comment,
			radt3.short_code,
			radt3.Document_ref,
			radt3.document_type,
			radt3.party_type,
			radt3.type_description,
			radt3.risk_code,
			radt3.risk_description,
			radt3.cover_start_date,
			sum(radt2.amount) as 'insurer_premium',
			A1.short_code as 'insurer_premium_short_code',
			radt3.account_handler,
			radt3.Company_id,
			radt3.Company_desc,
			radt3.resolved_name,
			radt3.document_date,
			radt3.account_id
		from 	report_audit_debit_table3			radt3,
				report_audit_debit_table2			radt2
		JOIN	Account 			A1
			ON A1.Account_id = radt2.Account_id
		WHERE	radt3.transdetail_id = @document_id
		and 	A1.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN') --PSL16/06/2003 Remove Hard coded ledger_id
		and A1.short_code not in
		(	select insurer_premium_short_code
			from report_audit_debit_table3
			where transdetail_id = @document_id
		)
		group by radt3.year_name,
			radt3.period_name,
			radt3.transdetail_id,
			radt3.accounting_date,
			radt3.insurance_ref,
			radt3.comment,
			radt3.short_code,
			radt3.Document_ref,
			radt3.document_type,
			radt3.party_type,
			radt3.type_description,
			radt3.risk_code,
			radt3.risk_description,
			radt3.cover_start_date,
			A1.short_code,
			radt3.account_handler,
			radt3.Company_id,
			radt3.Company_desc,
			radt3.resolved_name,
			radt3.document_date,
			radt3.account_id

	END

-- Set Commission Amounts as posted against the insurer
--	These commission amounts are posted aagainst the insurer so no additional lines
--	will be required as they have been added above

	UPDATE	radt3
	SET	radt3.commission_amount = -TD.amount,
		radt3.insurer_commission_short_code = A.short_code
	FROM	report_audit_debit_table3			radt3
	JOIN	Account 					A
		ON radt3.insurer_premium_short_code = A.short_code
	JOIN	TransDetail					TD
		ON radt3.transdetail_id = TD.document_id
		AND A.account_id = TD.account_id
	where radt3.transdetail_id = @document_id
-- DJM 16/07/2002 : Don't use sequence number as it doesn't work if an adjustment has been done.
-- MJ Either of these should be valid but I'm not too confident in the spare field
	and TD.spare like '%COMM'
--	and TD.document_sequence =
--		(	select max(document_sequence)
--			from 	orion_for_broking.dbo.TransDetail
--			where  account_id = TD.account_id
--			and document_id = TD.document_id
--			group by account_id
--		)

-- there can be multiple agents so we may nee additional lines in report_audit_debit_table3
-- the while statement will ad a line to table 3 for each agent greater than the number of insurers.
-- henec the check against the number of lines already present.  The same logic is used below for other items.
	while	(	SELECT	COUNT(DISTINCT account_id) 
			FROM 	report_audit_debit_table2
			WHERE 	ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'AG') --PSL16/06/2003 Remove Hard coded ledger_id
		) > 
	 	(	SELECT	sum(1) 
			FROM 	report_audit_debit_table3
			WHERE 	transdetail_id = @document_id
		)
	BEGIN
		insert into report_audit_debit_table3
		(	year_name,
			period_name,
			transdetail_id,
			accounting_date,
			insurance_ref,
			comment,
			short_code,
			Document_ref,
			document_type,
			party_type,
			type_description,
			risk_code,
			risk_description,
			cover_start_date,
			account_handler,
			Company_id,
			Company_desc,
			resolved_name,
			document_date,
			account_id
		)
		select 	radt3.year_name,
			radt3.period_name,
			radt3.transdetail_id,
			radt3.accounting_date,
			radt3.insurance_ref,
			radt3.comment,
			radt3.short_code,
			radt3.Document_ref,
			radt3.document_type,
			radt3.party_type,
			radt3.type_description,
			radt3.risk_code,
			radt3.risk_description,
			radt3.cover_start_date,
			radt3.account_handler,
			radt3.Company_id,
			radt3.Company_desc,
			radt3.resolved_name,
			radt3.document_date,
			radt3.account_id
		from 	report_audit_debit_table3			radt3
		WHERE	radt3.transdetail_id = @document_id
		and radt3.main_key = 
		(	select min(main_key)
			from 	report_audit_debit_table3			
			WHERE	transdetail_id = radt3.transdetail_id 
		)
		group by radt3.year_name,
			radt3.period_name,
			radt3.transdetail_id,
			radt3.accounting_date,
			radt3.insurance_ref,
			radt3.comment,
			radt3.short_code,
			radt3.Document_ref,
			radt3.document_type,
			radt3.party_type,
			radt3.type_description,
			radt3.risk_code,
			radt3.risk_description,
			radt3.cover_start_date,
			radt3.account_handler,
			radt3.Company_id,
			radt3.Company_desc,
			radt3.resolved_name,
			radt3.document_date,
			radt3.account_id

	END

-- Set any agent amounts
-- having added any additional lines that may be required we must set the amounts and short_codes for the agnets
-- again a loop is used so that numbers are not duplicated where fewer agents exist than lines in table 3

	while	(	SELECT	COUNT(DISTINCT account_id) 
			FROM 	report_audit_debit_table2
			WHERE 	ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'AG') --PSL16/06/2003 Remove Hard coded ledger_id
		) >
 		(	SELECT	count(main_key)
			FROM 	report_audit_debit_table3
			WHERE 	transdetail_id = @document_id
			and agent_fees_short_code is not null
		)
	begin
	
		update radt3
		set 	radt3.agent_fees_short_code = A.short_code,
			radt3.agent_fees = 
			(	select sum(amount)
				FROM	report_audit_debit_table2
				where	a.account_id = account_id
			)
		FROM	report_audit_debit_table2			radt2
		JOIN	TransDetail		TD
			ON radt2.transdetail_id = TD.transdetail_id
		JOIN	Account 			A
			ON A.account_id = TD.account_id
		JOIN	report_audit_debit_table3			radt3
			ON radt3.transdetail_id = TD.document_id
		where A.account_id =
			(	select min(radt2.account_id)
				from report_audit_debit_table2		radt2
				join Account 	A2
					on radt2.account_id = A2.account_id
				where radt2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'AG') --PSL16/06/2003 Remove Hard coded ledger_id
				and A2.short_code not in
				(	select ISNULL(agent_fees_short_code, '')
					from report_audit_debit_table3	
					where transdetail_id = radt3.transdetail_id
				)
			)
		and radt3.main_key =
			(	select min(main_key)
				from report_audit_debit_table3	
				where transdetail_id = radt3.transdetail_id
				and agent_fees_short_code is null
			)
		and radt3.transdetail_id = @document_id
	end

-- there can be multiple sub agents so we may nee additional lines in report_audit_debit_table3
-- you can not post to more than one sub-agent but as you can change the agent type between agent
-- and sub-agent a transaction could seem to have more than one sub-agent on it
-- the same logic as above is use to populate the table with info 
	while	(	SELECT	COUNT(DISTINCT account_id) 
			FROM 	report_audit_debit_table2
			WHERE 	ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB') --PSL16/06/2003 Remove Hard coded ledger_id
		) > 
	 	(	SELECT	sum(1) 
			FROM 	report_audit_debit_table3
			WHERE 	transdetail_id = @document_id
		)
	BEGIN
		insert into report_audit_debit_table3
		(	year_name,
			period_name,
			transdetail_id,
			accounting_date,
			insurance_ref,
			comment,
			short_code,
			Document_ref,
			document_type,
			party_type,
			type_description,
			risk_code,
			risk_description,
			cover_start_date,
			account_handler,
			Company_id,
			Company_desc,
			resolved_name,
			document_date,
			account_id
		)
		select 	radt3.year_name,
			radt3.period_name,
			radt3.transdetail_id,
			radt3.accounting_date,
			radt3.insurance_ref,
			radt3.comment,
			radt3.short_code,
			radt3.Document_ref,
			radt3.document_type,
			radt3.party_type,
			radt3.type_description,
			radt3.risk_code,
			radt3.risk_description,
			radt3.cover_start_date,
			radt3.account_handler,
			radt3.Company_id,
			radt3.Company_desc,
			radt3.resolved_name,
			radt3.document_date,
			radt3.account_id
		from 	report_audit_debit_table3			radt3
		WHERE	radt3.transdetail_id = @document_id
		and radt3.main_key = 
		(	select min(main_key)
			from 	report_audit_debit_table3			
			WHERE	transdetail_id = radt3.transdetail_id 
		)
		group by radt3.year_name,
			radt3.period_name,
			radt3.transdetail_id,
			radt3.accounting_date,
			radt3.insurance_ref,
			radt3.comment,
			radt3.short_code,
			radt3.Document_ref,
			radt3.document_type,
			radt3.party_type,
			radt3.type_description,
			radt3.risk_code,
			radt3.risk_description,
			radt3.cover_start_date,
			radt3.account_handler,
			radt3.Company_id,
			radt3.Company_desc,
			radt3.resolved_name,
			radt3.document_date,
			radt3.account_id
	END

-- Set any sub-agent amounts
-- the same logic as the agent is used to populate the table with info
	while	(	SELECT	COUNT(DISTINCT account_id) 
			FROM 	report_audit_debit_table2
			WHERE 	ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB') --PSL16/06/2003 Remove Hard coded ledger_id
		) >
 		(	SELECT	count(main_key)
			FROM 	report_audit_debit_table3
			WHERE 	transdetail_id = @document_id
			and sub_agent_short_code is not null
		)
	begin
		update radt3
		set 	radt3.sub_agent_short_code = A.short_code,
			radt3.sub_agent_fees = 
			(	select sum(amount)
				FROM	report_audit_debit_table2
				where	a.account_id = account_id
			)
		FROM	report_audit_debit_table2			radt2
		JOIN	TransDetail		TD
			ON radt2.transdetail_id = TD.transdetail_id
		JOIN	Account 			A
			ON A.account_id = TD.account_id
		JOIN	report_audit_debit_table3			radt3
			ON radt3.transdetail_id = TD.document_id
		where A.account_id =
			(	select min(radt2.account_id)
				from report_audit_debit_table2		radt2
				join Account 				A2
					on radt2.account_id = A2.account_id
				where radt2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'UB') --PSL16/06/2003 Remove Hard coded ledger_id
				and A2.short_code not in
				(	select ISNULL(sub_agent_short_code, '')
					from report_audit_debit_table3	
					where transdetail_id = radt3.transdetail_id
				)
			)
		and radt3.main_key =
			(	select min(main_key)
				from report_audit_debit_table3	
				where transdetail_id = radt3.transdetail_id
				and sub_agent_short_code is null
			)
		and radt3.transdetail_id = @document_id
	end

-- there can be multiple fees so we may need additional lines in report_audit_debit_table3
-- again same set of logic as above
	while	(	SELECT	COUNT(DISTINCT account_id) 
			FROM 	report_audit_debit_table2
			WHERE 	ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'FE') --PSL16/06/2003 Remove Hard coded ledger_id
		) > 
	 	(	SELECT	sum(1) 
			FROM 	report_audit_debit_table3
			WHERE 	transdetail_id = @document_id
		)
	BEGIN
		insert into report_audit_debit_table3
		(	year_name,
			period_name,
			transdetail_id,
			accounting_date,
			insurance_ref,
			comment,
			short_code,
			Document_ref,
			document_type,
			party_type,
			type_description,
			risk_code,
			risk_description,
			cover_start_date,
			account_handler,
			Company_id,
			Company_desc,
			resolved_name,
			document_date,
			account_id
		)
		select 	radt3.year_name,
			radt3.period_name,
			radt3.transdetail_id,
			radt3.accounting_date,
			radt3.insurance_ref,
			radt3.comment,
			radt3.short_code,
			radt3.Document_ref,
			radt3.document_type,
			radt3.party_type,
			radt3.type_description,
			radt3.risk_code,
			radt3.risk_description,
			radt3.cover_start_date,
			radt3.account_handler,
			radt3.Company_id,
			radt3.Company_desc,
			radt3.resolved_name,
			radt3.document_date,
			radt3.account_id
		from 	report_audit_debit_table3			radt3
		WHERE	radt3.transdetail_id = @document_id
		and radt3.main_key = 
		(	select min(main_key)
			from 	report_audit_debit_table3			
			WHERE	transdetail_id = radt3.transdetail_id 
		)
		group by radt3.year_name,
			radt3.period_name,
			radt3.transdetail_id,
			radt3.accounting_date,
			radt3.insurance_ref,
			radt3.comment,
			radt3.short_code,
			radt3.Document_ref,
			radt3.document_type,
			radt3.party_type,
			radt3.type_description,
			radt3.risk_code,
			radt3.risk_description,
			radt3.cover_start_date,
			radt3.account_handler,
			radt3.Company_id,
			radt3.Company_desc,
			radt3.resolved_name,
			radt3.document_date,
			radt3.account_id
	END

-- Set any fee amounts
-- again same set of logic as above
	while	(	SELECT	COUNT(DISTINCT account_id) 
			FROM 	report_audit_debit_table2
			WHERE 	ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'FE') --PSL16/06/2003 Remove Hard coded ledger_id
		) >
 		(	SELECT	count(main_key)
			FROM 	report_audit_debit_table3
			WHERE 	transdetail_id = @document_id
			and fees_short_code is not null
		)
	begin
		update radt3
		set 	radt3.fees_short_code = A.short_code,
			radt3.fees = 
			(	select sum(amount)
				FROM	report_audit_debit_table2
				where	a.account_id = account_id
			)
		FROM	report_audit_debit_table2			radt2
		JOIN	TransDetail		TD
			ON radt2.transdetail_id = TD.transdetail_id
		JOIN	Account 			A
			ON A.account_id = TD.account_id
		JOIN	report_audit_debit_table3			radt3
			ON radt3.transdetail_id = TD.document_id
		where A.account_id =
			(	select min(radt2.account_id)
				from report_audit_debit_table2		radt2
				join  Account 	A2
					on radt2.account_id = A2.account_id
				where radt2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'FE') --PSL16/06/2003 Remove Hard coded ledger_id
				and A2.short_code not in
				(	select ISNULL(fees_short_code, '')
					from report_audit_debit_table3	
					where transdetail_id = radt3.transdetail_id
				)
			)
		and radt3.main_key =
			(	select min(main_key)
				from report_audit_debit_table3	
				where transdetail_id = radt3.transdetail_id
				and fees_short_code is null
			)
		and radt3.transdetail_id = @document_id
	end

-- there can be multiple disounts so we may need additional lines in report_audit_debit_table3
-- again same set of logic as above
	while	(	SELECT	COUNT(DISTINCT account_id) 
			FROM 	report_audit_debit_table2
			WHERE 	ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'DI') --PSL16/06/2003 Remove Hard coded ledger_id
		) > 
	 	(	SELECT	sum(1) 
			FROM 	report_audit_debit_table3
			WHERE 	transdetail_id = @document_id
		)
	BEGIN
		insert into report_audit_debit_table3
		(	year_name,
			period_name,
			transdetail_id,
			accounting_date,
			insurance_ref,
			comment,
			short_code,
			Document_ref,
			document_type,
			party_type,
			type_description,
			risk_code,
			risk_description,
			cover_start_date,
			account_handler,
			Company_id,
			Company_desc,
			resolved_name,
			document_date,
			account_id
		)
		select 	radt3.year_name,
			radt3.period_name,
			radt3.transdetail_id,
			radt3.accounting_date,
			radt3.insurance_ref,
			radt3.comment,
			radt3.short_code,
			radt3.Document_ref,
			radt3.document_type,
			radt3.party_type,
			radt3.type_description,
			radt3.risk_code,
			radt3.risk_description,
			radt3.cover_start_date,
			radt3.account_handler,
			radt3.Company_id,
			radt3.Company_desc,
			radt3.resolved_name,
			radt3.document_date,
			radt3.account_id
		from 	report_audit_debit_table3			radt3
		WHERE	radt3.transdetail_id = @document_id
		and radt3.main_key = 
		(	select min(main_key)
			from 	report_audit_debit_table3			
			WHERE	transdetail_id = radt3.transdetail_id 
		)
		group by radt3.year_name,
			radt3.period_name,
			radt3.transdetail_id,
			radt3.accounting_date,
			radt3.insurance_ref,
			radt3.comment,
			radt3.short_code,
			radt3.Document_ref,
			radt3.document_type,
			radt3.party_type,
			radt3.type_description,
			radt3.risk_code,
			radt3.risk_description,
			radt3.cover_start_date,
			radt3.account_handler,
			radt3.Company_id,
			radt3.Company_desc,
			radt3.resolved_name,
			radt3.document_date,
			radt3.account_id
	END

-- Set any discounts amounts
-- again same set of logic as above
	while	(	SELECT	COUNT(DISTINCT account_id) 
			FROM 	report_audit_debit_table2
			WHERE 	ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'DI') --PSL16/06/2003 Remove Hard coded ledger_id
		) >
 		(	SELECT	count(main_key)
			FROM 	report_audit_debit_table3
			WHERE 	transdetail_id = @document_id
			and disc_short_code is not null
		)
	begin
		update radt3
		set 	radt3.disc_short_code = A.short_code,
			radt3.disc = 
			(	select sum(amount)
				FROM	report_audit_debit_table2
				where	a.account_id = account_id
			)
		FROM	report_audit_debit_table2			radt2
		JOIN	TransDetail		TD
			ON radt2.transdetail_id = TD.transdetail_id
		JOIN	Account 			A
			ON A.account_id = TD.account_id
		JOIN	report_audit_debit_table3			radt3
			ON radt3.transdetail_id = TD.document_id
		where A.account_id =
			(	select min(radt2.account_id)
				from report_audit_debit_table2		radt2
				join Account 	A2
					on radt2.account_id = A2.account_id
				where radt2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'DI') --PSL16/06/2003 Remove Hard coded ledger_id
				and A2.short_code not in
				(	select ISNULL(disc_short_code, '')
					from report_audit_debit_table3	
					where transdetail_id = radt3.transdetail_id
				)
			)
		and radt3.main_key =
			(	select min(main_key)
				from report_audit_debit_table3	
				where transdetail_id = radt3.transdetail_id
				and disc_short_code is null
			)
		and radt3.transdetail_id = @document_id
	end

-- Set the net commission amounts
-- only ever a single transaction?
	update radt3
	set 	radt3.net_commission_short_code = A.short_code,
		radt3.net_commission = 
		(	select sum(amount)
			FROM	report_audit_debit_table2
			where	a.account_id = account_id
		)
	FROM	report_audit_debit_table2			radt2
	JOIN	TransDetail					TD
		ON radt2.transdetail_id = TD.transdetail_id
	JOIN	Account 					A
		ON A.account_id = TD.account_id
	JOIN	report_audit_debit_table3			radt3
		ON radt3.transdetail_id = TD.document_id
	where radt2.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'CO') --PSL16/06/2003 Remove Hard coded ledger_id
	and radt3.main_key =
		(	select min(main_key)
			from report_audit_debit_table3	
			where transdetail_id = radt3.transdetail_id
		)
	and radt3.transdetail_id = @document_id

	FETCH NEXT FROM c_Cursor INTO @document_id

END

close c_Cursor
deallocate c_Cursor

set nocount off

select *
from report_audit_debit_table3
order by company_id, document_ref, main_key

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO