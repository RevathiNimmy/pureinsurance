SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_SAM_PMWrk_Task_Inst_Key_Upd'
GO

-- Start (Sankar) - (Tech Spec - UIIC WR33 - Work Manager - Update Task.doc) - 6.2

CREATE PROCEDURE spu_SAM_PMWrk_Task_Inst_Key_Upd(
@pmwrk_task_instance_cnt integer ,  
@key_name   varchar(30) ,  
@key_value   varchar(255))
  
AS  
BEGIN  
  
	DECLARE @pmnav_key_id integer  

	/* Get the ID for this Key*/  
	SELECT  @pmnav_key_id = pmnav_key_id  
	FROM  pmnav_key  
	WHERE RTRIM(@key_name) = RTRIM(name)  

	IF (@pmnav_key_id IS NULL)  
	BEGIN
		SELECT @pmnav_key_id = ISNULL(MAX(pmnav_key_id) + 1, 1) FROM pmnav_key

		INSERT INTO pmnav_key
		( pmnav_key_id, name, description, data_type, is_deleted, effective_date )
		VALUES
		( @pmnav_key_id,  @key_name, @key_name, 0, 0, GetDate() )
	END
	ELSE
		UPDATE 	pmnav_key
		SET	effective_date = GetDate()
		WHERE	pmnav_key_id = @pmnav_key_id

	IF NOT EXISTS (SELECT null FROM pmwrk_task_inst_key WHERE pmwrk_task_instance_cnt=@pmwrk_task_instance_cnt AND  
	pmnav_key_id=@pmnav_key_id) 
		/* Insert the Key */  
		INSERT  
		INTO  pmwrk_task_inst_key  
		(pmwrk_task_instance_cnt ,  
		pmnav_key_id ,  
		key_value)  
		VALUES (@pmwrk_task_instance_cnt ,  
		@pmnav_key_id ,  
		@key_value)  
	ELSE
		UPDATE 	pmwrk_task_inst_key
		SET 	key_value = @key_value
		WHERE 	pmwrk_task_instance_cnt=@pmwrk_task_instance_cnt 
		AND  	pmnav_key_id=@pmnav_key_id

END
 -- End (Sankar) - (Tech Spec - UIIC WR33 - Work Manager - Update Task.doc) - 6.2
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
