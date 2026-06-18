SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmnav_start_Nav_Batch_Set'
GO


CREATE PROCEDURE spu_pmnav_start_Nav_Batch_Set
    @batch_set_id int,
    @user_id smallint
AS

/******************************************************************************************/
/* sp_pmnav_start_Nav_Batch_Record Starts a Navigator Batch Set */
/******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 03/10/1997 TF */
/******************************************************************************************/
BEGIN
    /* Update the Started fields */
    UPDATE PMNav_Batch_Set
    SET started_date = GetDate(),
        started_by_id = @user_id
    WHERE pmnav_batch_set_id = @batch_set_id
END
GO


