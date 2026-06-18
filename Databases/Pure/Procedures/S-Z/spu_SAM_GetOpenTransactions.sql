SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SAM_GetOpenTransactions'
GO


/*******************************************************************************************************/
/* spu_SAM_GetOpenTransactions                                                                         */
/* Get Open Transactions for specified party                                                           */
/* MKW27/03/06                                                                                         */
/*******************************************************************************************************/
CREATE PROCEDURE spu_SAM_GetOpenTransactions
    @party_cnt int = NULL
AS
BEGIN
select 
	d.document_id 'DocumentKey',
	d.created_date as 'TransactionDate',
	d.document_date as 'EffectiveDate',
	d.document_ref 'DocRef',
	d.comment as 'Reference',
	t.amount as 'Amount',
	c.iso_code as 'ISO',
	t.amount_updated as 'PaidDate',
	t.outstanding_amount as 'OSAmount',
	d.reason 'Reason',
	t.outstanding_account_amount as 'Balance'
FROM 
	Account a 
	INNER JOIN TransDetail t ON t.account_id = a.account_id 
	INNER JOIN Document d ON d.document_id = t.document_id 
	INNER JOIN Currency c ON c.currency_id = t.currency_id 
	INNER JOIN TransDetail td2 ON td2.document_id=t.document_id and td2.document_sequence=1
WHERE 
	a.account_key=@party_cnt and 
	td2.fully_matched=0
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO