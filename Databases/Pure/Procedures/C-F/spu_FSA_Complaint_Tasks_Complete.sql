SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_FSA_Complaint_Tasks_Complete'
GO
CREATE PROCEDURE spu_FSA_Complaint_Tasks_Complete 
	@FSA_complaint_folder_cnt int
AS
BEGIN
	DECLARE @NavKeyId INTEGER
	DECLARE @lPMWrkTaskInstanceCnt INTEGER

	SELECT @NavKeyId = ( SELECT pmnav_key_id FROM pmnav_key WHERE [name] = 'fsa_complaint_folder_cnt' )
	
	DECLARE c_cursor CURSOR FAST_FORWARD FOR
	SELECT 	pti.pmwrk_task_instance_cnt 
	FROM	pmwrk_task_instance pti
	JOIN	pmwrk_task_inst_key ptik
	ON	pti.pmwrk_task_instance_cnt = ptik.pmwrk_task_instance_cnt
	WHERE	ptik.pmnav_key_id = @NavKeyId
	AND	ptik.key_value = @FSA_complaint_folder_cnt

	OPEN c_cursor

    	FETCH NEXT FROM c_cursor INTO @lPMWrkTaskInstanceCnt

   	WHILE @@FETCH_STATUS = 0 BEGIN
		
		SELECT @lPMwrkTaskInstanceCnt

		UPDATE pmwrk_task_instance
		SET task_status = 3
		WHERE pmwrk_task_instance_cnt = @lPMWrkTaskInstanceCnt
		AND [description] LIKE 'Maintain Complaint - Compliance Requirement - Follow Up Within%'
		AND task_status <> 1

		FETCH NEXT FROM c_cursor INTO @lPMWrkTaskInstanceCnt
	END
    	CLOSE c_cursor
    	DEALLOCATE c_cursor

END
GO