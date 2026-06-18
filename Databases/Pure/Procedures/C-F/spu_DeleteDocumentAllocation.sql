DDLDropProcedure 'spu_DeleteDocumentAllocation'
Go

-- delete all allocations for this document
CREATE PROCEDURE spu_DeleteDocumentAllocation @DocRef varchar(25)
AS
BEGIN

DECLARE @ReturnValue int

	SELECT @ReturnValue = 0
	
	Begin Transaction

	-- create temporary tables	
	CREATE TABLE #TmpMatchGroup (MatchID int)
	IF @@ERROR <> 0
		GOTO Error_Routine

	CREATE TABLE #TmpAllocation (AllocationID int)
	IF @@ERROR <> 0
		GOTO Error_Routine
	
	-- disable constraints
	ALTER TABLE MatchGroup NOCHECK CONSTRAINT ALL
	ALTER TABLE TransMatch NOCHECK CONSTRAINT ALL
	ALTER TABLE Allocation NOCHECK CONSTRAINT ALL
	ALTER TABLE AllocationDetail NOCHECK CONSTRAINT ALL

	-- get all match group ids for this document
	INSERT INTO #TmpMatchGroup 
	SELECT 	tm.match_id 
	FROM 	TransMatch tm JOIN AllocationDetail ad ON tm.transdetail_id = ad.transdetail_id
	WHERE	ad.document_ref = @DocRef
	IF @@ERROR <> 0
		GOTO Error_Routine
	
	-- get all allocation ids for this document
	INSERT INTO #TmpAllocation
	SELECT	allocation_id FROM AllocationDetail WHERE document_ref = @DocRef
	IF @@ERROR <> 0
		GOTO Error_Routine
	
	-- delete TransMatch
	DELETE	TransMatch
	FROM	TransMatch tm JOIN AllocationDetail ad ON tm.transdetail_id = ad.transdetail_id
	WHERE	ad.document_ref = @DocRef
	IF @@ERROR <> 0
		GOTO Error_Routine
	
	-- delete match group if it doesn't link to any other documents
	DELETE	MatchGroup
	FROM	MatchGroup mg LEFT JOIN TransMatch tm ON mg.match_id = tm.match_id
	WHERE	tm.match_id Is Null
	AND		mg.match_id IN (SELECT MatchID FROM #TmpMatchGroup)
	IF @@ERROR <> 0
		GOTO Error_Routine
	
	-- delete AllocationDetail
	DELETE	AllocationDetail
	WHERE	document_ref = @DocRef
	IF @@ERROR <> 0
		GOTO Error_Routine
	
	-- delete allocation if its doesn't link to any other documents
	DELETE	Allocation
	FROM	Allocation a LEFT JOIN AllocationDetail ad ON a.allocation_id = ad.allocation_id
	WHERE	ad.allocation_id  Is Null
	AND		a.allocation_id IN (SELECT AllocationID FROM #TmpAllocation)
	IF @@ERROR <> 0
		GOTO Error_Routine
	
	
	Commit Transaction
	
	GOTO End_Routine
	
Error_Routine:

	SELECT @ReturnValue = -1
	
 	RollBack Transaction 	
 	
	GOTO End_Routine
	
End_Routine:

	-- enable constraints
	ALTER TABLE MatchGroup CHECK CONSTRAINT ALL
	ALTER TABLE TransMatch CHECK CONSTRAINT ALL
	ALTER TABLE Allocation CHECK CONSTRAINT ALL
	ALTER TABLE AllocationDetail CHECK CONSTRAINT ALL


	DROP TABLE #TmpMatchGroup
	DROP TABLE #TmpAllocation

	RETURN @ReturnValue
END
