SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Screen_children'
GO


CREATE PROCEDURE spu_GIS_Screen_children
    @GIS_screen_id INT
AS


BEGIN
SELECT  DISTINCT gsd.child_screen_id, 
                 gs.screen_type
FROM    GIS_screen_detail gsd,
        GIS_screen gs
WHERE   gsd.GIS_screen_id = @GIS_screen_id
AND     gsd.child_screen_id = gs.GIS_screen_id
END
GO
