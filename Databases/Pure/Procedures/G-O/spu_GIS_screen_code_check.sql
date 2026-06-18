SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_screen_code_check'
GO


CREATE PROCEDURE spu_GIS_screen_code_check
    @GIS_screen_id INT,
    @code VARCHAR(10)
AS


SELECT  H.gis_screen_id
FROM    GIS_screen H
WHERE   H.code = @code
AND H.GIS_screen_id <> @GIS_screen_id
GO


