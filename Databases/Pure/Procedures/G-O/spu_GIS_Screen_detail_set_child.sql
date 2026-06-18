SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Screen_detail_set_child'
GO


CREATE PROCEDURE spu_GIS_Screen_detail_set_child
    @GIS_screen_id INT,
    @OLD_Child_screen_id INT,
    @NEW_Child_screen_id INT
AS


BEGIN

UPDATE  GIS_Screen_detail
SET child_screen_id = @NEW_child_screen_id
WHERE   GIS_Screen_id = @GIS_Screen_id
AND child_screen_id = @OLD_child_screen_id
END
GO


