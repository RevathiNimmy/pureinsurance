SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Sel_Comm_for_DID'
GO 


/****** Object:  Stored Procedure dbo.sp_ACT_Sel_Comm_for_DID    Script Date: 30/10/00 12:03:17 ******/
/***eck261102 Return sum of all commission posted to potential brokerage *****/
/***DJM 11/12/2002 : return currency_amount for use with matching***/

CREATE PROCEDURE spu_ACT_Sel_Comm_for_DID
    @transdetail_id int 
 
AS
--Select commission amounts for normal direct debits.
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
	Document 		D
WHERE T.document_id IN 
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
	AND	T.spare like 'DDREV%' 
	AND	RIGHT(T.spare,11) = D.document_ref
  	AND	D.document_id = T2.document_id
 	AND   	T2.spare in ('BROK','BROK ADJ')
	AND 	T.company_id = T2.company_id
	
UNION

--Select commission amounts for manual direct debits.
SELECT
	T.account_id		'account_id',
	T.amount			'amount',
	T.insurance_ref		'insurance_ref',
	T.transdetail_id	'transdetail_id',
	T.company_id		'company_id',
	T.currency_amount	'currency_amount'
FROM transdetail		T
WHERE T.transdetail_id in 
(
	SELECT comm_td.transdetail_id 
	FROM transmatch	pay_tm
	JOIN allocationdetail pay_ad
	ON pay_ad.allocationdetail_id = pay_tm.allocationdetail_id
	JOIN allocationdetail dd_ad
	ON dd_ad.allocation_id = pay_ad.allocation_id
	AND dd_ad.transdetail_id <> pay_ad.transdetail_id
	JOIN transdetail dd_td
	ON dd_td.transdetail_id = dd_ad.transdetail_id
	JOIN transdetail dd_td2
	ON dd_td2.document_id = dd_td.document_id
	AND dd_td2.transdetail_id <> dd_td.transdetail_id
	JOIN allocationdetail dd_ad2
	ON dd_ad2.transdetail_id = dd_td2.transdetail_id
	JOIN allocationdetail client_ad
	ON client_ad.allocation_id = dd_ad2.allocation_id
	AND client_ad.transdetail_id <> dd_ad2.transdetail_id
	JOIN transdetail client_td
	ON client_td.transdetail_id = client_ad.transdetail_id
	JOIN transdetail comm_td
	ON comm_td.document_id = client_td.document_id
	AND comm_td.spare IN ('BROK','BROK ADJ')
	WHERE pay_tm.transdetail_id = @transdetail_id
	AND	dd_td.amount =
	(
		SELECT SUM(base_match_amount)
		FROM transmatch
		WHERE transdetail_id = dd_td.transdetail_id
	)
	AND dd_td.spare = 'DDREV'
	GROUP BY comm_td.transdetail_id
)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

