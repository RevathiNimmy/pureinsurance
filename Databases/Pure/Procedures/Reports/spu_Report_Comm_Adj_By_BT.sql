SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Report_Comm_Adj_By_BT
GO

CREATE PROCEDURE spu_Report_Comm_Adj_By_BT
(
	@branch_id	INT,
	@start_date	DATETIME,
	@end_date	DATETIME,
	@party_code	VARCHAR(20),
	@date_type VARCHAR(50),
	@group_by VARCHAR(50),
        @then_by VARCHAR(50)
)
AS

DECLARE	@iBranchID	int,
	@document_ref   varchar(25),
	@document_id	int,
	@transdetail_id int,
	@account_id	int,
	@account_code	varchar(30),
	@account_name	varchar(60),
	@gross_comm	numeric(19,3),
	@opening_bal	numeric(19,3),
	@company_id	int,
	@effective_date	datetime,
	@policy_number  varchar(30),
	@business_type_code varchar(20),
	@business_type_desc varchar(50),
	@insurer_code	varchar(20),
        @agencyorunderwriting varchar(1),
	@business_type varchar(20)

SELECT	@iBranchID = ISNULL(@branch_id, 0)
SELECT  @party_code = ISNULL(@party_code, 'ALL')
SELECT  @business_type =  'ALL'
SELECT  @agencyorunderwriting = ( select value from hidden_options where option_number = 1 )

IF @then_by=@group_by OR @then_by=''
    SET @then_by ='None'

CREATE TABLE #Comm_temp
(
    transdetail_id 		INT,
    document_id 		INT,
    account_id			INT,
    branch_id			INT,
    business_type_code		VARCHAR(10),
    business_type_desc		VARCHAR(255),
    client_code			VARCHAR(20),
    client			VARCHAR(255),
    insurer_code		VARCHAR(20),
    insurer_name		VARCHAR(255),
    policy_number		VARCHAR(30),
    client_ref_date		DATETIME,
    effective_date		DATETIME,
    document_ref		VARCHAR(30),
    document_date		DATETIME,
    gross_commission		NUMERIC(19,2),
    agent_code			VARCHAR(20),
    gross_value			NUMERIC(19,2),
    agent_commissions		NUMERIC(19,2),
    agencyorunderwriting	VARCHAR(20)
)

