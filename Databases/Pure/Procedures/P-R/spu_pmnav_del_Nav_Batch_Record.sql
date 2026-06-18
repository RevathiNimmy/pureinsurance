SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmnav_del_Nav_Batch_Record'
GO


CREATE PROCEDURE spu_pmnav_del_Nav_Batch_Record
    @pmnav_batch_set_id int,
    @pmnav_batch_record_id int
AS

/******************************************************************************************/
/* sp_pmnav_del_Nav_Batch_Record Deletes the Record from a Navigator Batch Set */
/******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 14/10/1997 TF */
/******************************************************************************************/
BEGIN
    /* Delete the Key values */
    DELETE
    FROM PMNav_Batch_Key_Value
    WHERE pmnav_batch_set_id = @pmnav_batch_set_id
    AND pmnav_batch_record_id = @pmnav_batch_record_id

    /* Delete the record */
    DELETE
    FROM PMNav_Batch_Record
    WHERE pmnav_batch_set_id = @pmnav_batch_set_id
    AND pmnav_batch_record_id = @pmnav_batch_record_id

/*
    /* Delete Batch Set if empty */
    SELECT pmnav_batch_record_id
    FROM PMNav_Batch_Record
    WHERE pmnav_batch_set_id = @pmnav_batch_set_id

    IF @@ROWCOUNT = 0
    BEGIN
        DELETE
        FROM PMNav_Batch_Set
        WHERE pmnav_batch_set_id = @pmnav_batch_set_id
    END
*/

END
GO


