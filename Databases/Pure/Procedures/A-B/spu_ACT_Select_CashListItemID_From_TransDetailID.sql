EXECUTE DDLDropProcedure 'spu_ACT_Select_CashListItemID_From_TransDetailID'
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_ACT_Select_CashListItemID_From_TransDetailID
	@transdetail_id int
AS

SELECT
	CLI.cashlistitem_id,
	T2.transdetail_id		-- opposite Bank Credit/Debit from CashListItem
FROM
	CashListItem CLI
INNER JOIN
	TransDetail T ON T.transdetail_id=@transdetail_id
INNER JOIN
	Document D ON D.document_id=T.document_id
INNER JOIN
	TransDetail T2 ON T2.document_id=D.document_id 
	AND T2.transdetail_id<>@transdetail_id
WHERE
	CLI.transdetail_id = @transdetail_id
AND
	NOT EXISTS (SELECT * FROM PFInstalments WHERE PFTransaction_id = @transdetail_id)
GO
