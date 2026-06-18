SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_Report_AlexanderForbes_Business_Trans_By_Area'
GO

/*if exists (select * from sysobjects where id = object_id(N'[dbo].[sp_Report_AlexanderForbes_Business_Trans_By_Area]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_AlexanderForbes_Business_Trans_By_Area]
*/--GO

/*** eck181102 Modification for new style report ***/
CREATE PROCEDURE spu_Report_AlexanderForbes_Business_Trans_By_Area
	@branch_id	int,
	@start_date	datetime,
	@end_date	datetime,
	@Area		varchar(255)
AS 

DECLARE		
	@iBranchID	int,
	@iAreaID	int

SELECT	
	@iBranchID = ISNULL(@branch_id, 0)

SELECT
	@iAreaID = MIN(Area_Id) FROM Area WHERE Description = @Area


-- Generate Temporary Table
CREATE TABLE    #Report_Transaction
(
	document_date			datetime,
        document_ref			varchar(25),
	document_type			varchar(255),
	document_id			int,
	trans_code			char(10),
	ac_id				int,
	client				varchar(60),
	client_code			char(30),
	policy_no			varchar(30),
	renewal_date			datetime,
	subagent			char(20),
	Insurer				char(60),
	area_id				int,
	area_description		varchar(255),
	risk_description		varchar(255),
	gross_premium			numeric(9,2),
	net_commission			numeric(9,2),
	agent_commission		numeric(9,2),
	this_agent_commission		numeric(9,2),
	this_subagent_commission	numeric(9,2)
)

-- Get the required transactions
INSERT INTO #Report_Transaction
(
	document_date,
        document_ref,
	document_type,
	document_id,
	trans_code,
	ac_id,
	policy_no,
	renewal_date,
	subagent,
	risk_description,
	gross_premium,
	net_commission,
	agent_commission,
	this_agent_commission,
	this_subagent_commission
)

SELECT	DISTINCT D.document_date			Transaction_Date,
	D.document_ref          	Document_ref,
	T.description			Document_Type,
	TD.document_id			Document_id,
	T.code				Trans_Code,
	TD.Account_Id			account_id,
	TD.insurance_ref		Policy_No,
	I.Expiry_Date			Renewal_date,
	A.short_code			SubAgent,
	RK.Description			Risk_Description,
	ISNULL((	 SELECT	SUM(TD1.amount)		
			FROM	Transdetail	TD1
			WHERE	TD1.document_id = D.document_id
			AND	TD1.document_sequence = 
			(	select min(TD2.document_sequence)
				FROM	Transdetail	TD2
				JOIN 	Account		A
					on A.account_id = TD2.account_id
				where	TD1.document_id = TD2.document_id
				and a.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
			)
		) , 0) Gross_Premium,
 
		(
		ISNULL(( SELECT SUM(amount) * -1			-- Amount posted to potential comm
			FROM	Transdetail	TD1
			JOIN 	Account		A
				on A.account_id = TD1.account_id
			JOIN	Ledger		L
				on A.Ledger_id = L.Ledger_id
			where	TD1.document_id = D.document_id
--			and a.ledger_id = 9
			and L.LedgerType_Id = 9
			) , 0)  
		+
		ISNULL(( SELECT	SUM(amount)  					-- Amount posted to earned 
			FROM	Transdetail	TD1		-- via commission adjustments
			JOIN 	Account		A
				on A.account_id = TD1.account_id
			where	TD1.document_id = D.document_id
			and TD1.spare = 'COMM ADJ' 
			) , 0)
		+
		ISNULL(( SELECT	SUM(amount) * -1 				-- Amount posted to earned 
			FROM	Transdetail	TD1		-- via agent adjustments
			JOIN 	Account		A
				on A.account_id = TD1.account_id
			JOIN	Ledger		L
				on A.Ledger_id = L.Ledger_id
			where	TD1.document_id = D.document_id
			and TD1.spare = 'AGENT ADJ' 
--			and A.ledger_id = 1
			and L.LedgerType_id = 1
		) , 0)
		) Net_Commission,

		ISNULL(( SELECT SUM(amount) * -1					-- Agent Payaways
			FROM	Transdetail	TD1
			JOIN 	Account		A
				on A.account_id = TD1.account_id
			JOIN	Ledger		L
				on A.Ledger_id = L.Ledger_id
			where	TD1.document_id = D.document_id
--			and a.ledger_id = 5
			and L.LedgerType_id = 5
			) , 0) 

			+
			(
			ISNULL(( SELECT  amount 						-- SubAgent Payaways
				FROM	Transdetail	TD1, Account A2
 				where	TD1.document_id = D.document_id
				and TD1.document_sequence = 1
				and TD1.account_id = A2.Account_id			--PN4770
				and 
				(	select  sum(td2.amount) from transdetail td2	--PN4770
						where td2.account_id = a2.account_id
							and  td2.document_id = D.document_id) = 0 	--PN4700
				) 
				+
			 	( SELECT SUM(amount) * -1					 
				FROM	Transdetail	TD1
				JOIN 	Account		A
					on A.account_id = TD1.account_id
				JOIN	Ledger		L
					on A.Ledger_id = L.Ledger_id
				where	TD1.document_id = D.document_id
--				and a.ledger_id = 10
				and L.LedgerType_id = 10
				) , 0)
			)
			Agent_Commission,
 
		ISNULL(	(	SELECT	SUM(amount) * -1	
			FROM	Transdetail	TD1
			JOIN 	Account		A1
				on A1.account_id = TD1.account_id
			JOIN	Ledger		L
				on A.Ledger_id = L.Ledger_id
			where	TD1.document_id = D.document_id
			and A1.account_id = A.account_id
--			and A.ledger_id = 5
			and L.LedgerType_id = 5
		) , 0) This_Agent_Commission,

		(
			ISNULL(( SELECT  amount 						 
				FROM	Transdetail	TD1, Account A2
 				where	TD1.document_id = D.document_id
				and TD1.document_sequence = 1
				and TD1.account_id = A2.Account_id			--PN4770
				and 
				(	select  sum(td2.amount) from transdetail td2	--PN4770
						where td2.account_id = a2.account_id
							and  td2.document_id = D.document_id) = 0 	--PN4700
				)  
				+
			 	( SELECT SUM(amount) * -1					 
				FROM	Transdetail	TD1
				JOIN 	Account		A1
					on A1.account_id = TD1.account_id
				JOIN	Ledger		L
					on A.Ledger_id = L.Ledger_id
				where	TD1.document_id = D.document_id
				AND A1.account_id = A.account_id
--				and a.ledger_id = 10
				and L.LedgerType_id = 10
				) , 0)
			)
			This_SubAgent_Commission


FROM	Document			D
JOIN	Transdetail		TD
	ON TD.document_id = D.document_id
JOIN	DocumentType		T
	ON T.documenttype_id = D.documenttype_id
JOIN	Account			A
	ON A.account_id = TD.account_id
JOIN	Ledger			L
	ON A.Ledger_id = L.Ledger_id
JOIN	Insurance_file					I
	ON TD.Insurance_ref = I.Insurance_ref
LEFT OUTER JOIN	Insurance_File_Risk_Link			RL
	ON I.Insurance_file_cnt = RL.Insurance_File_cnt
LEFT OUTER JOIN	Risk						RK
	ON RL.Risk_cnt = RK.Risk_cnt
WHERE	
	
	(	D.document_date >= @start_date
		AND 
		D.document_date <= @end_date
	)
AND	(	I.Cover_Start_Date >= @start_date
		AND
		I.Cover_Start_Date <= @end_date
	)

AND 	D.documenttype_id IN(4, 5, 15, 16, 17, 18, 31, 32, 35, 36)
AND	(	@iBranchID = 0
		OR
		(	@iBranchID <> 0
			AND
			D.company_id = @iBranchID	
		)
	)
--AND	A.Ledger_Id = 2
AND L.LedgerType_id = 2
	
-- Set client details	
UPDATE 	#Report_Transaction
SET	client = A1.account_name,
	client_code = A1.short_code,
	area_id = ISNULL(AR.area_id,0),
	area_description = AR.Description
FROM	#Report_Transaction			RT
JOIN	TransDetail 	TD1
	ON TD1.document_id = RT.document_id
JOIN	Account		A1
	ON A1.account_id = TD1.Account_Id
JOIN	Ledger		L
	ON A1.Ledger_id = L.Ledger_id
LEFT OUTER JOIN	Party				P
	ON A1.short_code = P.shortname
LEFT OUTER JOIN	Area				AR
	ON P.area_id = AR.area_id
--WHERE	a1.ledger_id=2
WHERE L.LedgerType_id = 2

DELETE FROM	#Report_Transaction
WHERE	area_id <> @iAreaID
	OR area_id = NULL	

-- Set insurer details
UPDATE 	#Report_Transaction
SET	insurer = A1.account_name
FROM	#Report_Transaction	RT
JOIN	TransDetail TD1
	ON TD1.document_id = RT.document_id
JOIN	Account	A1
	ON A1.account_id = TD1.Account_Id
JOIN	Ledger		L
	ON A1.Ledger_id = L.Ledger_id
--WHERE	a1.ledger_id=4
WHERE 	L.LedgerType_Id = 4



-- Extract the data
SET NOCOUNT OFF
SELECT DISTINCT * FROM	#Report_Transaction
ORDER BY client_code, document_id
SET NOCOUNT ON

DELETE FROM	#Report_Transaction
DROP TABLE	#Report_Transaction

SET NOCOUNT OFF


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


