SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Setup_Export_Map'
GO


CREATE PROCEDURE spu_Setup_Export_Map
    @new_target_field_name varchar(40),
    @new_acc_type_leading_characters varchar(255),
    @new_mapping_leading_characters varchar(255)
AS


/****************************************************************************************************/
/* 'spu_Setup_Export_Map'                            */
/*   adds records into the Export_Map_Detail & Export_Map_Folder tables for */
/* given new codes.                             */
/*                                                                                                      */
/* Called by 'spu_Setup_Trans_Mapping_RSA'.                  */
/****************************************************************************************************/
/* Revision Description of Modification     Date        Who         */
/* --------         ---------------------------         ----        ---     */
/* 1.0      Original                    02/05/2001  RWH */
/*                                      */
/***************************************************************************************************/

DECLARE @export_map_model_id int,
    @export_map_detail_id int,
    @sequence int,
    @format_count tinyint,
    @previous_map_folder_name varchar(255)

-- Set @export_map_model_id to Orion
SELECT @export_map_model_id = 1

SELECT  @export_map_detail_id = 0

-- Does the required code already exist.
SELECT  @export_map_detail_id = export_map_detail_id
FROM        Export_Map_Detail
WHERE   target_field_name = @new_target_field_name

-- If it doesn't exist then add it in.
IF @export_map_detail_id = 0
BEGIN

    SELECT  @export_map_detail_id = MAX(export_map_detail_id) + 1,
            @sequence = MAX([sequence]) + 1
    FROM        Export_Map_Detail

    EXEC spe_Export_Map_Detail_add  @export_map_model_id,
                        @export_map_detail_id,
                        @new_target_field_name,
                        @sequence

    -- We are changing the  mapping code of existing nominal accounts here so we have
    -- to do a bit of a fudge. This is only necessary for upgrading existing databases in test.
    IF @new_target_field_name = 'INCGWP'
    BEGIN

        EXEC spu_ACT_Setup_update_parent_node     'INCOME',
                                        @new_mapping_leading_characters

    END

END
ELSE
BEGIN
-- If it already exists then see if corresponding Format records exist and if so then just delete them.
    SELECT  @format_count = count(*)
    FROM        Export_Map_Format
    WHERE   export_map_model_id = @export_map_model_id
    AND     export_map_detail_id = @export_map_detail_id

    IF @format_count > 0
    BEGIN

        -- Need to  move all accounts in previously mapped folder to new one.

        -- Get previous parent node.
        SELECT  @previous_map_folder_name = leading_characters
        FROM        Export_Map_Format
        WHERE   export_map_model_id = @export_map_model_id
        AND     export_map_detail_id = @export_map_detail_id
        AND     source_field_name = '{mapping_code}'

        EXEC spu_ACT_Setup_update_parent_node     @previous_map_folder_name,
                                        @new_mapping_leading_characters

        -- Remove previous Export_map_Format Records.
        DELETE  Export_Map_Format
        WHERE   export_map_model_id = @export_map_model_id
        AND     export_map_detail_id = @export_map_detail_id
    END
END

-- Insert required Export_Map_Format records.
EXEC spe_Export_Map_Format_add  @export_map_model_id,
                        @export_map_detail_id,
                        1,
                        '{account_type_id}',            -- source_field_name
                        1,                  -- sequence
                        @new_acc_type_leading_characters,   -- leading_characters
                        Null,                   -- trailing_characters
                        Null,                   -- start_position
                        Null,                   -- number_of_chars
                        Null,                   -- valid_value
                        '\',                    -- field_separator
                    0                   -- is_upper_case

EXEC spe_Export_Map_Format_add  @export_map_model_id,
                        @export_map_detail_id,
                        2,
                        '{mapping_code}',           -- source_field_name
                        2,                  -- sequence
                        @new_mapping_leading_characters,    -- leading_characters
                        Null,                   -- trailing_characters
                        Null,                   -- start_position
                        Null,                   -- number_of_chars
                        Null,                   -- valid_value
                        Null,                   -- field_separator
                    0                   -- is_upper_case
GO


