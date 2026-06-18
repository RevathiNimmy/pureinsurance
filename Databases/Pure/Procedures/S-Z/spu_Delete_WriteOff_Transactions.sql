SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO 
EXEC DDLDropProcedure 'spu_Delete_WriteOff_Transactions'
GO 

CREATE PROCEDURE spu_Delete_WriteOff_Transactions
@nDocumentId INT

AS

DELETE FROM transmatch WHERE transdetail_id IN (SELECT transdetail_id FROM transdetail WHERE spare LIKE 'WRITEOFF' AND Document_id = @nDocumentId)

IF NOT EXISTS (SELECT NULL FROM document WHERE document_id = @nDocumentId AND document_ref LIKE 'SWD%')
BEGIN	
	DELETE FROM transdetail WHERE spare LIKE 'WRITEOFF' AND document_id =@nDocumentId
END
