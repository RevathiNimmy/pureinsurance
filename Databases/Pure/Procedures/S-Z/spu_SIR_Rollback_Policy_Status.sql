EXEC DDLDropProcedure 'spu_SIR_Rollback_Policy_Status'
GO

CREATE PROCEDURE spu_SIR_Rollback_Policy_Status
	@insurance_file_cnt INT,
	@old_policy_number VARCHAR(30)

AS
	DECLARE @old_file_type_id INT
	DECLARE @current_file_type_id INT

	SELECT @current_file_type_id=insurance_file_type_id 
	FROM insurance_file
	WHERE insurance_file_cnt = @insurance_file_cnt
	
	-- Reset the file type	
	IF @current_file_type_id = 2
		SELECT @old_file_type_id = 1
	ELSE IF (@current_file_type_id = 5 OR @current_file_type_id = 8)
		SELECT @old_file_type_id = 4
	ELSE IF @current_file_type_id = 6
		SELECT @old_file_type_id = 7
	ELSE IF @current_file_type_id = 9
		SELECT @old_file_type_id = 10
	
	-- and the Policy Number
	UPDATE event_insurance_file 
	SET	insurance_ref = @old_policy_number,
		insurance_file_type_id = @old_file_type_id,
		insurance_file_status_id = NULL
	WHERE insurance_file_cnt = @insurance_file_cnt

	UPDATE insurance_file 
	SET	insurance_ref = @old_policy_number,
		insurance_file_type_id = @old_file_type_id,
		insurance_file_status_id = NULL
	WHERE insurance_file_cnt = @insurance_file_cnt

	-- Remove the last event log entry (this is the make live line)
	DELETE Event_Log
	WHERE event_cnt IN (SELECT MAX(event_cnt) FROM Event_Log WHERE insurance_file_cnt = @insurance_file_cnt)

	-- Remove the Stats and Export data
	EXEC spu_DeleteStatsFolder @InsuranceFileCnt=@insurance_file_cnt, @DocumentRef=NULL
GO