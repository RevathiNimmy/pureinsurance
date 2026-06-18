SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmnav_sel_Nav_Batch_Record'
GO


CREATE PROCEDURE spu_pmnav_sel_Nav_Batch_Record
    /*@pmnav_batch_record_id int OUTPUT, */

    @pmnav_batch_set_id int
AS

/******************************************************************************************/
/* sp_pmnav_sel_Nav_Batch_Record Returns the next Record in a Navigator Batch Set */
/******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 14/10/1997 TF */
/******************************************************************************************/
BEGIN
    DECLARE @pmnav_batch_record_id int

    SET NOCOUNT ON

    /* Get the next record id */
    SELECT @pmnav_batch_record_id = MIN(pmnav_batch_record_id)
    FROM PMNav_Batch_Record
    WHERE pmnav_batch_set_id = @pmnav_batch_set_id

    /* Return -1 if none remaining */
    IF @@ROWCOUNT = 0
    BEGIN
        SELECT @pmnav_batch_record_id = -1
        RETURN
    END

    /* Get the Key values */
    SELECT K.name,
        V.key_value
    FROM PMNav_Key K,
        PMNav_Batch_Key_Value V
    WHERE V.pmnav_batch_set_id = @pmnav_batch_set_id
    AND V.pmnav_batch_record_id = @pmnav_batch_record_id
    AND K.pmnav_key_id = V.pmnav_key_id

END
GO


