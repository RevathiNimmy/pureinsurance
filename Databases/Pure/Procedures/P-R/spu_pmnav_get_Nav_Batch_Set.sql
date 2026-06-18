SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmnav_get_Nav_Batch_Set'
GO


CREATE PROCEDURE spu_pmnav_get_Nav_Batch_Set
    @batch_set_id int
AS

/******************************************************************************************/
/* sp_pmnav_get_Nav_Batch_Record Returns all records from a Navigator Batch Set */
/******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 03/10/1997 TF */
/******************************************************************************************/
BEGIN
    /* Select the rows */
    SELECT V.pmnav_batch_record_id,
        V.pmnav_key_id,
        V.key_value

    FROM PMNav_Batch_Key_Value V,
        PMNav_Batch_Key K

    WHERE V.pmnav_batch_set_id = @batch_set_id
    AND (K.pmnav_batch_id = V.pmnav_batch_id
        AND K.pmnav_key_id = V.pmnav_key_id)

    ORDER BY V.pmnav_batch_record_id,
        K.sequence_number

END
GO


