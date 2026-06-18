SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Sel_Comm_for_insurer'
GO 

/****** Object:  Stored Procedure dbo.sp_ACT_Sel_Comm_for_insurer    Script Date: 16/10/00 12:03:17 ******/
/***eck090701 Retrieve original company for earned transaction***/
/***eck251001 Added extra check to ensure full allaocation */
/***eck141102 Extra logic to select commission if extra is being paid and there is no main insurer *******/
/***eck261102 Return sum of all commission posted to potential brokerage *****/
/***DJM 11/12/2002 : Return currency_amount for use with matching***/
/***DJM 28/04/2003 : Group by document id and select minimum transdetail***/
/***ECK 06/06/2003 : Revise link between party and account tables ***/

CREATE PROCEDURE spu_ACT_Sel_Comm_for_insurer
    @transdetail_id int 
 
AS
 
SELECT
	T2.account_id		'account_id',
	T2.amount			'amount',
 	T2.insurance_ref	'insurance_ref', 
	T2.transdetail_id	'transdetail_id', 
	T2.company_id		'company_id',
	T2.currency_amount	'currency_amount'
FROM
	Transdetail		T ,
	Transdetail		T2,
	Document 		D,
	Account			A,
	Transdetail		TC
WHERE   T.document_id IN 
	(
		SELECT	distinct T.document_id 
		FROM 	Transmatch		M,
			AllocationDetail	AD,
			AllocationDetail	AD2,
			Allocation		A,
			Transdetail		T

		WHERE 	M.transdetail_id = @transdetail_id
		AND	M.allocationdetail_id = AD.allocationdetail_id
		AND     A.allocation_id = AD.allocation_id
		AND	A.allocation_id = AD2.allocation_id
		AND	AD2.transdetail_id <> @transdetail_id
		AND	AD2.transdetail_id = T.transdetail_id
		AND	(select sum(base_match_amount) from transmatch
			where transdetail_id = T.transdetail_id)  = T.amount 
	)
	AND	TC.transdetail_id = @transdetail_id
	AND	TC.account_id = T.account_id
	AND	T.spare like 'GROSS' 
	AND	T2.document_id = T.document_id
  	AND	D.document_id = T2.document_id
 	AND T2.spare in ('BROK','BROK ADJ')
	AND T.company_id = T2.company_id
	AND	A.account_id = T.account_id
	AND	(	EXISTS (SELECT p.party_cnt FROM
					 party p,
					 party_type pt 
--eck06062003 				WHERE  ((P.source_id - 1) * 268435456) + P.party_id = a.account_key 
 					WHERE  P.party_cnt = a.account_key 
					AND p.party_type_id = pt.party_type_id
					AND pt.code = 'IN' 
				)
		 
 		 
  		OR
 			NOT EXISTS 
				(SELECT t2.transdetail_id FROM 
			      	 party p,
				 party_type pt,
				transdetail t2,
				account a 
 			     	WHERE  t2.document_id = t.document_id
					AND t2.account_id = a.account_id
--eck 06062003                		AND  ((P.source_id - 1) * 268435456) + P.party_id = a.account_key 
                			AND  P.party_cnt = a.account_key 
					AND p.party_type_id = pt.party_type_id
					AND pt.code = 'IN'
			   	)
		 
		)

GO
 
 
