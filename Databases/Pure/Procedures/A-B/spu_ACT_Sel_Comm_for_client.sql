SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Sel_Comm_for_client'
GO

/****** Object:  Stored Procedure dbo.sp_ACT_Sel_Comm_for_client    Script Date: 16/10/00 12:03:17 ******/
/*** eck 5/1/01 added extra check-  OR M.allocationdetail_id = A.allocationdetail_id */
/***eck090701 Retrieve original company for earned transaction*/
/***eck301001 Don't move commission when matching with a document*/
/***eck121101 Extra check when match on allocationdetail to return correct debit transaction***/
/***eck211101 Extra check to eliminate  irrelevant  allocation record ***/
/***eck150102 Completely overhauled to enhance speed */
/***eck261102 Return sum of all commission posted to potential brokerage *****/
/***DJM 11/12/2002 : Return currency_amount for use with matching***/
/***DJM 25/04/2003 : Don't check M.allocationdetail_id = A.allocation_id.***/
/***DJM 28/04/2003 : Sorted out last SELECT so that it works if two items from same policy paid off at the same time.***/
/***DJM 28/04/2003 : Doesn't use unrounded amounts anymore and made sure that transmatch amounts are rounded.***/
/***ECK 12/10/2004 : Don't move commission if the transaction is in the suspense file. ***/
/*** AR 26/11/2004 : Add exclude_did parameter. Exclude Direct Debit transactions if this flag is set. APPLIED BY DC011204***/

CREATE PROCEDURE spu_ACT_Sel_Comm_for_client 
    @transdetail_id int,
    @exclude_did bit
AS

DECLARE @DocumentId INT

CREATE TABLE #TempDoc  
	(document_id INT,
	 is_did BIT)
INSERT INTO #TempDoc
SELECT	DISTINCT T.document_id, 0
FROM 	Transmatch M,		
	AllocationDetail	A,
	AllocationDetail	A2,
	Transdetail		T
WHERE 	M.transdetail_id = @transdetail_id AND	
	M.allocationdetail_id = A2.allocationdetail_id	AND	
	A2.transdetail_id = @transdetail_id AND	
	A2.allocation_id = A.allocation_id AND	
	A.transdetail_id <> @transdetail_id AND	
	A.transdetail_id = T.transdetail_id 
	AND 
	(
		ISNULL((
			SELECT SUM(ISNULL(td.amount,0))
			FROM transdetail td
			JOIN account a
			ON a.account_id = td.account_id
			JOIN ledger l
			ON l.ledger_id = a.ledger_id
			WHERE td.document_id = T.document_id
			AND l.ledger_short_name = 'SA'
		),0) =
		ISNULL((
			SELECT SUM(ISNULL(tm.base_match_amount,0))
			FROM transdetail td
			JOIN transmatch tm
			ON tm.transdetail_id = td.transdetail_id
			JOIN account a
			ON a.account_id = td.account_id
			JOIN ledger l
			ON l.ledger_id = a.ledger_id
			WHERE td.document_id = T.document_id
			AND l.ledger_short_name = 'SA'	
		),0)
		OR
		ISNULL((
			SELECT SUM(ISNULL(td.currency_amount,0))
			FROM transdetail td
			JOIN account a
			ON a.account_id = td.account_id
			JOIN ledger l
			ON l.ledger_id = a.ledger_id
			WHERE td.document_id = T.document_id
			AND l.ledger_short_name = 'SA'
		),0) =
		ISNULL((
			SELECT SUM(ISNULL(tm.currency_match_amount,0))
			FROM transdetail td
			JOIN transmatch tm
			ON tm.transdetail_id = td.transdetail_id
			JOIN account a
			ON a.account_id = td.account_id
			JOIN ledger l
			ON l.ledger_id = a.ledger_id
			WHERE td.document_id = T.document_id
			AND l.ledger_short_name = 'SA'	
		),0)
	)

IF @exclude_did=1

BEGIN

	DECLARE DOC_CURSOR CURSOR FORWARD_ONLY FOR SELECT document_id FROM #TempDoc

	OPEN DOC_CURSOR

	FETCH NEXT FROM DOC_CURSOR INTO @DocumentId

	WHILE @@FETCH_STATUS=0
		BEGIN
			IF EXISTS
					(
					SELECT D.Document_Id
					FROM
					TransDetail TD
					INNER JOIN TransMatch TM ON TM.TransDetail_Id=TD.TransDetail_Id
					INNER JOIN TransMatch TM2 ON TM2.Match_Id=TM.Match_Id AND TM2.TransDetail_Id<>TM.TransDetail_Id
					INNER JOIN TransDetail TD2 ON TD2.TransDetail_Id=TM2.TransDetail_Id
					INNER JOIN Document D ON D.Document_Id=TD2.Document_Id AND D.DocumentType_Id IN (33,34)
					INNER JOIN Account A ON A.Account_Id=TD.Account_Id
					INNER JOIN Ledger L ON L.Ledger_Id=A.Ledger_Id
					WHERE TD.Document_Id=@DocumentId AND L.Ledger_short_name IN ('SA','UB')
					)
					UPDATE #TempDoc SET is_did=1 WHERE CURRENT OF DOC_CURSOR
				
			FETCH NEXT FROM DOC_CURSOR INTO @DocumentId
		END
	
	CLOSE DOC_CURSOR
	DEALLOCATE DOC_CURSOR

END

SELECT
	T.account_id		'account_id',
	T.amount			'amount',
	T.insurance_ref		'insurance_ref',
	T.transdetail_id	'transdetail_id',
	T.company_id		'company_id',
	T.currency_amount	'currency_amount'
FROM
	Transdetail		T,
	Transdetail		T2 
WHERE   
	(T.document_id IN 
		(SELECT document_id FROM #TempDoc WHERE is_did=0)  
	)
AND	T.spare IN ('BROK','BROK ADJ')
AND	T2.transdetail_id = @transdetail_id
AND	T2.document_Id <> T.document_Id
AND     NOT EXISTS (select transdetail_id from PF_accounts_transactions
			where transdetail_id = T.transdetail_id)
DROP TABLE #TempDoc

GO
 
 