SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmnav_stop_Nav_Batch_Set'
GO


CREATE PROCEDURE spu_pmnav_stop_Nav_Batch_Set
    @batch_set_id int
AS

/******************************************************************************************/
/* sp_pmnav_stop_Nav_Batch_Record Records completion of a Navigator Batch Set */
/******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 03/10/1997 TF */
/******************************************************************************************/
BEGIN
    /* Update the Completed fields */
    UPDATE PMNav_Batch_Set
    SET completed_date = GetDate()
    WHERE pmnav_batch_set_id = @batch_set_id
END
GO


