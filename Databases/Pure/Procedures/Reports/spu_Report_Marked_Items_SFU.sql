SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Marked_Items_SFU'
GO 

/** 01/08/2002	AMJ	- branch specific change **/
/** 28/04/2003  KB PN Issue 3074 the join on the coinsurer table needs to be an inner join rather
than a left outer join as it is returning transactions relating  to coinsured policuies twice **/
/** 06/05/2003  KB put the outer join back and rule out duplicates in another way **/
/**ECK 160603 PN4707 **/
/** 15/08/03 PN 6108 remove checks for policy coinsurer number = null **/
CREATE PROCEDURE spu_Report_Marked_Items_SFU
    	@company_id int,
	@insurer_code	varchar(20),
	@PaymentGroups Int = 0 
AS

SELECT	DISTINCT
	ISNULL(P.name, '')		Insurer,
	ISNULL(AD.address1, '')		Address1,
	ISNULL(AD.address2, '')		Address2,
	ISNULL(AD.address3, '')		Address3,
	ISNULL(AD.address4, '')		Address4,
	ISNULL(AD.postal_code, '')	Postal_Code,

	DCli.document_ref		Document_Ref,
	DCli.document_id		Document_ID,
	DCli.document_date		Policy_Doc_Date,
	DTCli.description		Document_Type,

	ISNULL(ACli.account_name, '')	Client,
	ISNULL(ACli.short_code, '')	Client_Code,
	--ISNULL(ACli.ledger_id, 0)	Ledger_ID,
        (CASE l.ledger_short_name
         WHEN 'NO' THEN 1
         WHEN 'SA' THEN 2
         WHEN 'PU' THEN 3
         WHEN 'IN' THEN 4
         WHEN 'AG' THEN 5
         WHEN 'RF' THEN 6
         WHEN 'FE' THEN 7 
         WHEN 'DI' THEN 8
         WHEN 'CO' THEN 9
         WHEN 'UB' THEN 10
         ELSE 0 END) ledger_id,
	ISNULL(TCli.insurance_ref, '')	Policy_Ref,
	ISNULL(CoIns.coinsurer_policy_number,'') Coinsurer_Policy_ref,	--1.6.9 Upgrade

	ISNULL
	(
		(
		SELECT	SUM(round(amount,2))
		FROM	Transdetail
		WHERE	document_id = DCli.document_id 
		AND	account_id = A.account_id
		AND	(	
				(
					A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN' )--PN4707

					AND
					(
						(spare = 'GROSS'
						OR
						spare = '')
						OR
						DCli.documenttype_id IN (33, 34)
					)
				)
			OR
				(
					A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'AG' ) --PN4707

					AND	--1.6.9
					(spare = 'AGENT'
					OR
					(spare = ''
					AND
					DCli.documenttype_id = 1))
				)
			)
		)
	, 0)				Premium,

	ISNULL
	(
		(
		SELECT	SUM(round(amount,2))
		FROM	Transdetail
		WHERE	document_id = DCli.document_id
		AND	account_id = A.account_id
		AND	(
				(
					A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN' )--PN4707

					AND
					spare like 'COMM%'
				)
			OR
				(
					A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'AG' )--PN4707

					AND
					spare = 'AGENT ADJ'
				)
			)
		)
	, 0)				Commission,

	ISNULL
	(
		(
		SELECT	SUM(ROUND((ref_amount * amount/abs(amount)),2))              --DC101202 added rounding
															    --KB 06012003 pick up the sign so that
															    --the IPT will show correctly
		FROM	Transdetail
		WHERE	document_id = DCli.document_id 
		AND	account_id = A.account_id
		AND	(
				(
					A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN' )--PN4707

					AND			
					(
						(
						spare = 'GROSS'
						OR
						spare = ''
						)
						OR
						DCli.documenttype_id IN (33, 34)
					)
				)
				OR
				(
					A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'AG' ) --PN4707

					AND	--DC011101 added extra check if journal
					(spare = 'AGENT'
					OR
					(spare = ''
					AND
					DCli.documenttype_id = 1))
				)
			)
		)
	, 0)				IPT,

	ISNULL
	(
		(
		SELECT	SUM(round(base_match_amount,2))
		FROM	TransMatch	A1,
			Transdetail	T1
		WHERE	T1.transdetail_id = A1.transdetail_id
		AND	T1.document_id = DCli.document_id
		AND	T1.account_id = A.account_id
		AND	A1.allocationdetail_id IS NULL 
		) 
	, 0)				This_Payment,

	ISNULL
	(
		(
		SELECT	SUM(round(base_match_amount,2))
		FROM	TransMatch	A1,
			Transdetail	T1
		WHERE	T1.transdetail_id = A1.transdetail_id
		AND	T1.document_id = DCli.document_id
		AND	T1.account_id = A.account_id
		AND	A1.allocationdetail_id IS NOT NULL
        AND T1.spare <> 'COMM'
		)
	, 0)				Total_Payment
