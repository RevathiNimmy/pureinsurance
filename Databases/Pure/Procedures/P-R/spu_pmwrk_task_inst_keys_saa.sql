SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_inst_keys_saa'
GO


CREATE PROCEDURE spu_pmwrk_task_inst_keys_saa
    @pmwrk_task_instance_cnt integer
AS

/********************************************************************************************************/
/* sp_pmwrk_task_inst_keys_saa selects the the Keys associated with a Task Instance. */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 23/11/1998 RFC */
/********************************************************************************************************/
BEGIN
    SELECT nk.name,
        ti.key_value
    FROM
        pmnav_key nk
        INNER JOIN pmwrk_task_inst_key ti WITH (NOLOCK)
	    ON ti.pmnav_key_id = nk.pmnav_key_id
	        WHERE ti.pmwrk_task_instance_cnt = @pmwrk_task_instance_cnt
		    ORDER BY nk.name ASC

END
GO


