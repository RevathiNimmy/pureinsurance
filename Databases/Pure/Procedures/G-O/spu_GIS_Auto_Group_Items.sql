SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GIS_Auto_Group_Items'
GO
/*
    Procedure : sp_GIS_Auto_Group_Items
    History : 26/11/2001 CTAF Created

    Auto grouping works this way :
    For each list item in type @gis_list_type
        Create a gis_list_grouping record
        Create a gis_list_grouping_items record
    Next
*/
CREATE PROCEDURE spu_GIS_Auto_Group_Items
    @gis_scheme_id int,
    @gis_list_type_id int
AS
BEGIN
    DECLARE @gli_gis_list_items_id int
    DECLARE @gli_code char(20)
    DECLARE @gis_list_grouping_id int

    /* Declare the cursor */
    DECLARE cCursor CURSOR FAST_FORWARD FOR
        SELECT gli.gis_list_items_id, gli.code
        FROM gis_list_items gli
        INNER JOIN gis_list_type_usage gltu
            ON gltu.gis_list_items_id = gli.gis_list_items_id
        WHERE gltu.gis_list_type_id = @gis_list_type_id
        AND gltu.version = (SELECT MAX(gltu.version)
                      FROM gis_list_type_usage gltu
                      WHERE gltu.gis_list_type_id = @gis_list_type_id)

    /* Open the cursor */
    OPEN cCursor

    FETCH NEXT FROM cCursor INTO @gli_gis_list_items_id, @gli_code

    WHILE @@FETCH_STATUS = 0 BEGIN
        -- CTAF 28/06/02 - Added scheme and type id checks
        IF EXISTS (SELECT * FROM gis_list_grouping
                   WHERE code = @gli_code
                AND gis_scheme_id = @gis_scheme_id
                AND gis_list_type_id = @gis_list_type_id)
        BEGIN

            /* Delete existing */
            --jes 12/06/02 added further constraint to the deletion
            DELETE FROM gis_list_grouping_items
            WHERE gis_list_grouping_id IN (
                SELECT gis_list_grouping_id
                FROM gis_list_grouping
                WHERE code = @gli_code
                AND gis_scheme_id=@gis_scheme_id
                AND gis_list_type_id=@gis_list_type_id)

            DELETE FROM gis_list_grouping
            WHERE code = @gli_code
            AND gis_scheme_id=@gis_scheme_id
            AND gis_list_type_id=@gis_list_type_id
        END

        /* Create GIS_List_Grouping */
        INSERT INTO GIS_List_Grouping
        ( gis_scheme_id, gis_list_type_id, code, is_deleted, description )
        VALUES
        ( @gis_scheme_id, @gis_list_type_id, @gli_code, 0, @gli_code)

        SELECT @gis_list_grouping_id = @@IDENTITY

        /* Create GIS_List_Grouping_Items */
        INSERT INTO gis_list_grouping_items
        ( gis_list_grouping_id, gis_scheme_id, gis_list_items_id )
        VALUES
        ( @gis_list_grouping_id, @gis_scheme_id, @gli_gis_list_items_id )

        /* Get the next record */
        FETCH NEXT FROM cCursor INTO @gli_gis_list_items_id, @gli_code
    END

    /* Close Cursor and clear up */
    CLOSE cCursor
    DEALLOCATE cCursor
END
GO

