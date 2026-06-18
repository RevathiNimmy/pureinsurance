
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_DeleteStatsFolder'
GO

-- delete stats folder, details and exports
CREATE PROCEDURE spu_DeleteStatsFolder
	@InsuranceFileCnt int,
	@DocumentRef varchar(25)
	
AS

BEGIN

	Begin Transaction
		
	-- delete stats details and exports
	EXECUTE spu_DeleteStatsDetail @InsuranceFileCnt, @DocumentRef
	
	IF @@ERROR <> 0
		GOTO Err_Label
	
	-- delete stats folder
	DELETE 	FROM Stats_Folder
	WHERE	insurance_file_cnt = @InsuranceFileCnt
	AND		(document_ref = @DocumentRef OR @DocumentRef Is Null)
	
	IF @@ERROR <> 0
		GOTO Err_Label

	Commit Transaction
	
	RETURN 0
	
Err_Label:
	Rollback Transaction
	RETURN -1

END