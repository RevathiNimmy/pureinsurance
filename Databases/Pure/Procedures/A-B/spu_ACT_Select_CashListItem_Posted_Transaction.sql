EXEC DDLDropProcedure 'spu_ACT_Select_CashListItem_Posted_Transaction'
GO

CREATE PROCEDURE spu_ACT_Select_CashListItem_Posted_Transaction
	@transdetail_id INT
AS BEGIN

SELECT
	d.document_ref,
	t.currency_id,
	c.description,
	t.amount_currency_id,
	c2.description,
	t.amount,
	t.outstanding_amount
FROM
	TransDetail t
INNER JOIN Document d ON d.document_id=t.document_id
INNER JOIN Currency c ON c.currency_id=t.currency_id
INNER JOIN Currency c2 ON c2.currency_id=t.amount_currency_id
WHERE
	t.transdetail_id=@transdetail_id

END