INSERT INTO #Comm_temp
    (
     transdetail_id 	,
     document_id 	,
     account_id		,
     branch_id		,
     business_type_code	,
     business_type_desc	,
     client_code	,
     client		,
     insurer_code	,
     insurer_name	,
     policy_number	,
     client_ref_date	,
     effective_date	,
     document_ref	,
     document_date	,
     gross_commission	,
     agent_code		,
     gross_value	,
     agent_commissions	,
     agencyorunderwriting
    )

    SELECT
	T.Transdetail_id ,
	T.Document_id,
    	T.account_id,
	D.company_id,
	BT.code ,
	BT.description ,
	CP.shortname ,
	CP.name ,
	IP.shortname ,
	IP.resolved_name ,
	TEF.insurance_ref ,
	T2.ref_date ,
	T.accounting_date ,
	D.document_ref ,
	D.document_date ,
	ROUND(T.amount,2) * -1 ,
	ISNULL	(
		(
		SELECT AA.short_code
		FROM Account AA
		WHERE AA.account_id = T.account_id
		AND AA.ledger_id in (Select ledger_id from ledger where ledger_short_name IN ('AG', 'TR'))
		AND T.spare = 'AGENT ADJ')
		,'') ,
	0 ,
	0 ,
	@AgencyOrUnderwriting
    FROM Transdetail T
	JOIN document D
		ON T.document_id = D.document_id
	JOIN Transdetail T2
		ON D.document_id = T2.document_id
	JOIN Account 	A
		ON T2.account_id = A.account_id
	JOIN Party CP
		ON A.Account_key = CP.party_cnt
	JOIN Transaction_Export_Folder TEF
		ON D.document_ref = TEF.document_ref
	JOIN Insurance_File IFI
		ON TEF.insurance_file_cnt = IFI.insurance_file_cnt
		AND TEF.source_id = T.company_id
	JOIN Business_Type BT
		ON BT.business_type_id = IFI.business_type_id
	JOIN Party IP
		ON IFI.lead_insurer_cnt = IP.party_cnt
	WHERE
	 (
		T.document_sequence  IN
			(
				SELECT document_sequence
				FROM transdetail TI
				JOIN account AI
					ON AI.account_id = TI.account_id
				WHERE TI.document_id = D.document_id
				AND	TI.spare = 'COMM ADJ'
				AND AI.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name IN ('IN'))
			)
 		OR
		T.document_sequence IN
			(
				SELECT document_sequence
				FROM transdetail TDA
				JOIN account AA
					ON AA.account_id = TDA.account_id
				WHERE TDA.document_id = D.document_id
				AND TDA.spare = 'AGENT ADJ'
				AND AA.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name IN ('AG','TR'))

			)
	)
	AND	T2.transdetail_id =
		(
			SELECT MIN(transdetail_id)
			FROM Transdetail T3
			JOIN Account A3
				ON A3.account_id = T3.account_id
			WHERE T3.document_id = D.document_id
			AND A3.ledger_id IN (SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA')
		)
	AND
	(
		(
			@date_type = 'Adj. Transaction'
			AND
			T.accounting_date BETWEEN @start_date AND @end_date
		)
		OR
		(
			@date_type = 'Policy Effective'
			AND
			T2.ref_date BETWEEN @start_date AND @end_date
		)
	)
	AND
	(
		@iBranchID = 0
		OR
		(
			@iBranchID <> 0
			AND
			d.company_id = @iBranchID
		)
	)
	AND D.documenttype_id IN (4, 5, 15, 16, 17, 18, 31, 32, 35, 36)
	AND
	(
		@business_type = 'ALL'
		OR
		IFI.business_type_id =
			(
				SELECT business_type.business_type_id
				FROM business_type
				WHERE description = @business_type
			)
	)
	AND
	(
		@party_code = 'ALL'
		OR
		IFI.lead_insurer_cnt =
			(
				SELECT party.party_cnt
				FROM party
				WHERE shortname = @party_code
			)
	)

SELECT
    CT.*,
    (
	CASE @group_by
            WHEN 'None' THEN
		''
            WHEN 'Branch' THEN
		(
		SELECT ISNULL(S.code,'')
		FROM Source S
		where S.source_id=CT.branch_id
		)
            WHEN 'Account Executive' THEN
		ISNULL	((
		SELECT ISNULL(P_AccExec.shortname, '')
		FROM Account A
		JOIN party P_Client
		ON P_Client.party_cnt = A.account_key
		JOIN party P_AccExec
		ON P_AccExec.party_cnt = P_Client.consultant_cnt
		WHERE A.account_id in (select Account_id from transdetail where document_id=CT.document_id and document_sequence=1)
		),'')

            WHEN 'Account Handler' THEN
		ISNULL((
		SELECT ISNULL(AccH.shortname, '')
		FROM  Document D
		LEFT JOIN  insurance_file IFI
		ON IFI.insurance_file_cnt = D.insurance_file_cnt
		LEFT JOIN Party AccH
		ON AccH.party_cnt = IFI.account_handler_cnt
		WHERE D.document_id = CT.document_id
                ),'')

            WHEN 'Business Type' THEN
		(
		SELECT ISNULL(BT.description, '')
		FROM Document D
		LEFT JOIN  insurance_file IFI
		ON IFI.insurance_file_cnt = D.insurance_file_cnt
		LEFT JOIN business_type BT
		ON BT.business_type_id = IFI.business_type_id
		WHERE D.document_id = CT.document_id
		)
            WHEN 'Insurer' THEN
		CT.Insurer_code
            END
    ) group_code,
-----------------------------------------------------------------
-- Group Desc
-----------------------------------------------------------------
    (
        CASE @group_by
            WHEN 'None' THEN
		''
            WHEN 'Branch' THEN
		(
		SELECT ISNULL(S.description,'')
		FROM Source S
		where S.source_id=CT.branch_id
		)
            WHEN 'Account Executive' THEN
		ISNULL	((
			SELECT ISNULL(P_AccExec.resolved_name, '')
			FROM Account A
			JOIN party P_Client
			ON P_Client.party_cnt = A.account_key
			JOIN party P_AccExec
			ON P_AccExec.party_cnt = P_Client.consultant_cnt
			WHERE A.account_id in (select Account_id from transdetail where document_id=CT.document_id and document_sequence=1)
			),'')
            WHEN 'Account Handler' THEN
		ISNULL((
			SELECT ISNULL(AccH.resolved_name, '')
			FROM  Document D
			LEFT JOIN  insurance_file IFI
			ON IFI.insurance_file_cnt = D.insurance_file_cnt
			LEFT JOIN Party AccH
			ON AccH.party_cnt = IFI.account_handler_cnt
			WHERE D.document_id = CT.document_id
			),'')

            WHEN 'Business Type' THEN
		(
		SELECT ISNULL(BT.description, '')
		FROM Document D
		LEFT JOIN  insurance_file IFI
		ON IFI.insurance_file_cnt = D.insurance_file_cnt
		LEFT JOIN business_type BT
		ON BT.business_type_id = IFI.business_type_id
		WHERE D.document_id = CT.document_id
		)
		WHEN 'Insurer' THEN
		CT.insurer_name
        END
    ) group_desc,
--------------------------------------------------------------------
--Then Code
-----------------------------------------------------------------------
    (
        CASE @then_by
            WHEN 'None' THEN
		''
            WHEN 'Branch' THEN
		(
		SELECT ISNULL(S.code,'')
		FROM Source S
		WHERE S.source_id=CT.branch_id
		)

            WHEN 'Account Executive' THEN
		ISNULL((
			SELECT ISNULL(P_AccExec.shortname, '')
			FROM account A
			JOIN party P_Client
			ON P_Client.party_cnt = A.account_key
			JOIN party P_AccExec
			ON P_AccExec.party_cnt = P_Client.consultant_cnt
			WHERE A.account_id in (select Account_id from transdetail where document_id=CT.document_id and document_sequence=1)
			),'')

            WHEN 'Account Handler' THEN
		ISNULL((
			SELECT ISNULL(AccH.shortname, '')
			FROM  Document D
			LEFT JOIN  insurance_file IFI
			ON IFI.insurance_file_cnt = D.insurance_file_cnt
			LEFT JOIN Party AccH
			ON AccH.party_cnt = IFI.account_handler_cnt
			WHERE D.document_id = CT.document_id
			),'')

            WHEN 'Business Type' THEN
		(
		SELECT ISNULL(BT.description, '')
		FROM Document D
		LEFT JOIN  insurance_file IFI
		ON IFI.insurance_file_cnt = D.insurance_file_cnt
		LEFT JOIN business_type BT
		ON BT.business_type_id = IFI.business_type_id
		WHERE D.document_id = CT.document_id
		)
	   WHEN 'Insurer' THEN
		CT.Insurer_code
        END
    ) then_code,

--------------------------------------------------------------------
--Then Description
--------------------------------------------------------------------
    (
        CASE @then_by
            WHEN 'None' THEN
		''
            WHEN 'Branch' THEN
		(
		SELECT ISNULL(S.description,'')
		FROM Source S
		WHERE S.source_id=CT.branch_id
		)
            WHEN 'Account Executive' THEN
		ISNULL	((
			SELECT ISNULL(P_AccExec.resolved_name, '')
			FROM account A
			JOIN party P_Client
			ON P_Client.party_cnt = A.account_key
			JOIN party P_AccExec
			ON P_AccExec.party_cnt = P_Client.consultant_cnt
				WHERE A.account_id in (select Account_id from transdetail where document_id=CT.document_id and document_sequence=1)
			),'')

            WHEN 'Account Handler' THEN
            	ISNULL	((
                    	SELECT ISNULL(AccH.resolved_name, '')
			FROM  Document D
			LEFT JOIN  insurance_file IFI
			ON IFI.insurance_file_cnt = D.insurance_file_cnt
			LEFT JOIN Party AccH
			ON AccH.party_cnt = IFI.account_handler_cnt
			WHERE D.document_id = CT.document_id
			),'')

            WHEN 'Business Type' THEN
		(
		SELECT ISNULL(BT.description, '')
		FROM Document D
		LEFT JOIN  insurance_file IFI
		ON IFI.insurance_file_cnt = D.insurance_file_cnt
		LEFT JOIN business_type BT
		ON BT.business_type_id = IFI.business_type_id
		WHERE D.document_id = CT.document_id
		)
            WHEN 'Insurer' THEN
		CT.Insurer_name
        END
    ) then_desc

FROM #Comm_Temp  CT
ORDER BY
group_code,  
then_code,  
CT.document_date,
CT.effective_date

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO