SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmnav_del_Nav_Batch_Set'
GO


CREATE PROCEDURE spu_pmnav_del_Nav_Batch_Set
    @pmnav_batch_set_id int
AS

/******************************************************************************************/
/* sp_pmnav_del_Nav_Batch_Set Deletes the Navigator Batch Set */
/******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 14/10/1997 CTAF */
/******************************************************************************************/
BEGIN
    /* Delete the Key values */
    DELETE
    FROM PMNav_Batch_Key_Value
    WHERE pmnav_batch_set_id = @pmnav_batch_set_id

    /* Delete the record */
    DELETE
    FROM PMNav_Batch_Record
    WHERE pmnav_batch_set_id = @pmnav_batch_set_id

    /* Delete the set */
    DELETE
    FROM PMNav_Batch_Set
    WHERE pmnav_batch_set_id = @pmnav_batch_set_id
END
GO