FROM	Account		A
	JOIN	Party				P 
	ON A.account_key = P.Party_cnt
	JOIN TransDetail	TD
	ON TD.account_id = A.account_id
	JOIN TransMatch	TM 
	ON TM.transdetail_id = TD.transdetail_id
	JOIN Party_Address_Usage		PAU
	ON PAU.party_cnt = P.party_cnt
 	JOIN Address_Usage_Type			AUT
 	ON AUT.address_usage_type_id = PAU.address_usage_type_id 
	JOIN Address				AD 
	ON AD.address_cnt = PAU.address_cnt	 
	JOIN Document		DCli 
	ON DCli.document_id = TD.document_id
	JOIN  DocumentType	DTCli 
	ON DTCli.documenttype_id = DCli.documenttype_id
	JOIN TransDetail	TCli
	ON TCli.account_id = A.Account_id
	AND TCli.document_id = TD.document_id
	JOIN Account		ACli 
	ON	ACli.account_id =
	(
		SELECT	T2.account_id 
		FROM	TransDetail	T2
		WHERE	transdetail_id = 
		(	SELECT	MIN(transdetail_id)
			FROM	Transdetail	T3,
				Account		A3
			WHERE	T3.document_id = DCli.document_id
			AND	A3.account_id = T3.account_id 
			AND		
			(
				(
				DTCli.description <> 'Journal'
/* Thinh Nguyen 02/09/2003 - comment this out as it won't return data for underwriting
				AND
				A3.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA' ) --eck4707
*/
				)
				OR
				(
				DTCli.description = 'Journal'
				AND
				TCli.account_id <> A3.account_id
				)
			)
		)
	)
JOIN Ledger L
ON L.Ledger_Id = A.Ledger_Id
LEFT OUTER JOIN	Transaction_Export_Folder 			TransExp	 
	ON DCli.Document_Ref = TransExp.Document_Ref				 
	AND TransExp.source_id = DCli.company_id				 
LEFT OUTER JOIN 	Insurance_File 				InsFile		 
	ON TransExp.Insurance_file_cnt = InsFile.insurance_file_cnt		 
LEFT OUTER JOIN Policy_Coinsurers				CoIns		

	ON	InsFile.insurance_file_cnt = CoIns.Insurance_file_cnt		 
	AND	CoIns.Party_cnt = P.Party_cnt					
WHERE	A.short_code = @insurer_code
AND 	TM.allocationdetail_id IS NULL
AND	AUT.code = '3131 XCO'
--AND (CoIns.COINSURER_POLICY_NUMBER) IS NULL -- KB 15/08/03 remove

-- SET 13112002 ISS1261 - check for match on company id if supplied
AND (
       ((
       (@company_id > 0 AND DCli.company_id = @company_id)
       OR
       (@company_id = 0)) 
        AND @PaymentGroups=0) 
    OR 
        (
        @PaymentGroups=1
        AND
        DCli.company_Id in (select company_id from insurerpayment where paymentgroup_id=@company_id and account_id=a.account_id)
        )
    )
-- SET 14112002 ISS1261 - End

UNION

