SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GIS_Child_Screen_cop'
GO
CREATE PROCEDURE spu_GIS_Child_Screen_cop
    @GIS_screen_id INT OUTPUT,
    @Parent_id INT
AS
BEGIN
    DECLARE @NEW_GIS_screen_id INT

    SELECT @NEW_GIS_Screen_id = MAX(GIS_Screen_id) + 1
        FROM GIS_Screen

    IF @NEW_GIS_Screen_id IS NULL
        SELECT @NEW_GIS_Screen_id = 1

    INSERT INTO GIS_Screen (
        GIS_screen_id,
        GIS_data_model_id,
        caption_id,
        code,
        description,
        is_deleted,
        effective_date,
        parent_id,
        is_maintainable,
        script_defaults,
        script_dynamic_logic,
        screen_type,
        screen_height,
        screen_width)
    SELECT @NEW_GIS_screen_id,
        GIS_data_model_id,
        caption_id,
        left(ltrim(str(@NEW_GIS_Screen_id)) + rtrim(code), 10),
        description,
        is_deleted,
        getdate(),
        @parent_id,
        is_maintainable,
        script_defaults,
        script_dynamic_logic,
        screen_type,
        screen_height,
        screen_width
    FROM GIS_Screen
    WHERE GIS_Screen_id = @GIS_Screen_id

    SELECT @GIS_screen_id = @NEW_GIS_screen_id
END
GO

