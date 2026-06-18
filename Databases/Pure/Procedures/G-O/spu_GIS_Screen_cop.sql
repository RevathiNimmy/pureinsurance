SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GIS_Screen_cop'
GO
CREATE PROCEDURE spu_GIS_Screen_cop
    @old_GIS_screen_id INT OUTPUT,
    @new_Parent_id INT,
    @new_code VARCHAR(10) = NULL,
    @new_caption_id INT = NULL,
    @new_description VARCHAR(255)= NULL
AS
BEGIN
DECLARE @NEW_GIS_screen_id INT
DECLARE @old_code VARCHAR(10)

If RTrim(@new_code) = ''
	SELECT @new_code = NULL

If RTrim(@new_description) = ''
	SELECT @new_description = NULL

-- get the next available id for the new screen
SELECT @NEW_GIS_Screen_id = MAX(GIS_Screen_id) + 1
    FROM GIS_Screen WITH(NOLOCK) 

IF @NEW_GIS_Screen_id IS NULL
    SELECT @NEW_GIS_Screen_id = 1

	
IF @new_code ='' 
SET   @new_code = NULL
   
   
IF @new_description =''
SET   @new_description = NULL
   

-- get details about the old screen and format details for the new one
SELECT
    @old_code = code,
    @new_code = ISNULL(@new_code, left(ltrim(str(@NEW_GIS_Screen_id)) + rtrim(code), 10)),
    @new_description = ISNULL(@new_description, description),
    @new_caption_id = ISNULL(@new_caption_id, caption_id)
FROM GIS_Screen WITH(NOLOCK)
WHERE GIS_Screen_id = @old_GIS_Screen_id
-- now copy the screen
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
    @new_caption_id,
    @new_code,
    @new_description,
    is_deleted,
    getdate(),
    @new_parent_id,
    is_maintainable,
    script_defaults,
    script_dynamic_logic,
	screen_type,
    screen_height,
    screen_width
FROM GIS_Screen WITH(NOLOCK)
WHERE GIS_Screen_id = @old_GIS_Screen_id
-- now return the results
SELECT
    @NEW_GIS_screen_id,
    @old_code,
    @new_code,
    @new_description,
    @new_caption_id
END
GO