SELECT	DISTINCT
	ISNULL(P.name, '')		Insurer,
	''		Address1,
	''		Address2,
	''		Address3,
	''		Address4,
	''		Postal_Code,

	DCli.document_ref		Document_Ref,
	DCli.document_id		Document_ID,
	DCli.document_date		Policy_Doc_Date,
	DTCli.description		Document_Type,

	ISNULL(ACli.account_name, '')	Client,
	ISNULL(ACli.short_code, '')	Client_Code,
	ISNULL(ACli.ledger_id, 0)	Ledger_ID,
	ISNULL(TCli.insurance_ref, '')	Policy_Ref,
	ISNULL(CoIns.coinsurer_policy_number,'') Coinsurer_Policy_ref,	--1.6.9
	ISNULL
	(
		(
		SELECT	SUM(round(amount,2))
		FROM	Transdetail
		WHERE	document_id = DCli.document_id 
		AND	account_id = A.account_id
		AND	(	
				(
					A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN' )--PN4707

					AND
					(
						(spare = 'GROSS'
						OR
						spare = '')
						OR
						DCli.documenttype_id IN (33, 34)
					)
				)
			OR
				(
					A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'AG' )--PN4707

					AND	--1.6.9
					(spare = 'AGENT'
					OR
					(spare = ''
					AND
					DCli.documenttype_id = 1))
				)
			)
		)
	, 0)				Premium,

	ISNULL
	(
		(
		SELECT	SUM(round(amount,2))
		FROM	Transdetail
		WHERE	document_id = DCli.document_id
		AND	account_id = A.account_id
		AND	(
				(
					A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN' )--PN4707

					AND
					spare like 'COMM%'
				)
			OR
				(
					A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'AG' )--PN4707

					AND
					spare = 'AGENT ADJ'
				)
			)
		)
	, 0)				Commission,

	ISNULL
	(
		(
		SELECT	SUM(round(ref_amount,2))
		FROM	Transdetail
		WHERE	document_id = DCli.document_id 
		AND	account_id = A.account_id
		AND	(
				(
					A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'IN' )--PN4707

					AND			
					(
						(
						spare = 'GROSS'
						OR
						spare = ''
						)
						OR
						DCli.documenttype_id IN (33, 34)
					)
				)
				OR
				(
					A.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'AG' )--PN4707

					AND	--DC011101 added extra check if journal
					(spare = 'AGENT'
					OR
					(spare = ''
					AND
					DCli.documenttype_id = 1))
				)
			)
		)
	, 0)				IPT,

	ISNULL
	(
		(
		SELECT	SUM(round(base_match_amount,2))
		FROM	TransMatch	A1,
			Transdetail	T1
		WHERE	T1.transdetail_id = A1.transdetail_id
		AND	T1.document_id = DCli.document_id
		AND	T1.account_id = A.account_id
		AND	A1.allocationdetail_id IS NULL 
		) 
	, 0)				This_Payment,

	ISNULL
	(
		(
		SELECT	SUM(round(base_match_amount,2))
		FROM	TransMatch	A1,
			Transdetail	T1
		WHERE	T1.transdetail_id = A1.transdetail_id
		AND	T1.document_id = DCli.document_id
		AND	T1.account_id = A.account_id
		AND	A1.allocationdetail_id IS NOT NULL
		)
	, 0)				Total_Payment

FROM	Account		A
	JOIN	Party				P 
	ON A.account_key = P.party_cnt
	JOIN TransDetail	TD
	ON TD.account_id = A.account_id
	JOIN TransMatch	TM 
	ON TM.transdetail_id = TD.transdetail_id 
	JOIN Document		DCli 
	ON DCli.document_id = TD.document_id
	JOIN  DocumentType	DTCli 
	ON DTCli.documenttype_id = DCli.documenttype_id
	JOIN TransDetail	TCli
	ON TCli.account_id = A.Account_id
	AND TCli.document_id = TD.document_id
	JOIN Account		ACli 
	ON	ACli.account_id =
	(
		SELECT	 T2.account_id
		FROM	TransDetail	T2
		WHERE	transdetail_id = 
		(	SELECT	MIN(transdetail_id)
			FROM	Transdetail	T3,
				Account		A3
			WHERE	T3.document_id = DCli.document_id
			AND	A3.account_id = T3.account_id 
			AND		
			(
				(
				DTCli.description <> 'Journal'

				AND
				A3.ledger_id IN ( SELECT ledger_id FROM ledger WHERE ledger_short_name = 'SA' )

				)
				OR
				(
				DTCli.description = 'Journal'
				AND
				TCli.account_id <> A3.account_id
				)
			)
		)
	)
LEFT OUTER JOIN	Transaction_Export_Folder 			TransExp	--1.6.9 
	ON DCli.Document_Ref = TransExp.Document_Ref				 
	AND TransExp.source_id = DCli.company_id				 
LEFT OUTER JOIN 	Insurance_File 				InsFile		 
	ON TransExp.Insurance_file_cnt = InsFile.insurance_file_cnt		 
LEFT OUTER JOIN Policy_Coinsurers				CoIns		 
                                    
	ON	InsFile.Insurance_File_cnt = CoIns.Insurance_File_cnt
	AND	CoIns.Party_cnt = P.Party_cnt					 
WHERE	A.short_code = @insurer_code
AND 	TM.allocationdetail_id IS NULL
AND	NOT EXISTS ( SELECT * FROM Party_Address_Usage WHERE Party_Cnt = P.Party_Cnt )
--SET 14112002 ISS1261 - check for match on company id if supplied
--AMJ
AND (
        (
	DCli.company_id = @company_id 
	AND @PaymentGroups=0
        ) 
    OR 
	(
	@PaymentGroups=1
        AND
        DCli.company_Id in (select company_id from insurerpayment where paymentgroup_id=@company_id and account_id=a.account_id)
	)
    )
--AND (CoIns.COINSURER_POLICY_NUMBER) IS NULL -- KB 15/08/03 remove
AND (
       (
       ((@company_id > 0 AND DCli.company_id = @company_id)
       OR
       (@company_id = 0)
       ) 
       AND @PaymentGroups=0) 
    OR 
       (
       @PaymentGroups=1
        AND
        DCli.company_Id in (select company_id from insurerpayment where paymentgroup_id=@company_id and account_id=a.account_id)
       )
    )

--DC161001 -end
-- SET 14112002 ISS1261 - End

ORDER BY 
	Client_Code

GO
 
