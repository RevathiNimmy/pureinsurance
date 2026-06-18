
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_DeleteStatsDetail'
GO

-- delete stats details insurance file cnt
CREATE PROCEDURE spu_DeleteStatsDetail
	@InsuranceFileCnt int,
	@DocumentRef varchar(25)
	
AS

BEGIN

	Begin Transaction
	
	-- delete transaction export detail and folder
	EXECUTE spu_DeleteTransactionExport @InsuranceFileCnt, @DocumentRef
	
	IF @@ERROR <> 0
		GOTO Err_Label
	
	-- delete stats detail
	DELETE 	Stats_Detail
	FROM	Stats_Detail sd JOIN Stats_Folder sf ON sd.stats_folder_cnt = sf.stats_folder_cnt
	WHERE	sf.insurance_file_cnt = @InsuranceFileCnt
	AND		(sf.document_ref = @DocumentRef OR @DocumentRef Is Null)
	
	IF @@ERROR <> 0
		GOTO Err_Label

	Commit Transaction
	
	RETURN 0
	
Err_Label:
	Rollback Transaction
	RETURN -1

END