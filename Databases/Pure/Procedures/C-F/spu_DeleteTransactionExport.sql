
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_DeleteTransactionExport'
GO

-- delete transaction export details and folder for this insurance file cnt
CREATE PROCEDURE spu_DeleteTransactionExport
	@InsuranceFileCnt int,
	@DocumentRef varchar(25)
	
AS

BEGIN

	Begin Transaction
	
	-- delete transaction export detail
	DELETE 	Transaction_Export_Detail
	FROM	Transaction_Export_Detail ted JOIN Transaction_Export_Folder tef ON ted.transaction_export_folder_cnt = tef.transaction_export_folder_cnt
	WHERE	tef.insurance_file_cnt = @InsuranceFileCnt
	AND		(tef.document_ref = @DocumentRef OR @DocumentRef Is Null)
	
	IF @@ERROR <> 0
		GOTO Err_Label

	-- delete transaction export folder
	DELETE FROM Transaction_Export_Folder WHERE insurance_file_cnt = @InsuranceFileCnt AND (document_ref = @DocumentRef OR @DocumentRef Is Null)

	IF @@ERROR <> 0
		GOTO Err_Label
	

	Commit Transaction
	
	RETURN 0
	
Err_Label:
	Rollback Transaction
	RETURN -1

END