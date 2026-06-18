SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmnav_add_Nav_Batch_Record'
GO


CREATE PROCEDURE spu_pmnav_add_Nav_Batch_Record
    @pmnav_batch_record_id int OUTPUT,
    @batch_record_id int,
    @batch_set_id int,
    @key_id int,
    @key_value varchar(40)
AS

/******************************************************************************************/
/* sp_pmnav_add_Nav_Batch_Record Adds a new Record to a Navigator Batch Set */
/******************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 02/10/1997 TF */
/******************************************************************************************/
BEGIN

    /* Declare variables used for processing */
    DECLARE @pmnav_batch_key_value_id int,
        @pmnav_batch_id int

    /* Get Batch Record ID if not passed in */
    IF ((@batch_record_id = 0)
        OR (@batch_record_id = NULL))
    BEGIN
        SELECT @pmnav_batch_record_id = MAX(pmnav_batch_record_id) + 1
        FROM PMNav_Batch_Record
        WHERE pmnav_batch_set_id = @batch_set_id

        IF (@pmnav_batch_record_id = NULL)
            SELECT @pmnav_batch_record_id = 1

        /* Create Batch_Record */
        INSERT INTO PMNav_Batch_Record (
            pmnav_batch_set_id,
            pmnav_batch_record_id)
        VALUES (
            @batch_set_id,
            @pmnav_batch_record_id)
    END
    ELSE
    BEGIN
        /* Check if the Batch_Record already exists */
        SELECT @pmnav_batch_record_id = pmnav_batch_record_id
        FROM PMNav_Batch_Record
        WHERE (pmnav_batch_set_id = @batch_set_id
        AND pmnav_batch_record_id = @batch_record_id)

        /* Create Batch_Record */
        IF (@pmnav_batch_record_id = NULL)
        BEGIN
            SELECT @pmnav_batch_record_id = @batch_record_id
            INSERT INTO PMNav_Batch_Record (
                pmnav_batch_set_id,
                pmnav_batch_record_id)
            VALUES (
                @batch_set_id,
                @pmnav_batch_record_id)
        END
    END

    /* Get the Batch_Key_Value ID */
    SELECT @pmnav_batch_key_value_id = MAX(pmnav_batch_key_value_id) + 1
    FROM PMNav_Batch_Key_Value
    WHERE (pmnav_batch_set_id = @batch_set_id
    AND pmnav_batch_record_id = @pmnav_batch_record_id)

    IF (@pmnav_batch_key_value_id = NULL)
        SELECT @pmnav_batch_key_value_id = 1

    /* Get the Batch ID */
    SELECT @pmnav_batch_id = pmnav_batch_id
    FROM PMNav_Batch_Set
    WHERE pmnav_batch_set_id = @batch_set_id

    /* Add the Batch_Key_Value */
    INSERT INTO PMNav_Batch_Key_Value (
        pmnav_batch_set_id,

        pmnav_batch_record_id,
        pmnav_batch_key_value_id,
        pmnav_batch_id,
        pmnav_key_id,
        key_value)
    VALUES (
        @batch_set_id,
        @pmnav_batch_record_id,
        @pmnav_batch_key_value_id,
        @pmnav_batch_id,
        @key_id,
        @key_value)

END
GO


