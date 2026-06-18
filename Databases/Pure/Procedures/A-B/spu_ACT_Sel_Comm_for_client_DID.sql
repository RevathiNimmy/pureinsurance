SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Sel_Comm_for_client_DID'
GO 

/***eck261102 Return sum of all commission posted to potential brokerage *****/
/***DJM 11/12/2002 : return currency_amount for use with matching***/
/***DJM 25/04/2003 : Don't check M.allocationdetail_id = A.allocation_id.***/
/***DJM 28/04/2003 : Sorted out last SELECT so that it works if two items from same policy paid off at the same time.***/

CREATE PROCEDURE spu_ACT_Sel_Comm_for_client_DID 
    @transdetail_id int 

AS
CREATE TABLE #TempDoc  
	(document_id INT)

INSERT INTO #TempDoc
SELECT	DISTINCT T.document_id
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
		(SELECT * FROM #TempDoc)  
	)
AND	T.spare IN ('BROK','BROK ADJ')
AND	T2.transdetail_id = @transdetail_id
AND	T2.document_Id <> T.document_Id

DROP TABLE #TempDoc


GO
 
 