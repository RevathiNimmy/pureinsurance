SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_inst_key_add'
GO


CREATE PROCEDURE spu_pmwrk_task_inst_key_add

@pmwrk_task_instance_cnt	integer ,
@key_name			varchar(30) ,
@key_value			varchar(8000)

AS
BEGIN


/********************************************************************************************************/
/* Stored Procedure sp_pmwrk_task_inst_key_add Adds a Key associated with a Task Instance.              */
/********************************************************************************************************/


/*********************************************************************************************************/
/* Revision            	Description of Modification                                   	Date	    Who  */
/* --------            	---------------------------                                   	----        ---- */
/* 1.0                 	Original							23/11/1998  RFC	 */
/* 1.1                 	Changed to created pmnav_key if it doesnt exist			05/03/2002  CTAF */
/*													 */
/*********************************************************************************************************/

DECLARE @pmnav_key_id	integer

	/* Get the ID for this Key*/
	SELECT 	@pmnav_key_id = pmnav_key_id
	FROM 	pmnav_key
	WHERE	RTRIM(@key_name) = RTRIM(name)

	IF (@pmnav_key_id IS NULL)
	BEGIN
		SELECT @pmnav_key_id = ISNULL(MAX(pmnav_key_id) + 1, 1) FROM pmnav_key

		/*  CTAF 20020305 START Insert the key if it doesn't exist */
		INSERT INTO pmnav_key
		( pmnav_key_id, name, description, data_type, is_deleted, effective_date )
		VALUES
		( @pmnav_key_id,  @key_name, @key_name, 0, 0, GetDate() )
		/* CTAF END */
	END


	IF NOT EXISTS (SELECT null FROM pmwrk_task_inst_key WHERE pmwrk_task_instance_cnt=@pmwrk_task_instance_cnt AND
		pmnav_key_id=@pmnav_key_id) BEGIN
		/* Insert the Key */
		INSERT
		INTO 	pmwrk_task_inst_key
			(pmwrk_task_instance_cnt ,
			pmnav_key_id ,
			key_value)
		VALUES	(@pmwrk_task_instance_cnt ,
			@pmnav_key_id ,
			@key_value)
	END

END
GO
