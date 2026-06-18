SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Screen_detail_del'
GO


CREATE PROCEDURE spu_GIS_Screen_detail_del
    @GIS_screen_id int
AS


DELETE FROM GIS_Screen_detail

WHERE GIS_screen_id = @GIS_screen_id
GO


