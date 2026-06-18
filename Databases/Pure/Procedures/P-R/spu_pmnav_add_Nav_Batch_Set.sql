SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmnav_add_Nav_Batch_Set'
GO


CREATE PROCEDURE spu_pmnav_add_Nav_Batch_Set
    @batch_set_id int OUTPUT,
    @batch_code char(10),
    @created_by_id smallint
AS
/******************************************************************************************/
/* sp_pmnav_add_Nav_Batch_Set Adds a new Navigator Batch Set                              */
/******************************************************************************************/
/* Revision Description of Modification Date        Who                                   */
/* -------- --------------------------- ----        ---                                   */
/* 1.0      Original                    02/10/1997  TF                                    */
/******************************************************************************************/
BEGIN

    /* Declare variables used for processing */
    DECLARE @pmnav_batch_id int

    /* Get the Batch ID */
    SELECT @pmnav_batch_id = pmnav_batch_id
    FROM PMNav_Batch
    WHERE RTRIM(code) = RTRIM(@batch_code)

    /* Add the Batch Set */
    INSERT INTO PMNav_Batch_Set (
        pmnav_batch_id,
        created_by_id,
        date_created,
        started_date,
        started_by_id,
        completed_date)
    VALUES (
        @pmnav_batch_id,
        @created_by_id,

        GetDate(),
        NULL,
        NULL,
        NULL)

    /* Return the new ID */
    SELECT @batch_set_id = @@IDENTITY

END
GO


