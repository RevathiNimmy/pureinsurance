SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmnav_Get_Nav_Batch_Keys'
GO


CREATE PROCEDURE spu_pmnav_Get_Nav_Batch_Keys
    @nav_batch_code char(10)
AS

/******************************************************************************************/
/* sp_pmnav_get_Nav_Batch_Keys - gets keys for adding batch process                       */
/******************************************************************************************/
/* Revision Description of Modification Date        Who                                   */
/* -------- --------------------------- ----        ---                                   */
/* 1.0      Original                    28/11/1997  JRW                                   */
/******************************************************************************************/
BEGIN
    /* Declare variables used for processing */
    DECLARE @pmnav_batch_id int

    /* Get the Batch ID */
    SELECT @pmnav_batch_id = pmnav_batch_id
    FROM PMNav_Batch
    WHERE RTRIM(code) = RTRIM(@nav_batch_code)

    /* select keys */

    SELECT pmnav_key_id
    FROM PMNav_Batch_Key
    WHERE PMNav_Batch_id = @pmnav_batch_id
    ORDER BY sequence_number

END
GO


